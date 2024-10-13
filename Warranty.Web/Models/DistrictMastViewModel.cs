using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using System.Diagnostics.Contracts;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class DistrictMastViewModel : BaseApplicationViewModel
    {
        public DistrictMastModel DistrictMastModel { get; set; }

        public TempFilterModel TempFilterModel { get; set; }

        public List<SelectListItem> StateList { get; set; }
    }
}
