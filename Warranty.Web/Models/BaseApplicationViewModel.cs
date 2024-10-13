namespace Warranty.Web.Models
{
    public class BaseApplicationViewModel : BaseBreadCrumbViewModel
    {
        public string Title { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool IsEdit { get; set; }
        public bool IsView { get; set; }
        public bool IsAdd { get; set; }
        public bool IsSave { get; set; }
        public bool IsAddImages { get; set; }

    }
}
