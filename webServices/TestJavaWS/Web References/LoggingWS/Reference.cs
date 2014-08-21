﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.3082.
// 
#pragma warning disable 1591

namespace TestJavaWS.LoggingWS {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="LoggingServiceSoap11", Namespace="http://uicds.org/LoggingMessage")]
    public partial class LoggingServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetLogByHostnameOperationCompleted;
        
        private System.Threading.SendOrPostCallback LogOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLogByLoggerOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public LoggingServiceService() {
            this.Url = global::TestJavaWS.Properties.Settings.Default.TestJavaWS_LoggingWS_LoggingServiceService;
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
        public event GetLogByHostnameCompletedEventHandler GetLogByHostnameCompleted;
        
        /// <remarks/>
        public event LogCompletedEventHandler LogCompleted;
        
        /// <remarks/>
        public event GetLogByLoggerCompletedEventHandler GetLogByLoggerCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetLogByHostnameResponse", Namespace="http://uicds.org/LoggingMessage")]
        public GetLogByHostnameResponse GetLogByHostname([System.Xml.Serialization.XmlElementAttribute(Namespace="http://uicds.org/LoggingMessage")] GetLogByHostnameRequest GetLogByHostnameRequest) {
            object[] results = this.Invoke("GetLogByHostname", new object[] {
                        GetLogByHostnameRequest});
            return ((GetLogByHostnameResponse)(results[0]));
        }
        
        /// <remarks/>
        public void GetLogByHostnameAsync(GetLogByHostnameRequest GetLogByHostnameRequest) {
            this.GetLogByHostnameAsync(GetLogByHostnameRequest, null);
        }
        
        /// <remarks/>
        public void GetLogByHostnameAsync(GetLogByHostnameRequest GetLogByHostnameRequest, object userState) {
            if ((this.GetLogByHostnameOperationCompleted == null)) {
                this.GetLogByHostnameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLogByHostnameOperationCompleted);
            }
            this.InvokeAsync("GetLogByHostname", new object[] {
                        GetLogByHostnameRequest}, this.GetLogByHostnameOperationCompleted, userState);
        }
        
        private void OnGetLogByHostnameOperationCompleted(object arg) {
            if ((this.GetLogByHostnameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLogByHostnameCompleted(this, new GetLogByHostnameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("LogResponse", Namespace="http://uicds.org/LoggingMessage")]
        public LogResponse Log([System.Xml.Serialization.XmlElementAttribute(Namespace="http://uicds.org/LoggingMessage")] LogType LogRequest) {
            object[] results = this.Invoke("Log", new object[] {
                        LogRequest});
            return ((LogResponse)(results[0]));
        }
        
        /// <remarks/>
        public void LogAsync(LogType LogRequest) {
            this.LogAsync(LogRequest, null);
        }
        
        /// <remarks/>
        public void LogAsync(LogType LogRequest, object userState) {
            if ((this.LogOperationCompleted == null)) {
                this.LogOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLogOperationCompleted);
            }
            this.InvokeAsync("Log", new object[] {
                        LogRequest}, this.LogOperationCompleted, userState);
        }
        
        private void OnLogOperationCompleted(object arg) {
            if ((this.LogCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LogCompleted(this, new LogCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetLogByLoggerResponse", Namespace="http://uicds.org/LoggingMessage")]
        public GetLogByLoggerResponse GetLogByLogger([System.Xml.Serialization.XmlElementAttribute(Namespace="http://uicds.org/LoggingMessage")] GetLogByLoggerRequest GetLogByLoggerRequest) {
            object[] results = this.Invoke("GetLogByLogger", new object[] {
                        GetLogByLoggerRequest});
            return ((GetLogByLoggerResponse)(results[0]));
        }
        
        /// <remarks/>
        public void GetLogByLoggerAsync(GetLogByLoggerRequest GetLogByLoggerRequest) {
            this.GetLogByLoggerAsync(GetLogByLoggerRequest, null);
        }
        
        /// <remarks/>
        public void GetLogByLoggerAsync(GetLogByLoggerRequest GetLogByLoggerRequest, object userState) {
            if ((this.GetLogByLoggerOperationCompleted == null)) {
                this.GetLogByLoggerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLogByLoggerOperationCompleted);
            }
            this.InvokeAsync("GetLogByLogger", new object[] {
                        GetLogByLoggerRequest}, this.GetLogByLoggerOperationCompleted, userState);
        }
        
        private void OnGetLogByLoggerOperationCompleted(object arg) {
            if ((this.GetLogByLoggerCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLogByLoggerCompleted(this, new GetLogByLoggerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://uicds.org/LoggingMessage")]
    public partial class GetLogByHostnameRequest {
        
        private string hostnameField;
        
        /// <remarks/>
        public string hostname {
            get {
                return this.hostnameField;
            }
            set {
                this.hostnameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uicds.org/LoggingMessage")]
    public partial class LogType {
        
        private string loggerField;
        
        private System.DateTime timestampField;
        
        private string hostnameField;
        
        private LogLevelType typeField;
        
        private string messageField;
        
        /// <remarks/>
        public string logger {
            get {
                return this.loggerField;
            }
            set {
                this.loggerField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime timestamp {
            get {
                return this.timestampField;
            }
            set {
                this.timestampField = value;
            }
        }
        
        /// <remarks/>
        public string hostname {
            get {
                return this.hostnameField;
            }
            set {
                this.hostnameField = value;
            }
        }
        
        /// <remarks/>
        public LogLevelType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uicds.org/LoggingMessage")]
    public enum LogLevelType {
        
        /// <remarks/>
        Fatal,
        
        /// <remarks/>
        Error,
        
        /// <remarks/>
        Warn,
        
        /// <remarks/>
        Debug,
        
        /// <remarks/>
        Info,
        
        /// <remarks/>
        Trace,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://uicds.org/LoggingMessage")]
    public partial class GetLogByHostnameResponse {
        
        private string hostnameField;
        
        private int numberOfRecordsField;
        
        private LogType[] logRecordField;
        
        /// <remarks/>
        public string hostname {
            get {
                return this.hostnameField;
            }
            set {
                this.hostnameField = value;
            }
        }
        
        /// <remarks/>
        public int numberOfRecords {
            get {
                return this.numberOfRecordsField;
            }
            set {
                this.numberOfRecordsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("logRecord")]
        public LogType[] logRecord {
            get {
                return this.logRecordField;
            }
            set {
                this.logRecordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://uicds.org/LoggingMessage")]
    public partial class LogResponse {
        
        private LogResponseType responseField;
        
        /// <remarks/>
        public LogResponseType response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://uicds.org/LoggingMessage")]
    public enum LogResponseType {
        
        /// <remarks/>
        Success,
        
        /// <remarks/>
        Failure,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://uicds.org/LoggingMessage")]
    public partial class GetLogByLoggerRequest {
        
        private string loggerField;
        
        /// <remarks/>
        public string logger {
            get {
                return this.loggerField;
            }
            set {
                this.loggerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://uicds.org/LoggingMessage")]
    public partial class GetLogByLoggerResponse {
        
        private string loggerField;
        
        private int numberOfRecordsField;
        
        private LogType[] logRecordField;
        
        /// <remarks/>
        public string logger {
            get {
                return this.loggerField;
            }
            set {
                this.loggerField = value;
            }
        }
        
        /// <remarks/>
        public int numberOfRecords {
            get {
                return this.numberOfRecordsField;
            }
            set {
                this.numberOfRecordsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("logRecord")]
        public LogType[] logRecord {
            get {
                return this.logRecordField;
            }
            set {
                this.logRecordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void GetLogByHostnameCompletedEventHandler(object sender, GetLogByHostnameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLogByHostnameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLogByHostnameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public GetLogByHostnameResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((GetLogByHostnameResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void LogCompletedEventHandler(object sender, LogCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LogCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LogCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public LogResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((LogResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void GetLogByLoggerCompletedEventHandler(object sender, GetLogByLoggerCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLogByLoggerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLogByLoggerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public GetLogByLoggerResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((GetLogByLoggerResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591