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
        public ComboData(int Id, string Text)
        {
            this.id = Id;
            this.text = Text;
        }
    }
}
