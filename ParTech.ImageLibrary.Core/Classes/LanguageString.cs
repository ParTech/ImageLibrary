using System.Runtime.Serialization;

namespace ParTech.ImageLibrary.Core.Classes
{
    [DataContract]
    public class LanguageString
    {
        [DataMember]
        public LanguageStringValue[] Values;
    }
}
