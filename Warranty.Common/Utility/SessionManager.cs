using Warranty.Common.Utility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.Utility
{
    public class SessionManager : ISessionManager
    {
        private string sessionKey = "Lead-Session-Key";
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private SessionModel SessionData { get; set; }

        public int UserId
        {
            get
            {
                return GetSession().UserId;
            }
            set
            {
                SessionData.UserId = value;
                SetSession();
            }
        }
        public string Username
        {
            get
            {
                return GetSession().Username;
            }
            set
            {
                SessionData.Username = value;
                SetSession();
            }

        }
        public string FirstName
        {
            get
            {
                return GetSession().FirstName;
            }
            set
            {
                SessionData.FirstName = value;
                SetSession();
            }

        }
        public string RoleName
        {
            get
            {
                return GetSession().RoleName;
            }
            set
            {
                SessionData.RoleName = value;
                SetSession();
            }

        }
        public string LastName
        {
            get
            {
                return GetSession().LastName;
            }
            set
            {
                SessionData.LastName = value;
                SetSession();
            }

        }
        public string Emailid
        {
            get
            {
                return GetSession().Emailid;
            }
            set
            {
                SessionData.Emailid = value;
                SetSession();
            }
        }
        public int RoleId
        {
            get
            {
                return GetSession().RoleId;
            }
            set
            {
                SessionData.RoleId = value;
                SetSession();
            }
        }
        public int EnggId
        {
            get
            {
                return GetSession().EnggId;
            }
            set
            {
                SessionData.EnggId = value;
                SetSession();
            }
        }
        public string CaptchaCode
        {
            get
            {
                return GetSession().CaptchaCode;
            }
            set
            {
                SessionData.CaptchaCode = value;
                SetSession();
            }
        }
        public string UserImage
        {
            get
            {
                return GetSession().UserImage;
            }
            set
            {
                SessionData.UserImage = value;
                SetSession();
            }
        }
        public string CurrentVersion
        {
            get
            {
                return GetSession().CurrentVersion;
            }
            set
            {
                SessionData.CurrentVersion = value;
                SetSession();
            }
        }
        public string CurrentVersionDate
        {
            get
            {
                return GetSession().CurrentVersionDate;
            }
            set
            {
                SessionData.CurrentVersionDate = value;
                SetSession();
            }
        }
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            SessionData = GetSession();
            if (SessionData == null)
            {
                SessionData = new SessionModel();
            }
        }
        public SessionModel GetSession()
        {
            var session = _httpContextAccessor.HttpContext.Session.Get(sessionKey);
            return (SessionModel)(session != null ? FromByteArray<SessionModel>(session) : new SessionModel());

        }
        public void SetSession()
        {
            _httpContextAccessor.HttpContext.Session.Set(sessionKey, ObjectToByteArray(SessionData));
        }
        public string GetSessionId()
        {
            return _httpContextAccessor.HttpContext.Session.Id.ToString();
        }
        public void ClearSession()
        {
            SessionData = new SessionModel();
            _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
            _httpContextAccessor.HttpContext.Session.Clear();
        }
        public string GetIP()
        {
            return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        private static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        private static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
    }
}
