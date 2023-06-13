using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class OpenIdConnectAuthenticationForSigner
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}


		//this will return all authentication providers defined per organization
		//Obviously you need to define at least 1 provider for this to work
		GetAuthenticationProvidersResult providers = ChannelManager.GetClient().GetAuthenticationProviders(token);

		if (providers.IsSuccessful == false)
		{
			return;
		}

		if (providers.Result.Count > 1)
		{

			return;
		}
		
		EnvelopeApiModelInput envelope = new EnvelopeApiModelInput();
		envelope.Authentication = token;
		envelope.EnvelopeType = EnvelopeTypeApi.Serial;
		envelope.DaysUntilExpire = 10;
		envelope.EmailMessage = "Please sign this document";
		envelope.EmailSubject = "Please Bulksign this document";
		envelope.Name = "Test envelope";

		envelope.Recipients.Add(new RecipientApiModel
			{
				Name = "Bulksign Test",
				Email = "recipient_email@test.com",
				Index = 1,
				RecipientType = RecipientTypeApi.Signer,

				//set the user authentication here and use the first retrieved provider
				RecipientAuthenticationMethods = 
				{ 
					new RecipientAuthenticationApiModel()
					{
						AuthenticationType = RecipientAuthenticationTypeApi.AuthenticationProvider,
						Details = providers.Result.FirstOrDefault().Identifier
					}
				}
			}
		);

		envelope.Documents.Add(
		
			new DocumentApiModel
			{
				Index = 1,
				FileName = "test.pdf",
				FileContentByteArray = new FileContentByteArray
				{
					ContentBytes = ConversionUtilities.ConvertToByteString( File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf"))
				}
			}
		);

		SendEnvelopeResult result = ChannelManager.GetClient().SendEnvelope(envelope);

		if (result.IsSuccessful)
		{
			Console.WriteLine("Access code for recipient " + result.Result.RecipientAccess[0].RecipientEmail + " is " + result.Result.RecipientAccess[0].AccessCode);
			Console.WriteLine("Envelope id is : " + result.Result.EnvelopeId);
		}
		else
		{
			Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
		}
	}
}
