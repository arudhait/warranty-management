using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class ActionMastModel
    {
        public short ActionId { get; set; }

        public string ActionName { get; set; }

        public bool? IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string Ip { get; set; } = null!;
    }
}
