using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class ReplaceEnvelopeRecipientSample
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		ReplaceEnvelopeRecipientApiModelInput re = new ReplaceEnvelopeRecipientApiModelInput
		{
			Authentication = token,
			EnvelopeId  = "",
			Name        = "New Recipient Name",
			PhoneNumber = "+000000000000",
			Email       = "new_email_address",

			//here is how to set up password authentication for the new recipient
			RecipientAuthenticationMethods =
			{
				new RecipientAuthenticationApiModel
				{
					AuthenticationType = RecipientAuthenticationTypeApi.Password,
					Details            = "_insert_recipient_password"
				}
			},

			//specifying the recipient to be replace can be done in multiple ways 
			//here is an example by specifying the email of the existing recipient
			ByEmail = new FindRecipientByEmailApiModel
			{
				RecipientEmail = "existing_recipient_email",
				RecipientType  = RecipientTypeApi.Signer
			}
		};

		try
		{
			EmptyResult result = ChannelManager.GetClient().ReplaceEnvelopeRecipient(re);

			if (result.IsSuccessful)
			{
				Console.WriteLine("Recipient has been successfully replaced");
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
