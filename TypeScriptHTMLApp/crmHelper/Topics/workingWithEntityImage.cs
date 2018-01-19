using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TypeScriptHTMLApp.crmHelper.Topics
{
    public class workingWithEntityImage
    {

        void uploadBtn_Click(object sender, EventArgs e)
        {
            byte[] fileData = null;
            if (fileUpload != null && fileUpload.PostedFile.ContentLength > 0)
            {
                DeleteExistingImage();
                using (var binaryReader = new BinaryReader(fileUpload.FileContent))
                    fileData = binaryReader.ReadBytes(fileUpload.PostedFile.ContentLength);

                //----------------------
                //CrmQuery crmQuery = new CrmQuery(Person.EntityLogicalName);
                //crmQuery.AddSimpleCondition(Person.AttributeNames.PersonID, LoggedInUserId.Value.ToString());
                //CoreContact externalAgent = WebControlUtility.CrmData.RetrieveMultiple<Person>(crmQuery).First<Person>();
                //externalAgent.Attributes["entityimage"] = fileData;
                //WebControlUtility.CrmData.Update(externalAgent);

                OrganizationServiceContext context = new OrganizationServiceContext(WebControlUtility.CrmData.Connection.OrganizationService);
                Entity contact = context.CreateQuery(Person.EntityLogicalName).Where(c => c.GetAttributeValue<Guid>(Person.AttributeNames.PersonID)
                .Equals(LoggedInUserId.Value.ToString())).First<Entity>();
                contact.Attributes["entityimage"] = fileData;
                context.UpdateObject(contact);
                try
                { context.SaveChanges(); }
                catch (Exception ex)
                {
                    InvalidFileTypesErrorMessage = "Invalid File, Retty with a valid picture file: " + ex.Message;
                }

                //---------------------------
                WebControlUtility.CrmData.AddAttachmentToEntity(fileUpload.FileContent, fileUpload.PostedFile.ContentLength, fileUpload.PostedFile.ContentType,
                    "prospectimageupload_" + fileUpload.FileName, "", Person.EntityLogicalName, LoggedInUserId.Value);

            }
            CrmQuery query = new CrmQuery(Person.EntityLogicalName);
            query.Attributes = new List<string>() { "entityimage" };
            query.AddSimpleCondition(Person.AttributeNames.PersonID, LoggedInUserId.Value.ToString());
            var attachments = WebControlUtility.CrmData.RetrieveMultiple<Person>(query).ToList();
            byte[] imageBytes = attachments[0].GetAttributeValue<byte[]>("entityimage");
            string imgB = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
            img.ImageUrl = "data:image/png;base64," + imgB;

            ShowImage();
            // this.Context.Response.Redirect(RedirectUrl);
        }
    }
}