using System;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class SendEnvelopeFromTemplateOverwriteRecipients
	{
		public void RunSample()
		{

			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}
			

			BulksignApiClient api = new BulksignApiClient();

			//replace the identifier with your template Id
			string templateId = "d8a308e8-dd03-ec11-908d-d050997b638e";


			//we are sending 
			BulksignResult<SendEnvelopeResultApiModel> result = api.SendEnvelopeFromTemplate(token, new EnvelopeFromTemplateApiModel()
			{
				ReplaceRecipients = new[]
				{
					new TemplateReplaceRecipientApiModel()
					{
						//determine the recipient that we are replacing by specifying the email address
						ByEmail = new FindRecipientByEmailApiModel()
						{
							RecipientEmail = "a@a.com",
							RecipientType = RecipientTypeApi.Signer
						},
						//specify the information for the new recipient
						Name = "Test A", 
						Email = "myemail@email.com"
					},
					new TemplateReplaceRecipientApiModel()
					{
						ByEmail = new FindRecipientByEmailApiModel()
						{
							RecipientEmail = "b@b.com",
							RecipientType = RecipientTypeApi.Signer
						},
						Name = "Test B", 
						Email = "myemailbb@email.com"
					}
				},
				TemplateId = templateId
			});

			if (result.IsSuccessful)
			{
				Console.WriteLine("Access code for recipient " + result.Response.RecipientAccess[0].RecipientEmail + " is " + result.Response.RecipientAccess[0].AccessCode);
				Console.WriteLine("EnvelopeId is : " + result.Response.EnvelopeId);
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}

		}
	}
}