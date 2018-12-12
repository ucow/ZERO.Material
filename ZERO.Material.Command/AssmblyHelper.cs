using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ZERO.Material.Model.AttributeCustom;

namespace ZERO.Material.Command
{
    public class AssmblyHelper
    {
        public static string GetDisplayAttributeInfo<T>() where T : class
        {
            Type type = typeof(T);
            ArrayList modelInfos = new ArrayList();
            PropertyInfo[] infos = type.GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (info.GetCustomAttribute(typeof(DisplayAttribute), false) is DisplayAttribute customAttribute)
                {
                    var modelInfo = new
                    {
                        name = info.Name,
                        value = customAttribute.Name
                    };
                    modelInfos.Add(modelInfo);
                }
            }

            if (type.GetCustomAttributes(typeof(MetadataTypeAttribute), false).FirstOrDefault() is MetadataTypeAttribute metadata)
            {
                PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (propertyInfo.GetCustomAttribute(typeof(DisplayAttribute), false) is DisplayAttribute customAttribute)
                    {
                        bool sort = propertyInfo.GetCustomAttribute(typeof(IsSortAttribute), false) is IsSortAttribute isSort;
                        var modelInfo = new
                        {
                            name = propertyInfo.Name,
                            value = customAttribute.Name,
                            isSort = sort
                        };
                        modelInfos.Add(modelInfo);
                    }
                }
            }

            return JsonConvert.SerializeObject(modelInfos);
        }
    }
}