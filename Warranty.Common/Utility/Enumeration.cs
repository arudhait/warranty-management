using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.Utility
{
    public class Enumeration
    {
        public enum Role
        {
            SuperAdmin = 1,
            SubAdmin = 2,
            ServiceEngineer = 3          
        }

        public enum AppPages
        {
            Dashboard = 1,
            Engineer = 2,
            Allocation=3,
            BreakDownList= 4,
            BreakDownStatusMast =5,
            ContractList=6,
            ContractTypeMaster=7,
            Customer = 8,
            DistrictMaster =9,
            Due_ExpriredAMC_CMCContract=10,
            Due_ExpiredWarranty =11,
            Ledger = 12,
            InwardOutward=13,
            ProductMaster = 14,
            StateMaster = 15,
            SupplierMaster =16,
            UserMaster = 17,
            WarrantyList =18,

        }
        public enum Category
        {
            Old_Machine = 1,
            New_Machine = 2,
        }
        public enum Gender
        {
            Male = 1,
            Female = 2,
            Other = 3,
        }
        
        public enum Payment
        {
            Online_Payment = 1,
            Cash = 2,
            Card = 3,
        }

        public enum NoOfService
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Eleven = 11,
            Twelve = 12
        }

        public enum Warranty
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Eleven = 11,
            Twelve = 12
        }

        public enum ServiceType
        {
            Free = 1,
            Paid = 2,

        }
        public enum FileType
        {
            File = 1, // Assuming this is for audio files
            Image = 2,
        }
        public enum MachineAge
        {
            Old = 1, // Assuming this is for audio files
            New = 2,
        }

        public enum Conclusion
        {
            Continue = 1,
            Close = 2
        }
    }
}
