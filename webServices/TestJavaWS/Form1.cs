using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Xml.Serialization;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;


namespace TestJavaWS
{


    public partial class Form1 : Form
    {
        Microsoft.Office.Interop.Excel.ApplicationClass excelApp;
        Microsoft.Office.Interop.Excel.Workbook excelBook;
        Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        int excelRow = 1;
        string strIncidentID;
        TestJavaWS.AlertWS.IdentifierType alertIdentifer;
        TestJavaWS.AlertWS.IdentificationType alertWorkProductIdentification;

        // WVS - get the identification structure!
       // IncidentManagementWS.IdentificationType1 IncidentManagementWorkProductIdentification;
       // TestJavaWS.IncidentCommandWS.IdentificationType IncidentCommandWorkProductIdentification;
       // IncidentManagementWS.IncidentInfoType incident;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            excelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
            excelApp.Visible = false;
            object oMissing = System.Reflection.Missing.Value;
            string excelFile = @"c:\SomeFileName.xls";

            //Add worksheet file       
            excelBook = excelApp.Workbooks.Add(oMissing);
            excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.ActiveSheet;

            //ServicePointManager.ServerCertificateValidationCallback 
            ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            ServicePointManager.Expect100Continue = false;

            // WVS - This is still the old profile stuff...
            // Done
            CreateProfile();

            // Done
            GetProfile();

            // Done
            GetProfileList();

            // Done
            CreateAlert();

            // WVS - get the identification structure!
            //GetAlert();
            
            // Done
            CreateIncident();
            
            // WVS - get the identification structure!
            //            GetIncident();
            //            UpdateIncident();
            
            // Done
            CreateIncidentFromCap();
            
            // Done
            TestSensorWS();
            
            // Done
            UpdateOrganization();
            
            // WVS - get the identification structure!
            //              GetOrganization();

            // Done
            TestNotificationWS();
            // Done
            GetIncidentList();
            // Done
            GetUserList();

            // WVS - get the identification structure!
            //            TestWorkProductWS(alertIdentifer);

            // WVS - get the identification structure!
            //            CancelAlert(alertIdentifer);


            // WVS - This is still the old profile stuff...
            // Done
            DeleteProfile();

            // WVS - this test doesnt seem to do anything
            // TestAgreementWS();

            // WVS - this test doesnt seem to do anything
            // TestLoggingWS();

            // WVS - this test doesnt seem to do anything
            // TestMapWS();

            excelBook.SaveAs(excelFile, oMissing, oMissing, oMissing, oMissing, oMissing, XlSaveAsAccessMode.xlNoChange, oMissing, oMissing, oMissing, oMissing, oMissing);
            excelBook.Close(true, excelFile, oMissing);
            excelApp.Quit();

        }

        private void LogInfo(string strMethod, StringWriter requestStrWriter, StringWriter responseStrWriter)
        {
            StringBuilder sbRequest = requestStrWriter.GetStringBuilder();
            StringBuilder sbResponse = responseStrWriter.GetStringBuilder();

            excelSheet.Cells[excelRow, 1] = strMethod;
            excelSheet.Cells[excelRow, 2] = sbRequest.ToString();
            excelSheet.Cells[excelRow, 3] = sbResponse.ToString();
            excelRow++;
        }


        private void LogInfo(string strMethod, Exception ex)
        {
            excelSheet.Cells[excelRow, 1] = strMethod;
            excelSheet.Cells[excelRow, 2] = ex.Message.ToString();
            if (ex.InnerException != null) excelSheet.Cells[excelRow, 3] = ex.InnerException.ToString();
            excelRow++;
        }

        private bool CreateProfile()
        {
            bool retv = false;

            try
            {
                ProfileWS.ProfileServiceService profileWS = new TestJavaWS.ProfileWS.ProfileServiceService();
                ProfileWS.CreateProfileRequest createProfileRequest = new ProfileWS.CreateProfileRequest();
                ProfileWS.CreateProfileResponse createProfileResponse = new ProfileWS.CreateProfileResponse();

                profileWS.Credentials = new NetworkCredential("user1", "user1");
                createProfileRequest.Profile = new TestJavaWS.ProfileWS.UserProfileType();
                createProfileRequest.Profile.@ref = new TestJavaWS.ProfileWS.EntityRecordType();
                createProfileRequest.Profile.@ref.id = "IncidentCommander@core1.saic.com";
                createProfileRequest.Profile.@ref.name = "Core 1 IncidentCommander";
                createProfileRequest.Profile.agreements = new TestJavaWS.ProfileWS.EntityRecordType[1];
                createProfileRequest.Profile.agreements[0] = new TestJavaWS.ProfileWS.EntityRecordType();
                createProfileRequest.Profile.agreements[0].id = "http://tempuri.org";
                createProfileRequest.Profile.agreements[0].name = "prof:name";
                //createProfileRequest.Profile.agreements[0].workProductID = "prof:workProductID";
                createProfileRequest.Profile.entityInfo = new TestJavaWS.ProfileWS.UserProfileTypeEntityInfo();
                TestJavaWS.ProfileWS.PersonType personInfo = new TestJavaWS.ProfileWS.PersonType();
                createProfileRequest.Profile.entityInfo.contactInfo = new TestJavaWS.ProfileWS.ContactInformationType();
                createProfileRequest.Profile.entityInfo.personInfo = new TestJavaWS.ProfileWS.PersonType2();
                createProfileRequest.Profile.entityInfo.personInfo.PersonName = new TestJavaWS.ProfileWS.PersonNameType1[1];
                createProfileRequest.Profile.entityInfo.personInfo.PersonName[0] = new TestJavaWS.ProfileWS.PersonNameType1();
                createProfileRequest.Profile.entityInfo.personInfo.PersonName[0].PersonFullName = new TestJavaWS.ProfileWS.PersonNameTextType[1];
                createProfileRequest.Profile.entityInfo.personInfo.PersonName[0].PersonFullName[0] = new TestJavaWS.ProfileWS.PersonNameTextType();
                createProfileRequest.Profile.entityInfo.personInfo.PersonName[0].PersonFullName[0].Value = "Michael Kolb";
                createProfileRequest.Profile.entityInfo.orgInfo = new TestJavaWS.ProfileWS.OrganizationType2();
                createProfileRequest.Profile.entityInfo.orgInfo.OrganizationName = new TestJavaWS.ProfileWS.TextType[1];
                createProfileRequest.Profile.entityInfo.orgInfo.OrganizationName[0] = new TestJavaWS.ProfileWS.TextType();
                createProfileRequest.Profile.entityInfo.orgInfo.OrganizationName[0].Value = "Fairfax Co. FD";
                createProfileRequest.Profile.interests = new TestJavaWS.ProfileWS.Interest[5];
                createProfileRequest.Profile.interests[0] = new TestJavaWS.ProfileWS.Interest();
                createProfileRequest.Profile.interests[0].TopicExpression = "Incident";
                createProfileRequest.Profile.interests[1] = new TestJavaWS.ProfileWS.Interest();
                createProfileRequest.Profile.interests[1].TopicExpression = "Alert";
                createProfileRequest.Profile.interests[2] = new TestJavaWS.ProfileWS.Interest();
                createProfileRequest.Profile.interests[2].TopicExpression = "MapViewContext";
                createProfileRequest.Profile.interests[3] = new TestJavaWS.ProfileWS.Interest();
                createProfileRequest.Profile.interests[3].TopicExpression = "IncidentCommandSystem";
                createProfileRequest.Profile.interests[4] = new TestJavaWS.ProfileWS.Interest();
                createProfileRequest.Profile.interests[4].TopicExpression = "MultiagencyCoordinationSystem";
                string tmp = createProfileRequest.ToString();

                XmlSerializer requestSerializer = new XmlSerializer(typeof(ProfileWS.CreateProfileRequest));
                XmlSerializer responseSerializer = new XmlSerializer(typeof(ProfileWS.CreateProfileResponse));
                StringWriter requestStrWriter;
                requestStrWriter = new StringWriter();
                StringWriter responseStrWriter;
                responseStrWriter = new StringWriter();

                requestSerializer.Serialize(requestStrWriter, createProfileRequest);
                createProfileResponse = profileWS.CreateProfile(createProfileRequest);
                responseSerializer.Serialize(responseStrWriter, createProfileResponse);
                LogInfo("CreateProfile", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();

                ProfileWS.CreateProfileRequest createProfileRequest2 = new TestJavaWS.ProfileWS.CreateProfileRequest();
                ProfileWS.CreateProfileResponse createProfileResponse2 = new TestJavaWS.ProfileWS.CreateProfileResponse();
                createProfileRequest2.Profile = new TestJavaWS.ProfileWS.UserProfileType();
                createProfileRequest2.Profile.@ref = new TestJavaWS.ProfileWS.EntityRecordType();
                createProfileRequest2.Profile.@ref.id = "EOCHAZMAT@core1.saic.com";
                createProfileRequest2.Profile.@ref.name = "Core 1 EOC HAZMAT Spc";
                createProfileRequest2.Profile.agreements = new TestJavaWS.ProfileWS.EntityRecordType[1];
                createProfileRequest2.Profile.agreements[0] = new TestJavaWS.ProfileWS.EntityRecordType();
                createProfileRequest2.Profile.agreements[0].id = "http://tempuri.org";
                createProfileRequest2.Profile.agreements[0].name = "prof:name";
                //createProfileRequest2.Profile.agreements[0].workProductID = "prof:workProductID";
                createProfileRequest2.Profile.entityInfo = new TestJavaWS.ProfileWS.UserProfileTypeEntityInfo();
                createProfileRequest2.Profile.entityInfo.contactInfo = new TestJavaWS.ProfileWS.ContactInformationType();
                createProfileRequest2.Profile.entityInfo.personInfo = new TestJavaWS.ProfileWS.PersonType2();
                createProfileRequest2.Profile.entityInfo.personInfo.PersonName = new TestJavaWS.ProfileWS.PersonNameType1[1];
                createProfileRequest2.Profile.entityInfo.personInfo.PersonName[0] = new TestJavaWS.ProfileWS.PersonNameType1();
                createProfileRequest2.Profile.entityInfo.personInfo.PersonName[0].PersonFullName = new TestJavaWS.ProfileWS.PersonNameTextType[1];
                createProfileRequest2.Profile.entityInfo.personInfo.PersonName[0].PersonFullName[0] = new TestJavaWS.ProfileWS.PersonNameTextType();
                createProfileRequest2.Profile.entityInfo.personInfo.PersonName[0].PersonFullName[0].Value = "Christopher Lakey";
                createProfileRequest2.Profile.entityInfo.orgInfo = new TestJavaWS.ProfileWS.OrganizationType2();
                createProfileRequest2.Profile.entityInfo.orgInfo.OrganizationName = new TestJavaWS.ProfileWS.TextType[1];
                createProfileRequest2.Profile.entityInfo.orgInfo.OrganizationName[0] = new TestJavaWS.ProfileWS.TextType();
                createProfileRequest2.Profile.entityInfo.orgInfo.OrganizationName[0].Value = "Fairfax Co. EOC";
                createProfileRequest2.Profile.interests = new TestJavaWS.ProfileWS.Interest[3];
                createProfileRequest2.Profile.interests[0] = new TestJavaWS.ProfileWS.Interest();
                createProfileRequest2.Profile.interests[0].TopicExpression = "Incident";
                //createProfileRequest2.Profile.interests[0].MessageContent = new TestJavaWS.ProfileWS.QueryExpressionType();
                //System.Xml.XmlNode[] mynodes = new System.Xml.XmlNode [1];
                //mynodes[0] = new System.Xml.XmlNode();
                //mynodes[0].InnerXml  = "inc:incident[nc:ActivityCategoryText='CBRNE']";
                //createProfileRequest2.Profile.interests[0].MessageContent.Any = mynodes;

                createProfileRequest2.Profile.interests[0].NamespaceMap = new TestJavaWS.ProfileWS.NamespaceMapItemType[3];
                createProfileRequest2.Profile.interests[0].NamespaceMap[0] = new TestJavaWS.ProfileWS.NamespaceMapItemType();
                createProfileRequest2.Profile.interests[0].NamespaceMap[0].prefix = "inc";
                createProfileRequest2.Profile.interests[0].NamespaceMap[0].uri = "http://uicds.dctd.saic.com/schemas/incident";
                createProfileRequest2.Profile.interests[0].NamespaceMap[1] = new TestJavaWS.ProfileWS.NamespaceMapItemType();
                createProfileRequest2.Profile.interests[0].NamespaceMap[1].prefix = "de";
                createProfileRequest2.Profile.interests[0].NamespaceMap[1].uri = "urn:oasis:names:tc:emergency:EDXL:DE:1.0";
                createProfileRequest2.Profile.interests[0].NamespaceMap[2] = new TestJavaWS.ProfileWS.NamespaceMapItemType();
                createProfileRequest2.Profile.interests[0].NamespaceMap[2].prefix = "nc";
                createProfileRequest2.Profile.interests[0].NamespaceMap[2].uri = "http://niem.gov/niem/niem-core/2.0";
                createProfileRequest2.Profile.interests[1] = new TestJavaWS.ProfileWS.Interest();
                createProfileRequest2.Profile.interests[1].TopicExpression = "Alert";
                createProfileRequest2.Profile.interests[2] = new TestJavaWS.ProfileWS.Interest();
                createProfileRequest2.Profile.interests[2].TopicExpression = "MapViewContext";
                createProfileResponse2 = profileWS.CreateProfile(createProfileRequest2);

                requestStrWriter = new StringWriter();
                responseStrWriter = new StringWriter();
                requestSerializer.Serialize(requestStrWriter, createProfileRequest2);
                responseSerializer.Serialize(responseStrWriter, createProfileResponse2);
                LogInfo("CreateProfile", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();

                //    TestIncidentManagementWS();
                ProfileWS.GetProfileRequest getProfileRequest = new TestJavaWS.ProfileWS.GetProfileRequest();
                ProfileWS.GetProfileResponse getProfileResponse = new TestJavaWS.ProfileWS.GetProfileResponse();
                getProfileRequest.entityID = "IncidentCommander@core1.saic.com";
                getProfileResponse = profileWS.GetProfile(getProfileRequest);
                XmlSerializer getProfileRequestSerializer = new XmlSerializer(typeof(ProfileWS.GetProfileRequest));
                XmlSerializer getProfileResponseSerializer = new XmlSerializer(typeof(ProfileWS.GetProfileResponse));

                requestStrWriter = new StringWriter();
                responseStrWriter = new StringWriter();
                getProfileRequestSerializer.Serialize(requestStrWriter, getProfileRequest);
                getProfileResponseSerializer.Serialize(responseStrWriter, getProfileResponse);
                LogInfo("GetProfile", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();


                ProfileWS.GetProfileListRequest getProfileListRequest = new TestJavaWS.ProfileWS.GetProfileListRequest();
                ProfileWS.GetProfileListResponse getProfileListResponse = new TestJavaWS.ProfileWS.GetProfileListResponse();
                getProfileListRequest.queryString = "core1.saic.com";
                getProfileListResponse = profileWS.GetProfileList(getProfileListRequest);

                XmlSerializer getProfileListRequestSerializer = new XmlSerializer(typeof(ProfileWS.GetProfileListRequest));
                XmlSerializer getProfileListResponseSerializer = new XmlSerializer(typeof(ProfileWS.GetProfileListResponse));

                requestStrWriter = new StringWriter();
                responseStrWriter = new StringWriter();
                getProfileListRequestSerializer.Serialize(requestStrWriter, getProfileListRequest);
                getProfileListResponseSerializer.Serialize(responseStrWriter, getProfileListResponse);
                LogInfo("GetProfileList", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();
            }
            catch (Exception e)
            {

            }
            return retv;
        }

        private bool GetProfile()
        {
            bool retv = false;

            try
            {
                ProfileWS.ProfileServiceService profileWS = new TestJavaWS.ProfileWS.ProfileServiceService();
                ProfileWS.GetProfileRequest getProfileRequest = new TestJavaWS.ProfileWS.GetProfileRequest();
                ProfileWS.GetProfileResponse getProfileResponse = new TestJavaWS.ProfileWS.GetProfileResponse();

                profileWS.Credentials = new NetworkCredential("user1", "user1");
                getProfileRequest.entityID = "IncidentCommander@core1.saic.com";
                getProfileResponse = profileWS.GetProfile(getProfileRequest);


                XmlSerializer requestSerializer = new XmlSerializer(typeof(ProfileWS.GetProfileRequest));
                XmlSerializer responseSerializer = new XmlSerializer(typeof(ProfileWS.GetProfileResponse));
                StringWriter requestStrWriter;
                StringWriter responseStrWriter;
                requestStrWriter = new StringWriter();
                responseStrWriter = new StringWriter();
                requestSerializer.Serialize(requestStrWriter, getProfileRequest);
                responseSerializer.Serialize(responseStrWriter, getProfileResponse);
                LogInfo("GetProfile", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();
            }
            catch (Exception e)
            {
                LogInfo("GetProfile", e);
            }
            return retv;
        }

        private bool GetProfileList()
        {
            bool retv = false;

            try
            {
                ProfileWS.ProfileServiceService profileWS = new TestJavaWS.ProfileWS.ProfileServiceService();
                ProfileWS.GetProfileListRequest getProfileListRequest = new TestJavaWS.ProfileWS.GetProfileListRequest();
                ProfileWS.GetProfileListResponse getProfileListResponse = new TestJavaWS.ProfileWS.GetProfileListResponse();

                profileWS.Credentials = new NetworkCredential("user1", "user1");
                getProfileListRequest.queryString = "core1.saic.com";
                getProfileListResponse = profileWS.GetProfileList(getProfileListRequest);

                XmlSerializer getProfileListRequestSerializer = new XmlSerializer(typeof(ProfileWS.GetProfileListRequest));
                XmlSerializer getProfileListResponseSerializer = new XmlSerializer(typeof(ProfileWS.GetProfileListResponse));
                StringWriter requestStrWriter;
                StringWriter responseStrWriter;

                requestStrWriter = new StringWriter();
                responseStrWriter = new StringWriter();
                getProfileListRequestSerializer.Serialize(requestStrWriter, getProfileListRequest);
                getProfileListResponseSerializer.Serialize(responseStrWriter, getProfileListResponse);
                LogInfo("GetProfileList", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();
            }
            catch (Exception e)
            {
                LogInfo("GetProfileList", e);
            }
            return retv;
        }

        private bool DeleteProfile()
        {
            bool retv = false;

            try
            {
                ProfileWS.ProfileServiceService profileWS = new TestJavaWS.ProfileWS.ProfileServiceService();

                profileWS.Credentials = new NetworkCredential("user1", "user1");
                XmlSerializer deleteProfileRequestSerializer = new XmlSerializer(typeof(ProfileWS.DeleteProfileRequest));
                XmlSerializer deleteProfileResponseSerializer = new XmlSerializer(typeof(ProfileWS.DeleteProfileResponse));

                ProfileWS.DeleteProfileRequest deleteProfileRequest = new TestJavaWS.ProfileWS.DeleteProfileRequest();
                ProfileWS.DeleteProfileResponse deleteProfileResponse = new TestJavaWS.ProfileWS.DeleteProfileResponse();
                deleteProfileRequest.entityID = "IncidentCommander@core1.saic.com";
                deleteProfileResponse = profileWS.DeleteProfile(deleteProfileRequest);

                StringWriter requestStrWriter;
                StringWriter responseStrWriter;
                requestStrWriter = new StringWriter();
                responseStrWriter = new StringWriter();

                deleteProfileRequestSerializer.Serialize(requestStrWriter, deleteProfileRequest);
                deleteProfileResponseSerializer.Serialize(responseStrWriter, deleteProfileResponse);
                LogInfo("DeleteProfile", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();


                ProfileWS.DeleteProfileRequest deleteProfileRequest2 = new TestJavaWS.ProfileWS.DeleteProfileRequest();
                ProfileWS.DeleteProfileResponse deleteProfileResponse2 = new TestJavaWS.ProfileWS.DeleteProfileResponse();
                deleteProfileRequest2.entityID = "EOCHAZMAT@core1.saic.com";
                deleteProfileResponse2 = profileWS.DeleteProfile(deleteProfileRequest2);

                requestStrWriter = new StringWriter();
                responseStrWriter = new StringWriter();
                deleteProfileRequestSerializer.Serialize(requestStrWriter, deleteProfileRequest2);
                deleteProfileResponseSerializer.Serialize(responseStrWriter, deleteProfileResponse2);
                LogInfo("DeleteProfile", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();

            }
            catch (Exception e)
            {
                LogInfo("DeleteProfile", e);
            }
            return retv;
        }

           private bool GetIncidentList()
               {
                   bool retv = false;

                   try
                   {
                       DirectoryWS.DirectoryServiceService directoryWS = new TestJavaWS.DirectoryWS.DirectoryServiceService();
                       DirectoryWS.GetUserListRequestType getUserListRequest = new TestJavaWS.DirectoryWS.GetUserListRequestType();
                       DirectoryWS.GetUserListResponseType getUserListResponse = new TestJavaWS.DirectoryWS.GetUserListResponseType();
                       DirectoryWS.GetIncidentListRequestType getIncidentListRequest = new TestJavaWS.DirectoryWS.GetIncidentListRequestType();
                       DirectoryWS.GetIncidentListResponseType getIncidentListResponse = new TestJavaWS.DirectoryWS.GetIncidentListResponseType();

                       XmlSerializer getUserListRequestSerializer = new XmlSerializer(typeof(DirectoryWS.GetUserListRequestType));
                       XmlSerializer getUserListResponseSerializer = new XmlSerializer(typeof(DirectoryWS.GetUserListResponseType));

                       XmlSerializer getIncidentListRequestSerializer = new XmlSerializer(typeof(DirectoryWS.GetIncidentListRequestType));
                       XmlSerializer getIncidentListResponseSerializer = new XmlSerializer(typeof(DirectoryWS.GetIncidentListResponseType));

                       directoryWS.Credentials = new NetworkCredential("user1", "user1");

                       getUserListRequest.coreName = "uicds-test2";
                       getIncidentListResponse = directoryWS.GetIncidentList(getIncidentListRequest);
                       StringWriter requestStrWriter;
                       StringWriter responseStrWriter;
                       requestStrWriter = new StringWriter();
                       responseStrWriter = new StringWriter();

                       getIncidentListRequestSerializer.Serialize(requestStrWriter, getIncidentListRequest);
                       getIncidentListResponseSerializer.Serialize(responseStrWriter, getIncidentListResponse);
                       LogInfo("GetIncidentList", requestStrWriter, responseStrWriter);
                       requestStrWriter.Close();
                       responseStrWriter.Close();
                   }
                   catch (Exception e)
                   {
                       LogInfo("GetIncidentList", e);
                   }

                   return retv;
               }


        
                private bool GetUserList()
                {
                    bool retv = false;

                    try
                    {
                        DirectoryWS.DirectoryServiceService directoryWS = new TestJavaWS.DirectoryWS.DirectoryServiceService();
                        DirectoryWS.GetUserListRequestType getUserListRequest = new TestJavaWS.DirectoryWS.GetUserListRequestType();
                        DirectoryWS.GetUserListResponseType getUserListResponse = new TestJavaWS.DirectoryWS.GetUserListResponseType();
                        XmlSerializer getUserListRequestSerializer = new XmlSerializer(typeof(DirectoryWS.GetUserListRequestType));
                        XmlSerializer getUserListResponseSerializer = new XmlSerializer(typeof(DirectoryWS.GetUserListResponseType));
                        directoryWS.Credentials = new NetworkCredential("user1", "user1");

                        getUserListRequest.coreName = "uicds-test2";
                        getUserListResponse = directoryWS.GetUserList(getUserListRequest);

                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        getUserListRequestSerializer.Serialize(requestStrWriter, getUserListRequest);
                        getUserListResponseSerializer.Serialize(responseStrWriter, getUserListResponse);
                        LogInfo("GetUserList", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();
                    }
                    catch (Exception e)
                    {
                        LogInfo("GetUserList", e);
                    }

                    return retv;
                }
        
        /*
                private bool TestWorkProductWS(TestJavaWS.AlertWS.IdentifierType AlertIdentifer)
                {
                    bool retv = false;

                    try
                    {
                        WorkProductWS.WorkProductServiceService workProductWS = new TestJavaWS.WorkProductWS.WorkProductServiceService();
                        TestJavaWS.WorkProductWS.GetProductRequest getProductRequest = new TestJavaWS.WorkProductWS.GetProductRequest();
                        TestJavaWS.WorkProductWS.GetProductResponse getProductResponse = new TestJavaWS.WorkProductWS.GetProductResponse();

                        workProductWS.Credentials = new NetworkCredential("user1", "user1");
                        getProductRequest.WorkProductIdentification = new TestJavaWS.WorkProductWS.IdentificationType();
                        getProductRequest.WorkProductIdentification.Identifier = new TestJavaWS.WorkProductWS.IdentifierType();
                        getProductRequest.WorkProductIdentification.Identifier.label = "";
                        getProductRequest.WorkProductIdentification.Identifier = new TestJavaWS.WorkProductWS.IdentifierType();
                        getProductRequest.WorkProductIdentification.Identifier.label = AlertIdentifer.label;
                        getProductRequest.WorkProductIdentification.Identifier.Value = AlertIdentifer.Value;
                        getProductResponse = workProductWS.GetProduct(getProductRequest);


                        XmlSerializer workProductRequestSerializer = new XmlSerializer(typeof(TestJavaWS.WorkProductWS.GetProductRequest));
                        XmlSerializer workProductResponseSerializer = new XmlSerializer(typeof(TestJavaWS.WorkProductWS.GetProductResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        workProductRequestSerializer.Serialize(requestStrWriter, getProductRequest);
                        workProductResponseSerializer.Serialize(responseStrWriter, getProductResponse);
                        LogInfo("GetProduct", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();
           
                
                        /*
                        WorkProductWS.WorkProductType publishProductRequest = new TestJavaWS.WorkProductWS.WorkProductType();
               
                        publishProductRequest.EDXLDistribution = new TestJavaWS.WorkProductWS.EDXLDistribution();
                        publishProductRequest.EDXLDistribution.senderID = "user@core1";
                        publishProductRequest.EDXLDistribution.contentObject = new TestJavaWS.WorkProductWS.contentObjectType[1];
                        publishProductRequest.EDXLDistribution.contentObject[0] = new TestJavaWS.WorkProductWS.contentObjectType();
                        publishProductRequest.EDXLDistribution.contentObject[0].contentKeyword = new TestJavaWS.WorkProductWS.valueListType[1];
                        publishProductRequest.EDXLDistribution.contentObject[0].contentKeyword[0] = new TestJavaWS.WorkProductWS.valueListType();
                        publishProductRequest.EDXLDistribution.contentObject[0].contentKeyword[0].valueListUrn = "urn:uicds:workproduct:type";
                        publishProductRequest.EDXLDistribution.contentObject[0].contentKeyword[0].value = new string[1];
                        publishProductRequest.EDXLDistribution.contentObject[0].contentKeyword[0].value[0] = "Alert";
                
                        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                        xmlDoc.LoadXml(@"<alert>
                                <identifier>identifier</identifier>
                                <sender>sender</sender>
                                <sent>2009-02-03T16:09:51.487-05:00
                                </sent>
                                <status>Actual</status>
                                <msgType>Alert</msgType>
                                <scope>Public</scope>
                                <info>
                                    <category>CBRNE</category>
                                    <event>a test alert event</event>
                                    <urgency>Expected</urgency>
                                    <severity>Moderate</severity>
                                    <certainty>Likely</certainty>
                                    <eventCode>
                                        <valueName>name</valueName>
                                        <value>value</value>
                                    </eventCode>
                                    <headline>a test alert headline</headline>
                                </info>
                            </alert>");
        */
        /*
                        WorkProductWS.xmlContentType xmlContent = new TestJavaWS.WorkProductWS.xmlContentType ();
                        xmlContent.embeddedXMLContent = new TestJavaWS.WorkProductWS.anyXMLType[1];
                        System.Xml.XmlElement [] tst = new System.Xml.XmlElement [1];
                        tst[0] = xmlDoc.DocumentElement;
                        xmlContent.embeddedXMLContent[0].Any = tst;
                        publishProductRequest.EDXLDistribution.contentObject[0].Item = xmlContent;
                        publishProductResponse = workProductWS.PublishProduct(publishProductRequest);
        */
        /*
                    }
                    catch (Exception e)
                    {
                        LogInfo("GetProduct", e);
                    }

                    return retv;
                }
        */


           private bool TestNotificationWS()
                {
                    bool retv = false;

                    try
                    {
                        NotificationWS.NotificationServiceService notificationWS = new TestJavaWS.NotificationWS.NotificationServiceService ();
                        NotificationWS.GetMessagesRequest getMessageRequest = new TestJavaWS.NotificationWS.GetMessagesRequest();
                        NotificationWS.GetMessagesResponse getMessageResponse = new TestJavaWS.NotificationWS.GetMessagesResponse ();

                        notificationWS.Credentials = new NetworkCredential("user1", "user1");
                        getMessageRequest.Any = new System.Xml.XmlElement[1];
                        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                        xmlDoc.LoadXml(@"<add:To xmlns:add=""http://www.w3.org/2005/08/addressing"">http://uicds-test2:8080/uicds/ws/EOCHAZMAT@core1.saic.com</add:To>");
                        getMessageRequest.Any[0] = xmlDoc.DocumentElement;
                        getMessageResponse = notificationWS.GetMessages(getMessageRequest);

                        XmlSerializer getMessageRequestSerializer = new XmlSerializer(typeof(NotificationWS.GetMessagesRequest));
                        XmlSerializer getMessageResponseSerializer = new XmlSerializer(typeof(NotificationWS.GetMessagesResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        getMessageRequestSerializer.Serialize(requestStrWriter, getMessageRequest);
                        getMessageResponseSerializer.Serialize(responseStrWriter, getMessageResponse);
                        LogInfo("GetMessage", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();
                    }
                    catch (Exception e)
                    {
                        LogInfo("GetMessage", e);
                    }

                    return retv;
                }


        /*
                private void CancelAlert(TestJavaWS.AlertWS.IdentifierType AlertIdentifer)
                {
                    try
                    {
                        AlertWS.AlertServiceService alertWS = new TestJavaWS.AlertWS.AlertServiceService();
                        TestJavaWS.AlertWS.CancelAlertRequest cancelAlertRequest = new TestJavaWS.AlertWS.CancelAlertRequest();
                        TestJavaWS.AlertWS.CancelAlertResponse cancelAlertResponse = new TestJavaWS.AlertWS.CancelAlertResponse();

                        alertWS.Credentials = new NetworkCredential("user1", "user1");
                        cancelAlertRequest.WorkProductIdentification = alertWorkProductIdentification;
         */
        /*
        cancelAlertRequest.WorkProductIdentification = new TestJavaWS.AlertWS.IdentificationType();
        cancelAlertRequest.WorkProductIdentification.Identifier = new TestJavaWS.AlertWS.IdentifierType();
        cancelAlertRequest.WorkProductIdentification.Identifier.label = "";  //TODO what goes here?
        cancelAlertRequest.WorkProductIdentification.Identifier.label = AlertIdentifer.label;
        cancelAlertRequest.WorkProductIdentification.Identifier.Value = AlertIdentifer.Value;
         */
        /*
                        alertWS.CancelAlert(cancelAlertRequest);
                

                        XmlSerializer alertAdapterTypeSerializer = new XmlSerializer(typeof(AlertWS.CancelAlertRequest));
                        StringWriter requestStrWriter = new StringWriter();
                        StringWriter responseStrWriter = new StringWriter();
                        alertAdapterTypeSerializer.Serialize(requestStrWriter, cancelAlertRequest);
                        responseStrWriter.Write("");
                        LogInfo("CancelAlert", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();
                    }
                    catch (Exception e)
                    {
                        LogInfo("CancelAlert", e);
                    }

                }
        */

        private void GetAlert()
        {
            try
            {
                AlertWS.AlertServiceService alertWS = new TestJavaWS.AlertWS.AlertServiceService();
                TestJavaWS.AlertWS.GetAlertRequest getAlertRequest = new TestJavaWS.AlertWS.GetAlertRequest();
                TestJavaWS.AlertWS.GetAlertResponse getAlertResponse = new TestJavaWS.AlertWS.GetAlertResponse();

                alertWS.Credentials = new NetworkCredential("user1", "user1");
                getAlertRequest.WorkProductIdentification = alertWorkProductIdentification;
                getAlertResponse = alertWS.GetAlert(getAlertRequest);

                XmlSerializer getAlertRequestSerializer = new XmlSerializer(typeof(AlertWS.GetAlertRequest));
                XmlSerializer getAlertResponseSerializer = new XmlSerializer(typeof(AlertWS.GetAlertResponse));
                StringWriter requestStrWriter = new StringWriter();
                StringWriter responseStrWriter = new StringWriter();
                getAlertRequestSerializer.Serialize(requestStrWriter, getAlertRequest);
                getAlertResponseSerializer.Serialize(responseStrWriter, getAlertResponse);
                LogInfo("GetAlert", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();
            }
            catch (Exception e)
            {
                LogInfo("GetAlert", e);
            }

        }

        private void CreateAlert()
        {
            bool retv = false;
            string strRetv = "";
            TestJavaWS.AlertWS.IdentifierType AlertIdentifer = null;

            try
            {
                AlertWS.AlertServiceService alertWS = new TestJavaWS.AlertWS.AlertServiceService();
                AlertWS.CreateAlertRequest createAlertRequest = new AlertWS.CreateAlertRequest();
                AlertWS.CreateAlertResponse createAlertResponse = new AlertWS.CreateAlertResponse();


                alertWS.Credentials = new NetworkCredential("user1", "user1");
                alertWS.PreAuthenticate = true;

                createAlertRequest.alert = new TestJavaWS.AlertWS.alert();
                createAlertRequest.alert.identifier = System.Guid.NewGuid().ToString();
                createAlertRequest.alert.sender = "CAD@core1.saic.com";
                createAlertRequest.alert.sent = new DateTime(2001, 12, 13, 12, 0, 0);
                createAlertRequest.alert.status = TestJavaWS.AlertWS.alertStatus.Actual;
                createAlertRequest.alert.msgType = TestJavaWS.AlertWS.alertMsgType.Alert;
                createAlertRequest.alert.source = "cap:source";
                createAlertRequest.alert.scope = TestJavaWS.AlertWS.alertScope.Public;
                createAlertRequest.alert.restriction = "cap:restriction";
                createAlertRequest.alert.addresses = "Mile Marker 138";
                createAlertRequest.alert.code = new string[1];
                createAlertRequest.alert.code[0] = "cap:code";
                createAlertRequest.alert.note = "cap:note";
                createAlertRequest.alert.references = "cap:references";
                createAlertRequest.alert.incidents = "cap:incidents";
                createAlertRequest.alert.info = new TestJavaWS.AlertWS.alertInfo[1];
                createAlertRequest.alert.info[0] = new TestJavaWS.AlertWS.alertInfo();
                createAlertRequest.alert.info[0].language = "en-US";
                createAlertRequest.alert.info[0].category = new TestJavaWS.AlertWS.alertInfoCategory[1];
                createAlertRequest.alert.info[0].category[0] = TestJavaWS.AlertWS.alertInfoCategory.Transport;
                createAlertRequest.alert.info[0].@event = "cap:event";
                createAlertRequest.alert.info[0].responseType = new TestJavaWS.AlertWS.alertInfoResponseType[1];
                createAlertRequest.alert.info[0].responseType[0] = TestJavaWS.AlertWS.alertInfoResponseType.Shelter;
                createAlertRequest.alert.info[0].urgency = TestJavaWS.AlertWS.alertInfoUrgency.Immediate;
                createAlertRequest.alert.info[0].severity = TestJavaWS.AlertWS.alertInfoSeverity.Extreme;
                createAlertRequest.alert.info[0].certainty = TestJavaWS.AlertWS.alertInfoCertainty.Observed;
                createAlertRequest.alert.info[0].audience = "cap:audience";
                createAlertRequest.alert.info[0].eventCode = new TestJavaWS.AlertWS.alertInfoEventCode[1];
                createAlertRequest.alert.info[0].eventCode[0] = new TestJavaWS.AlertWS.alertInfoEventCode();
                createAlertRequest.alert.info[0].eventCode[0].valueName = "name";
                createAlertRequest.alert.info[0].eventCode[0].value = "value";
                createAlertRequest.alert.info[0].effective = new DateTime(2001, 12, 31, 12, 0, 0);
                createAlertRequest.alert.info[0].onset = new DateTime(2001, 12, 31, 12, 0, 0);
                createAlertRequest.alert.info[0].expires = new DateTime(2001, 12, 31, 12, 0, 0);
                createAlertRequest.alert.info[0].senderName = "Joe Smith";
                createAlertRequest.alert.info[0].headline = "cap:headline";
                createAlertRequest.alert.info[0].description = "cap:description";
                createAlertRequest.alert.info[0].instruction = "cap:instruction";
                createAlertRequest.alert.info[0].web = "http://tempuri.org";
                createAlertRequest.alert.info[0].contact = "cap:contact";
                createAlertRequest.alert.info[0].parameter = new TestJavaWS.AlertWS.alertInfoParameter[1];
                createAlertRequest.alert.info[0].parameter[0] = new TestJavaWS.AlertWS.alertInfoParameter();
                createAlertRequest.alert.info[0].parameter[0].valueName = "cap:valueName";
                createAlertRequest.alert.info[0].parameter[0].value = "cap:value";
                createAlertRequest.alert.info[0].resource = new TestJavaWS.AlertWS.alertInfoResource[1];
                createAlertRequest.alert.info[0].resource[0] = new TestJavaWS.AlertWS.alertInfoResource();
                createAlertRequest.alert.info[0].resource[0].resourceDesc = "cap:resourceDesc";
                createAlertRequest.alert.info[0].resource[0].mimeType = "cap:mimeType";
                createAlertRequest.alert.info[0].resource[0].size = "0";
                createAlertRequest.alert.info[0].resource[0].uri = "http://tempuri.org";
                createAlertRequest.alert.info[0].resource[0].derefUri = "cap:derefUri";
                createAlertRequest.alert.info[0].resource[0].digest = "cap:digest";
                createAlertRequest.alert.info[0].area = new TestJavaWS.AlertWS.alertInfoArea[1];
                createAlertRequest.alert.info[0].area[0] = new TestJavaWS.AlertWS.alertInfoArea();
                createAlertRequest.alert.info[0].area[0].areaDesc = "cap:areaDesc";
                createAlertRequest.alert.info[0].area[0].polygon = new string[1];
                createAlertRequest.alert.info[0].area[0].polygon[0] = "-77.3,38.8 -77.1,38.8 -77.1,39.0 -77.3,39.0 -77.3,38.8";
                //createAlertRequest.alert.info[0].area[0].circle = new string[1];
                //createAlertRequest.alert.info[0].area[0].circle[0] = "cap:circle";
                createAlertRequest.alert.info[0].area[0].geocode = new TestJavaWS.AlertWS.alertInfoAreaGeocode[1];
                createAlertRequest.alert.info[0].area[0].geocode[0] = new TestJavaWS.AlertWS.alertInfoAreaGeocode();
                createAlertRequest.alert.info[0].area[0].geocode[0].valueName = "cap:valueName";
                createAlertRequest.alert.info[0].area[0].geocode[0].value = "cap:value";
                createAlertRequest.alert.info[0].area[0].altitude = "cap:altitude";
                createAlertRequest.alert.info[0].area[0].ceiling = "cap:ceiling";
                //string test = createAlertRequest.ToString();
                createAlertResponse = alertWS.CreateAlert(createAlertRequest);

                //alertIdentifer = createAlertResponse.WorkProductPublicationResponse.WorkProductSummary.WorkProductIdentification.Identifier;
                //alertWorkProductIdentification = createAlertResponse.WorkProductPublicationResponse.WorkProductSummary.WorkProductIdentification;

                AlertWS.IdentificationType alertIdentification = new TestJavaWS.AlertWS.IdentificationType();

                
                /*
                // how to extract the identification structure (need to find it and cast it?)
                AlertWS.PackageMetadataType packageMetadata = createAlertResponse.WorkProductPublicationResponse.WorkProduct.PackageMetadata;
                foreach (Object item in packageMetadata.Items) {
                    if (item is AlertWS.IdentificationType) {
                        System.Diagnostics.Debug.WriteLine("TRUE!");
                    }
                }
                */
                
                

                XmlSerializer alertRequestSerializer = new XmlSerializer(typeof(TestJavaWS.AlertWS.CreateAlertRequest));
                XmlSerializer alertResponseSerializer = new XmlSerializer(typeof(TestJavaWS.AlertWS.CreateAlertResponse));

                StringWriter requestStrWriter = new StringWriter();
                StringWriter responseStrWriter = new StringWriter();
                alertRequestSerializer.Serialize(requestStrWriter, createAlertRequest);
                alertResponseSerializer.Serialize(responseStrWriter, createAlertResponse);
                LogInfo("CreateAlert", requestStrWriter, responseStrWriter);
                requestStrWriter.Close();
                responseStrWriter.Close();
            }
            catch (Exception e)
            {
                LogInfo("CreateAlert", e);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }


        }

        /*
                private bool TestAgreementWS()
                {
                    bool retv = false;

                    try
                    {
                        AgreementWS.AgreementServiceService agreementWS = new TestJavaWS.AgreementWS.AgreementServiceService();
                        AgreementWS.CreateAgreementRequest createAgreementRequest = new TestJavaWS.AgreementWS.CreateAgreementRequest();
                        AgreementWS.CreateAgreementResponse createAgreementResponse = new TestJavaWS.AgreementWS.CreateAgreementResponse();

                        createAgreementRequest.Agreement = new TestJavaWS.AgreementWS.AgreementType();

                    }
                    catch (Exception e)
                    {
                    }

                    return retv;
                }
        */

        /*
                private bool TestLoggingWS()
                {
                    bool retv = false;

                    try
                    {
                        LoggingWS.LoggingServiceService loggingWS = new TestJavaWS.LoggingWS.LoggingServiceService();

                    }
                    catch (Exception e)
                    {
                    }

                    return retv;
                }
        */

        /*
                private bool TestMapWS()
                {
                    bool retv = false;

                    try
                    {
                        MapWS.MapServiceService mapWS = new TestJavaWS.MapWS.MapServiceService();
                        MapWS.SubmitMapRequest submitMapRequest = new TestJavaWS.MapWS.SubmitMapRequest();
                        MapWS.SubmitMapResponse submitMapResponse = new TestJavaWS.MapWS.SubmitMapResponse();
               
                        submitMapRequest.incidentId = "";
                        submitMapRequest.map = new TestJavaWS.MapWS.ViewContextType();

                        submitMapResponse = mapWS.SubmitMap(submitMapRequest);
                    }
                    catch (Exception e)
                    {
                    }

                    return retv;
                }
        */

        private bool UpdateOrganization()
                {
                    bool retv = false;

                    try
                    {
                        IncidentCommandWS.IncidentCommandServiceService ICWS = new TestJavaWS.IncidentCommandWS.IncidentCommandServiceService();
                        TestJavaWS.IncidentCommandWS.UpdateCommandStructureRequest updateCommandRequest = new TestJavaWS.IncidentCommandWS.UpdateCommandStructureRequest();
                        TestJavaWS.IncidentCommandWS.UpdateCommandStructureResponse updateCommandResponse = new TestJavaWS.IncidentCommandWS.UpdateCommandStructureResponse();

                        ICWS.Credentials = new NetworkCredential("user1", "user1");

                
                        updateCommandRequest.organization = new TestJavaWS.IncidentCommandWS.UICDSOrganizationElementType();
                        updateCommandRequest.organization.OrganizationName = "Incident Command";
                        updateCommandRequest.organization.OrganizationType = "Incident Command";
                        updateCommandRequest.organization.PersonInCharge = new TestJavaWS.IncidentCommandWS.UICDSOrganizationPositionType();
                        updateCommandRequest.organization.PersonInCharge.PersonProfileRef = "IncidentCommander@core1.saic.com";
                        updateCommandRequest.organization.PersonInCharge.RoleProfileRef = "Incident Commander";
                        updateCommandRequest.organization.Organization = new TestJavaWS.IncidentCommandWS.UICDSOrganizationElementType[2];
                        updateCommandRequest.organization.Organization[0] = new TestJavaWS.IncidentCommandWS.UICDSOrganizationElementType();
                        updateCommandRequest.organization.Organization[0].OrganizationName = "Operations";
                        updateCommandRequest.organization.Organization[0].OrganizationType = "Section";
                        updateCommandRequest.organization.Organization[0].PersonInCharge = new TestJavaWS.IncidentCommandWS.UICDSOrganizationPositionType();
                        updateCommandRequest.organization.Organization[0].PersonInCharge.RoleProfileRef = "Operations Chief";
                        updateCommandRequest.organization.Organization[0].PersonInCharge.PersonProfileRef = "OpsChief@core1.saic.com";
                        updateCommandRequest.organization.Organization[1] = new TestJavaWS.IncidentCommandWS.UICDSOrganizationElementType();
                        updateCommandRequest.organization.Organization[1].OrganizationName = "HAZMAT";
                        updateCommandRequest.organization.Organization[1].OrganizationType = "Group";
                        updateCommandRequest.organization.Organization[1].PersonInCharge = new TestJavaWS.IncidentCommandWS.UICDSOrganizationPositionType();
                        updateCommandRequest.organization.Organization[1].PersonInCharge.RoleProfileRef = "HAZMAT Group Supervisor";
                        updateCommandRequest.organization.Organization[1].PersonInCharge.PersonProfileRef = "EOCHAZMAT@core1.saic.com";
                        updateCommandResponse = ICWS.UpdateCommandStructure(updateCommandRequest);

                        // WVS - get the identification structure!
                        // IncidentCommandWorkProductIdentification = updateCommandResponse.WorkProductPublicationResponse.WorkProductSummary.WorkProductIdentification;

                        XmlSerializer requestSerializer = new XmlSerializer(typeof(IncidentCommandWS.UpdateCommandStructureRequest));
                        XmlSerializer responseSerializer = new XmlSerializer(typeof(IncidentCommandWS.UpdateCommandStructureResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        requestSerializer.Serialize(requestStrWriter, updateCommandRequest);
                        responseSerializer.Serialize(responseStrWriter, updateCommandResponse);
                        LogInfo("UpdateOrganization", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();
                
                    }
                    catch (Exception e)
                    {
                        LogInfo("UpdateOrganization", e);
                    }

                    return retv;
                }

        /*
                private bool GetOrganization()
                {
                    bool retv = false;

                    try
                    {
                        IncidentCommandWS.IncidentCommandServiceService ICWS = new TestJavaWS.IncidentCommandWS.IncidentCommandServiceService();
                        TestJavaWS.IncidentCommandWS.GetCommandStructureRequest getCommandRequest = new TestJavaWS.IncidentCommandWS.GetCommandStructureRequest();
                        TestJavaWS.IncidentCommandWS.GetCommandStructureResponse getCommandResponse = new TestJavaWS.IncidentCommandWS.GetCommandStructureResponse();

                        ICWS.Credentials = new NetworkCredential("user1", "user1");
                        getCommandRequest.WorkProductIdentification = IncidentCommandWorkProductIdentification;
                        getCommandResponse = ICWS.GetCommandStructure(getCommandRequest);
                

                        XmlSerializer requestSerializer = new XmlSerializer(typeof(IncidentCommandWS.GetCommandStructureRequest));
                        XmlSerializer responseSerializer = new XmlSerializer(typeof(IncidentCommandWS.GetCommandStructureResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        requestSerializer.Serialize(requestStrWriter, getCommandRequest);
                        responseSerializer.Serialize(responseStrWriter, getCommandResponse);
                        LogInfo("GetOrganization", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();

                    }
                    catch (Exception e)
                    {
                        LogInfo("GetOrganization", e);
                    }

                    return retv;
                }
        */

         private bool TestSensorWS()
                {
                    bool retv = false;

                    try
                    {
                        SensorWS.SensorServiceService sensorWS = new TestJavaWS.SensorWS.SensorServiceService();

                        SensorWS.CreateSOIRequest createSOIRequest = new TestJavaWS.SensorWS.CreateSOIRequest();
                        SensorWS.CreateSOIResponse createSOIResponse = new TestJavaWS.SensorWS.CreateSOIResponse();

                        sensorWS.Credentials = new NetworkCredential("user1", "user1");
                        createSOIRequest.SensorObservationInfo = new TestJavaWS.SensorWS.SensorObservationInfo();
                        
                        // WVS - i don't think this is correct anymore...
                        //createSOIRequest.SensorObservationInfo.IncidentID = strIncidentID;
                        
                        createSOIRequest.SensorObservationInfo.sosURN = "http://www.sensortest.com/MySensor";
                        createSOIRequest.SensorObservationInfo.SensorInfo = new TestJavaWS.SensorWS.SensorInfo[1];
                        createSOIRequest.SensorObservationInfo.SensorInfo[0] = new TestJavaWS.SensorWS.SensorInfo();
                        
                        // WVS - not sure what this was...
                        //createSOIRequest.SensorObservationInfo.SensorInfo[0].id = "TRAFFIC_SENSOR_1";

                        createSOIRequest.SensorObservationInfo.SensorInfo[0].name = "Traffic Sensor 1";
                        createSOIRequest.SensorObservationInfo.SensorInfo[0].description = "Test Sensor 1";

                        createSOIResponse = sensorWS.CreateSOI(createSOIRequest);

                        XmlSerializer requestSerializer = new XmlSerializer(typeof(TestJavaWS.SensorWS.CreateSOIRequest));
                        XmlSerializer responseSerializer = new XmlSerializer(typeof(TestJavaWS.SensorWS.CreateSOIResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();

                        requestSerializer.Serialize(requestStrWriter, createSOIRequest);
                        responseSerializer.Serialize(responseStrWriter, createSOIResponse);
                        LogInfo("CreateSOI", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();
                    }
                    catch (Exception e)
                    {
                        LogInfo("CreateSOI", e);
                    }

                    return retv;
                }


        private bool CreateIncidentFromCap()
                {
                    bool retv = false;

                    try
                    {
                        IncidentManagementWS.IncidentManagementServiceService imSrv = new TestJavaWS.IncidentManagementWS.IncidentManagementServiceService();
                        IncidentManagementWS.CreateIncidentFromCapRequest alertAdapterType = new TestJavaWS.IncidentManagementWS.CreateIncidentFromCapRequest();
                        IncidentManagementWS.CreateIncidentFromCapResponse createIncidentFromCapResponse = new TestJavaWS.IncidentManagementWS.CreateIncidentFromCapResponse();

                        imSrv.Credentials = new NetworkCredential("user1", "user1");
                        alertAdapterType.alert = new TestJavaWS.IncidentManagementWS.alert();
                        alertAdapterType.alert.identifier = new string("1".ToCharArray());
                        alertAdapterType.alert.sender = "CAD@core1.saic.com";
                        DateTime sentDate = new DateTime(2008, 12, 25);
                        alertAdapterType.alert.sent = sentDate;
                        alertAdapterType.alert.status = TestJavaWS.IncidentManagementWS.alertStatus.Actual;
                        alertAdapterType.alert.msgType = TestJavaWS.IncidentManagementWS.alertMsgType.Alert;
                        alertAdapterType.alert.source = "source";
                        alertAdapterType.alert.scope = TestJavaWS.IncidentManagementWS.alertScope.Public;
                        alertAdapterType.alert.restriction = "restrictions";
                        alertAdapterType.alert.addresses = "1 Main Street";
                        alertAdapterType.alert.code = new string[1];
                        alertAdapterType.alert.code[0] = "code";
                        alertAdapterType.alert.note = "note";
                        alertAdapterType.alert.references = "refs";
                        alertAdapterType.alert.incidents = "incidents";
                        alertAdapterType.alert.info = new TestJavaWS.IncidentManagementWS.alertInfo[1];
                        alertAdapterType.alert.info[0] = new TestJavaWS.IncidentManagementWS.alertInfo();
                        alertAdapterType.alert.info[0].language = "en-US";
                        alertAdapterType.alert.info[0].category = new TestJavaWS.IncidentManagementWS.alertInfoCategory[1];
                        alertAdapterType.alert.info[0].category[0] = TestJavaWS.IncidentManagementWS.alertInfoCategory.Transport;
                        alertAdapterType.alert.info[0].responseType = new TestJavaWS.IncidentManagementWS.alertInfoResponseType[1];
                        alertAdapterType.alert.info[0].responseType[0] = TestJavaWS.IncidentManagementWS.alertInfoResponseType.Shelter;
                        alertAdapterType.alert.info[0].urgency = TestJavaWS.IncidentManagementWS.alertInfoUrgency.Immediate;
                        alertAdapterType.alert.info[0].severity = TestJavaWS.IncidentManagementWS.alertInfoSeverity.Extreme;
                        alertAdapterType.alert.info[0].certainty = TestJavaWS.IncidentManagementWS.alertInfoCertainty.Observed;
                        alertAdapterType.alert.info[0].audience = "audience";
                        alertAdapterType.alert.info[0].eventCode = new TestJavaWS.IncidentManagementWS.alertInfoEventCode[1];
                        alertAdapterType.alert.info[0].eventCode[0] = new TestJavaWS.IncidentManagementWS.alertInfoEventCode();
                        alertAdapterType.alert.info[0].eventCode[0].valueName = "name";
                        alertAdapterType.alert.info[0].eventCode[0].value = "value";
                        alertAdapterType.alert.info[0].onset = sentDate;
                        alertAdapterType.alert.info[0].effective = sentDate;
                        alertAdapterType.alert.info[0].expires = sentDate;
                        alertAdapterType.alert.info[0].senderName = "Joe Smith";
                        alertAdapterType.alert.info[0].headline = "headline";
                        alertAdapterType.alert.info[0].description = "description";
                        alertAdapterType.alert.info[0].instruction = "instruction";
                        alertAdapterType.alert.info[0].web = "http://tempuri.org";
                        alertAdapterType.alert.info[0].contact = "contact";
                        alertAdapterType.alert.info[0].parameter = new TestJavaWS.IncidentManagementWS.alertInfoParameter[1];
                        alertAdapterType.alert.info[0].parameter[0] = new TestJavaWS.IncidentManagementWS.alertInfoParameter();
                        alertAdapterType.alert.info[0].parameter[0].valueName = "value name";
                        alertAdapterType.alert.info[0].parameter[0].value = "value";
                        alertAdapterType.alert.info[0].resource = new TestJavaWS.IncidentManagementWS.alertInfoResource[1];
                        alertAdapterType.alert.info[0].resource[0] = new TestJavaWS.IncidentManagementWS.alertInfoResource();
                        alertAdapterType.alert.info[0].resource[0].resourceDesc = "resource desc";
                        alertAdapterType.alert.info[0].resource[0].mimeType = "mime type";
                        alertAdapterType.alert.info[0].resource[0].size = "0";
                        alertAdapterType.alert.info[0].resource[0].uri = "http://tempuri.org";
                        alertAdapterType.alert.info[0].resource[0].derefUri = "derefuri";
                        alertAdapterType.alert.info[0].resource[0].digest = "digest";
                        alertAdapterType.alert.info[0].area = new TestJavaWS.IncidentManagementWS.alertInfoArea[1];
                        alertAdapterType.alert.info[0].area[0] = new TestJavaWS.IncidentManagementWS.alertInfoArea();
                        alertAdapterType.alert.info[0].area[0].areaDesc = "areadescr";
                        alertAdapterType.alert.info[0].area[0].polygon = new string[1];
                        alertAdapterType.alert.info[0].area[0].polygon[0] = "polygon";
                        //alertAdapterType.alert.info[0].area[0].circle = new string[1];
                        //alertAdapterType.alert.info[0].area[0].circle[0] = "circle";
                        alertAdapterType.alert.info[0].area[0].geocode = new TestJavaWS.IncidentManagementWS.alertInfoAreaGeocode[1];
                        alertAdapterType.alert.info[0].area[0].geocode[0] = new TestJavaWS.IncidentManagementWS.alertInfoAreaGeocode();
                        alertAdapterType.alert.info[0].area[0].geocode[0].valueName = "value name";
                        alertAdapterType.alert.info[0].area[0].geocode[0].value = "value";
                        alertAdapterType.alert.info[0].area[0].altitude = "altitude";
                        alertAdapterType.alert.info[0].area[0].ceiling = "ceiling";

                        createIncidentFromCapResponse = imSrv.CreateIncidentFromCap(alertAdapterType);
                        
                        // WVS - get the identification structure!
                        //strIncidentID = createIncidentFromCapResponse.WorkProductPublicationResponse.WorkProduct.WorkProductIdentification.Identifier.Value;
               
                        XmlSerializer alertAdapterTypeSerializer = new XmlSerializer(typeof(IncidentManagementWS.CreateIncidentFromCapRequest));
                        XmlSerializer responseSerializer = new XmlSerializer(typeof(IncidentManagementWS.CreateIncidentFromCapResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        alertAdapterTypeSerializer.Serialize(requestStrWriter, alertAdapterType);
                        responseSerializer.Serialize(responseStrWriter, createIncidentFromCapResponse);
                        LogInfo("CreateIncidentFromCap", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();
                
                        retv = true;
                    }
                    catch (Exception e)
                    {
                        LogInfo("CreateIncidentFromCap", e);
                    }

                    return (retv);
                }


               private bool CreateIncident()
                {
                    bool retv = false;

                    try
                    {
                        IncidentManagementWS.IncidentManagementServiceService imSrv = new TestJavaWS.IncidentManagementWS.IncidentManagementServiceService();
                        IncidentManagementWS.CreateIncidentRequest createIncidentRequest = new TestJavaWS.IncidentManagementWS.CreateIncidentRequest();
                        IncidentManagementWS.CreateIncidentResponse createIncidentResponse = new IncidentManagementWS.CreateIncidentResponse();
                
                        imSrv.Credentials = new NetworkCredential("user1", "user1");
                        createIncidentRequest.Incident = PopulateIncident("Incident used in CreateIncident Test");
                        createIncidentResponse = imSrv.CreateIncident(createIncidentRequest);
                        
                        // WVS - get the identification structure!
                        //IncidentManagementWorkProductIdentification = createIncidentResponse.WorkProductPublicationResponse.WorkProduct.WorkProductIdentification;
   
        /*
        XmlSerializer ser = new XmlSerializer(typeof(TestJavaWS.IncidentManagementWS.UICDSIncidentType));
        FileStream fileStream = File.Create(@"c:\test.txt");
        ser.Serialize(fileStream, createIncidentRequest.incident);
        fileStream.Close();
         */
               
                        XmlSerializer requestSerializer = new XmlSerializer(typeof(IncidentManagementWS.CreateIncidentRequest));
                        XmlSerializer responseSerializer = new XmlSerializer(typeof(IncidentManagementWS.CreateIncidentResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        requestSerializer.Serialize(requestStrWriter, createIncidentRequest);
                        responseSerializer.Serialize(responseStrWriter, createIncidentResponse);
                        LogInfo("CreateIncident", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();

                        retv = true;
                    }
                    catch (Exception e)
                    {
                        LogInfo("CreateIncident", e);
                    }

                    return (retv);
                }


        /*
                private bool GetIncident()
                {
                    bool retv = false;

                    try
                    {
                        IncidentManagementWS.IncidentManagementServiceService imSrv = new TestJavaWS.IncidentManagementWS.IncidentManagementServiceService();
                        IncidentManagementWS.GetIncidentRequest getIncidentRequest = new TestJavaWS.IncidentManagementWS.GetIncidentRequest();
                        IncidentManagementWS.GetIncidentResponse getIncidentResponse = new TestJavaWS.IncidentManagementWS.GetIncidentResponse();

                        imSrv.Credentials = new NetworkCredential("user1", "user1");
                        getIncidentRequest.WorkProductIdentification = IncidentManagementWorkProductIdentification;
                        getIncidentResponse = imSrv.GetIncident(getIncidentRequest);
                        IncidentManagementWS.StructuredPayloadType[] structuredPayloads = getIncidentResponse.WorkProduct.StructuredPayload.ToArray();
                        IncidentManagementWS.StructuredPayloadType structuredPayload = structuredPayloads[0];

                        XmlSerializer ser = new XmlSerializer(typeof(TestJavaWS.IncidentManagementWS.UICDSIncidentType));
                        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                        xmlDoc.LoadXml(structuredPayload.Any.OuterXml);
                        System.Xml.XmlNamespaceManager xmlnsManager = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);

                        //Add the namespaces used in books.xml to the XmlNamespaceManager.
                        xmlnsManager.AddNamespace("ns1", "urn:Books");


                        System.Xml.XmlNode ai = xmlDoc.DocumentElement.SelectSingleNode("ActivityIdentification", xmlnsManager);
                        //System.Xml.XmlReader xmlReader;
                        System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xmlDoc.DocumentElement);
                        object obj = ser.Deserialize(reader);
         */

        /*
        FileStream fileStream = File.Open(@"c:\test.txt", FileMode.Open);
        System.Xml.XmlTextReader nn = new System.Xml.XmlTextReader(fileStream);
                
        object obj = ser.Deserialize(nn); 
        */

        /*
                        // Then you just need to cast obj into whatever type it is eg:
                        IncidentManagementWS.IncidentInfoType myObj = (IncidentManagementWS.IncidentInfoType)obj;
                
                        //incident = structuredPayload.Any;
                        XmlSerializer requestSerializer = new XmlSerializer(typeof(IncidentManagementWS.GetIncidentRequest));
                        XmlSerializer responseSerializer = new XmlSerializer(typeof(IncidentManagementWS.GetIncidentResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        requestSerializer.Serialize(requestStrWriter, getIncidentRequest);
                        responseSerializer.Serialize(responseStrWriter, getIncidentResponse);
                        LogInfo("GetIncident", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();

                        retv = true;
                    }
                    catch (Exception e)
                    {
                        LogInfo("GetIncident", e);
                    }

                    return (retv);
                }
        */


        private TestJavaWS.IncidentManagementWS.UICDSIncidentType PopulateIncident(string description)
                {
                    TestJavaWS.IncidentManagementWS.UICDSIncidentType newIncident = new TestJavaWS.IncidentManagementWS.UICDSIncidentType();
            
                    newIncident.ActivityCategoryText = new TestJavaWS.IncidentManagementWS.TextType[1];
                    newIncident.ActivityCategoryText[0] = new TestJavaWS.IncidentManagementWS.TextType();
                    newIncident.ActivityCategoryText[0].Value = "CBRNE";
                    newIncident.ActivityDescriptionText = new TestJavaWS.IncidentManagementWS.TextType[1];
                    newIncident.ActivityDescriptionText[0] = new TestJavaWS.IncidentManagementWS.TextType();
                    newIncident.ActivityDescriptionText[0].Value = description;
                    newIncident.ActivityName = new TestJavaWS.IncidentManagementWS.TextType[1];
                    newIncident.ActivityName[0] = new TestJavaWS.IncidentManagementWS.TextType();
                    newIncident.ActivityName[0].Value = "Flood Incident";
                    newIncident.IncidentLocation = new TestJavaWS.IncidentManagementWS.LocationType1[1];
                    newIncident.IncidentLocation[0] = new TestJavaWS.IncidentManagementWS.LocationType1();
                    newIncident.IncidentLocation[0].LocationArea = new TestJavaWS.IncidentManagementWS.AreaType[1];
                    newIncident.IncidentLocation[0].LocationArea[0] = new TestJavaWS.IncidentManagementWS.AreaType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType[6];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0] = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude = new TestJavaWS.IncidentManagementWS.LatitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeDegreeValue[0].Value = 38;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeMinuteValue[0].Value = 47;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLatitude.LatitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude = new TestJavaWS.IncidentManagementWS.LongitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeDegreeValue[0].Value = -77;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeMinuteValue[0].Value = 17;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[0].GeographicCoordinateLongitude.LongitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1] = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude = new TestJavaWS.IncidentManagementWS.LatitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeDegreeValue[0].Value = 38;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeMinuteValue[0].Value = 47;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLatitude.LatitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude = new TestJavaWS.IncidentManagementWS.LongitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeDegreeValue[0].Value = -77;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeMinuteValue[0].Value = 5;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[1].GeographicCoordinateLongitude.LongitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2] = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude = new TestJavaWS.IncidentManagementWS.LatitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeDegreeValue[0].Value = 39;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeMinuteValue[0].Value = 0;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLatitude.LatitudeSecondValue[0].Value = 0.005m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude = new TestJavaWS.IncidentManagementWS.LongitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeDegreeValue[0].Value = -77;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeMinuteValue[0].Value = 5;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[2].GeographicCoordinateLongitude.LongitudeSecondValue[0].Value = 60.00m;


                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3] = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude = new TestJavaWS.IncidentManagementWS.LatitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeDegreeValue[0].Value = 39;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeMinuteValue[0].Value = 0;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLatitude.LatitudeSecondValue[0].Value = 0.005m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude = new TestJavaWS.IncidentManagementWS.LongitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeDegreeValue[0].Value = -77;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeMinuteValue[0].Value = 17;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[3].GeographicCoordinateLongitude.LongitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4] = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude = new TestJavaWS.IncidentManagementWS.LatitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeDegreeValue[0].Value = 39;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeMinuteValue[0].Value = 17;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLatitude.LatitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude = new TestJavaWS.IncidentManagementWS.LongitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeDegreeValue[0].Value = -77;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeMinuteValue[0].Value = 17;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[4].GeographicCoordinateLongitude.LongitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5] = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude = new TestJavaWS.IncidentManagementWS.LatitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeDegreeValue[0].Value = 38;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeMinuteValue[0].Value = 47;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLatitude.LatitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude = new TestJavaWS.IncidentManagementWS.LongitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeDegreeValue[0].Value = -77;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeMinuteValue[0].Value = 17;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaPolygonGeographicCoordinate[5].GeographicCoordinateLongitude.LongitudeSecondValue[0].Value = 60.00m;


                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion = new TestJavaWS.IncidentManagementWS.CircularRegionType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0] = new TestJavaWS.IncidentManagementWS.CircularRegionType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0] = new TestJavaWS.IncidentManagementWS.TwoDimensionalGeographicCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude = new TestJavaWS.IncidentManagementWS.LatitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LatitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeDegreeValue[0].Value = 38;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeMinuteValue[0].Value = 17;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLatitude.LatitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude = new TestJavaWS.IncidentManagementWS.LongitudeCoordinateType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeDegreeValue = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeDegreeValue[0] = new TestJavaWS.IncidentManagementWS.LongitudeDegreeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeDegreeValue[0].Value = -77;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeMinuteValue = new TestJavaWS.IncidentManagementWS.AngularMinuteType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeMinuteValue[0] = new TestJavaWS.IncidentManagementWS.AngularMinuteType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeMinuteValue[0].Value = 17;
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeSecondValue = new TestJavaWS.IncidentManagementWS.AngularSecondType[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeSecondValue[0] = new TestJavaWS.IncidentManagementWS.AngularSecondType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0].GeographicCoordinateLongitude.LongitudeSecondValue[0].Value = 60.00m;

                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionRadiusLengthMeasure = new TestJavaWS.IncidentManagementWS.LengthMeasureType2[1];
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionRadiusLengthMeasure[0] = new TestJavaWS.IncidentManagementWS.LengthMeasureType2();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionRadiusLengthMeasure[0].LengthUnitCode = new TestJavaWS.IncidentManagementWS.LengthCodeType();
                    newIncident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionRadiusLengthMeasure[0].LengthUnitCode.Value = "1.23";

                    return newIncident;
                }

        /*
                private bool UpdateIncident()
                {
                    bool retv = false;

                    try
                    {
                        IncidentManagementWS.IncidentManagementServiceService imSrv = new TestJavaWS.IncidentManagementWS.IncidentManagementServiceService();
                        IncidentManagementWS.UpdateIncidentRequest updateIncidentRequest = new TestJavaWS.IncidentManagementWS.UpdateIncidentRequest();
                        IncidentManagementWS.UpdateIncidentResponse updateIncidentResponse = new TestJavaWS.IncidentManagementWS.UpdateIncidentResponse();

                        imSrv.Credentials = new NetworkCredential("user1", "user1");
                        updateIncidentRequest.WorkProductIdentification = IncidentManagementWorkProductIdentification;
                        updateIncidentRequest.incident = PopulateIncident("Incident used in UpdateIncident Test");
                        updateIncidentResponse = imSrv.UpdateIncident(updateIncidentRequest);

                        XmlSerializer requestSerializer = new XmlSerializer(typeof(IncidentManagementWS.UpdateIncidentRequest));
                        XmlSerializer responseSerializer = new XmlSerializer(typeof(IncidentManagementWS.UpdateIncidentResponse));
                        StringWriter requestStrWriter;
                        StringWriter responseStrWriter;
                        requestStrWriter = new StringWriter();
                        responseStrWriter = new StringWriter();
                        requestSerializer.Serialize(requestStrWriter, updateIncidentRequest);
                        responseSerializer.Serialize(responseStrWriter, updateIncidentResponse);
                        LogInfo("UpdateIncident", requestStrWriter, responseStrWriter);
                        requestStrWriter.Close();
                        responseStrWriter.Close();

                        retv = true;
                    }
                    catch (Exception e)
                    {
                        LogInfo("UpdateIncident", e);
                    }

                    return (retv);
                }
    
            }
        */


        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {

            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                // Always accept

                return true;
            }
        }

    }
}