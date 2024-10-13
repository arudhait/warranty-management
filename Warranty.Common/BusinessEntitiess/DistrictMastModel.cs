using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class DistrictMastModel
    {
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "Please enter District Name")]
        public string DistrictName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter State Name")]
        public short StateId { get; set; }

        [Required(ErrorMessage = "Please enter State Name")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "Please select a status")]
        public bool? IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedDateName { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string Ip { get; set; } = null!;

        public string EncId { get; set; }
    }
}
