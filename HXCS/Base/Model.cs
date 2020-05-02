using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HXCS.Base
{
    public abstract class Model
    {
        public int id { set; get; }

       
    }

    [Table]
    public class Oji : Model
    {
        [Length(2)]
        public int Fauzi { set; get; }
    }

    [Table]
    public class SecondTable : Model
    {
        
        public int Id { set; get; }
      
        public string Name { set; get; }
    }

    [Table]
    public class Iops : Model
    {
        [Length(10)]
        public string Name1 { set; get; }
        public string Name2 { set; get; }
        [Length(10)]
        public int Name3 { set; get; }

        [NotNull]
        [Unique]
        public int Field1 { get; set; }
        [NotNull]
        [ForeignKey]
        public SecondTable SecondTable { get; set; }
        [NotNull]
        public Boolean Field3 { get; set; }
        [NotNull]
        public bool asd { get; set; }
  
        public String asdf() { return ""; }
    }
}
