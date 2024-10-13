﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IBreakDownListProvider
    {
        DatatablePageResponseModel<BreakdownDetModel> GetBreakdownListDetailList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate);

        BreakdownDetModel GetById(int id);

        ResponseModel Save(BreakdownDetModel inputModel, SessionProviderModel sessionProvider);

        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
