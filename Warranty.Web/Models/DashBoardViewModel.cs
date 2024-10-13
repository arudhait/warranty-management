using Warranty.Common.BusinessEntitiess;

namespace Warranty.Web.Models
{
    public class DashBoardViewModel : BaseApplicationViewModel
    {
        public DashBoardModel DashboardModel { get; set; }
        public CustMastModel CustMastModel { get; set; }
        public WarrantyDetailsModel WarrantyDetailsModel { get; set; }
        public EnggMastModel EnggMastModel { get; set; }
    
        public int RoleId { get; set; }
        public int TotalCount { get; set; }
        public int TotalDueCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalExpiredCount { get; set; }

        public int ContractTotalDueCount { get; set; }

        public int ContractTotalExpiredCount { get; set; }
        public int TotalEnggCount { get; set; }
        public int TotalCustCount { get; internal set; }
    }
}
