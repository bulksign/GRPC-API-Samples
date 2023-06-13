using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class MultipleSignersConcurrentFlow
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		EnvelopeApiModelInput envelope = new EnvelopeApiModelInput();
		envelope.Authentication = token;
		envelope.EnvelopeType = EnvelopeTypeApi.Concurrent;
		envelope.DaysUntilExpire = 10;
		envelope.DisableSignerEmailNotifications = false;
		envelope.EmailMessage = "Please sign this document";
		envelope.EmailSubject = "Please Bulksign this document";
		envelope.Name = "Test envelope";

		envelope.Recipients.AddRange(new []
		{
			//the Index property will determine the order in which the recipients will sign the document(s). 

			new RecipientApiModel()
			{
				Name = "Bulksign First User",
				Email = "email@email.com",
				RecipientType = RecipientTypeApi.Signer
			},
			new RecipientApiModel()
			{
				Name = "Bulksign Second User",
				Email = "contact@bulksign.com",
				RecipientType = RecipientTypeApi.Signer,
			}
		});

		envelope.Documents.Add(
			new DocumentApiModel()
			{
				Index = 1,
				FileName = "test.pdf",
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes = ConversionUtilities.ConvertToByteString( File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf"))
				}
			}
		);

		try
		{
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
		catch (Exception ex)
		{
			//handle failed requests
			Console.WriteLine(ex.Message);
		}
	}
}
