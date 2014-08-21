using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UICDS_Services.IncidentManagementService 
{
    public partial class PackageMetadataType
    {
        private UICDS_Services.IncidentManagementService.IdentificationType1 workProductIdentificationField;

        private UICDS_Services.IncidentManagementService.PropertiesType workProductPropertiesField;

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "WorkProductIdentification", Namespace = "http://www.saic.com/precis/2009/06/structures")]
        public UICDS_Services.IncidentManagementService.IdentificationType1 WorkProductIdentification
        {
            get { return this.workProductIdentificationField; }
            set { this.workProductIdentificationField = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "WorkProductProperties", Namespace = "http://www.saic.com/precis/2009/06/structures")]
        public UICDS_Services.IncidentManagementService.PropertiesType WorkProductProperties
        {
            get { return this.workProductPropertiesField; }
            set { this.workProductPropertiesField = value; }
        }
    }

    public partial class DataItemPackageType
    {
        private UICDS_Services.IncidentManagementService.DigestType digestField;

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "Digest", Namespace = "http://ucore.gov/ucore/2.0")]
        public UICDS_Services.IncidentManagementService.DigestType Digest
        {
            get { return this.digestField; }
            set { this.digestField = value; }
        }
    }

    // Force a prefix and type name on the Incident element
    //[System.Xml.Serialization.XmlType(TypeName = "Incident")]
    public partial class IncidentType
    {
        [XmlNamespaceDeclarations()]
        public XmlSerializerNamespaces xmlsn
        {
            get
            {
                XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                xsn.Add("inc", "http://uicds.org/incident");
                return xsn;
            }
            set
            {
                //Just provide an empty setter. 
            }
        }

        public XmlTypeAttribute XmlType { 
           get {
               XmlTypeAttribute typ = new XmlTypeAttribute("Incident");
                return typ; 
            }
            set { } 
        }
    }

}

namespace UICDS_Services.WorkProductService
{
    public partial class PackageMetadataType
    {
        private UICDS_Services.WorkProductService.IdentificationType workProductIdentificationField;

        private UICDS_Services.WorkProductService.PropertiesType workProductPropertiesField;

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "WorkProductIdentification", Namespace = "http://www.saic.com/precis/2009/06/structures")]
        public UICDS_Services.WorkProductService.IdentificationType WorkProductIdentification
        {
            get { return this.workProductIdentificationField; }
            set { this.workProductIdentificationField = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "WorkProductProperties", Namespace = "http://www.saic.com/precis/2009/06/structures")]
        public UICDS_Services.WorkProductService.PropertiesType WorkProductProperties
        {
            get { return this.workProductPropertiesField; }
            set { this.workProductPropertiesField = value; }
        }
    }

    public partial class DataItemPackageType
    {
        private UICDS_Services.WorkProductService.DigestType digestField;

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "Digest", Namespace = "http://ucore.gov/ucore/2.0")]
        public UICDS_Services.WorkProductService.DigestType Digest
        {
            get { return this.digestField; }
            set { this.digestField = value; }
        }
    }

}

namespace UICDS_Services.NotificationService
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://uicds.org/NotificationService")]
    public partial class WorkProductDeletedNotification
    {
        private UICDS_Services.WorkProductService.IdentificationType workProductIdentificationField;
        private UICDS_Services.WorkProductService.PropertiesType workProductPropertiesField;

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "WorkProductIdentification", Namespace = "http://www.saic.com/precis/2009/06/structures")]
        public UICDS_Services.WorkProductService.IdentificationType WorkProductIdentification
        {
            get { return this.workProductIdentificationField; }
            set { this.workProductIdentificationField = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "WorkProductProperties", Namespace = "http://www.saic.com/precis/2009/06/structures")]
        public UICDS_Services.WorkProductService.PropertiesType WorkProductProperties
        {
            get { return this.workProductPropertiesField; }
            set { this.workProductPropertiesField = value; }
        }

    }
}
