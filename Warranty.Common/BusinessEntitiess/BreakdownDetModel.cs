using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class BreakdownDetModel
    {
        public long BreakdownId { get; set; }

        public long CustId { get; set; }
        public string CustName { get; set; }

        [Required(ErrorMessage = "Please select a Doctor Name")]
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Please select a Doctor Name")]
        public DateTime CallRegDate { get; set; }
        public string CallRegDateString { get; set; }

        [Required(ErrorMessage = "Please select a Doctor Name")]
        public short TypeId { get; set; }
        public string BreakdownType { get; set; }

        [Required(ErrorMessage = "Please select an Engineer Name")]
        public int EnggId { get; set; }

        public string EngineerName { get; set; }

        [Required(ErrorMessage = "Please select an Engg First Visit Date")]
        public DateTime EnggFirstVisitDate { get; set; }
        public string EnggFirstVisitDateString { get; set; }

        [Required(ErrorMessage = "Please enter CrmNo")]
        public string CrmNo { get; set; } = null!;

        [Required(ErrorMessage = "Please select a Problems")]
        public string Problems { get; set; } = null!;

        [Required(ErrorMessage = "Please select a Req Action")]
        public short ReqAction { get; set; }
        public string ReqActionName { get; set; }

        [Required(ErrorMessage = "Please select an Action Taken")]
        public short ActionTaken { get; set; }
        public string ActionTakenName { get; set; }

        [Required(ErrorMessage = "Please select a Conclusion")]
        public short Conclusion { get; set; }

        [Required(ErrorMessage = "Please select a Status")]
        public bool? IsActive { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedDateString { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string EncId { get; set; }
    }
}
