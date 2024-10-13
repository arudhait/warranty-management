using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.CommonEntities
{
    public class DatatablePageRequestModel
    {
        public int StartIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string SearchText { get; set; } = "";
        public string SortColumnName { get; set; } = "";
        public string SortDirection { get; set; } = "";
        public object Draw { get; set; } = "";
        public string DateRange { get; set; }
        public string ExtraSearch { get; set; } = "";
        public string LeadsStatus { get; set; }
        public int RoleId { get; set; }
        public int UserTypeId { get; set; }
        public int EnggId { get; set; }
        public int StatusId { get; set; }
        public string StartDateFilter { get; set; }
        public string EndDateFilter { get; set; }
        public string Is_Adjudicated { get; set; }
        public int LeadId { get; set; }
        public int UserMasterId { get; set; }
        public int ProductId { get; set; }
        public int LeadStatusId { get; set; }
        public int ClientId { get; set; }
        public int ContainerDetailsId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

    }
}
