using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BussinessEntities;

namespace Warranty.Common.BusinessEntitiess
{
    public class DashBoardModel
    {
        public UserHeaderDetailModel UserHeaderDetail { get; set; }
        public TerritoryAllocationModel TerritoryAllocationModel { get; set; }
        public StateMastModel StateMastModel { get; set; }
        public DistrictMastModel DistrictMastModel { get; set; }
        public int DueCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExpiredCount { get; set; }

       

        public string ContractFirstName { get; set; }
        public string ContractLastName { get; set; }
       
        public int ExpiredAMCCMCCount { get; set; }
        public int DueAMCCMCCount { get; set; }
        public int ContractExpiredCount { get; set; }
        public int TotalEnggCount { get; set; }
        public int TotalCustCount { get; set; }
    }
}
