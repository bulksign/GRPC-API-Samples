
using BulksignGrpc;
using Google.Protobuf.Collections;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class CustomDocumentAccess
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
		envelope.EnvelopeType   = EnvelopeTypeApi.Serial;
		envelope.ExpirationDays = 10;
		envelope.Messages.AddRange([
			new MessageApiModel()
			{
				Message = "Please Bulksign this document",
				Subject = "Please sign this document"
			}
		]);
		envelope.Name           = "Test envelope";

		envelope.Recipients.AddRange([
			new RecipientApiModel()
			{
				Name = "Bulksign Test",
				Email = "contact@bulksign.com",
				Index = 1,
				RecipientType = RecipientTypeApi.Signer
			},

			new RecipientApiModel()
			{
				Name = "Second Recipient",
				Email = "second@bulksign.com",
				Index = 1,
				RecipientType = RecipientTypeApi.Signer
			}
		]);

		envelope.Documents.AddRange([
			new DocumentApiModel()
			{
				Index = 1,
				FileName = "bulksign_test_Sample.pdf",
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes = ConversionUtilities.ConvertToByteString(File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf"))
				}
			},
			new DocumentApiModel()
			{
				Index = 2,
				FileName = "forms.pdf",
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes = ConversionUtilities.ConvertToByteString(File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\forms.pdf"))
				}
			}
		]);

		envelope.FileAccessMode = FileAccessModeTypeApi.Custom;

		//assign different files to different recipients

		envelope.CustomFileAccess.AddRange([
			//assign first file to first recipient
			new CustomFileAccessApiModel()
			{
				RecipientEmail = envelope.Recipients[0].Email,
				FileNames = { "bulksign_test_Sample.pdf" }
			},

			//assign first file to first recipient
			new CustomFileAccessApiModel()
			{
				RecipientEmail = envelope.Recipients[1].Email,
				FileNames = { "forms.pdf" }
			}
		]);

		try
		{
			SendEnvelopeResult result = ChannelManager.GetClient().SendEnvelope(envelope);

			if (result.IsSuccess)
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
			//handle failed request here
			Console.WriteLine(ex.Message);
		}
	}
}
