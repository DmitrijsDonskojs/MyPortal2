using System;
using System.Text.RegularExpressions;
using Microsoft.Xrm.Sdk;

namespace ValidationLibrary
{
    public class ContactValidation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            string ErrorString = string.Empty;
            try
            {
                if (executionContext.InputParameters.Contains("Target") && executionContext.InputParameters["Target"] is Entity)
                {
                    Entity contact = (Entity)executionContext.InputParameters["Target"];

                    if (executionContext.MessageName.ToUpper() == "CREATE" || executionContext.MessageName.ToUpper() == "UPDATE")
                    {
                        if (contact.Attributes.Contains("firstname") && !Regex.IsMatch(contact.Attributes["firstname"].ToString(), @"^[\p{L}\p{M}' \.\-]+$"))
                        {
                            ErrorString += "Incorrect Name format! \n";
                        }

                        if (contact.Attributes.Contains("lastname") && !Regex.IsMatch(contact.Attributes["lastname"].ToString(), @"^[\p{L}\p{M}' \.\-]+$"))
                        {
                            ErrorString += "Incorrect Surname format! \n";
                        }

                        if (contact.Attributes.Contains("emailaddress1") && !Regex.IsMatch(contact.Attributes["emailaddress1"].ToString(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                        {
                            ErrorString += "Incorrect E-mail adress format! \n";
                        }

                        if (contact.Attributes.Contains("telephone1") && !Regex.IsMatch(contact.Attributes["telephone1"].ToString(), @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"))
                        {
                            ErrorString += "Incorrect Phone number format! \n";
                        }
                        if (ErrorString != string.Empty)
                        {
                            throw new Exception(ErrorString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(OperationStatus.Canceled, $"Validation Process Failed: \n {ex.Message}");
            }
        }
    }
}
