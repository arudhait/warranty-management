using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.CommonEntities
{
    public class SessionProviderModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Ip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsLight { get; set; }
        public int LeadStatusId { get; set; }
        public string LeadStatus { get; set; }
        public int UserTypeId { get; set; }
        public int EnggId { get; set; }

    }
}
