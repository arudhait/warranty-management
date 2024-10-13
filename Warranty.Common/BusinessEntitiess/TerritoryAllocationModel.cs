using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class TerritoryAllocationModel
    {
        public int AlloctionId { get; set; }

        public int EnggId { get; set; }

        public int DistrictId { get; set; }

        public short StateId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string Ip { get; set; } = null!;
        public string EncId { get; set; }
       
        
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public EnggMastModel EnggMastModel { get; set; }
        public String EnggName { get; set; }
        public string CreatedDatestring { get; set; }
        public string CreatedByName { get; set; }
    }
}
