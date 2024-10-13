using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class ProbDetailModel
    {
        public long ProbId { get; set; }

        public long WarrantyId { get; set; }

        [Required(ErrorMessage = "Please enter Prob Name")]
        public string ProbName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Prob Serial No.")]
        public string? ProbSerialNo { get; set; }
        public string EncId { get; set; }
    }
}
