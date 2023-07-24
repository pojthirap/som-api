using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.ModelsReport;
using MyFirstAzureWebApp.ModelCriteriaReport;
using MyFirstAzureWebApp.Controllers;

namespace MyFirstAzureWebApp.Business.rep
{
    public interface IReport
    {
        Task<SearchResultBase<Rep01VisitPlanRawDataResult>> SearchRep01VisitPlanRawData(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep16AssignResult>> SearchRep16Assign(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep03VisitPlanDailyResult>> Search03VisitPlanDaily(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep04VisitPlanMonthlyResult>> Search04VisitPlanMonthly(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep02VisitPlanTransactionResult>> SearchRep02VisitPlanTransaction(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep06MeterRawDataResult>> SearchRep06MeterRawData(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep13SaleOrderRawDataResult>> SearchRep13SaleOrderRawData(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep07MeterTransactionResult>> SearchRep07MeterTransaction(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep08StockCardRawDataResult>> SearchRep08StockCardRawData(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep10ProspectRawDataResult>> SearchRep10ProspectRawData(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep05VisitPlanActualResult>> Search05VisitPlanActual(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep11ProspectPerformancePerSaleRepResult>> Search11ProspectPerformancePerSaleRep(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep12ProspectPerformancePerSaleGroupResult>> Search12ProspectPerformancePerSaleGroup(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep14SaleOrderByChannelResult>> search14SaleOrderByChannel(SearchCriteriaBase<ReportCriteria> searchCriteria, InterfaceSapConfig ic);
        Task<SearchResultBase<Rep15SaleOrderByCompanyResult>> search15SaleOrderByCompany(SearchCriteriaBase<ReportCriteria> searchCriteria, InterfaceSapConfig ic);
        Task<SearchResultBase<Rep09Rult>> search09StockCardCustomerSummary(SearchCriteriaBase<ReportCriteria> searchCriteria);
        Task<SearchResultBase<Rep17ImageInTemplateResult>> SearchRep17ImageInTemplate(SearchCriteriaBase<ReportCriteria> searchCriteria);
    }
}
