using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class UserMastModel
    {
        public int UserId { get; set; }
        public int? EnggId { get; set; }
        public string EncId{ get; set; }

        [Required(ErrorMessage = "Please enter User Name")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter User Password")]
        public string UserPassword { get; set; } = null!;

        [Required(ErrorMessage = "Please select Role")]
        public short? RoleId { get; set; }
        public string UserTypeName { get; set; } = null!;
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please select Status")]
        public bool IsActive { get; set; }
        public bool UserStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }
        public string CreatedOnString { get; set; }
        public DateTime LastVisited { get; set; }
    }
}
