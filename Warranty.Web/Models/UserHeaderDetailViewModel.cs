using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class UserHeaderDetailViewModel
    {
        public UserHeaderDetailModel UserHeaderDetail { get; set; }
        public int RoleId { get; set; }
        public bool IsLight { get; set; }
    }
}
