using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Exceptions
{
    public class LogicalErrorException : Exception
    {
        public LogicalErrorException(string message) : base(message)
        {

        }
    }
}
