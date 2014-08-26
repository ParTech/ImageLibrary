using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Westwind.Globalization;

namespace ParTech.ImageLibrary.Core.Providers
{
    public class CustomLocalizationProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
                             IEnumerable<Attribute> attributes,
                             Type containerType,
                             Func<object> modelAccessor,
                             Type modelType,
                             string propertyName)
        {
            var enumerable = attributes as IList<Attribute> ?? attributes.ToList();
            foreach (var attr in enumerable)
            {
                if (attr != null)
                {
                    var typeName = attr.GetType().Name;
                    string attrAppKey;

                    string sKey;
                    string sLocalizedText;
                    if (typeName.Equals("DisplayAttribute"))
                    {
                        attrAppKey = string.Format("{0}.{1}.{2}",
                            containerType.Name, propertyName, typeName);

                        ((DisplayAttribute)attr).Name = DbRes.T(attrAppKey, "Resources");
                    }
                    else if (attr is ValidationAttribute)
                    {
                        attrAppKey = string.Format("{0}.{1}.{2}",
                            containerType.Name, propertyName, typeName);

                        ((ValidationAttribute)attr).ErrorMessage = DbRes.T(attrAppKey, "Resources");
                    }
                }
            }

            return base.CreateMetadata
              (enumerable, containerType, modelAccessor, modelType, propertyName);
        }
    }
}
