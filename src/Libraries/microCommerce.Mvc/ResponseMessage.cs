using System;
using System.Collections.Generic;
using System.Text;

namespace microCommerce.Mvc
{
    public class ResponseMessage
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public object Data { get; set; }
    }
}