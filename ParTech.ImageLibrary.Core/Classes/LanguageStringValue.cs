using System.Collections;
using System.Runtime.Serialization;

namespace ParTech.ImageLibrary.Core.Classes
{
    [DataContract]
    public class LanguageStringValue
    {
        [DataMember]
        public string Code;

        [DataMember]
        public string Value;

        public LanguageStringValue(string code, string value)
        {
            Code = code;
            Value = value;
        }
    }
}
