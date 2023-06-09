using System;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class DisableEmailNotifications
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

			//this will disable notifications for this envelope
			envelope.DisableSignerEmailNotifications = true;

			envelope.Recipients = new[]
			{
					new RecipientApiModel()
					{
						Name = "Bulksign Test",
						Email = "contact@bulksign.com",
						Index = 1,
						RecipientType = RecipientTypeApi.Signer
					}
			};

			envelope.Documents = new[] { new DocumentApiModel()
				{
					Index = 1,
					FileName = "test.pdf",
					FileContentByteArray = new FileContentByteArray()
					{
						ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf")
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