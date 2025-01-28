using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class SendEnvelopeFromTemplateOverwriteRecipients
{
	public void RunSample()
	{

		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		//replace the identifier with your template Id
		string templateId = "__insert_your_template_id_here__";


		EnvelopeFromTemplateApiModelInput model = new EnvelopeFromTemplateApiModelInput()
		{
			ReplaceRecipients = 
			{
				new TemplateReplaceRecipientApiModel()
				{
					//determine the recipient that we are replacing by specifying the email address
					ByEmail = new FindRecipientByEmailApiModel()
					{
						RecipientEmail = "a@a.com",
						RecipientType  = RecipientTypeApi.Signer
					},
					//specify the information for the new recipient
					Name  = "Test A",
					Email = "myemail@email.com"
				},
				new TemplateReplaceRecipientApiModel()
				{
					ByEmail = new FindRecipientByEmailApiModel()
					{
						RecipientEmail = "b@b.com",
						RecipientType  = RecipientTypeApi.Signer
					},
					Name  = "Test B",
					Email = "myemailbb@email.com"
				}
			},
			TemplateId = templateId
		};


		try
		{

			SendEnvelopeFromTemplateResult result = ChannelManager.GetClient().SendEnvelopeFromTemplate(model);

			if (result.IsSuccess)
			{
				Console.WriteLine("Access code for recipient " + result.Result.RecipientAccess[0].RecipientEmail + " is " + result.Result.RecipientAccess[0].AccessCode);
				Console.WriteLine("EnvelopeId is : " + result.Result.EnvelopeId);
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			//handle failed request
			Console.WriteLine(ex.Message);
		}

	}
}
