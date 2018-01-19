using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TypeScriptHTMLApp.crmHelper.Topics
{
    public class CustomizeSysFormsAndPublish
    {
        private void ShowImage()
        {
            CrmQuery qe = new CrmQuery("systemform");
            qe.AddSimpleCondition("type", "2"); //main form
            qe.AddSimpleCondition("objecttypecode", Person.EntityLogicalName.ToLower());
            qe.Attributes = new List<string>() { "formxml" };
            var allSysforms = WebControlUtility.CrmData.RetrieveMultiple<SystemForm>(qe);
            SystemForm ImageAttributeDemoMainForm = allSysforms.First<SystemForm>();

            XDocument ImageAttributeDemoMainFormXml = XDocument.Parse(ImageAttributeDemoMainForm.formxml);
            if (ImageAttributeDemoMainFormXml.Root.Attribute("showImage").Value == "true")
                ImageAttributeDemoMainFormXml.Root.SetAttributeValue("showImage", false);
            else
                ImageAttributeDemoMainFormXml.Root.SetAttributeValue("showImage", true);           //Updating the entity form definition
            ImageAttributeDemoMainForm.formxml = ImageAttributeDemoMainFormXml.ToString();
            WebControlUtility.CrmData.Update(ImageAttributeDemoMainForm);

            OrganizationServiceContext context = new OrganizationServiceContext(WebControlUtility.CrmData.Connection.OrganizationService);
            PublishXmlRequest pxReq1 = new PublishXmlRequest { ParameterXml = String.Format(@"
               <importexportxml>
                <entities>
                 <entity>{0}</entity>
                </entities>
               </importexportxml>", Person.EntityLogicalName.ToLower()) };
            context.Execute(pxReq1);

        }
    }
}