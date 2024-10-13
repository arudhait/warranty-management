using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class ModelDetailModel
    {
        public long ModelDetId { get; set; }

        public long WarrantyId { get; set; }

        [Required(ErrorMessage = "Please select a Model Name")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "Please select a Model Serial No.")]
        public string? ModelSerialNo { get; set; }
        public string ModelNo { get; set; }
        public string EncId { get; set; }
    }
}
