using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Saleae
{
    public class SaleaeSocketApiException : Exception
    {
        public SaleaeSocketApiException()
            : base()
        {
        }

        public SaleaeSocketApiException(string message)
            : base(message)
        {
        }

        public SaleaeSocketApiException(string message, Exception inner_exception)
            : base(message, inner_exception)
        {
        }

    }
}
