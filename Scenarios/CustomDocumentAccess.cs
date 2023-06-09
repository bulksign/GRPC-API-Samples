using System;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
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


			BulksignApiClient api = new BulksignApiClient();

			EnvelopeApiModel envelope = new EnvelopeApiModel();
			envelope.EnvelopeType    = EnvelopeTypeApi.Serial;
			envelope.DaysUntilExpire = 10;
			envelope.EmailMessage    = "Please sign this document";
			envelope.EmailSubject    = "Please Bulksign this document";
			envelope.Name            = "Test envelope";

			envelope.Recipients = new[]
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
			};



			envelope.Documents = new[]
			{
				new DocumentApiModel()
				{
					Index = 1,
					FileName = "bulksign_test_Sample.pdf",
					FileContentByteArray = new FileContentByteArray()
					{
						ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf")
					}
				},
				new DocumentApiModel()
				{
					Index = 2,
					FileName = "forms.pdf",
					FileContentByteArray = new FileContentByteArray()
					{
						ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\forms.pdf")
					}
				}
			};

			envelope.FileAccessMode = FileAccessModeTypeApi.Custom;

			//assign different files to different recipients

			envelope.CustomFileAccess = new[]
			{
				//assign first file to first recipient
				new CustomFileAccessApiModel()
				{
					RecipientEmail = envelope.Recipients[0].Email,
					FileNames = new []{ "bulksign_test_Sample.pdf" }
				},

				//assign first file to first recipient
				new CustomFileAccessApiModel()
				{
					RecipientEmail = envelope.Recipients[1].Email,
					FileNames = new[]
					{
						"forms.pdf"
					}
				}
			};

			BulksignResult<SendEnvelopeResultApiModel> result = api.SendEnvelope(token, envelope);


			if (result.IsSuccessful)
			{
				Console.WriteLine("Access code for recipient " + result.Response.RecipientAccess[0].RecipientEmail + " is " + result.Response.RecipientAccess[0].AccessCode);
				Console.WriteLine("Envelope id is : " + result.Response.EnvelopeId);
			}
			else
			{
				Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}


		}
	}
}