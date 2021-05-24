using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.IO;

namespace UI.Models.Label
{
    public class Label : BaseModel
    {
        public bool IsUsed { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public string RootPath { get; set; }
        public string ImageBase64 { get; set; }

        public Label() : base()
        {
            IsUsed = false;
            IsActive = true;
            Code = string.Empty;
            Description = string.Empty;
            Image = string.Empty;
            RootPath = string.Empty;
            ImageBase64 = string.Empty;
        }
        public Label(HttpRequest request, Entities.Concrete.Label label, IStringLocalizer _localizerShared, string rootPath) : base(request)
        {
            EntityId = label.Id;
            CustomerId = label.CustomerId;
            IsActive = label.IsActive;
            IsUsed = label.IsUsed;
            Code = label.Code;
            Description = label.Description;
            Image = label.Image;
            RootPath = rootPath;

            if (!string.IsNullOrWhiteSpace(Image))
            {
                string fileName = rootPath + Image;
                byte[] binaryContent = File.ReadAllBytes(fileName);
                this.ImageBase64 = Convert.ToBase64String(binaryContent, 0, binaryContent.Length);
            }
            else
            {
                this.Image = "/assets/images/placeholder.png";
                this.ImageBase64 = string.Empty;
            }

        }
        public Entities.Concrete.Label GetBusinessModel()
        {
            Entities.Concrete.Label label = new Entities.Concrete.Label();
            if (EntityId > 0)
            {
                label.Id = EntityId;
            }

            label.CustomerId = CustomerId;
            label.IsActive = IsActive;
            label.IsUsed = IsUsed;
            label.Code = Code;
            label.Description = Description;
            label.Image = Image;
            
            if (!string.IsNullOrWhiteSpace(this.ImageBase64))
            {
                var textBase64 = "data:image/jpeg;base64,";
                bool IsContains = this.ImageBase64.Contains(textBase64);
                if (IsContains)
                {
                    var img = Helpers.ImageHelper.GetImageFromBase64(this.ImageBase64);
                    if (img != null)
                    {
                        string fileName = RootPath + "\\assets\\photos\\" + img.FileName;
                        File.WriteAllBytes(fileName, img.ImageContent);
                        label.Image = "\\assets\\photos\\" + img.FileName;
                    }
                }
                else
                {
                    string[] imageInfo2 = ImageBase64.Split(',');
                    if (imageInfo2.Length > 1)
                    {
                        this.ImageBase64 = imageInfo2[imageInfo2.Length - 1];
                    }

                    this.ImageBase64 = textBase64 + this.ImageBase64;
                    var img = Helpers.ImageHelper.GetImageFromBase64(this.ImageBase64);
                    if (img != null && imageInfo2.Length > 1)
                    {
                        string fileName = RootPath + "\\assets\\photos\\" + img.FileName;
                        File.WriteAllBytes(fileName, img.ImageContent);
                        label.Image = "\\assets\\photos\\" + img.FileName;
                    }
                }

            }
            else
                label.Image = string.Empty;

            

            return label;
        }
    }
}
