using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class SingleDocumentApproverAndSigner
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
		envelope.Authentication  = token;
		envelope.EnvelopeType    = EnvelopeTypeApi.Serial;
		envelope.DaysUntilExpire = 10;
		envelope.EmailMessage    = "Please sign this document";
		envelope.EmailSubject    = "Please Bulksign this document";
		envelope.Name            = "Test envelope";

		envelope.Recipients.AddRange(new[]
		{
			new RecipientApiModel
			{
				Name          = "Approver Recipient",
				Email         = "recipient_email_approver@test.com",
				Index         = 1,
				RecipientType = RecipientTypeApi.Approver
			},
			new RecipientApiModel
			{
				Name          = "Bulksign Test",
				Email         = "recipient_email@test.com",
				Index         = 2,
				RecipientType = RecipientTypeApi.Signer
			}
		});

		envelope.Documents.Add(new DocumentApiModel
			{
				Index    = 1,
				FileName = "test.pdf",
				FileContentByteArray = new FileContentByteArray
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
			//handle failed request
			Console.WriteLine(ex.Message);
		}

	}
}
