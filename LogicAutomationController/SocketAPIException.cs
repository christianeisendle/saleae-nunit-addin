using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Saleae
{
    public class SocketApiException : Exception
    {
        public SocketApiException()
            : base()
        {
        }

        public SocketApiException(string message)
            : base(message)
        {
        }

        public SocketApiException(string message, Exception inner_exception)
            : base(message, inner_exception)
        {
        }

    }
}
