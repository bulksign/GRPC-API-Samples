using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class DocumentTimestamping
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
		envelope.EnvelopeType = EnvelopeTypeApi.Serial;
		envelope.ExpirationDays = 10;
		envelope.Name = "Test envelope";

		envelope.Recipients.Add(new RecipientApiModel()
			{
				Name = "Bulksign Test",
				Email = "contact@bulksign.com",
				Index = 1,
				RecipientType = RecipientTypeApi.Signer
			}
		);

		envelope.Documents.Add(new DocumentApiModel()
			{
				Index = 1,
				FileName = "test.pdf",
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes = ConversionUtilities.ConvertToByteString(
						File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf"))
				}
			}
		);

		//Timestamping must be enabled from Settings\Signing for this to work

		//enable timestamping once per document after all signers have signed the document(s)
		envelope.DocumentTimestampType = DocumentTimestampTypeApi.Envelope;

		//enable timestamping after each signer finishes signing the document(s)
		//envelope.DocumentTimestampType = DocumentTimestampTypeApi.Recipient;

		try
		{
			SendEnvelopeResult result = ChannelManager.GetClient().SendEnvelope(envelope);

			if (result.IsSuccess)
			{
				Console.WriteLine("Access code for recipient " + result.Result.RecipientAccess[0].RecipientEmail + " is " +
				                  result.Result.RecipientAccess[0].AccessCode);
				Console.WriteLine("Envelope id is : " + result.Result.EnvelopeId);
			}
			else
			{
				Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}
		}
		catch (Exception ex)
		{
			//handle failed request here			
			Console.WriteLine(ex.Message);
		}
	}
}
