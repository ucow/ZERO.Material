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

        public static void ClassEvaluate<TSource, TTarget>(TSource source, TTarget target)
        {
            if (source == null)
            {
                return;
            }

            Type sourceType = source.GetType();
            Type targetType = target.GetType();
            PropertyInfo[] sourceInfos = sourceType.GetProperties();
            PropertyInfo[] targetInfos = targetType.GetProperties();
            foreach (PropertyInfo targetInfo in targetInfos)
            {
                foreach (PropertyInfo sourceInfo in sourceInfos)
                {
                    var sourceInfoValue = sourceInfo.GetValue(source);
                    if (targetInfo.Name == sourceInfo.Name)
                    {
                        targetInfo.SetValue(target, sourceInfoValue);
                    }
                }
            }
        }
    }
}