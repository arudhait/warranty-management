using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.CommonEntities
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public object Result1 { get; set; }
       
    }
    public class GoogleAPIResponseModel
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string mimeType { get; set; }
    }
    public class APIResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
