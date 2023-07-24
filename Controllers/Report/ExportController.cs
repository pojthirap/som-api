using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyFirstAzureWebApp.Business.rep;
using MyFirstAzureWebApp.Controllers;
using MyFirstAzureWebApp.ModelCriteriaReport;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.ModelsReport;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace MyFirstAzureWebApp.Controllers.Report
{
    [Route("exp")]
    [ApiController]
    public class ExportController : BaseController
    {
        private IReport reportImp;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExportController(IWebHostEnvironment hostingEnvironment)
        {
            reportImp = new ReportImp();
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("rep01VisitPlanRawData")]
        public async Task<IActionResult> rep01VisitPlanRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "01VisitPlanRawData.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp01VisitPlanRawData", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep01VisitPlanRawDataResult> result = await reportImp.SearchRep01VisitPlanRawData(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var no = 1;
                foreach (var item in result.records)
                {
                    var col = 1;
                    worksheet.Cells[row, col++].Value = no++;
                    worksheet.Cells[row, col++].Value = item.saleRepName;
                    worksheet.Cells[row, col++].Value = item.saleRepId;
                    worksheet.Cells[row, col++].Value = item.visitPlanId;
                    worksheet.Cells[row, col++].Value = item.visitPlanName;
                    worksheet.Cells[row, col++].Value = item.visitType;
                    worksheet.Cells[row, col++].Value = item.visitDate;
                    worksheet.Cells[row, col++].Value = item.accName;
                    worksheet.Cells[row, col++].Value = item.prospectType;
                    worksheet.Cells[row, col++].Value = item.startMileNo;
                    worksheet.Cells[row, col++].Value = item.finishMileNo;
                    worksheet.Cells[row, col++].Value = item.totalKmInput;
                    worksheet.Cells[row, col++].Value = item.totalKmSystem;
                    worksheet.Cells[row, col++].Value = item.planStartTime;
                    worksheet.Cells[row, col++].Value = item.planEndTime;
                    worksheet.Cells[row, col++].Value = item.visitCheckinDtm;
                    worksheet.Cells[row, col++].Value = item.visitCheckoutDtm;
                    worksheet.Cells[row, col++].Value = item.reasonNameTh;

                    row++;
                }
                onBeforeSendResponse("exp01VisitPlanRawData", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/"+ MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }

        [HttpPost("rep16Assign")]
        public async Task<IActionResult> rep16Assign(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "16Assign.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp16Assign", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep16AssignResult> results = await reportImp.SearchRep16Assign(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");
            Rep16AssignResult result = results.records[0];
            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);

                var row = 3;
                ExcelCellAddress start = worksheet.Cells[row, 1].Start;
                ExcelCellAddress end = worksheet.Cells[row, int.Parse(result.totalColm)].End;
                using (ExcelRange range = worksheet.Cells[start.Row, start.Column, end.Row, end.Column])
                {
                    range.Merge = true;
                    range.Value = "ชื่อโครงการ : " + result.tpNameTh;
                    //range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#A9D08E"));
                    //range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    row++;
                }
                start = worksheet.Cells[row, 1].Start;
                end = worksheet.Cells[row, int.Parse(result.totalColm)].End;
                using (ExcelRange range = worksheet.Cells[start.Row, start.Column, end.Row, end.Column])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#A5A5A5"));
                }

                do {
                    if (result.headColumn != null)
                    {
                        var col = 1;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo1; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo2; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo3; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo4; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo5; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo6; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo7; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo8; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo9; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo10; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo11; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo12; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo13; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo14; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo15; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo16; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo17; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo18; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo19; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo20; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo21; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo22; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo23; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo24; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo25; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo26; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo27; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo28; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo29; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo30; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo31; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo32; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo33; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo34; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo35; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo36; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo37; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo38; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo39; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo40; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo41; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo42; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo43; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo44; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo45; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo46; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo47; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo48; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo49; if (col > int.Parse(result.totalColm)) break;
                        worksheet.Cells[row, col++].Value = result.headColumn.titleColmNo50; 
                    }
                    
                } while (false);

                
                foreach (var item in result.bodyColumn)
                {
                    row++;
                    var col = 1;
                    worksheet.Cells[row, col++].Value = item.titleColmNo1; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo2; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo3; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo4; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo5; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo6; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo7; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo8; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo9; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo10; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo11; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo12; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo13; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo14; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo15; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo16; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo17; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo18; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo19; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo20; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo21; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo22; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo23; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo24; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo25; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo26; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo27; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo28; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo29; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo30; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo31; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo32; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo33; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo34; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo35; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo36; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo37; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo38; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo39; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo40; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo41; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo42; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo43; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo44; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo45; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo46; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo47; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo48; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo49; if (col > int.Parse(result.totalColm)) continue;
                    worksheet.Cells[row, col++].Value = item.titleColmNo50; 
                                
                }
                
                onBeforeSendResponse("exp16Assign", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }

        [HttpPost("rep03VisitPlanDaily")]
        public async Task<IActionResult> rep03VisitPlanDaily(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "03VisitPlanDaily.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp03VisitPlanDaily", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep03VisitPlanDailyResult> result = await reportImp.Search03VisitPlanDaily(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                worksheet.Cells[3, 1].Value = "Sale Rep Name : " + c.model.saleRepName;
                worksheet.Cells[4, 1].Value = "Sale Rep ID : " + c.model.saleRepId;
                var row = 6;
                var no = 1;
                decimal totalKmInput = 0;
                decimal totalKmSystem = 0;
                foreach (var item in result.records)
                {
                    var col = 1;
                    worksheet.Cells[row, col++].Value = no++;
                    worksheet.Cells[row, col++].Value = item.visitDate;
                    worksheet.Cells[row, col++].Value = item.startCheckinMileNo;
                    worksheet.Cells[row, col++].Value = item.stopCheckinMileNo;
                    worksheet.Cells[row, col++].Value = item.totalKmInput;
                    worksheet.Cells[row, col++].Value = item.totalKmSystem;
                    worksheet.Cells[row, col++].Value = item.locationName;
                    worksheet.Cells[row, col++].Value = item.status;

                    totalKmInput += Convert.ToDecimal(item.totalKmInputForCalc);
                    totalKmSystem += Convert.ToDecimal(item.totalKmSystemForCalc);
                    row++;
                }

                if(row > 4)
                {
                    var cRang = "A" + row + ":H" + row;
                    using (ExcelRange range = worksheet.Cells[cRang])
                    {
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6E0B4"));
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    var col = 1;
                    worksheet.Cells[row, col++].Value = "Summary";
                    worksheet.Cells[row, col++].Value = "";// item.visitDate;
                    worksheet.Cells[row, col++].Value = "";// item.startCheckinMileNo;
                    worksheet.Cells[row, col++].Value = "";// item.stopCheckinMileNo;
                    worksheet.Cells[row, col++].Value = totalKmInput;
                    worksheet.Cells[row, col++].Value = totalKmSystem;
                    worksheet.Cells[row, col++].Value = "";// item.locationName;
                    worksheet.Cells[row, col++].Value = "";// item.status;
                }
                onBeforeSendResponse("exp03VisitPlanDaily", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }

        [HttpPost("rep04VisitPlanMonthly")]
        public async Task<IActionResult> rep04VisitPlanMonthly(
                [FromHeader(Name = "Accept-Language")] string language,
                [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "04VisitPlanMonthly.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp04VisitPlanMonthly", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep04VisitPlanMonthlyResult> result = await reportImp.Search04VisitPlanMonthly(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                worksheet.Cells[3, 1].Value = "Sale Rep Name : " + c.model.saleRepName;
                worksheet.Cells[4, 1].Value = "Sale Rep ID : " + c.model.saleRepId;
                var row = 6;
                var totalKmInput = 0;
                foreach (var item in result.records)
                {
                    var col = 1;
                    worksheet.Cells[row, col++].Value = item.visitDate;
                    worksheet.Cells[row, col++].Value = item.visitTimeStart;
                    worksheet.Cells[row, col++].Value = item.startDheckinMileNo;
                    worksheet.Cells[row, col++].Value = item.locStartName;
                    worksheet.Cells[row, col++].Value = item.visitTimeStop;
                    worksheet.Cells[row, col++].Value = item.stopCheckinMileNo;
                    worksheet.Cells[row, col++].Value = item.locEndName;
                    worksheet.Cells[row, col++].Value = item.contactName;
                    worksheet.Cells[row, col++].Value = item.address;
                    worksheet.Cells[row, col++].Value = item.visitDetail;
                    worksheet.Cells[row, col++].Value = item.totalKmInput;

                    if(!String.IsNullOrEmpty(item.totalKmInput))
                        totalKmInput+= int.Parse(item.totalKmInput);

                    row++;
                }
                                
                if(row > 4)
                {
                    var cRang = "A" + row + ":K" + row;
                    using (ExcelRange range = worksheet.Cells[cRang])
                    {
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C6E0B4"));
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    var col = 1;
                    worksheet.Cells[row, col++].Value = "Summary";
                    worksheet.Cells[row, col++].Value = "";// item.visitTimeStart;
                    worksheet.Cells[row, col++].Value = "";// item.startDheckinMileNo;
                    worksheet.Cells[row, col++].Value = "";// item.locStartName;
                    worksheet.Cells[row, col++].Value = "";// item.visitTimeStop;
                    worksheet.Cells[row, col++].Value = "";// item.stopCheckinMileNo;
                    worksheet.Cells[row, col++].Value = "";// item.locEndName;
                    worksheet.Cells[row, col++].Value = "";// item.contactName;
                    worksheet.Cells[row, col++].Value = "";// item.address;
                    worksheet.Cells[row, col++].Value = "";// item.visitDetail;
                    worksheet.Cells[row, col++].Value = totalKmInput;
                }
                onBeforeSendResponse("exp04VisitPlanMonthly", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }

        // BCW


        [HttpPost("rep02VisitPlanTransaction")]
        public async Task<IActionResult> rep02VisitPlanTransaction(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "02VisitPlanTransaction.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp02VisitPlanTransaction", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep02VisitPlanTransactionResult> result = await reportImp.SearchRep02VisitPlanTransaction(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var no = 1;
                foreach (var item in result.records)
                {
                    worksheet.Cells[row, 1].Value = no++;
                    worksheet.Cells[row, 2].Value = item.SaleRepId;
                    worksheet.Cells[row, 3].Value = item.SaleRepName;
                    worksheet.Cells[row, 4].Value = c != null && c.model != null ? MyFirstAzureWebApp.Utility.Utility.ConvertTimeFrontToBackFormat(c.model.startDate) : "";
                    worksheet.Cells[row, 5].Value = c != null && c.model != null ? MyFirstAzureWebApp.Utility.Utility.ConvertTimeFrontToBackFormat(c.model.endDate) : "";
                    worksheet.Cells[row, 6].Value = item.TotalKmInput;
                    worksheet.Cells[row, 7].Value = item.TotalKmSystem;

                    row++;
                }
                onBeforeSendResponse("exp02VisitPlanTransaction", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }

        [HttpPost("rep06MeterRawData")]
        public async Task<IActionResult> rep06MeterRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "06MeterRawData.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp06MeterRawData", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep06MeterRawDataResult> result = await reportImp.SearchRep06MeterRawData(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var no = 1;
                foreach (var item in result.records)
                {
                    worksheet.Cells[row, 1].Value = item.CustNameTh;
                    worksheet.Cells[row, 2].Value = item.CustCode;
                    worksheet.Cells[row, 3].Value = item.PlanTripId;
                    worksheet.Cells[row, 4].Value = item.PlanTripName;
                    worksheet.Cells[row, 5].Value = item.CreateDtm;
                    worksheet.Cells[row, 6].Value = item.RecordDtm;
                    worksheet.Cells[row, 7].Value = item.PrevGasCode;
                    worksheet.Cells[row, 8].Value = item.PrevGasNameTh;
                    worksheet.Cells[row, 9].Value = item.GasCode;
                    worksheet.Cells[row, 10].Value = item.GasNameTh;
                    worksheet.Cells[row, 11].Value = item.DispenserNo;
                    worksheet.Cells[row, 12].Value = item.NozzleNo;
                    worksheet.Cells[row, 13].Value = item.RecRunNo;
                    worksheet.Cells[row, 14].Value = item.CntDispenserNo;
                    worksheet.Cells[row, 15].Value = item.CntNozzle;
                    worksheet.Cells[row, 16].Value = item.VisitLatitude;
                    worksheet.Cells[row, 17].Value = item.VisitLongitude;
                    worksheet.Cells[row, 18].Value = item.SaleRepId;
                    worksheet.Cells[row, 19].Value = item.SaleRepName;

                    row++;
                }
                onBeforeSendResponse("exp06MeterRawData", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }


        [HttpPost("rep13SaleOrderRawData")]
        public async Task<IActionResult> rep13SaleOrderRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "13SaleOrderRawData.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp13SaleOrderRawData", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep13SaleOrderRawDataResult> result = await reportImp.SearchRep13SaleOrderRawData(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var no = 1;
                foreach (var item in result.records)
                {
                    worksheet.Cells[row, 1].Value = item.SapOrderNo;
                    worksheet.Cells[row, 2].Value = item.SomOrderNo;
                    worksheet.Cells[row, 3].Value = item.OrderStatus;
                    worksheet.Cells[row, 4].Value = item.OrgNameTh;
                    worksheet.Cells[row, 5].Value = item.ChannelNameTh;
                    worksheet.Cells[row, 6].Value = item.DivisionNameTh;
                    worksheet.Cells[row, 7].Value = item.DescriptionTh;
                    worksheet.Cells[row, 8].Value = item.NetValue;
                    worksheet.Cells[row, 9].Value = item.CreateDate;
                    worksheet.Cells[row, 10].Value = item.EmpName;

                    row++;
                }
                onBeforeSendResponse("exp13SaleOrderRawData", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }




        [HttpPost("rep07MeterTransaction")]
        public async Task<IActionResult> rep07MeterTransaction(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "07MeterTransaction.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp07MeterTransaction", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep07MeterTransactionResult> result = await reportImp.SearchRep07MeterTransaction(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var no = 1;
                foreach (var item in result.records)
                {
                    worksheet.Cells[row, 1].Value = item.EmpId;
                    worksheet.Cells[row, 2].Value = item.SaleName;
                    worksheet.Cells[row, 3].Value = item.GroupCode;
                    worksheet.Cells[row, 4].Value = item.DescriptionTh;
                    worksheet.Cells[row, 5].Value = item.CustNameTh;
                    worksheet.Cells[row, 6].Value = item.CustCode;
                    worksheet.Cells[row, 7].Value = item.CntDispenserNno;
                    worksheet.Cells[row, 8].Value = item.CntNozzle;
                    worksheet.Cells[row, 9].Value = item.PrevRecDate;
                    worksheet.Cells[row, 10].Value = item.RecDate;
                    worksheet.Cells[row, 11].Value = item.PrevPlanTripId;
                    worksheet.Cells[row, 12].Value = item.PlanTripId;
                    worksheet.Cells[row, 13].Value = item.MeterVisit;
                    worksheet.Cells[row, 14].Value = item.MeterSummation;

                    row++;
                }
                onBeforeSendResponse("exp07MeterTransaction", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }


        [HttpPost("rep08StockCardRawData")]
        public async Task<IActionResult> rep08StockCardRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "08StockCardRawData.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp08StockCardRawData", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep08StockCardRawDataResult> result = await reportImp.SearchRep08StockCardRawData(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var no = 1;
                foreach (var item in result.records)
                {
                    worksheet.Cells[row, 1].Value = item.PlanTripName;
                    worksheet.Cells[row, 2].Value = item.PlanTripId;
                    worksheet.Cells[row, 3].Value = item.TerritoryId;
                    worksheet.Cells[row, 4].Value = item.TerritoryNameTh;
                    worksheet.Cells[row, 5].Value = item.VisitCheckinDtm;
                    worksheet.Cells[row, 6].Value = item.VisitCheckoutDtm;
                    worksheet.Cells[row, 7].Value = item.EmpName;
                    worksheet.Cells[row, 8].Value = item.EmpId;
                    worksheet.Cells[row, 9].Value = item.CustNameTh;
                    worksheet.Cells[row, 10].Value = item.CustCode;
                    worksheet.Cells[row, 11].Value = item.ProdCateDesc;
                    worksheet.Cells[row, 12].Value = item.ProdCateCode;
                    worksheet.Cells[row, 13].Value = item.ProdNameTh;
                    worksheet.Cells[row, 14].Value = item.ProdCode;
                    worksheet.Cells[row, 15].Value = item.RecQty;
                    worksheet.Cells[row, 16].Value = item.AltUnit;

                    row++;
                }
                onBeforeSendResponse("exp08StockCardRawData", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }


        [HttpPost("rep10ProspectRawData")]
        public async Task<IActionResult> rep10ProspectRawData(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "10ProspectRawData.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp10ProspectRawData", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep10ProspectRawDataResult> result = await reportImp.SearchRep10ProspectRawData(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var no = 1;
                foreach (var item in result.records)
                {
                    worksheet.Cells[row, 1].Value = item.ProspectId;
                    worksheet.Cells[row, 2].Value = item.AccName;
                    worksheet.Cells[row, 3].Value = item.ProspectType;
                    worksheet.Cells[row, 4].Value = item.Latitude;
                    worksheet.Cells[row, 5].Value = item.Longitude;
                    worksheet.Cells[row, 6].Value = item.SaleRepId;
                    worksheet.Cells[row, 7].Value = item.EmpName;
                    worksheet.Cells[row, 8].Value = item.GroupCode;
                    worksheet.Cells[row, 9].Value = item.DescriptionTh;
                    worksheet.Cells[row, 10].Value = item.CreateDate;
                    worksheet.Cells[row, 11].Value = item.Address;
                    worksheet.Cells[row, 12].Value = item.SubdistrictNameTh;
                    worksheet.Cells[row, 13].Value = item.DistrictNameTh;
                    worksheet.Cells[row, 14].Value = item.ProvinceNameTh;

                    row++;
                }
                onBeforeSendResponse("exp10ProspectRawData", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }
        // BCW

        [HttpPost("rep05VisitPlanActual")]
        public async Task<IActionResult> rep05VisitPlanActual(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "05VisitPlanActual.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp05VisitPlanActual", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep05VisitPlanActualResult> result = await reportImp.Search05VisitPlanActual(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                worksheet.Cells[3, 1].Value = "Sale Rep Name : " + c.model.saleRepName;
                worksheet.Cells[4, 1].Value = "Sale Rep ID : " + c.model.saleRepId;
                var row = 6;
                var col = 0;
                var planTotal = 0;
                var actualTotal = 0;
                foreach (var item in result.records)
                {

                    if ("Y".Equals(item.lastRecordFlag))
                    {
                        /*
                        if (item.orderNo.Equals("1"))
                        {
                            col = 1;
                            worksheet.Cells[row, col++].Value = item.visitDate;
                            worksheet.Cells[row, col++].Value = item.saleRepName;
                            worksheet.Cells[row, col++].Value = item.saleRepId;
                            worksheet.Cells[row, col++].Value = item.planAccName;
                            worksheet.Cells[row, col++].Value = item.actualAccName;

                            if (!item.planAccName.Equals(""))
                                planTotal++;
                            if (!item.actualAccName.Equals(""))
                                actualTotal++;
                            row++;
                        }
                        */

                        col = 1;
                        worksheet.Cells[row, col++].Value = item.visitDate;
                        worksheet.Cells[row, col++].Value = item.saleRepName;
                        worksheet.Cells[row, col++].Value = item.saleRepId;
                        worksheet.Cells[row, col++].Value = item.planAccName;
                        worksheet.Cells[row, col++].Value = item.actualAccName;

                        if (!item.planAccName.Equals(""))
                            planTotal++;
                        if (!item.actualAccName.Equals(""))
                            actualTotal++;
                        row++;

                        worksheet.Cells[row, 4].Value = planTotal;
                        worksheet.Cells[row, 5].Value = actualTotal;

                        row++;
                        planTotal = 0;
                        actualTotal = 0;
                        continue;
                    }

                    col = 1;
                    worksheet.Cells[row, col++].Value = item.visitDate;
                    worksheet.Cells[row, col++].Value = item.saleRepName;
                    worksheet.Cells[row, col++].Value = item.saleRepId;
                    worksheet.Cells[row, col++].Value = item.planAccName;
                    worksheet.Cells[row, col++].Value = item.actualAccName;

                    if (!item.planAccName.Equals(""))
                        planTotal++;
                    if (!item.actualAccName.Equals(""))
                        actualTotal++;
                    row++;
                }
                onBeforeSendResponse("exp05VisitPlanActual", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }

        [HttpPost("rep11ProspectPerformancePerSaleRep")]
        public async Task<IActionResult> rep11ProspectPerformancePerSaleRep(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "11ProspectPerformancePerSaleRep.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp11ProspectPerformancePerSaleRep", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep11ProspectPerformancePerSaleRepResult> result = await reportImp.Search11ProspectPerformancePerSaleRep(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var col = 0;
                var sumProspect = 0;
                var sumProspectChange = 0;
                foreach (var item in result.records)
                {

                    col = 1;
                    worksheet.Cells[row, col++].Value = item.saleRepId;
                    worksheet.Cells[row, col++].Value = item.saleRepName;
                    worksheet.Cells[row, col++].Value = item.saleGroupDesc;
                    worksheet.Cells[row, col++].Value = item.totalProspect;
                    worksheet.Cells[row, col++].Value = item.totalProspectChange;
                    worksheet.Cells[row, col++].Value = item.performPercent;

                    sumProspect += int.Parse(item.totalProspect);
                    sumProspectChange += int.Parse(item.totalProspectChange);
                    row++;
                }

                worksheet.Cells[row, 1].Value =  "Summary";
                worksheet.Cells[row, 4].Value = sumProspect;
                worksheet.Cells[row, 5].Value = sumProspectChange;
                worksheet.Cells[row, 6].Value = Math.Round(((new Decimal(sumProspectChange) / new Decimal(sumProspect)) * 100),2);

                onBeforeSendResponse("exp11ProspectPerformancePerSaleRep", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }
        }

        [HttpPost("rep12ProspectPerformancePerSaleGroup")]
        public async Task<IActionResult> rep12ProspectPerformancePerSaleGroup(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "12ProspectPerformancePerSaleGroup.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp12ProspectPerformancePerSaleGroup", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep12ProspectPerformancePerSaleGroupResult> result = await reportImp.Search12ProspectPerformancePerSaleGroup(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var col = 0;
                var sumProspect = 0;
                var sumProspectChange = 0;
                foreach (var item in result.records)
                {

                    col = 1;
                    worksheet.Cells[row, col++].Value = item.saleGroupCode;
                    worksheet.Cells[row, col++].Value = item.saleGroupDesc;
                    worksheet.Cells[row, col++].Value = item.totalProspect;
                    worksheet.Cells[row, col++].Value = item.totalProspectChange;
                    worksheet.Cells[row, col++].Value = item.performPercent;

                    sumProspect += int.Parse(item.totalProspect);
                    sumProspectChange += int.Parse(item.totalProspectChange);
                    row++;
                }

                worksheet.Cells[row, 1].Value = "Summary";
                worksheet.Cells[row, 3].Value = sumProspect;
                worksheet.Cells[row, 4].Value = sumProspectChange;
                worksheet.Cells[row, 5].Value = Math.Round(((new Decimal(sumProspectChange) / new Decimal(sumProspect)) * 100), 2);

                onBeforeSendResponse("exp12ProspectPerformancePerSaleGroup", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }
        }

        [HttpPost("rep14SaleOrderByChannel")]
        public async Task<IActionResult> rep14SaleOrderByChannel(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "14SaleOrderByChannel.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp14SaleOrderByChannel", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
            SearchResultBase<Rep14SaleOrderByChannelResult> result = await reportImp.search14SaleOrderByChannel(c, ic);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var col = 0;
                var sumOrder = 0;
                var sumSapOrder = 0;
                var sumSomOrder = 0;
                foreach (var item in result.records)
                {
                    var totalOrder = int.Parse(item.totalOrder);
                    var totalSapOrder = int.Parse(item.totalSapOrder);
                    var totalSomOrder = int.Parse(item.totalSomOrder);
                    col = 1;
                    worksheet.Cells[row, col++].Value = item.channelDesc;
                    worksheet.Cells[row, col++].Value = item.saleGroupDesc;
                    worksheet.Cells[row, col++].Value = item.totalOrder;
                    worksheet.Cells[row, col++].Value = item.totalSapOrder;
                    worksheet.Cells[row, col++].Value = item.totalSomOrder;
                    worksheet.Cells[row, col++].Value = Math.Round(((new Decimal(totalSapOrder) / new Decimal(totalOrder)) * 100), 2);
                    worksheet.Cells[row, col++].Value = Math.Round(((new Decimal(totalSomOrder) / new Decimal(totalOrder)) * 100), 2);
                    sumOrder += totalOrder;
                    sumSapOrder += totalSapOrder;
                    sumSomOrder += totalSomOrder;
                    row++;
                }


                ExcelCellAddress start = worksheet.Cells[row, 1].Start;
                ExcelCellAddress end = worksheet.Cells[row, 2].End;
                using (ExcelRange range = worksheet.Cells[start.Row, start.Column, end.Row, end.Column])
                {
                    range.Merge = true;
                    range.Value = "Summary";
                    //range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#A9D08E"));
                    //range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                col = 3;
                worksheet.Cells[row, col++].Value = sumOrder;
                worksheet.Cells[row, col++].Value = sumSapOrder;
                worksheet.Cells[row, col++].Value = sumSomOrder;
                worksheet.Cells[row, col++].Value = Math.Round(((new Decimal(sumSapOrder) / new Decimal(sumOrder)) * 100), 2);
                worksheet.Cells[row, col++].Value = Math.Round(((new Decimal(sumSomOrder) / new Decimal(sumOrder)) * 100), 2);

                onBeforeSendResponse("exp14SaleOrderByChannel", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }
        }

        [HttpPost("rep15SaleOrderByCompany")]
        public async Task<IActionResult> rep15SaleOrderByCompany(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "15SaleOrderByCompany.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp15SaleOrderByCompany", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            InterfaceSapConfig ic = await getInterfaceSapConfigAsync();
            SearchResultBase<Rep15SaleOrderByCompanyResult> result = await reportImp.search15SaleOrderByCompany(c, ic);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var col = 0;

                if(result.totalRecords > 0)
                {
                    foreach (var company in result.records[0].Company)
                    {
                        var rowHead = row;

                        var totalOrder = 0;
                        var totalSapOrder = 0;
                        var totalSomOrder = 0;

                        foreach (var channel in company.Distribution)
                        {
                            row++;

                            totalOrder += int.Parse(channel.Sale_Order_Report[0].Total_Sale_Order);
                            totalSapOrder += int.Parse(channel.Sale_Order_Report[0].SO_SAP);
                            totalSomOrder += int.Parse(channel.Sale_Order_Report[0].SO_SOM);

                            col = 1;
                            worksheet.Cells[row, col++].Value = "   - " + channel.Distribution_Channel_Name;
                            worksheet.Cells[row, col++].Value = channel.Sale_Order_Report[0].Total_Sale_Order;
                            worksheet.Cells[row, col++].Value = channel.Sale_Order_Report[0].SO_SAP;
                            worksheet.Cells[row, col++].Value = channel.Sale_Order_Report[0].SO_SOM;
                            worksheet.Cells[row, col++].Value = channel.Sale_Order_Report[0].SO_div_SAP;
                            worksheet.Cells[row, col++].Value = channel.Sale_Order_Report[0].SO_div_SOM;
                        }

                        
                        col = 1;
                        worksheet.Cells[rowHead, col++].Value = company.Company_Name_EN;
                        worksheet.Cells[rowHead, col++].Value = totalOrder;
                        worksheet.Cells[rowHead, col++].Value = totalSapOrder;
                        worksheet.Cells[rowHead, col++].Value = totalSomOrder;
                        worksheet.Cells[rowHead, col++].Value = Math.Round(((new Decimal(totalSapOrder) / new Decimal(totalOrder)) * 100), 2);
                        worksheet.Cells[rowHead, col++].Value = Math.Round(((new Decimal(totalSomOrder) / new Decimal(totalOrder)) * 100), 2);
                        
                        row++;
                    }
                }
               
                onBeforeSendResponse("exp15SaleOrderByCompany", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }
        }

        [HttpPost("rep09StockCardCustomerSummary")]
        public async Task<IActionResult> rep09StockCardCustomerSummary(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "09StockCardCustomerSummary.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp09StockCardCustomerSummary", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep09Rult> results = await reportImp.search09StockCardCustomerSummary(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");
            Rep09Rult result = results.records[0];
            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Column(1).Width = 60;
                for(int i = 2; i < 52; i++)
                {
                    worksheet.Column(i).Width = 30;
                }
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);

                var row = 3;
                ExcelCellAddress start = worksheet.Cells[row, 1].Start;
                ExcelCellAddress end = worksheet.Cells[row, result.mapColmn.Count + 1].End;
                using (ExcelRange range = worksheet.Cells[start.Row, start.Column, end.Row, end.Column])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#A5A5A5"));
                }

                if (result.mapColmn.Count > 0)
                {
                    foreach (var m in result.mapColmn)
                    {
                        worksheet.Cells[row, int.Parse(m.Value[1])].Value = m.Value[0];

                        if (m.Value[0].IndexOf("Report Unit") == 0)
                        {
                            ExcelCellAddress cellsReportUnit = worksheet.Cells[row, int.Parse(m.Value[1])].Start;
                            using (ExcelRange range = worksheet.Cells[cellsReportUnit.Row, cellsReportUnit.Column, cellsReportUnit.Row, cellsReportUnit.Column])
                            {
                                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F4B084"));
                            }
                        }
                        
                    }
                }


                Dictionary<int, Dictionary<string, List<int>>> sumDic = new Dictionary<int, Dictionary<string, List<int>>>();
                foreach (var m in result.mapColmn)
                {
                    Dictionary<string, List<int>> sumByColmNo = new Dictionary<string, List<int>>();
                    sumByColmNo.Add("A", new List<int>() { 0 });
                    sumByColmNo.Add("G", new List<int>() { 0 });
                    sumByColmNo.Add("S", new List<int>() { 0 });
                    sumByColmNo.Add("C", new List<int>() { 0 });
                    sumDic.Add(int.Parse(m.Value[1]), sumByColmNo);
                }

                var gRow = 0;
                var sRow = 0;
                var cRow = 0;
                foreach (var saleGroup in result.listSaleGroup)
                {
                    row++;
                    gRow = row;
                    worksheet.Cells[row, 1].Value = saleGroup.groupDesc;
                    foreach (var saleRep in saleGroup.listSaleRep)
                    {
                        row++;
                        sRow = row;
                        worksheet.Cells[row, 1].Value = "   - " + saleRep.empName;
                        foreach (var cust in saleRep.listCust)
                        {
                            row++;
                            cRow = row;
                            worksheet.Cells[row, 1].Value = "       - " + cust.custNameTh;
                            foreach (var mon in cust.listMonth)
                            {
                                row++;
                                worksheet.Cells[row, 1].Value = "           - " + mon.mon;

                                start = worksheet.Cells[row, 2].Start;
                                end = worksheet.Cells[row, result.mapColmn.Count + 1].End;
                                using (ExcelRange range = worksheet.Cells[start.Row, start.Column, end.Row, end.Column])
                                {
                                    range.Value = 0;
                                }

                                foreach (var qty in mon.listQty)
                                {
                                    worksheet.Cells[row, int.Parse(qty.colmNo)].Value = int.Parse(qty.recQty);

                                    if (sumDic.TryGetValue(int.Parse(qty.colmNo), out Dictionary<string, List<int>> sumGroup))
                                    {
                                        if (sumGroup.TryGetValue("C", out List<int> cItems))
                                            cItems.Add(int.Parse(qty.recQty));
                                    }
                                }
                            }

                            //Break Customer
                            foreach (var m in result.mapColmn)
                            {
                                if (sumDic.TryGetValue(int.Parse(m.Value[1]), out Dictionary<string, List<int>> sumGroup))
                                {
                                    if (sumGroup.TryGetValue("C", out List<int> items))
                                    {
                                        worksheet.Cells[cRow, int.Parse(m.Value[1])].Value = items.Sum();

                                        if (sumGroup.TryGetValue("S", out List<int> sItems))
                                            sItems.Add(items.Sum());

                                        items.Clear();
                                    }

                                }
                            }

                            /*

                            foreach (var x in sumDic)
                            {
                                Console.WriteLine("Column No : " + x.Key);
                                foreach (var y in x.Value)
                                {
                                    Console.WriteLine("Key : " + y.Key);
                                    foreach (var z in y.Value)
                                    {
                                        Console.WriteLine("-vallue : " + z);

                                    }
                                }
                            }
                            */
                        }

                        //Break Sale Rep
                        foreach (var m in result.mapColmn)
                        {
                            if (sumDic.TryGetValue(int.Parse(m.Value[1]), out Dictionary<string, List<int>> sumGroup))
                            {
                                if (sumGroup.TryGetValue("S", out List<int> items))
                                {
                                    worksheet.Cells[sRow, int.Parse(m.Value[1])].Value = items.Sum();

                                    if (sumGroup.TryGetValue("G", out List<int> gItems))
                                        gItems.Add(items.Sum());

                                    items.Clear();
                                }

                            }

                        }


                    }

                    //Break Sale Group
                    foreach (var m in result.mapColmn)
                    {
                        if (sumDic.TryGetValue(int.Parse(m.Value[1]), out Dictionary<string, List<int>> sumGroup))
                        {
                            if (sumGroup.TryGetValue("G", out List<int> items))
                            {
                                worksheet.Cells[gRow, int.Parse(m.Value[1])].Value = items.Sum();

                                if (sumGroup.TryGetValue("A", out List<int> aItems))
                                    aItems.Add(items.Sum());

                                items.Clear();
                            }

                        }

                    }

                }

                if (result.mapColmn.Count > 0)
                {
                    row++;
                    start = worksheet.Cells[row, 1].Start;
                    end = worksheet.Cells[row, result.mapColmn.Count + 1].End;
                    using (ExcelRange range = worksheet.Cells[start.Row, start.Column, end.Row, end.Column])
                    {
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#A5A5A5"));
                        worksheet.Cells[row, 1].Value = "Grand Total";
                        foreach (var m in result.mapColmn)
                        {
                            if (sumDic.TryGetValue(int.Parse(m.Value[1]), out Dictionary<string, List<int>> sumGroup))
                            {
                                if (sumGroup.TryGetValue("A", out List<int> items))
                                {
                                    worksheet.Cells[row, int.Parse(m.Value[1])].Value = items.Sum();
                                }

                            }

                        }
                    }

                }


                onBeforeSendResponse("exp09StockCardCustomerSummary", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }

        [HttpPost("rep17ImageInTemplate")]
        public async Task<IActionResult> rep17ImageInTemplate(
            [FromHeader(Name = "Accept-Language")] string language,
            [FromHeader(Name = "User-Agent")] string agent, SearchCriteriaBase<ReportCriteria> c)
        {
            string repExcelFileNm = "17ImageInTemplate.xlsx";
            string nameMapReqRes = getNameMaping();
            onAfterReceiveRequest("exp17ImageInTemplate", nameMapReqRes, c);
            string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
            SearchResultBase<Rep17ImageInTemplateResult> result = await reportImp.SearchRep17ImageInTemplate(c);
            string timeZone = await getConfigAsync("HOUR_TIME_ZONE");

            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/TMP" + repExcelFileNm);
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["rep"];
                worksheet.Cells[1, 1].Value = "โดย : " + userProfileForBack.getUserData().FirstName;
                worksheet.Cells[2, 1].Value = "วันที่สร้าง : " + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("dd/MM/yyyy HH:mm", timeZone);
                var row = 4;
                var no = 1;
                var host = "https://" + HttpContext.Request.Headers["Host"];
                foreach (var item in result.records)
                {
                    worksheet.Cells[row, 1].Value = no++;
                    worksheet.Cells[row, 2].Value = item.SaleRepId;
                    worksheet.Cells[row, 3].Value = item.SaleRepName;
                    worksheet.Cells[row, 4].Value = item.VisitPlanId;
                    worksheet.Cells[row, 5].Value = item.VisitPlanName;
                    worksheet.Cells[row, 6].Value = item.VisitDate;
                    worksheet.Cells[row, 7].Value = item.TemplateName;

                    var hyperlink = "HYPERLINK(\"" + host + item.ImageUrl + "\",\"" + "Download" + "\")";
                    worksheet.Cells[row, 8].Formula = hyperlink;
                    worksheet.Cells[row, 8].Style.Font.Bold = true;
                    worksheet.Cells[row, 8].Style.Font.Color.SetColor(Color.Blue);
                    worksheet.Cells[row, 8].Style.Font.UnderLine = true;

                    row++;

                    
                }
                onBeforeSendResponse("exp17ImageInTemplate", nameMapReqRes, result);


                FileInfo fileTmp = new(_hostingEnvironment.ContentRootPath + "/excel_template/" + MyFirstAzureWebApp.Utility.Utility.getCurrentDtm("yyyyMMddHHmmss", timeZone) + repExcelFileNm);
                xlPackage.SaveAs(fileTmp);
                FileContentResult fileReturn = File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", repExcelFileNm);
                System.IO.File.Delete(fileTmp.FullName);
                return fileReturn;
            }

        }
    }
}
