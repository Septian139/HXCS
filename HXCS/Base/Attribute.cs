using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXCS.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ForeignKey : Attribute{ }
   
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NotNull : Attribute { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    sealed class Unique : Attribute { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class Length : Attribute
    {
        uint s = 1;
        public int Size { get { return (int)s; } }
        public Length(uint length)
        {
            if(length == 0)
            {
                throw new HXCSException("Length cannot be less than 1");
            }
            else
            {
                s = length;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    sealed class Table : Attribute { }
}
