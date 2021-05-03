using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Common
{
    public class ComboData
    {
        public int id { get; set; }
        public string text { get; set; }
        public object obj { get; set; }
        public ComboData(int Id, string Text)
        {
            this.id = Id;
            this.text = Text;
        }

        public ComboData(int Id, string Text,object Obj)
        {
            this.id = Id;
            this.text = Text;
            this.obj = Obj;
        }
    }
}
