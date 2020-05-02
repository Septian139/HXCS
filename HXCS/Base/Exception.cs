using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXCS.Base
{
    class HXCSException : Exception
    {
        public HXCSException(string msg) : base(msg) {}
    }
}
