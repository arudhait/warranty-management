using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BussinessEntities
{
    public class MenuModel
    {
        public short MenuId { get; set; }

        public string MenuName { get; set; } = null!;

        public string MenuNameId { get; set; } = null!;

        public string MenuUrl { get; set; } = null!;

        public string Icon { get; set; } = null!;

        public bool IsActive { get; set; }

        public short DisplayOrder { get; set; }

        public int ParentId { get; set; }

        public bool HaveChild { get; set; }
    }
}
