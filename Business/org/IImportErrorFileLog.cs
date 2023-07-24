﻿using Entity;
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
using MyFirstAzureWebApp.Models.importfile;

namespace MyFirstAzureWebApp.Business.org
{
    public interface IImportErrorFileLog
    {
        Task<int> GetImportErrorFileLogSeq();
        Task<int> uploadErrorFileLog(ImportErrorLogFileModel importErrorLogFileModel, UserProfileForBack userProfileForBack);
    }
}