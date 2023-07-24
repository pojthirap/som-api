using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.org;
using MyFirstAzureWebApp.Models.importfile;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.profile;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IImportExcelFile
    {

        Task<ImportProspectCustom> importProspect(IFileManagerLogic _fileManagerLogic, IImportErrorFileLog importErrorFileLog, ImportProspectModel importProspectModel, UserProfileForBack userProfile);
        Task<ImportEmpToSaleGroupCustom> importEmpToSaleGroup(IFileManagerLogic _fileManagerLogic, IImportErrorFileLog importErrorFileLog, ImportEmpToSaleGroupModel importEmpToSaleGroupModel, UserProfileForBack userProfile);
    }
}
