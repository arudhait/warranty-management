using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class ModelMasterModel
    {
        public int ModelId { get; set; }

        [Required(ErrorMessage = "Please enter Model No")]
        public string ModelNo { get; set; } = null!;
        public string EncId { get; set; }

        [Required(ErrorMessage = "Please select a status")]
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Ip { get; set; }
        public string CreatedOnString { get; set; }
        public string CreatedByName { get; set; }
    }
}
