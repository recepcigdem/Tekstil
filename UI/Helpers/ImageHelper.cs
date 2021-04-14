using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Models.Common;


namespace UI.Helpers
{
    public class ImageHelper
    {
        public static Image GetImageFromBase64(string imageContent)
        {
            string[] imageInfo = imageContent.Split(',');
            if (imageInfo == null || imageInfo.Length != 2)
                return null;
            else
            {
                string ext = string.Empty;
                if (imageInfo[0].Contains("jpg") || imageInfo[0].Contains("jpeg"))
                    ext = "jpg";
                else if (imageInfo[0].Contains("png"))
                    ext = "png";
                else if (imageInfo[0].Contains("bmp"))
                    ext = "bmp";
                else
                    return null;

                byte[] image = Convert.FromBase64String(imageInfo[1]);
                return new Image(ext, image);
            }
        }
    }
}
