using System;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class MultipleSignersConcurrentFlow
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
			envelope.EnvelopeType = EnvelopeTypeApi.Concurrent;
			envelope.DaysUntilExpire = 10;
			envelope.DisableSignerEmailNotifications = false;
			envelope.EmailMessage = "Please sign this document";
			envelope.EmailSubject = "Please Bulksign this document";
			envelope.Name = "Test envelope";

			envelope.Recipients = new[]
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
			};

			envelope.Documents = new[]
			{
				new DocumentApiModel()
				{
					Index = 1,
					FileName = "test.pdf",
					FileContentByteArray = new FileContentByteArray()
					{
						ContentBytes    = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf")
					}
				}
			};


			BulksignResult<SendEnvelopeResultApiModel> result = api.SendEnvelope(token, envelope);

			Console.WriteLine("Api request is successful: " + result.IsSuccessful);

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