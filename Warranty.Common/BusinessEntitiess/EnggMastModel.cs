using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class EnggMastModel
    {

        public UserMastModel UserMastModel { get; set; }
        public int EnggId { get; set; }

        [Required(ErrorMessage = "Please enter Engineer Name")]
        public string EnggName { get; set; } = null!;

        [Required(ErrorMessage = "Please select a status")]
        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Ip { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedDatestring { get; set; }
        public string CreatedByName { get; set; }
        public string EncId { get; set; }
    }
}
