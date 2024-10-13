using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.Utility
{
    [Serializable]
    public class SessionModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public string Emailid { get; set; }
        public int RoleId { get; set; }
        public string CaptchaCode { get; set; }
        public string UserImage { get; set; }
        public string CurrentVersion { get; set; }
        public string CurrentVersionDate { get; set; }
        public int ServiceEngineerId { get; set; }
        public int EnggId { get; set; }
        public string LeadStatus { get; set; }
    }
}
