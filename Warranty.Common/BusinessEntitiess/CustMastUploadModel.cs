using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class CustMastUploadModel
    {      
        public string doctorname { get; set; } = null!;

        public string hospitalname { get; set; } = null!;

        public string postaladdress { get; set; } = null!;

        public string designation { get; set; }

        public string mobileno { get; set; } = null!;

        public string phoneno { get; set; }

        public string email { get; set; }

        public string pincode { get; set; } = null!;

        public short stateid { get; set; }
        public string state { get; set; }

        public int districtid { get; set; }

        public string city { get; set; }

        public string pndtcertino { get; set; } = null!;

        public DateTime regdate { get; set; }

        public bool isactive { get; set; }
        public bool IsInvalid { get; set; }
        public bool IsDuplicate { get; set; }
        public string ErrorMessage { get; set; }

    }
}
