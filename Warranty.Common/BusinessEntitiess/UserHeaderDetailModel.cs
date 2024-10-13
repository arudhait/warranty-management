using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BussinessEntities
{
    public class UserHeaderDetailModel
    {
        public int UserMasterId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string UserImage { get; set; }
    }
}
