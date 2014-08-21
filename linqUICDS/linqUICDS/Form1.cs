using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace linqUICDS
{

    public partial class Form1 : Form
    {

        // Instantiate Service Endpoint names
        String AlertServiceEndpoint = "http://127.0.0.1/uicds/core/ws/services/AlertService";
        
        // this is where we'll store the identification element 
        XElement ident = null;
        // this is where we'll store the payload element
        XElement payload = null;

        // Set up some namespaces
        //  for XML
        XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        //  for SOAP
        XNamespace env = "http://schemas.xmlsoap.org/soap/envelope/";
        //  for UCore
        XNamespace ulex = "ulex:message:structure:1.0";
        XNamespace ucore = "http://ucore.gov/ucore/2.0";
        XNamespace ddms = "http://metadata.dod.mil/mdr/ns/DDMS/2.0/";
        //  for UICDS Infrastructure
        XNamespace precisb = "http://www.saic.com/precis/2009/06/base";
        XNamespace preciss = "http://www.saic.com/precis/2009/06/structures";
        XNamespace precisp = "http://www.saic.com/precis/2009/06/payloads";
        //  for UICDS Alert Service
        XNamespace alertsvc = "http://uicds.org/AlertService";
        //  for CAP
        XNamespace cap = "urn:oasis:names:tc:emergency:cap:1.1";


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create the createAlertRequest
            XElement createAlertRequest = CreateAlertRequest();

            // Wrap it in SOAP
            XElement message = WrapInSOAP(createAlertRequest);

            // print the request
            textBox1.Text = formatXML(message.ToString());

            // POST it to the core
            NetworkCredential credentials = new NetworkCredential("user1","user1");
            String response = POST(AlertServiceEndpoint, credentials, message.ToString());

            // print the result
            String formattedXML = formatXML(response);

            if (formattedXML != null)
            {
                textBox2.Text = formattedXML;
                
                // Read the identification structure from the document
                XElement responseXML = XElement.Parse(response);
                ident = responseXML.Descendants(preciss + "WorkProductIdentification").ElementAt(0);

                // enable the next button
                button2.Enabled = true;
                button3.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create the getAlertRequest
            XElement getAlertRequest = GetAlertRequest();

            // Wrap it in SOAP
            XElement message = WrapInSOAP(getAlertRequest);

            // print the request
            textBox1.Text = formatXML(message.ToString());

            // set the username and password
            NetworkCredential credentials = new NetworkCredential("user1", "user1");

            // POST it to the core
            String response = POST(AlertServiceEndpoint, credentials, message.ToString());

            // print the result
            textBox2.Text = formatXML(response);

            // Read the payload structure from the document
            XElement responseXML = XElement.Parse(response);
            payload = (XElement) responseXML.Descendants(ulex + "StructuredPayloadMetadata").ElementAt(0).NextNode;

            // enable the next button
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frmPayloadDisplay = new Form2();
            frmPayloadDisplay.setXMLText(payload.GetDefaultNamespace().NamespaceName, formatXML(payload.ToString()));
            frmPayloadDisplay.ShowDialog(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // reset the buttons
            button2.Enabled = false;
            button3.Enabled = false;
            
            // reset the request/response text boxes
            textBox1.Text = "";
            textBox2.Text = "";

            // reset the ident and payload variables
            ident = null;
            payload = null;

        }

        // Wrap an XElement in SOAP 
        public XElement WrapInSOAP(XElement content)
        {
            XElement message = new XElement(env + "Envelope",
                new XElement(env + "Body",
                    new XElement(content)
                )
            );
            return message;
        }

        // Create an alert using LINQ
        public XElement CreateAlertRequest() {

            XElement alertRequest = new XElement(alertsvc + "CreateAlertRequest",
                new XElement(cap + "alert",
                    new XElement(cap +"identifier", Guid.NewGuid().ToString()),
                    new XElement(cap +"sender", "CAD@core1.saic.com"),
                    new XElement(cap + "sent", DateTime.Now),
                    new XElement(cap + "status", "Actual"),
                    new XElement(cap + "msgType", "Alert"),
                    new XElement(cap + "scope", "Public"),
                    new XElement(cap + "incidents", "incident-id"),
                    new XElement(cap + "info",
                        new XElement(cap + "category", "Transport"),
                        new XElement(cap + "event", "event goes here"),
                        new XElement(cap + "urgency", "Immediate"),
                        new XElement(cap + "severity", "Extreme"),
                        new XElement(cap + "certainty", "Observed"),
                        new XElement(cap + "senderName", "Joe McGillicuddy"),
                        new XElement(cap + "headline", "headline goes here"),
                        new XElement(cap + "description", "description goes here")
                    )
                )
            );
            return alertRequest;
        }

        // Get an alert using LINQ
        public XElement GetAlertRequest()
        {
            XElement alertRequest = new XElement(alertsvc + "GetAlertRequest",
                ident
            );
            return alertRequest;
        }


        // convenience method for nicely formatted xml
        public String formatXML(String xml)
        {
            using (StringWriter stringWriter = new StringWriter())
        {
            try {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNodeReader xmlReader = new XmlNodeReader(doc);
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

                // formatting options
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 3;
                xmlWriter.IndentChar = ' ';

                // write the formatted output
                xmlWriter.WriteNode(xmlReader, true);
                return stringWriter.ToString();
            }
            catch (Exception ex) 
            {
                Form2 frmErrorDisplay = new Form2();
                frmErrorDisplay.setXMLText(ex.Message, xml);
                frmErrorDisplay.ShowDialog(this);

                return null;
            }
        }
        }

        // convenience method for performing an HTTP POST
        // method returns "String" which will catch xml response or errors messages
        //    ...should probably return HttpWebResponse Object 
        public String POST(String endpoint, NetworkCredential credentials, String data)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(endpoint);

                byte[] buffer = Encoding.UTF8.GetBytes(data);

                httpRequest.KeepAlive = false;

                httpRequest.Method = "POST";
                httpRequest.Credentials = credentials;
                httpRequest.ContentType = "text/xml";

                httpRequest.ContentLength = buffer.Length;

                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Close();

                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                StreamReader responseReader = new StreamReader(httpResponse.GetResponseStream());
                string response = responseReader.ReadToEnd();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }

        // convenience method for performing an HTTP GET
        // method returns "String" which will catch xml response or errors messages
        //    ...should probably return HttpWebResponse Object
        public String GET(String endpoint, NetworkCredential credentials)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(endpoint);

                httpRequest.Method = "GET";
                httpRequest.Credentials = credentials;

                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                StreamReader responseReader = new StreamReader(httpResponse.GetResponseStream());
                string response = responseReader.ReadToEnd();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }



    }
}
