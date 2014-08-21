﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.235.
// 
#pragma warning disable 1591

namespace UICDS_Services.BroadcastService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BroadcastServiceSoap11", Namespace="http://uicds.org/BroadcastService")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComplexObjectType))]
    public partial class BroadcastServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback BroadcastMessageOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public BroadcastServiceService() {
            this.Url = global::UICDS_Services.Properties.Settings.Default.UICDS_Services_BroadcastService_BroadcastServiceService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event BroadcastMessageCompletedEventHandler BroadcastMessageCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("BroadcastMessageResponse", Namespace="http://uicds.org/BroadcastService")]
        public BroadcastMessageResponseType BroadcastMessage([System.Xml.Serialization.XmlElementAttribute(Namespace="http://uicds.org/BroadcastService")] BroadcastMessageType BroadcastMessageRequest) {
            object[] results = this.Invoke("BroadcastMessage", new object[] {
                        BroadcastMessageRequest});
            return ((BroadcastMessageResponseType)(results[0]));
        }
        
        /// <remarks/>
        public void BroadcastMessageAsync(BroadcastMessageType BroadcastMessageRequest) {
            this.BroadcastMessageAsync(BroadcastMessageRequest, null);
        }
        
        /// <remarks/>
        public void BroadcastMessageAsync(BroadcastMessageType BroadcastMessageRequest, object userState) {
            if ((this.BroadcastMessageOperationCompleted == null)) {
                this.BroadcastMessageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnBroadcastMessageOperationCompleted);
            }
            this.InvokeAsync("BroadcastMessage", new object[] {
                        BroadcastMessageRequest}, this.BroadcastMessageOperationCompleted, userState);
        }
        
        private void OnBroadcastMessageOperationCompleted(object arg) {
            if ((this.BroadcastMessageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.BroadcastMessageCompleted(this, new BroadcastMessageCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uicds.org/BroadcastService")]
    public partial class BroadcastMessageType : ComplexObjectType {
        
        private EDXLDistribution eDXLDistributionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
        public EDXLDistribution EDXLDistribution {
            get {
                return this.eDXLDistributionField;
            }
            set {
                this.eDXLDistributionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public partial class EDXLDistribution {
        
        private string distributionIDField;
        
        private string senderIDField;
        
        private System.DateTime dateTimeSentField;
        
        private statusValues distributionStatusField;
        
        private typeValues distributionTypeField;
        
        private string combinedConfidentialityField;
        
        private string languageField;
        
        private valueListType[] senderRoleField;
        
        private valueListType[] recipientRoleField;
        
        private valueListType[] keywordField;
        
        private string[] distributionReferenceField;
        
        private valueSchemeType[] explicitAddressField;
        
        private targetAreaType[] targetAreaField;
        
        private contentObjectType[] contentObjectField;
        
        /// <remarks/>
        public string distributionID {
            get {
                return this.distributionIDField;
            }
            set {
                this.distributionIDField = value;
            }
        }
        
        /// <remarks/>
        public string senderID {
            get {
                return this.senderIDField;
            }
            set {
                this.senderIDField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime dateTimeSent {
            get {
                return this.dateTimeSentField;
            }
            set {
                this.dateTimeSentField = value;
            }
        }
        
        /// <remarks/>
        public statusValues distributionStatus {
            get {
                return this.distributionStatusField;
            }
            set {
                this.distributionStatusField = value;
            }
        }
        
        /// <remarks/>
        public typeValues distributionType {
            get {
                return this.distributionTypeField;
            }
            set {
                this.distributionTypeField = value;
            }
        }
        
        /// <remarks/>
        public string combinedConfidentiality {
            get {
                return this.combinedConfidentialityField;
            }
            set {
                this.combinedConfidentialityField = value;
            }
        }
        
        /// <remarks/>
        public string language {
            get {
                return this.languageField;
            }
            set {
                this.languageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("senderRole")]
        public valueListType[] senderRole {
            get {
                return this.senderRoleField;
            }
            set {
                this.senderRoleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("recipientRole")]
        public valueListType[] recipientRole {
            get {
                return this.recipientRoleField;
            }
            set {
                this.recipientRoleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("keyword")]
        public valueListType[] keyword {
            get {
                return this.keywordField;
            }
            set {
                this.keywordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("distributionReference")]
        public string[] distributionReference {
            get {
                return this.distributionReferenceField;
            }
            set {
                this.distributionReferenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("explicitAddress")]
        public valueSchemeType[] explicitAddress {
            get {
                return this.explicitAddressField;
            }
            set {
                this.explicitAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("targetArea")]
        public targetAreaType[] targetArea {
            get {
                return this.targetAreaField;
            }
            set {
                this.targetAreaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contentObject")]
        public contentObjectType[] contentObject {
            get {
                return this.contentObjectField;
            }
            set {
                this.contentObjectField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public enum statusValues {
        
        /// <remarks/>
        Actual,
        
        /// <remarks/>
        Exercise,
        
        /// <remarks/>
        System,
        
        /// <remarks/>
        Test,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public enum typeValues {
        
        /// <remarks/>
        Report,
        
        /// <remarks/>
        Update,
        
        /// <remarks/>
        Cancel,
        
        /// <remarks/>
        Request,
        
        /// <remarks/>
        Response,
        
        /// <remarks/>
        Dispatch,
        
        /// <remarks/>
        Ack,
        
        /// <remarks/>
        Error,
        
        /// <remarks/>
        SensorConfiguration,
        
        /// <remarks/>
        SensorControl,
        
        /// <remarks/>
        SensorStatus,
        
        /// <remarks/>
        SensorDetection,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public partial class valueListType {
        
        private string valueListUrnField;
        
        private string[] valueField;
        
        /// <remarks/>
        public string valueListUrn {
            get {
                return this.valueListUrnField;
            }
            set {
                this.valueListUrnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public string[] value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uicds.org/BroadcastService")]
    public partial class BroadcastMessageErrorType {
        
        private string coreNameField;
        
        private string errorField;
        
        /// <remarks/>
        public string coreName {
            get {
                return this.coreNameField;
            }
            set {
                this.coreNameField = value;
            }
        }
        
        /// <remarks/>
        public string error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uicds.org/BroadcastService")]
    public partial class BroadcastMessageResponseType {
        
        private bool errorExistsField;
        
        private string errorStringField;
        
        private BroadcastMessageErrorType[] coreErrorField;
        
        /// <remarks/>
        public bool errorExists {
            get {
                return this.errorExistsField;
            }
            set {
                this.errorExistsField = value;
            }
        }
        
        /// <remarks/>
        public string errorString {
            get {
                return this.errorStringField;
            }
            set {
                this.errorStringField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("coreError")]
        public BroadcastMessageErrorType[] coreError {
            get {
                return this.coreErrorField;
            }
            set {
                this.coreErrorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public partial class anyXMLType {
        
        private System.Xml.XmlElement[] anyField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public partial class xmlContentType {
        
        private anyXMLType[] keyXMLContentField;
        
        private anyXMLType[] embeddedXMLContentField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("keyXMLContent")]
        public anyXMLType[] keyXMLContent {
            get {
                return this.keyXMLContentField;
            }
            set {
                this.keyXMLContentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("embeddedXMLContent")]
        public anyXMLType[] embeddedXMLContent {
            get {
                return this.embeddedXMLContentField;
            }
            set {
                this.embeddedXMLContentField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public partial class nonXMLContentType {
        
        private string mimeTypeField;
        
        private string sizeField;
        
        private string digestField;
        
        private string uriField;
        
        private byte[] contentDataField;
        
        /// <remarks/>
        public string mimeType {
            get {
                return this.mimeTypeField;
            }
            set {
                this.mimeTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string size {
            get {
                return this.sizeField;
            }
            set {
                this.sizeField = value;
            }
        }
        
        /// <remarks/>
        public string digest {
            get {
                return this.digestField;
            }
            set {
                this.digestField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
        public string uri {
            get {
                return this.uriField;
            }
            set {
                this.uriField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] contentData {
            get {
                return this.contentDataField;
            }
            set {
                this.contentDataField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public partial class contentObjectType {
        
        private string contentDescriptionField;
        
        private valueListType[] contentKeywordField;
        
        private string incidentIDField;
        
        private string incidentDescriptionField;
        
        private valueListType[] originatorRoleField;
        
        private valueListType[] consumerRoleField;
        
        private string confidentialityField;
        
        private object itemField;
        
        private System.Xml.XmlElement[] anyField;
        
        /// <remarks/>
        public string contentDescription {
            get {
                return this.contentDescriptionField;
            }
            set {
                this.contentDescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contentKeyword")]
        public valueListType[] contentKeyword {
            get {
                return this.contentKeywordField;
            }
            set {
                this.contentKeywordField = value;
            }
        }
        
        /// <remarks/>
        public string incidentID {
            get {
                return this.incidentIDField;
            }
            set {
                this.incidentIDField = value;
            }
        }
        
        /// <remarks/>
        public string incidentDescription {
            get {
                return this.incidentDescriptionField;
            }
            set {
                this.incidentDescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("originatorRole")]
        public valueListType[] originatorRole {
            get {
                return this.originatorRoleField;
            }
            set {
                this.originatorRoleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("consumerRole")]
        public valueListType[] consumerRole {
            get {
                return this.consumerRoleField;
            }
            set {
                this.consumerRoleField = value;
            }
        }
        
        /// <remarks/>
        public string confidentiality {
            get {
                return this.confidentialityField;
            }
            set {
                this.confidentialityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("nonXMLContent", typeof(nonXMLContentType))]
        [System.Xml.Serialization.XmlElementAttribute("xmlContent", typeof(xmlContentType))]
        public object Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public partial class targetAreaType {
        
        private string[] circleField;
        
        private string[] polygonField;
        
        private string[] countryField;
        
        private string[] subdivisionField;
        
        private string[] locCodeUNField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("circle")]
        public string[] circle {
            get {
                return this.circleField;
            }
            set {
                this.circleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("polygon")]
        public string[] polygon {
            get {
                return this.polygonField;
            }
            set {
                this.polygonField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("country")]
        public string[] country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("subdivision")]
        public string[] subdivision {
            get {
                return this.subdivisionField;
            }
            set {
                this.subdivisionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("locCodeUN")]
        public string[] locCodeUN {
            get {
                return this.locCodeUNField;
            }
            set {
                this.locCodeUNField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:tc:emergency:EDXL:DE:1.0")]
    public partial class valueSchemeType {
        
        private string explicitAddressSchemeField;
        
        private string[] explicitAddressValueField;
        
        /// <remarks/>
        public string explicitAddressScheme {
            get {
                return this.explicitAddressSchemeField;
            }
            set {
                this.explicitAddressSchemeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("explicitAddressValue")]
        public string[] explicitAddressValue {
            get {
                return this.explicitAddressValueField;
            }
            set {
                this.explicitAddressValueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BroadcastMessageType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://niem.gov/niem/structures/2.0")]
    public abstract partial class ComplexObjectType {
        
        private string idField;
        
        private string metadataField;
        
        private string linkMetadataField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, DataType="ID")]
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, DataType="IDREFS")]
        public string metadata {
            get {
                return this.metadataField;
            }
            set {
                this.metadataField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, DataType="IDREFS")]
        public string linkMetadata {
            get {
                return this.linkMetadataField;
            }
            set {
                this.linkMetadataField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void BroadcastMessageCompletedEventHandler(object sender, BroadcastMessageCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BroadcastMessageCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal BroadcastMessageCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public BroadcastMessageResponseType Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((BroadcastMessageResponseType)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591