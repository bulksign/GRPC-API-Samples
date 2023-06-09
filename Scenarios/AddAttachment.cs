using System;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class AddAttachment
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

			EnvelopeApiModelInput envelope = new EnvelopeApiModelInput();
			envelope.EnvelopeType                    = EnvelopeTypeApi.Serial;
			envelope.DaysUntilExpire                 = 10;
			envelope.DisableSignerEmailNotifications = false;

			envelope.Recipients = new[]
			{
				new RecipientApiModel
				{
					Name          = "Recipient First",
					Email         = "add_email_address_here",
					Index         = 1,
					RecipientType = RecipientTypeApi.Signer
				}
			};

			envelope.Documents = new[]
			{
				new DocumentApiModel
				{
					Index    = 1,
					FileName = "singlepage.pdf",
					FileContentByteArray = new FileContentByteArray
					{
						ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_sample.pdf")
					},
					NewAttachments = new[]
					{
						new NewAttachmentApiModel
						{
							AssignedToRecipientEmail = "add_email_address_here",
							Id                       = "id_picture",
							Text                     = "Please attach your ID scanned image"
						},
						new NewAttachmentApiModel
						{
							AssignedToRecipientEmail = "add_email_address_here",
							Id                       = "id_passport",
							Text                     = "Please attach an image of your passport"
						}
					}
				}
			};

			BulksignResult<SendEnvelopeResultApiModel> result = api.SendEnvelope(token,envelope);

			if (result.IsSuccessful)
			{
				Console.WriteLine("Access code for recipient " + result.Response.RecipientAccess[0].RecipientEmail + " is " + result.Response.RecipientAccess[0].AccessCode);
				Console.WriteLine("EnvelopeId is : " + result.Response.EnvelopeId);
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
	}
}