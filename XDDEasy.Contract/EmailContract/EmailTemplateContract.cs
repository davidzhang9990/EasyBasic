using System;
using System.Runtime.Serialization;

namespace XDDEasy.Contract.EmailContract
{
    public class EmailTemplateResponse : ResponseBase
    {
        [DataMember]
        public string TemplateCulture { get; set; }
        [DataMember]
        public string TemplateTitle { get; set; }
        [DataMember]
        public string TemplateType { get; set; }
        [DataMember]
        public string TemplateValue { get; set; }
    }

    public class EmailTemplateRequest
    {
        [DataMember]
        public string TemplateCulture { get; set; }
        [DataMember]
        public string TemplateTitle { get; set; }
        [DataMember]
        public string TemplateType { get; set; }
        [DataMember]
        public string TemplateValue { get; set; }

        [DataMember]
        public Guid? SchoolId { get; set; }
    }

    public enum EnumEmailTemplateName
    {
        [DataMember]
        EmailRegistrationTemplate
    }
}
