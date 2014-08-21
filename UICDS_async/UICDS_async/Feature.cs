using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Security;

namespace UICDS_async
{
    class Feature : WorkProduct
    {
        static public String TYPE = "Feature";

        public static String UPDATED_ELEMENT = "updated";
        public static String TITLE_ELEMENT = "title";
        public static String ID_ELEMENT = "id";
        public static XNamespace ATOM_NS = "http://www.w3.org/2005/Atom";
        public static String ENTRY_ELEMENT = "entry";
        public static XNamespace GEORSS_NS = "http://www.georss.org/georss";

        public Feature(WorkProduct workProduct)
        {
            lastUpdateStatus = workProduct.GetLastUpdateStatus();
            workProductIdentification = workProduct.GetWorkProductIdentification();
            this.workProduct = workProduct.GetWorkProduct();
            this.structuredPayload = workProduct.GetLastFullStructuredPayload();
            type = TYPE;
        }


        public Feature(string title, string id, string latitude, string longitude, 
            string updatedTime, Dictionary<string, string> data)
        {
            createFeature(title, id, latitude, longitude, updatedTime, data);
        }

        public void createFeature(string title, string id, string latitude, string longitude,
            string updatedTime, Dictionary<string, string> data)
        {
            XElement htmlContent = createHTMLContent(data);

            XElement packageMetadata = createMetadata();
            digest = createDigest(htmlContent, latitude, longitude);
            structuredPayload = createStructuredPayload(title, id, latitude, longitude, updatedTime, htmlContent);

            workProduct = new XElement(ServiceProxy.precissNS + "WorkProduct",
                packageMetadata, digest, structuredPayload);

            type = TYPE;
        }


        public void updateFeature(string title, string id, string latitude, string longitude,
            string updatedTime, Dictionary<string, string> data)
        {
            // get the current metatdata
            XElement packageMetadata = WorkProductUtilities.GetWorkProductPackageMetadata(workProduct);

            XElement properties = WorkProductUtilities.GetWorkProductProperties(workProduct);

            XElement htmlContent = createHTMLContent(data);

            digest = createDigest(htmlContent, latitude, longitude);
            structuredPayload = createStructuredPayload(title, id, latitude, longitude, updatedTime, htmlContent);

            workProduct = new XElement(ServiceProxy.precissNS + "WorkProduct",
                packageMetadata, digest, structuredPayload);

            type = TYPE;

        }

        private XElement createMetadata()
        {
            XElement metadata = new XElement(ServiceProxy.ulexNS + "PackageMetadata",
                new XElement(ServiceProxy.ulexNS + "DataItemID"),
                new XElement(ServiceProxy.ulexNS + "DataItemReferenceID"),
                new XElement(ServiceProxy.ucoreNS + "DataItemStatus",
                    new XAttribute(ServiceProxy.ucoreNS + "label", "Active")
                ),
                new XElement(ServiceProxy.ulexNS + "DataOwnerMetadata",
                    new XElement(ServiceProxy.ucoreNS + "DataOwnerIdentifier",
                        new XElement(ServiceProxy.ddmsNS + "Organization",
                            new XElement(ServiceProxy.ddmsNS + "name", "")
                        )
                    ),
                    new XElement(ServiceProxy.ucoreNS + "DataOwnerContact",
                        new XElement(ServiceProxy.ddmsNS + "Organization",
                            new XElement(ServiceProxy.ddmsNS + "name", "")
                        )
                    )
                ),
                new XElement(ServiceProxy.ucoreNS + "DisseminationCriteria")
            );
            return metadata;
        }

        private XElement createDigest(XElement htmlContent, string latitude, string longitude)
        {
            XElement digest = new XElement(ServiceProxy.ucoreNS + "Digest",
                new XElement(ServiceProxy.ucoreNS + "DigestMetadata"),
                new XElement(ServiceProxy.ucoreNS + "Event",
                    new XAttribute("id", "EV1"),
                    new XElement(ServiceProxy.ucoreNS + "Descriptor", SecurityElement.Escape(htmlContent.ToString())),
                    new XElement(ServiceProxy.ucoreNS + "Identifier",
                        new XAttribute(ServiceProxy.ucoreNS + "code", "ActivityName"),
                        new XAttribute(ServiceProxy.ucoreNS + "codespace", ServiceProxy.niemCoreNS.ToString()),
                        new XAttribute(ServiceProxy.ucoreNS + "label", "ID"), "Truck ESN# Deployed"),
                    new XElement(ServiceProxy.ucoreNS + "What",
                        new XAttribute(ServiceProxy.ucoreNS + "code", "Cargo"),
                        new XAttribute(ServiceProxy.ucoreNS + "codespace", ServiceProxy.ucoreCodeSpaceNS.ToString())
                    )
                ),
                new XElement(ServiceProxy.ucoreNS + "Location",
                    new XAttribute("id", "Loc1"),
                    new XElement(ServiceProxy.ucoreNS + "GeoLocation",
                        new XElement(ServiceProxy.ucoreNS + "Point",
                            new XElement(ServiceProxy.gmlNS + "Point",
                                new XAttribute(ServiceProxy.gmlNS + "id", "point1"),
                                new XAttribute("srsName","urn:ogc:def:crs:EPSG:6.6:4326"),
                                new XElement(ServiceProxy.gmlNS + "pos", latitude + " " + longitude)
                            )
                        )
                    )
                ),
                new XElement(ServiceProxy.ucoreNS + "LocatedAt",
                    new XAttribute("id", "LA1"),
                    new XElement(ServiceProxy.ucoreNS + "EntityRef",
                        new XAttribute("ref", "EV1")
                    ),
                    new XElement(ServiceProxy.ucoreNS + "LocationRef", 
                        new XAttribute("ref", "Loc1")
                    )
                )
            );

            return digest;
        }

        private XElement createHTMLContent(Dictionary<string, string> data)
        {
            XElement tbody = new XElement("TBODY");

            foreach (KeyValuePair<string, string> pair in data)
            {
                XElement tr = new XElement("TR",
                                new XElement("TD", pair.Key),
                                new XElement("TD", pair.Value)
                            );
                tbody.Add(tr);
            }


            XElement html = new XElement("TABLE",
                new XAttribute("COLS", "2")
            );
            html.Add(tbody);

            return html;
        }



        private XElement createStructuredPayload(string title, string id, string latitude, string longitude, 
            string updatedTime, XElement htmlContent)
        {
            XElement payload = new XElement(ServiceProxy.ulexNS + "StructuredPayload",
                new XElement(ServiceProxy.ulexNS + "StructuredPayloadMetadata",
                    new XElement(ServiceProxy.ulexNS + "CommunityURI", TYPE),
                    new XElement(ServiceProxy.ulexNS + "CommunityDescription"),
                    new XElement(ServiceProxy.ulexNS + "CommunityVersion", "1.0"),
                    new XElement(ServiceProxy.ulexNS + "CommunityPedigreeURI", "1.0",
                        new XAttribute(ServiceProxy.ulexNS + "pedigreeNumber", "1")
                    )
                ),
                getAtomEntry(title, id, updatedTime, latitude, longitude, htmlContent)
            );
            return payload;
        }

        private object getAtomEntry(string title, string id, string updatedTime, 
            string latitude, string longitude, XElement htmlContent)
        {
            XElement atom = new XElement(ATOM_NS + "entry",
                new XElement(ATOM_NS + "title", title),
                new XElement(ATOM_NS + "id", id),
                new XElement(ATOM_NS + "updated", updatedTime),
                new XElement(ATOM_NS + "content",
                    new XAttribute("type", "html"),
                    SecurityElement.Escape(htmlContent.ToString())
                ),
                new XElement(GEORSS_NS + "where",
                    new XElement(ServiceProxy.gmlNS + "Point",
                        new XElement(ServiceProxy.gmlNS + "pos", latitude + " " + longitude)
                    )
                )
            );

            return atom;
        }
    }
}
