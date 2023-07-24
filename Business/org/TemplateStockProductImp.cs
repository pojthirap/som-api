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
using MyFirstAzureWebApp.Models.ms;

namespace MyFirstAzureWebApp.Business.org
{

    public class TemplateStockProductImp : ITemplateStockProduct
    {
        private Logger log = LogManager.GetCurrentClassLogger();




        public async Task<EntitySearchResultBase<SearchTemplateByStockCardCustom>> searchTemplateByStockCardId(SearchCriteriaBase<TemplateStockProductCriteria> searchCriteria)
        {

            using (var context = new MyAppContext())
            {
                
                TemplateStockProductCriteria criteria = searchCriteria.model;
                decimal cri_TpStockCardId = Convert.ToDecimal(criteria.TpStockCardId);
                var queryCommane = (from sp in context.TemplateStockProduct
                                    join p in context.MsProduct
                                    on sp.ProdCode equals p.ProdCode
                                    join d in context.OrgDivision
                                    on p.DivisionCode equals d.DivisionCode

                                    where ((criteria.TpStockCardId == null ? 1 == 1 : sp.TpStockCardId == cri_TpStockCardId))
                                    orderby (searchCriteria.searchOrder == 1 ? sp.UpdateDtm : sp.UpdateDtm) descending
                                    select new
                                    {
                                        TpStockProdId = sp.TpStockProdId,
                                        TpStockCardId = sp.TpStockCardId,
                                        ProdCode = sp.ProdCode,
                                        ActiveFlag = sp.ActiveFlag,
                                        CreateUser = sp.CreateUser,
                                        CreateDtm = sp.CreateDtm,
                                        UpdateUser = sp.UpdateUser,
                                        UpdateDtm = sp.UpdateDtm,
                                        DivisionCode = p.DivisionCode,
                                        ProdNameTh = p.ProdNameTh,
                                        ProdNameEn = p.ProdNameEn,
                                        ProdType = p.ProdType,
                                        ProdGroup = p.ProdGroup,
                                        IndustrySector = p.IndustrySector,
                                        OldProdNo = p.OldProdNo,
                                        BaseUnit = p.BaseUnit,
                                        DivisionNameTh = d.DivisionNameTh,
                                        DivisionNameEn = d.DivisionNameEn

                                    });
                var queryString = queryCommane.ToQueryString();
                log.Debug("Query:" + queryString);
                Console.WriteLine("Query:" + queryString);
                var query = queryCommane.ToList();
                EntitySearchResultBase<SearchTemplateByStockCardCustom> searchResult = new EntitySearchResultBase<SearchTemplateByStockCardCustom>();
                searchResult.totalRecords = query.Count();



                List<SearchTemplateByStockCardCustom> saleLst = new List<SearchTemplateByStockCardCustom>();
                foreach (var item in (searchCriteria.length != 0 ? queryCommane.ToList().Skip(searchCriteria.startRecord - QueryUtils.startRecordDecrease).Take(searchCriteria.length) : query.ToList()))
                {
                    SearchTemplateByStockCardCustom s = new SearchTemplateByStockCardCustom();
                    s.TpStockProdId = item.TpStockProdId;
                    s.TpStockCardId = item.TpStockCardId;
                    s.ProdCode = item.ProdCode;
                    s.ActiveFlag = item.ActiveFlag;
                    s.CreateUser = item.CreateUser;
                    s.CreateDtm = item.CreateDtm;
                    s.UpdateUser = item.UpdateUser;
                    s.UpdateDtm = item.UpdateDtm;
                    s.DivisionCode = item.DivisionCode;
                    s.ProdNameTh = item.ProdNameTh;
                    s.ProdNameEn = item.ProdNameEn;
                    s.ProdType = item.ProdType;
                    s.ProdGroup = item.ProdGroup;
                    s.IndustrySector = item.IndustrySector;
                    s.OldProdNo = item.OldProdNo;
                    s.BaseUnit = item.BaseUnit;
                    s.DivisionNameTh = item.DivisionNameTh;
                    s.DivisionNameEn = item.DivisionNameEn;
                    saleLst.Add(s);

                }

                searchResult.data = saleLst;
                return searchResult;
            }

        }



        public async Task<List<TemplateStockProduct>> addTemplateStockProduct(TemplateStockProductModel templateStockProductModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat(" UPDATE TEMPLATE_STOCK_CARD SET[UPDATE_USER] = @UPDATE_USER, [UPDATE_DTM] = dbo.GET_SYSDATETIME() WHERE TP_STOCK_CARD_ID = @TP_STOCK_CARD_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateStockProductModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_CARD_ID", templateStockProductModel.TpStockCardId));// Add New
                        log.Debug("Query:" + queryBuilder.ToString());
                        Console.WriteLine("Query:" + queryBuilder.ToString());
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                        log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                        Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);

                        List<TemplateStockProduct> list = new List<TemplateStockProduct>();
                        for (int i = 0; i < templateStockProductModel.ProdCode.Length; i++)
                        {
                            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                            p.Direction = System.Data.ParameterDirection.Output;
                            context.Database.ExecuteSqlRaw("set @result = next value for TEMPLATE_STOCK_PRODUCT_SEQ", p);
                            var nextVal = (int)p.Value;

                            sqlParameters = new List<SqlParameter>();// Add New
                            queryBuilder = new StringBuilder();
                            queryBuilder.AppendFormat("INSERT INTO  TEMPLATE_STOCK_PRODUCT (TP_STOCK_PROD_ID, TP_STOCK_CARD_ID, PROD_CODE, ACTIVE_FLAG, CREATE_USER, CREATE_DTM, UPDATE_USER, UPDATE_DTM ) ");
                            queryBuilder.AppendFormat("VALUES(@TP_STOCK_PROD_ID ,@TP_STOCK_CARD_ID, @PROD_CODE, 'Y', @USER, dbo.GET_SYSDATETIME(), @USER, dbo.GET_SYSDATETIME())");
                            sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_PROD_ID", nextVal));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_CARD_ID", templateStockProductModel.TpStockCardId));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("PROD_CODE", templateStockProductModel.ProdCode[i]));// Add New
                            sqlParameters.Add(QueryUtils.addSqlParameter("USER", templateStockProductModel.getUserName()));// Add New
                            log.Debug("Query:" + queryBuilder.ToString());
                            Console.WriteLine("Query:" + queryBuilder.ToString());
                            numberOfRowInserted = context.Database.ExecuteSqlRaw(queryBuilder.ToString(), sqlParameters.ToArray());
                            log.Debug("NumberOfRowEffective:" + numberOfRowInserted);
                            Console.WriteLine("NumberOfRowEffective:" + numberOfRowInserted);
                            TemplateStockProduct re = new TemplateStockProduct();
                            re.TpStockProdId = nextVal;
                            list.Add(re);
                        }
                        transaction.Commit();
                        return list;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }
        
        public async Task<int> DeleteUpdate(TemplateStockProductModel templateStockProductModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("UPDATE TEMPLATE_STOCK_PRODUCT SET ACTIVE_FLAG = 'N', UPDATE_USER=@UPDATE_USER, UPDATE_DTM=dbo.GET_SYSDATETIME()  WHERE TP_STOCK_PROD_ID=@TP_STOCK_PROD_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("UPDATE_USER", templateStockProductModel.getUserName()));// Add New
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_PROD_ID", templateStockProductModel.TpStockProdId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
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



        public async Task<int> delTemplateStockProduct(TemplateStockProductModel templateStockProductModel)
        {

            using (var context = new MyAppContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sqlParameters = new List<SqlParameter>();// Add New
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.AppendFormat("DELETE FROM  TEMPLATE_STOCK_PRODUCT   WHERE TP_STOCK_PROD_ID=@TP_STOCK_PROD_ID ");
                        sqlParameters.Add(QueryUtils.addSqlParameter("TP_STOCK_PROD_ID", templateStockProductModel.TpStockProdId));// Add New
                        string queryStr = queryBuilder.ToString();
                        log.Debug("Query:" + queryStr);
                        Console.WriteLine("Query:" + queryStr);
                        int numberOfRowInserted = context.Database.ExecuteSqlRaw(queryStr, sqlParameters.ToArray());
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







    }
}
