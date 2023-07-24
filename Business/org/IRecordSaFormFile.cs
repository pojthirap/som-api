using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.ms;
using MyFirstAzureWebApp.ModelCriteria;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.record;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IRecordSaFormFile
    {
        Task<int> GetRecordSaFromFileSeq();
        Task<RecordSaFormFile> uploadFileSaForm(RecordSaFormFileModel recordSaFormFileModel, UserProfileForBack userProfileForBack);
    }
}