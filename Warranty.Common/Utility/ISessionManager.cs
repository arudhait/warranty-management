using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.Utility
{
    public interface ISessionManager
    {
        int UserId { get; set; }
        string Username { get; set; }
        string Emailid { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string RoleName { get; set; }
        int RoleId { get; set; }
        string CaptchaCode { get; set; }
        string GetSessionId();
        void ClearSession();
        string CurrentVersion { get; set; }
        string CurrentVersionDate { get; set; }
        string GetIP();
        int EnggId { get; set; }
    }
}
