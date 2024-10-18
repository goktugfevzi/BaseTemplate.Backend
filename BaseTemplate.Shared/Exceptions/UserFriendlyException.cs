using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Exceptions
{
    public class UserFriendlyException : CustomException
    {
        public Enum ExceptionTypeEnum { get; set; }

        public UserFriendlyException(Enum exceptionTypeEnum, List<string>? errors = default, HttpStatusCode httpStatusCode = HttpStatusCode.NoContent)
            : base("Failures Occured.", errors, httpStatusCode)
        {
            ExceptionTypeEnum = exceptionTypeEnum;
        }
    }
}
