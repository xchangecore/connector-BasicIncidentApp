using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UICDS_async
{
    class IncidentCommandStructure : WorkProduct
    {
        static public String TYPE = "ICS";

        public IncidentCommandStructure(WorkProduct workProduct) 
        {
            lastUpdateStatus = workProduct.GetLastUpdateStatus();
            workProductIdentification = workProduct.GetWorkProductIdentification();
            this.workProduct = workProduct.GetWorkProduct();
            this.structuredPayload = workProduct.GetLastFullStructuredPayload();
            type = TYPE;
        }

         public IncidentCommandStructure(XElement getProductResponse) : base(getProductResponse)
        {
        }

         public IncidentCommandStructure(XElement getProductResponse, String status)
             : base(getProductResponse)
         {
         }

        // Set the incident commander name
        public void SetIncidentCommander(String commanderName) {
            XElement ics = GetPayload(ServiceProxy.organizationNS + "OrganizationElement");
            if (ics != null)
            {
                //System.Diagnostics.Debug.WriteLine(ics.Element(ServiceProxy.organizationNS + "PersonInCharge").Value);
                XElement personInCharge = ics.Element(ServiceProxy.organizationNS + "PersonInCharge");
                if (personInCharge != null)
                {
                    XElement roleProfileRef = personInCharge.Element(ServiceProxy.organizationNS + "RoleProfileRef");
                    if (roleProfileRef.Value.Equals("Incident Commander")) {
                        XElement personProfileRef = personInCharge.Element(ServiceProxy.organizationNS + "PersonProfileRef");
                        if (personProfileRef == null)
                        {
                            personInCharge.Add(new XElement(ServiceProxy.organizationNS + "PersonProfileRef", commanderName));
                        }
                        else
                        {
                            personProfileRef.Value = commanderName;
                        }
                    }
                }
            }
        }

        private XElement GetOrganizationElementPayload()
        {
            XElement payload = GetPayload(ServiceProxy.organizationNS + "OrganizationElement");
            if (payload == null) throw new NullReferenceException("No payload found");
            return payload;
        }

        public XElement GetOrganziationElement(string parentName, string parentType)
        {
            XElement payload = GetOrganizationElementPayload();
            try
            {
                // Find all OrganizationElements regardless of level 
                var elements = from e in payload.DescendantsAndSelf(ServiceProxy.organizationNS + "OrganizationElement")
                               where (e.Element(ServiceProxy.organizationNS + "OrganizationName").Value == parentName &&
                                      e.Element(ServiceProxy.organizationNS + "OrganizationType").Value == parentType)
                               select e;

                //foreach (var b in elements)
                //{
                //    System.Diagnostics.Debug.WriteLine("Found element: " + b);
                //}
                if (elements.Count() == 1)
                {
                    return elements.ElementAt(0);
                }
                else
                {
                    if (elements.Count() == 0) {
                        throw new ArgumentException("no elements found for: " + parentName + "/" + parentType);
                    }
                    else 
                    {
                        throw new ArgumentException("duplicate organization elements found for " + parentName + "/" + parentType);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                throw new ArgumentException(parentName + " organization element is not found", ex);
            }
        }

        public XElement GetStaffElement(string parentName, string parentType, string roleName, string personName)
        {
            XElement parentElement = GetOrganziationElement(parentName, parentType);
            try
            {
                // Find all matching Staff elements
                var elements = from e in parentElement.DescendantsAndSelf(ServiceProxy.organizationNS + "Staff")
                               where (e.Element(ServiceProxy.organizationNS + "RoleProfileRef").Value == roleName &&
                                      e.Element(ServiceProxy.organizationNS + "PersonProfileRef").Value == personName)
                               select e;

                if (elements.Count() == 1)
                {
                    return elements.ElementAt(0);
                }
                else
                {
                    if (elements.Count() == 0)
                    {
                        throw new ArgumentException("no staff elements found for: " + personName + "/" + roleName);
                    }
                    else
                    {
                        throw new ArgumentException("duplicate organization elements found for " + personName + "/" + roleName);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                throw new ArgumentException(personName + "/" + roleName + " element is not found", ex);
            }
        }

        public void AddSubordinateOrganziationElement(string parentName, string parentType, string orgName,
            string orgType, string personInChargeRole, string personInChargeName)
        {
            XElement parentElement = GetOrganziationElement(parentName, parentType);
            XElement element = new XElement(ServiceProxy.organizationNS + "OrganizationElement",
                new XElement(ServiceProxy.organizationNS + "OrganizationName", orgName),
                new XElement(ServiceProxy.organizationNS + "OrganizationType", orgType),
                new XElement(ServiceProxy.organizationNS + "PersonInCharge",
                    new XElement(ServiceProxy.organizationNS + "RoleProfileRef", personInChargeRole),
                    new XElement(ServiceProxy.organizationNS + "PersonProfileRef", personInChargeName)
                )
            );

            parentElement.Add(element);
        }

        public void UpdateOrganizationPersonInCharge(string organizationName, string organizationType, string newPersonInChargeRole,
            string newPersonInChargeName)
        {
            XElement parentElement = GetOrganziationElement(organizationName, organizationType);
            if (newPersonInChargeRole != null)
            {
                XElement personInChargeElement = parentElement.Element(ServiceProxy.organizationNS + "PersonInCharge");
                if (personInChargeElement != null)
                {
                    XElement role = personInChargeElement.Element(ServiceProxy.organizationNS + "RoleProfileRef");
                    if (role != null)
                    {
                        role.Value = newPersonInChargeRole;
                    }
                    else
                    {
                        personInChargeElement.Add(new XElement(ServiceProxy.organizationNS + "RoleProfileRef", newPersonInChargeRole));
                    }
                }
                else
                {
                    throw new ArgumentException(organizationName + "/" + organizationType + " does not contain a PersonInCharge element");
                }
            }
            if (newPersonInChargeName != null) {
                XElement personInChargeElement = parentElement.Element(ServiceProxy.organizationNS + "PersonInCharge");
                if (personInChargeElement != null) {
                    XElement name = personInChargeElement.Element(ServiceProxy.organizationNS + "PersonProfileRef");
                    if (name != null)
                    {
                        name.Value = newPersonInChargeName;
                    }
                    else
                    {
                        personInChargeElement.Add(new XElement(ServiceProxy.organizationNS + "PersonProfileRef", newPersonInChargeName));
                    }
                }
                else
                {
                    throw new ArgumentException(organizationName + "/" + organizationType + " does not contain a PersonInCharge element");
                }
            }
        }

        // Returns a copy of the removed element
        public XElement RemoveOrganizationElement(string name, string type)
        {
            XElement element = GetOrganziationElement(name, type);
            XElement elementCopy = new XElement(element);
            element.Remove();
            return elementCopy;
        }

        public void AddStaff(string parentName, string parentType, string roleName, string personName)
        {
            XElement parentElement = GetOrganziationElement(parentName, parentType);
            XElement element = new XElement(ServiceProxy.organizationNS + "Staff",
                   new XElement(ServiceProxy.organizationNS + "RoleProfileRef", roleName),
                   new XElement(ServiceProxy.organizationNS + "PersonProfileRef", personName)
           );

            parentElement.Add(element);
        }

        public void RemoveStaff(string parentName, string parentType, string roleName, string personName)
        {
            XElement staffElement = GetStaffElement(parentName, parentType, roleName, personName);
            staffElement.Remove();
        }

        // Get an UpdateCommandStructureRequest mesage based on the values of this object
        public XElement GetUpdateRequest()
        {
            XElement updateICS = UpdateICSRequest();
            return updateICS;
        }

        private XElement UpdateICSRequest()
        {
            XElement request = new XElement(ServiceProxy.icsNS + "UpdateCommandStructureRequest",
                GetWorkProductIdentification(),
                GetPayload(ServiceProxy.organizationNS + "OrganizationElement")
            );
            return request;
        }

        public bool processUpdateICSResponse(string response)
        {
            return processWorkProductProcessingResponse(response);
        }

    }
}
