using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class CustomDocumentAccess
{
	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
			return;
		}

		EnvelopeApiModelInput envelope = new EnvelopeApiModelInput();
		envelope.Authentication = token;
		envelope.EnvelopeType = EnvelopeTypeApi.Serial;
		envelope.DaysUntilExpire = 10;
		envelope.EmailMessage = "Please sign this document";
		envelope.EmailSubject = "Please Bulksign this document";
		envelope.Name = "Test envelope";

		envelope.Recipients.AddRange(new[]
		{
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
		});

		envelope.Documents.AddRange(new[]
		{
			new DocumentApiModel()
			{
				Index = 1,
				FileName = "bulksign_test_Sample.pdf",
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes = ConversionUtilities.ConvertToByteString(
						File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf"))
				}
			},
			new DocumentApiModel()
			{
				Index = 2,
				FileName = "forms.pdf",
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes =
						ConversionUtilities.ConvertToByteString(
							File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\forms.pdf"))
				}
			}
		});

		envelope.FileAccessMode = FileAccessModeTypeApi.Custom;

		//assign different files to different recipients

		envelope.CustomFileAccess.AddRange(new[]
		{
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
		});

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
			//handle failed request here
			Console.WriteLine(ex.Message);
		}
	}
}
