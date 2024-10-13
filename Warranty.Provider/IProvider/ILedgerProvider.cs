﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface ILedgerProvider
    {
        DatatablePageResponseModel<LedgerModel> GetList(DatatablePageRequestModel datatablePageRequest);
    }
}
