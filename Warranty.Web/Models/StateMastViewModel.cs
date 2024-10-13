using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class StateMastViewModel : BaseApplicationViewModel
    {
        public StateMastModel StateMastModel { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
    }
}
