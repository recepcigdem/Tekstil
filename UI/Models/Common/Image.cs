using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models.Common
{
    public class Image
    {
        public string Extension { get; set; }
        public byte[] ImageContent { get; set; }

        public string FileName { get; set; }

        public Image(string ext, byte[] img)
        {
            Extension = ext;
            ImageContent = img;
            FileName = Guid.NewGuid().ToString().Replace("-", "") + "." + Extension;
        }
    }
}
