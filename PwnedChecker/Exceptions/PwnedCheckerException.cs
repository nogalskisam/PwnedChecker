using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PwnedChecker.Exceptions
{
    public class PwnedCheckerException : Exception
    {
        public PwnedCheckerException()
        {
        }

        public PwnedCheckerException(string message) : base(message)
        {
        }

        public PwnedCheckerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PwnedCheckerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
