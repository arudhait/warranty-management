using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.CommonEntities
{
    public class AuthResultModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public short RoleId { get; set; }
        public string Emailid { get; set; }
        public string RoleName { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserImage { get; set; }
        public string BaseURL { get; set; }
        public object Result { get; set; }
        public int EnggId { get; set; }
    }
}
