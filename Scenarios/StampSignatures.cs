using System;
using System.IO;
using System.Linq;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class StampSignatures
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

			BulksignResult<string[]> stamps = api.GetSignatureStamps(token);

			//for this sample we require to define at least 1 signature stamp

			if (stamps.Response.Length == 0)
			{
				Console.WriteLine("This sample requires to have at least 1 signature stamp. ");
				return;
			}

			//load the imprints too
			BulksignResult<string[]> imprints = api.GetSignatureImprints(token);


			EnvelopeApiModel envelope = new EnvelopeApiModel();
			envelope.EnvelopeType                    = EnvelopeTypeApi.Serial;
			envelope.DaysUntilExpire                 = 10;
			envelope.DisableSignerEmailNotifications = false;

			envelope.Recipients = new[]
			{
				new RecipientApiModel
				{
					Name          = "Bulksign Test",
					Email         = "contact@bulksign.com",
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
						ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\singlepage.pdf")
					},
					NewSignatures = new[]
					{
						//see https://bulksign.com/docs/howdoi.htm#is-there-a-easy-way-to-determine-the-position-top-and-left-of-the-new-form-fields-on-the-page-
						//about how to set the signature field at a fixed position

						new NewSignatureApiModel
						{
							//width,height, left and top values are in pixels
							Height                   = 100,
							Width                    = 250,
							PageIndex                = 1,
							Left                     = 20,
							Top                      = 30,
							SignatureType            = SignatureTypeApi.Stamp,
							AssignedToRecipientEmail = envelope.Recipients[0].Email,
							StampSignatureConfiguration = new StampSignatureConfigurationApiModel
							{
								//user must upload the signature image
								StampType = StampSignatureTypeApi.UserProvided
							}
						},
						new NewSignatureApiModel
						{
							Height                   = 100,
							Width                    = 250,
							PageIndex                = 1,
							Left                     = 20,
							Top                      = 70,
							SignatureType            = SignatureTypeApi.Stamp,
							AssignedToRecipientEmail = envelope.Recipients[0].Email,
							StampSignatureConfiguration = new StampSignatureConfigurationApiModel
							{
								//user must upload the signature image
								StampType = StampSignatureTypeApi.PredefinedStamp,

								//we'll use the first stamp defined in the organization
								StampName = stamps.Response.FirstOrDefault(),

								//apply an imprint too for this signature
								ImprintName = imprints.Response.FirstOrDefault()

							}
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