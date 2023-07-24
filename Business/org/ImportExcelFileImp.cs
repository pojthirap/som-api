using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NLog;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Utils;
using MyFirstAzureWebApp.Models.importfile;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MyFirstAzureWebApp.common;
using System.Drawing;
using MyFirstAzureWebApp.Entity.custom;
using System.Data;
using MyFirstAzureWebApp.Models.profile;
using OfficeOpenXml.Style;

namespace MyFirstAzureWebApp.Business.org
{

    public class ImportExcelFileImp : IImportExcelFile
    {
        private Logger log = LogManager.GetCurrentClassLogger();
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImportExcelFileImp(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<ImportProspectCustom> importProspect(IFileManagerLogic _fileManagerLogic, IImportErrorFileLog importErrorFileLog, ImportProspectModel importProspectModel, UserProfileForBack userProfile)
        {
            //https://dzone.com/articles/import-and-export-excel-file-in-asp-net-core-31-ra
            //https://dev.to/moe23/aspnet-core-5-reading-and-exporting-excel-5de1

            long VAL_TOTAL_RECORD = 0;
            long VAL_TOTAL_SUCCESS = 0;
            long VAL_TOTAL_FAILED = 0;
            string VAL_PATH_FILE_ERROR = null;
            string fullPath = null;
            if (importProspectModel.ImageFile?.Length > 0)
                {

                IFormFile file = importProspectModel.ImageFile;
                string webRootPath = _hostingEnvironment.ContentRootPath + CommonConstant.IMPORT_FILE_TMP;
                string sFileExtension = Path.GetExtension(file.FileName);
                string todayNow = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string VAL_fileName = Path.GetFileNameWithoutExtension(file.FileName) + "E" + todayNow+ sFileExtension;
                fullPath = Path.Combine(webRootPath, VAL_fileName);
                
                using (var streams = System.IO.File.Create(fullPath))
                {
                    await file.CopyToAsync(streams);
                }

                var stream = importProspectModel.ImageFile.OpenReadStream();
                List<RowError> excelProspectFailed = new List<RowError>();

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();//package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var columnCount = worksheet.Dimension.Columns;

                    var lastRow = worksheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;
                    rowCount = lastRow;
                    VAL_TOTAL_RECORD = rowCount-1;
                    log.Debug("lastRow:" + lastRow);
                    Console.WriteLine("lastRow:" + lastRow);
                    for (var row = 2; row <= rowCount; row++)// skip Header is row 1
                    {
                        List<string> rowList = new List<string>();
                        for (var column = 1; column <= columnCount; column++)
                        {
                            rowList.Add(worksheet.Cells[row, column].Value?.ToString());
                        }
                        /*
                        // For Test
                            RowError re= new RowError();
                            re.row = row;
                            re.errorDesc = "Incorrect Format Data";
                            re.rowData = rowList;
                            excelProspectFailed.Add(re);
                        // For Test
                        */
                        
                        // process row
                        // Print Data of row
                        string rowStr = String.Join(", ", rowList);
                        log.Debug("Row:" + row + "=" + rowStr);
                        Console.WriteLine("Row:" + row + "=" + rowStr);

                        //Check Validate Data in row
                        bool chk = validateForImportProspect(rowList);
                        if (!chk)
                        {
                            RowError re = new RowError();
                            re.row = row;
                            re.errorDesc = "Incorrect Format Data";
                            re.rowData = rowList;
                            excelProspectFailed.Add(re);
                            VAL_TOTAL_FAILED++;
                            continue;
                        }
                        else
                        {
                            string VAL_PROSP_ACC_ID = null;
                            string valProspAccIdTmp = null;
                            /*switch (rowList.ElementAt(0)) // Column('DBD Flag')
                            {
                                case "Y":
                                    valProspAccIdTmp = await getProspAccIdCase_Y(rowList.ElementAt(3));//Column('Tax Number/ID Card')
                                    if (!String.IsNullOrEmpty(valProspAccIdTmp))
                                    {
                                        VAL_PROSP_ACC_ID = valProspAccIdTmp;
                                    }
                                    break;
                                case "N":
                                    valProspAccIdTmp = await getProspAccIdCase_N(rowList.ElementAt(3), rowList.ElementAt(1), rowList.ElementAt(15));//Column('Tax Number/ID Card') , Column('Prospect Name'), Column('ตำบล')) 
                                    if (!String.IsNullOrEmpty(valProspAccIdTmp))
                                    {
                                        VAL_PROSP_ACC_ID = valProspAccIdTmp;
                                    }
                                    break;
                                default:
                                    RowError re = new RowError();
                                    re.row = row;
                                    re.errorDesc = "Incorrect Data - DBD Flag";
                                    re.rowData = rowList;
                                    excelProspectFailed.Add(re);
                                    VAL_TOTAL_FAILED++;
                                    continue;
                            }// emd switch case
                            */

                            if("Y".Equals(rowList.ElementAt(0)) || "N".Equals(rowList.ElementAt(0))) // Column('DBD Flag'))
                            {
                                valProspAccIdTmp = await getProspAccIdCase_DBDFlag_Y_OR_N(rowList.ElementAt(3), rowList.ElementAt(1), rowList.ElementAt(15));//Column('Tax Number/ID Card')
                                if (!String.IsNullOrEmpty(valProspAccIdTmp))
                                {
                                    VAL_PROSP_ACC_ID = valProspAccIdTmp;
                                }
                            }
                            else
                            {

                                RowError re = new RowError();
                                re.row = row;
                                re.errorDesc = "Incorrect Data - DBD Flag";
                                re.rowData = rowList;
                                excelProspectFailed.Add(re);
                                VAL_TOTAL_FAILED++;
                                continue;
                            }


                            if (!String.IsNullOrEmpty(VAL_PROSP_ACC_ID))
                            {
                                RowError re = new RowError();
                                re.row = row;
                                re.errorDesc = "Duplicated Data";
                                re.rowData = rowList;
                                excelProspectFailed.Add(re);

                                List<string> dupRowList = await getProspAccIdDup(rowList.ElementAt(0), VAL_PROSP_ACC_ID);
                                re = new RowError();
                                re.row = row;
                                re.errorDesc = dupRowList.ElementAt(dupRowList.Count - 1);
                                re.rowData = dupRowList;
                                excelProspectFailed.Add(re);
                                VAL_TOTAL_FAILED++;
                            }
                            else
                            {
                                try
                                {
                                    // Process Data
                                    await importProspectProcess(rowList, userProfile);
                                    VAL_TOTAL_SUCCESS++;
                                }
                                catch (Exception ex)
                                {
                                    RowError re = new RowError();
                                    re.row = row;
                                    re.errorDesc = "Error : " + ex.Message + ":" + ex.ToString();
                                    re.rowData = rowList;
                                    excelProspectFailed.Add(re);
                                    VAL_TOTAL_FAILED++;
                                    continue;
                                }
                            }
                        }// end else valid
                    
                    }// end for loop
                }// end using package


                if (excelProspectFailed != null && excelProspectFailed.Count != 0)
                {
                    // Edit file
                    FileInfo fileInfo = new FileInfo(fullPath);

                    ExcelPackage packageEdit = new ExcelPackage(fileInfo);
                    ExcelWorksheet worksheetEdit = packageEdit.Workbook.Worksheets.FirstOrDefault();

                    int rowsEdit = worksheetEdit.Dimension.Rows;

                    var lastRow = worksheetEdit.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;
                    worksheetEdit.DeleteRow(2, lastRow, true);
                    //packageEdit.SaveAs(new FileInfo("output.xlsx"));

                    var columnCountEdit = worksheetEdit.Dimension.Columns + 1;
                    worksheetEdit.Cells[1, columnCountEdit].Value = "Error Description";
                    worksheetEdit.Column(columnCountEdit).Width = 40;

                    using (var r = worksheetEdit.Cells[1, columnCountEdit])
                    {
                        r.Merge = true;
                        r.Style.Font.Color.SetColor(Color.Black);
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        r.Style.Font.Bold = true;
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    int rowWrite = 2;
                    foreach(RowError re in excelProspectFailed)
                    {
                        int colWrite = 1;
                        for (; colWrite <= re.rowData.Count; colWrite++)
                        {
                            worksheetEdit.Cells[rowWrite, colWrite].Value = re.rowData.ElementAt(colWrite-1);
                        }
                        if (colWrite <= columnCountEdit)
                        {
                            worksheetEdit.Cells[rowWrite, colWrite++].Value = re.errorDesc;
                        }
                        rowWrite++;
                    }

                    // save changes
                    packageEdit.Save();

                    int fileId = await importErrorFileLog.GetImportErrorFileLogSeq();
                    VAL_PATH_FILE_ERROR = fileId.ToString();
                    // Upload file
                    await _fileManagerLogic.Upload(importProspectModel, fileId.ToString(), CommonConstant.IMG_CONTAINER_SOM_IMPORT_ERROR_FILE, sFileExtension, VAL_fileName, fullPath);
                    long length = new FileInfo(fullPath).Length;
                    ImportErrorLogFileModel importErrorLogFileModel = new ImportErrorLogFileModel();
                    importErrorLogFileModel.FileId = fileId.ToString();
                    importErrorLogFileModel.FileExt = sFileExtension;
                    importErrorLogFileModel.FileName = VAL_fileName;
                    importErrorLogFileModel.FileSize = length.ToString();
                    importErrorLogFileModel.ImportDataType = "P";
                    int recordInsert = await importErrorFileLog.uploadErrorFileLog(importErrorLogFileModel, userProfile);


                }

            }

            // Delete file
            File.Delete(fullPath);

            ImportProspectCustom responseData = new ImportProspectCustom();
            responseData.TotalRecord = VAL_TOTAL_RECORD.ToString();
            responseData.TotalSuccess = VAL_TOTAL_SUCCESS.ToString();
            responseData.TotalFailed = VAL_TOTAL_FAILED.ToString();
            responseData.PathFileError = VAL_PATH_FILE_ERROR;
            return responseData;
        }


        public async Task<int> importProspectProcess(List<string> rowList, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_ACCOUNT_SEQ", p);
                        var nextVal = (int)p.Value;

                        var PROSP_ACC_ID = nextVal;
                        var ACC_NAME = rowList.ElementAt(1);// rowData->Column('Prospect Name')
                        var BRAND_ID = rowList.ElementAt(2);// rowData->Column('Brand')
                        var IDENTIFY_ID = rowList.ElementAt(3);// rowData->Column('Tax Number/ID Card')
                        var ACC_GROUP_REF = rowList.ElementAt(4);// rowData->Column('Prospect Group Ref.')
                        var SOURCE_TYPE = '1';// -- Fix

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO PROSPECT_ACCOUNT ([PROSP_ACC_ID], [ACC_NAME], [BRAND_ID], [IDENTIFY_ID], [ACC_GROUP_REF], [REMARK], [SOURCE_TYPE], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                        queryBuilder.AppendFormat(" VALUES(@PROSP_ACC_ID, @ACC_NAME, @BRAND_ID, @IDENTIFY_ID, @ACC_GROUP_REF, @REMARK, @SOURCE_TYPE, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME() ) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ACC_ID", PROSP_ACC_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ACC_NAME", ACC_NAME));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BRAND_ID", BRAND_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("IDENTIFY_ID", IDENTIFY_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ACC_GROUP_REF", ACC_GROUP_REF));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("REMARK", null));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SOURCE_TYPE", SOURCE_TYPE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);



                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_SEQ", p);
                        nextVal = (int)p.Value;
                        var PROSPECT_ID = nextVal;
                        var BU_ID = userProfile.getBuId();
                        var SALE_REP_ID = userProfile.getEmpId();
                        var GROUP_CODE = userProfile.getAdmEmployeeGroupCode();
                        var PROSPECT_TYPE = '0';
                        var PROSPECT_STATUS = '0';
                        var DBD_CODE = rowList.ElementAt(21);// rowData->Column('Code DBD')
                        var DBD_CORP_TYPE = rowList.ElementAt(22);// rowData->Column('TypeName_corporate')
                        var DBD_JURISTIC_STATUS = rowList.ElementAt(23);// rowData->Column('JuristicStatus')
                        var DBD_REG_CAPITAL = rowList.ElementAt(24);// rowData->Column('RegisterCapital')
                        var DBD_TOTAL_INCOME = rowList.ElementAt(25);// rowData->Column('TotalIncome')
                        var DBD_PROFIT_LOSS = rowList.ElementAt(26);// rowData->Column('ProfitLoss')
                        var DBD_TOTAL_ASSET = rowList.ElementAt(27);// rowData->Column('TotalAssets')
                        var MAIN_FLAG = 'Y';

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO PROSPECT ([PROSPECT_ID], [PROSP_ACC_ID], [GROUP_CODE], [BU_ID], [SALE_REP_ID], [PROSPECT_TYPE], [MAIN_FLAG], [PROSPECT_STATUS], [DBD_CODE], [DBD_CORP_TYPE], [DBD_JURISTIC_STATUS], [DBD_REG_CAPITAL], [DBD_TOTAL_INCOME], [DBD_PROFIT_LOSS], [DBD_TOTAL_ASSET], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                        queryBuilder.AppendFormat(" VALUES(@PROSPECT_ID, @PROSP_ACC_ID, @GROUP_CODE, @BU_ID, @SALE_REP_ID, @PROSPECT_TYPE, @MAIN_FLAG, @PROSPECT_STATUS, @DBD_CODE, @DBD_CORP_TYPE, @DBD_JURISTIC_STATUS, @DBD_REG_CAPITAL, @DBD_TOTAL_INCOME, @DBD_PROFIT_LOSS, @DBD_TOTAL_ASSET, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME() ) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", PROSPECT_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ACC_ID", PROSP_ACC_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", GROUP_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("BU_ID", BU_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SALE_REP_ID", SALE_REP_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_TYPE", PROSPECT_TYPE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MAIN_FLAG", MAIN_FLAG));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_STATUS", PROSPECT_STATUS));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DBD_CODE", DBD_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DBD_CORP_TYPE", DBD_CORP_TYPE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DBD_JURISTIC_STATUS", DBD_JURISTIC_STATUS));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DBD_REG_CAPITAL", DBD_REG_CAPITAL));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DBD_TOTAL_INCOME", DBD_TOTAL_INCOME));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DBD_PROFIT_LOSS", DBD_PROFIT_LOSS));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DBD_TOTAL_ASSET", DBD_TOTAL_ASSET));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_ADDRESS_SEQ", p);
                        nextVal = (int)p.Value;
                        var PROSP_ADDR_ID = nextVal;
                        //var PROSPECT_ID = ;[dbo].[PROSPECT].[PROSPECT_ID]
                        //var PROSP_ACC_ID = [dbo].[PROSPECT_ACCOUNT].[PROSP_ACC_ID]
                        var ADDR_NO = rowList.ElementAt(5);// rowData->Column('เลขที่บ้าน')
                        var MOO = rowList.ElementAt(6);// rowData->Column('หมู่ที่')
                        var SOI = rowList.ElementAt(7);// rowData->Column('ซอย')
                        var STREET = rowList.ElementAt(8);// rowData->Column('ถนน')
                        var TELL_NO = rowList.ElementAt(9);// rowData->Column('Phone No')
                        var FAX_NO = rowList.ElementAt(10);// rowData->Column('Fax No')
                        var LATITUDE = rowList.ElementAt(11);// rowData->Column('Latitude')
                        var LONGITUDE = rowList.ElementAt(12);// rowData->Column('Longitude')
                        var PROVINCE_CODE = rowList.ElementAt(13);// rowData->Column('จังหวัด')
                        var DISTRICT_CODE = rowList.ElementAt(14);// rowData->Column('อำเภอ')
                        var SUBDISTRICT_CODE = rowList.ElementAt(15);// rowData->Column('ตำบล')
                        var POST_CODE = rowList.ElementAt(16);// rowData->Column('รหัสไปรษณีย์')
                        MAIN_FLAG = 'Y';

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO PROSPECT_ADDRESS ([PROSP_ADDR_ID], [PROSPECT_ID], [PROSP_ACC_ID], [ADDR_NO], [MOO], [SOI], [STREET], [TELL_NO], [FAX_NO], [LATITUDE], [LONGITUDE], [PROVINCE_CODE], [PROVINCE_DBD], [DISTRICT_CODE], [SUBDISTRICT_CODE], [POST_CODE], [MAIN_FLAG], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                        queryBuilder.AppendFormat(" VALUES(@PROSP_ADDR_ID, @PROSPECT_ID, @PROSP_ACC_ID, @ADDR_NO, @MOO, @SOI, @STREET, @TELL_NO, @FAX_NO, @LATITUDE, @LONGITUDE, @PROVINCE_CODE, @PROVINCE_DBD, @DISTRICT_CODE, @SUBDISTRICT_CODE, @POST_CODE, @MAIN_FLAG, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()  ) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ADDR_ID", PROSP_ADDR_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", PROSPECT_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ACC_ID", PROSP_ACC_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("ADDR_NO", ADDR_NO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MOO", MOO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SOI", SOI));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("STREET", STREET));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TELL_NO", TELL_NO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FAX_NO", FAX_NO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LATITUDE", LATITUDE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LONGITUDE", LONGITUDE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROVINCE_CODE", PROVINCE_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROVINCE_DBD", null));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("DISTRICT_CODE", DISTRICT_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("SUBDISTRICT_CODE", SUBDISTRICT_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("POST_CODE", POST_CODE));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MAIN_FLAG", MAIN_FLAG));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                        p.Direction = System.Data.ParameterDirection.Output;
                        context.Database.ExecuteSqlRaw("set @result = next value for PROSPECT_CONTACT_SEQ", p);
                        nextVal = (int)p.Value;
                        var PROSP_CONTACT_ID = nextVal;
                        //var PROSPECT_ID = [dbo].[PROSPECT].[PROSPECT_ID]
                        //var PROSP_ACC_ID = [dbo].[PROSPECT_ACCOUNT].[PROSP_ACC_ID]
                        var FIRST_NAME = rowList.ElementAt(17);// rowData->Column('First Name')
                        var LAST_NAME = rowList.ElementAt(18);// rowData->Column('Last Name')
                        var PHONE_NO = rowList.ElementAt(19);// rowData->Column('Mobile Phone')
                        var EMAIL = rowList.ElementAt(20);// rowData->Column('Email')
                        MAIN_FLAG = 'Y';

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO PROSPECT_CONTACT ([PROSP_CONTACT_ID], [PROSPECT_ID], [PROSP_ACC_ID], [FIRST_NAME], [LAST_NAME], [PHONE_NO], [EMAIL], [MAIN_FLAG], [ACTIVE_FLAG], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM]) ");
                        queryBuilder.AppendFormat(" VALUES(@PROSP_CONTACT_ID, @PROSPECT_ID, @PROSP_ACC_ID, @FIRST_NAME, @LAST_NAME, @PHONE_NO, @EMAIL, @MAIN_FLAG, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()  ) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_CONTACT_ID", PROSP_CONTACT_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", PROSPECT_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSP_ACC_ID", PROSP_ACC_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("FIRST_NAME", FIRST_NAME));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("LAST_NAME", LAST_NAME));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("PHONE_NO", PHONE_NO));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EMAIL", EMAIL));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("MAIN_FLAG", MAIN_FLAG));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());


                        var VAL_FUNCTION_TAB = 1;
                        var VAL_DESCRIPTION = rowList.ElementAt(0).Trim().Equals("Y") ? "Import Prospect (DBD)" : "Import Prospect";// rowData->Column('DBD Flag') = 'Y' ? 'Import Prospect (DBD)' : 'Import Prospect'

                        sqlParameters = new List<SqlParameter>();// Add New
                        queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" INSERT INTO PROSPECT_FEED ([FEED_ID], [PROSPECT_ID], [FUNCTION_TAB], [DESCRIPTION], [CREATE_USER], [CREATE_DTM], [UPDATE_USER], [UPDATE_DTM])  ");
                        queryBuilder.AppendFormat(" VALUES(NEXT VALUE FOR PROSPECT_FEED_SEQ, @PROSPECT_ID , @VAL_FUNCTION_TAB, @VAL_DESCRIPTION, @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME()) ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("PROSPECT_ID", PROSPECT_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_FUNCTION_TAB", VAL_FUNCTION_TAB));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("VAL_DESCRIPTION", VAL_DESCRIPTION));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);




                        transaction.Commit();
                        return numberOfRowInserted;



                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }


        /*public async Task<String> getProspAccIdCase_Y(string identifyId)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select PA.PROSP_ACC_ID from PROSPECT_ACCOUNT PA where PA.IDENTIFY_ID = @IDENTIFY_ID ");
                QueryUtils.addParam(command, "IDENTIFY_ID", identifyId);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "PROSP_ACC_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }
        public async Task<String> getProspAccIdCase_N(string identifyId, string accName, string subdistrictCode)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select AC.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT AC ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PA on PA.PROSP_ACC_ID = AC.PROSP_ACC_ID and PA.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" where IIF(AC.IDENTIFY_ID = '',null,AC.IDENTIFY_ID) = @IDENTIFY_ID ");
                queryBuilder.AppendFormat(" or (AC.ACC_NAME = @ACC_NAME  and PA.SUBDISTRICT_CODE = @SUBDISTRICT_CODE)  ");
                QueryUtils.addParam(command, "IDENTIFY_ID", identifyId);// Add new
                QueryUtils.addParam(command, "ACC_NAME", accName);// Add new
                QueryUtils.addParam(command, "SUBDISTRICT_CODE", subdistrictCode);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "PROSP_ACC_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }
        */
        public async Task<String> getProspAccIdCase_DBDFlag_Y_OR_N(string identifyId, string prospectName, string tumbol)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select AC.PROSP_ACC_ID ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT AC ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PA on PA.PROSP_ACC_ID = AC.PROSP_ACC_ID and PA.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" where IIF(AC.IDENTIFY_ID = '',null,AC.IDENTIFY_ID) = @identifyId  ");
                queryBuilder.AppendFormat(" or (AC.ACC_NAME = @prospectName and PA.SUBDISTRICT_CODE = @tumbol )  ");
                QueryUtils.addParam(command, "identifyId", identifyId);// Add new
                QueryUtils.addParam(command, "prospectName", prospectName);// Add new
                QueryUtils.addParam(command, "tumbol", tumbol);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "PROSP_ACC_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }


        public async Task<List<string>> getProspAccIdDup(string DBDFlag, string VAL_PROSP_ACC_ID)
        {
            List<string> resList = null;
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();

                queryBuilder.AppendFormat(" select @DBDFlag DBD_FLAG,AC.ACC_NAME,AC.BRAND_ID,AC.IDENTIFY_ID,AC.ACC_GROUP_REF ");
                queryBuilder.AppendFormat(" ,PA.ADDR_NO,PA.MOO,PA.SOI,PA.STREET,PA.TELL_NO,PA.FAX_NO,PA.LATITUDE,PA.LONGITUDE ");
                queryBuilder.AppendFormat(" ,PA.PROVINCE_CODE,PA.DISTRICT_CODE,PA.SUBDISTRICT_CODE,PA.POST_CODE ");
                queryBuilder.AppendFormat(" ,PC.FIRST_NAME,PC.LAST_NAME,PC.PHONE_NO,PC.EMAIL ");
                queryBuilder.AppendFormat(" ,PP.DBD_CODE,PP.DBD_CORP_TYPE,PP.DBD_JURISTIC_STATUS,PP.DBD_REG_CAPITAL,PP.DBD_TOTAL_INCOME,PP.DBD_PROFIT_LOSS,PP.DBD_TOTAL_ASSET ");
                queryBuilder.AppendFormat(" ,'SOM Data' ERROR_DESC ");
                queryBuilder.AppendFormat(" from PROSPECT_ACCOUNT AC ");
                queryBuilder.AppendFormat(" inner join PROSPECT_ADDRESS PA on PA.PROSP_ACC_ID = AC.PROSP_ACC_ID and PA.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT_CONTACT PC on PC.PROSP_ACC_ID = AC.PROSP_ACC_ID and PC.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" inner join PROSPECT PP on PP.PROSP_ACC_ID = AC.PROSP_ACC_ID and PP.MAIN_FLAG = 'Y' ");
                queryBuilder.AppendFormat(" where AC.PROSP_ACC_ID = @VAL_PROSP_ACC_ID  ");
                QueryUtils.addParam(command, "DBDFlag", DBDFlag);// Add new
                QueryUtils.addParam(command, "VAL_PROSP_ACC_ID", VAL_PROSP_ACC_ID);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resList = new List<string>();
                        IDataRecord record = (IDataRecord)reader;
                        resList.Add(QueryUtils.getValueAsString(record, "DBD_FLAG"));
                        resList.Add(QueryUtils.getValueAsString(record, "ACC_NAME"));
                        resList.Add(QueryUtils.getValueAsString(record, "BRAND_ID"));
                        resList.Add(QueryUtils.getValueAsString(record, "IDENTIFY_ID"));
                        resList.Add(QueryUtils.getValueAsString(record, "ACC_GROUP_REF"));
                        resList.Add(QueryUtils.getValueAsString(record, "ADDR_NO"));
                        resList.Add(QueryUtils.getValueAsString(record, "MOO"));
                        resList.Add(QueryUtils.getValueAsString(record, "SOI"));
                        resList.Add(QueryUtils.getValueAsString(record, "STREET"));
                        resList.Add(QueryUtils.getValueAsString(record, "TELL_NO"));
                        resList.Add(QueryUtils.getValueAsString(record, "FAX_NO"));
                        resList.Add(QueryUtils.getValueAsString(record, "LATITUDE"));
                        resList.Add(QueryUtils.getValueAsString(record, "LONGITUDE"));
                        resList.Add(QueryUtils.getValueAsString(record, "PROVINCE_CODE"));
                        resList.Add(QueryUtils.getValueAsString(record, "DISTRICT_CODE"));
                        resList.Add(QueryUtils.getValueAsString(record, "SUBDISTRICT_CODE"));
                        resList.Add(QueryUtils.getValueAsString(record, "POST_CODE"));
                        resList.Add(QueryUtils.getValueAsString(record, "FIRST_NAME"));
                        resList.Add(QueryUtils.getValueAsString(record, "LAST_NAME"));
                        resList.Add(QueryUtils.getValueAsString(record, "PHONE_NO"));
                        resList.Add(QueryUtils.getValueAsString(record, "EMAIL"));
                        resList.Add(QueryUtils.getValueAsString(record, "DBD_CODE"));
                        resList.Add(QueryUtils.getValueAsString(record, "DBD_CORP_TYPE"));
                        resList.Add(QueryUtils.getValueAsString(record, "DBD_JURISTIC_STATUS"));
                        resList.Add(QueryUtils.getValueAsString(record, "DBD_REG_CAPITAL"));
                        resList.Add(QueryUtils.getValueAsString(record, "DBD_TOTAL_INCOME"));
                        resList.Add(QueryUtils.getValueAsString(record, "DBD_PROFIT_LOSS"));
                        resList.Add(QueryUtils.getValueAsString(record, "DBD_TOTAL_ASSET"));
                        resList.Add(QueryUtils.getValueAsString(record, "ERROR_DESC"));
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return resList;
        }

        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
        private bool validateForImportProspect(List<string> row)
        {
            try
            {
                if (String.IsNullOrEmpty(row.ElementAt(1)))// col 2 Not Null
                    return false;

                if (!String.IsNullOrEmpty(row.ElementAt(2)) && !NumberUtils.isNumber(row.ElementAt(2)))// col 3
                    return false;
                if ("Y".Equals(row.ElementAt(0)) && String.IsNullOrEmpty(row.ElementAt(3)))// DBD Flag is "Y" check Tax Number/Id Card can't null
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(3)) && !NumberUtils.isNumber(row.ElementAt(3)))// col 4 Tax Number/Id Card
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(4)) && !NumberUtils.isNumber(row.ElementAt(4)))// col 5
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(6)) && !NumberUtils.isNumber(row.ElementAt(6)))// col 7
                    return false;

                /*if (!NumberUtils.isNumber(row.ElementAt(9)))// col 10
                    return false;
                if (!NumberUtils.isNumber(row.ElementAt(10)))// col 11
                    return false;*/

                if (!String.IsNullOrEmpty(row.ElementAt(11)) && !NumberUtils.isDecimal(row.ElementAt(11)))// col 12
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(12)) && !NumberUtils.isDecimal(row.ElementAt(12)))// col 13
                    return false;

                if (String.IsNullOrEmpty(row.ElementAt(13)) || !NumberUtils.isNumber(row.ElementAt(13)))// col 14  Not Null
                    return false;
                if (String.IsNullOrEmpty(row.ElementAt(14)) || !NumberUtils.isNumber(row.ElementAt(14)))// col 15 Not Null
                    return false;
                if (String.IsNullOrEmpty(row.ElementAt(15)))// col 16 Not Null Tombol
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(16)) && !NumberUtils.isNumber(row.ElementAt(16)))// col 17
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(19)) && !NumberUtils.isNumber(row.ElementAt(19)))// col 20
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(21)) && !NumberUtils.isNumber(row.ElementAt(21)))// col 22
                    return false;


                if (!String.IsNullOrEmpty(row.ElementAt(24)) && !NumberUtils.isDecimal(row.ElementAt(24)))// col 25
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(25)) && !NumberUtils.isDecimal(row.ElementAt(25)))// col 26
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(26)) && !NumberUtils.isDecimal(row.ElementAt(26)))// col 27
                    return false;
                if (!String.IsNullOrEmpty(row.ElementAt(27)) && !NumberUtils.isDecimal(row.ElementAt(27)))// col 28
                    return false;

            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }


        // New Service 
        
        public async Task<ImportEmpToSaleGroupCustom> importEmpToSaleGroup(IFileManagerLogic _fileManagerLogic, IImportErrorFileLog importErrorFileLog, ImportEmpToSaleGroupModel importEmpToSaleGroupModel, UserProfileForBack userProfile)
        {

            long VAL_TOTAL_RECORD = 0;
            long VAL_TOTAL_SUCCESS = 0;
            long VAL_TOTAL_FAILED = 0;
            string VAL_PATH_FILE_ERROR = null;
            string fullPath = null;
            if (importEmpToSaleGroupModel.ImageFile?.Length > 0)
                {

                IFormFile file = importEmpToSaleGroupModel.ImageFile;
                string webRootPath = _hostingEnvironment.ContentRootPath + CommonConstant.IMPORT_FILE_TMP;
                string sFileExtension = Path.GetExtension(file.FileName);
                string todayNow = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string VAL_fileName = Path.GetFileNameWithoutExtension(file.FileName) + "E" + todayNow+ sFileExtension;
                fullPath = Path.Combine(webRootPath, VAL_fileName);
                
                using (var streams = System.IO.File.Create(fullPath))
                {
                    await file.CopyToAsync(streams);
                }

                var stream = importEmpToSaleGroupModel.ImageFile.OpenReadStream();
                List<RowError> excelProspectFailed = new List<RowError>();

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();//package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var columnCount = worksheet.Dimension.Columns;

                    var lastRow = worksheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;
                    rowCount = lastRow;
                    VAL_TOTAL_RECORD = rowCount-1;
                    log.Debug("lastRow:" + lastRow);
                    Console.WriteLine("lastRow:" + lastRow);
                    for (var row = 2; row <= rowCount; row++)// skip Header is row 1
                    {
                        List<string> rowList = new List<string>();
                        for (var column = 1; column <= columnCount; column++)
                        {
                            rowList.Add(worksheet.Cells[row, column].Value?.ToString());
                        }
                        /*
                        // For Test
                            RowError re= new RowError();
                            re.row = row;
                            re.errorDesc = "Incorrect Format Data";
                            re.rowData = rowList;
                            excelProspectFailed.Add(re);
                        // For Test
                        */
                        
                        // process row
                        // Print Data of row
                        string rowStr = String.Join(", ", rowList);
                        log.Debug("Row:" + row + "=" + rowStr);
                        Console.WriteLine("Row:" + row + "=" + rowStr);

 

                        //Check Validate Data in row
                        bool chk = true;// validateForImportProspect(rowList);
                        if (!chk)
                        {
                            /*RowError re = new RowError();
                            re.row = row;
                            re.errorDesc = "Incorrect Format Data";
                            re.rowData = rowList;
                            excelProspectFailed.Add(re);
                            VAL_TOTAL_FAILED++;
                            continue;*/
                        }
                        else
                        {
                            string VAL_EMP_ID = null;
                            string valEmpIdTmp = null;

                            valEmpIdTmp = await getEmployeeIdForImportEmpToSaleGroup(rowList.ElementAt(0), importEmpToSaleGroupModel.GroupCode);//Column('Employee ID')
                            if (!String.IsNullOrEmpty(valEmpIdTmp))
                            {
                                VAL_EMP_ID = valEmpIdTmp;
                            }

                            if (!String.IsNullOrEmpty(VAL_EMP_ID))
                            {
                                RowError re = new RowError();
                                re.row = row;
                                re.errorDesc = "Duplicated Data";
                                re.rowData = rowList;
                                excelProspectFailed.Add(re);
                                VAL_TOTAL_FAILED++;
                            }else
                            {
                                try
                                {
                                    // Process Data
                                    int numberOfRowInserted = await importEmpToSaleGroupProcess(rowList, importEmpToSaleGroupModel, userProfile);
                                    if (numberOfRowInserted == 0)
                                    {
                                        RowError re = new RowError();
                                        re.row = row;
                                        re.errorDesc = "EMP_ID Not Found Update";
                                        re.rowData = rowList;
                                        excelProspectFailed.Add(re);
                                        VAL_TOTAL_FAILED++;
                                    }
                                    else
                                    {
                                        VAL_TOTAL_SUCCESS++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    RowError re = new RowError();
                                    re.row = row;
                                    re.errorDesc = "Error : " + ex.Message + ":" + ex.ToString();
                                    re.rowData = rowList;
                                    excelProspectFailed.Add(re);
                                    VAL_TOTAL_FAILED++;
                                    continue;
                                }
                            }
                        }// end else valid
                    
                    }// end for loop
                }// end using package


                if (excelProspectFailed != null && excelProspectFailed.Count != 0)
                {
                    // Edit file
                    FileInfo fileInfo = new FileInfo(fullPath);

                    ExcelPackage packageEdit = new ExcelPackage(fileInfo);
                    ExcelWorksheet worksheetEdit = packageEdit.Workbook.Worksheets.FirstOrDefault();

                    int rowsEdit = worksheetEdit.Dimension.Rows;

                    var lastRow = worksheetEdit.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;
                    worksheetEdit.DeleteRow(2, lastRow, true);
                    //packageEdit.SaveAs(new FileInfo("output.xlsx"));

                    var columnCountEdit = worksheetEdit.Dimension.Columns + 1;
                    worksheetEdit.Cells[1, columnCountEdit].Value = "Error Description";
                    worksheetEdit.Column(columnCountEdit).Width = 40;

                    using (var r = worksheetEdit.Cells[1, columnCountEdit])
                    {
                        r.Merge = true;
                        r.Style.Font.Color.SetColor(Color.Black);
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        r.Style.Font.Bold = true;
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    int rowWrite = 2;
                    foreach(RowError re in excelProspectFailed)
                    {
                        int colWrite = 1;
                        for (; colWrite <= re.rowData.Count; colWrite++)
                        {
                            worksheetEdit.Cells[rowWrite, colWrite].Value = re.rowData.ElementAt(colWrite-1);
                        }
                        if (colWrite <= columnCountEdit)
                        {
                            worksheetEdit.Cells[rowWrite, colWrite++].Value = re.errorDesc;
                        }
                        rowWrite++;
                    }

                    // save changes
                    packageEdit.Save();

                    int fileId = await importErrorFileLog.GetImportErrorFileLogSeq();
                    VAL_PATH_FILE_ERROR = fileId.ToString();
                    // Upload file
                    await _fileManagerLogic.Upload(importEmpToSaleGroupModel, fileId.ToString(), CommonConstant.IMG_CONTAINER_SOM_IMPORT_ERROR_FILE, sFileExtension, VAL_fileName, fullPath);
                    long length = new FileInfo(fullPath).Length;
                    ImportErrorLogFileModel importErrorLogFileModel = new ImportErrorLogFileModel();
                    importErrorLogFileModel.FileId = fileId.ToString();
                    importErrorLogFileModel.FileExt = sFileExtension;
                    importErrorLogFileModel.FileName = VAL_fileName;
                    importErrorLogFileModel.FileSize = length.ToString();
                    importErrorLogFileModel.ImportDataType = "E";
                    int recordInsert = await importErrorFileLog.uploadErrorFileLog(importErrorLogFileModel, userProfile);

                }

            }

            // Delete file
            File.Delete(fullPath);

            ImportEmpToSaleGroupCustom responseData = new ImportEmpToSaleGroupCustom();
            responseData.TotalRecord = VAL_TOTAL_RECORD.ToString();
            responseData.TotalSuccess = VAL_TOTAL_SUCCESS.ToString();
            responseData.TotalFailed = VAL_TOTAL_FAILED.ToString();
            responseData.PathFileError = VAL_PATH_FILE_ERROR;
            return responseData;
        }


        public async Task<int> importEmpToSaleGroupProcess(List<string> rowList, ImportEmpToSaleGroupModel importEmpToSaleGroupModel, UserProfileForBack userProfile)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        var EMP_ID = rowList.ElementAt(0);// rowData->Column('Employee ID')

                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE ADM_EMPLOYEE  ");
                        queryBuilder.AppendFormat(" SET [GROUP_CODE]=@GROUP_CODE, [UPDATE_USER]=@USER, [UPDATE_DTM]=dbo.GET_SYSDATETIME() ");
                        queryBuilder.AppendFormat(" WHERE EMP_ID = @EMP_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("GROUP_CODE", importEmpToSaleGroupModel.GroupCode));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("EMP_ID", EMP_ID));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("USER", userProfile.getUserName()));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);


                        transaction.Commit();
                        return numberOfRowInserted;



                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }

        public async Task<String> getEmployeeIdForImportEmpToSaleGroup(string empId, string groupCode)
        {
            String o = "";
            using (var context = new MyAppContext())
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                context.Database.OpenConnection();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendFormat(" select EMP_ID from ADM_EMPLOYEE where EMP_ID = @EMP_ID and GROUP_CODE = @GROUP_CODE ");
                QueryUtils.addParam(command, "EMP_ID", empId);// Add new
                QueryUtils.addParam(command, "GROUP_CODE", groupCode);// Add new
                log.Debug("Query:" + queryBuilder.ToString());
                Console.WriteLine("Query:" + queryBuilder.ToString());
                command.CommandText = queryBuilder.ToString();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;

                        o = QueryUtils.getValueAsString(record, "EMP_ID");
                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return o;
        }





    }
    public class RowError
    {
        public int row { get; set; }
        public string errorDesc { get; set; }
        public List<string> rowData { get; set; }
    }
    


}
