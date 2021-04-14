using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UI.Models.Common
{
    public class SubEntity<T>
    {
        public List<T> data { get; set; }
        public List<int> length { get; set; }
        public List<string> dataType { get; set; }
        public List<Boolean> visibility { get; set; }
        public List<string> header { get; set; }

        public string dataLine
        {
            get
            {
                var entity = (T)Activator.CreateInstance(typeof(T), new object[] { });
                return JsonConvert.SerializeObject(entity);
            }
        }

        public SubEntity()
        {
            data = new List<T>();

            var props = typeof(T).GetProperties();

            dataType = new List<string>();
            visibility = new List<bool>();
            header = new List<string>();

            foreach (PropertyInfo info in props)
            {
                dataType.Add(info.PropertyType.Name);
                header.Add(string.Empty);
                if (info.Name == "IsDeleted" || info.Name == "CreatedBy" || info.Name == "DateCreated" || info.Name == "LastModifiedBy" || info.Name == "DateLastModified" || info.Name == "WillReadRelatedRecords")
                    visibility.Add(false);
                else
                {
                    if (info.PropertyType.Name == "Int32" || info.PropertyType.Name == "Boolean" || info.PropertyType.Name == "String" || info.PropertyType.Name == "DateTime")
                        visibility.Add(true);
                    else
                        visibility.Add(false);
                }
            }

            int visibleCount = visibility.Where(x => x.Equals(true)).Count();
            int hiddenCount = props.Where(x => x.Name == "Id" || x.Name.EndsWith("Id")).Count();

            length = new List<int>();
            int len = 12 / (visibleCount - hiddenCount);

            foreach (bool b in visibility)
            {
                if (b)
                    length.Add(len);
                else
                    length.Add(0);
            }
        }

        public void SetLength(string fieldName, int len)
        {
            var props = typeof(T).GetProperties();

            int index = 0;
            foreach (PropertyInfo info in props)
            {
                if (info.Name == fieldName)
                    length[index] = len;
                index++;
            }
        }

        public void SetVisibility(string fieldName, bool visible)
        {
            var props = typeof(T).GetProperties();

            int index = 0;
            foreach (PropertyInfo info in props)
            {
                if (info.Name == fieldName)
                    visibility[index] = visible;
                index++;
            }
        }

        public void SetHeader(string fieldName, string hdr)
        {
            var props = typeof(T).GetProperties();

            int index = 0;
            foreach (PropertyInfo info in props)
            {
                if (info.Name == fieldName)
                    header[index] = hdr;
                index++;
            }
        }
    }
}
