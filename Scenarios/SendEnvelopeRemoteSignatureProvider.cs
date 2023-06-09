using System;
using System.Collections.Generic;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{

	//This sample demo shows how to create a signature of type RemoteSignature and configure it
	//To run this, you need to have a RemoteSignature provided registered in Bulksign
	//This samples assumes you are testing this with sample provider https://github.com/bulksign/RemoteSigningProvider

	public class SendEnvelopeRemoteSignatureProvider
	{
		//this is the signature name set in the remote signature provider
		private const string REMOTE_SIGNATURE_NAME = "DemoSignature";

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
			envelope.EnvelopeType                    = EnvelopeTypeApi.Serial;
			envelope.DaysUntilExpire                 = 10;
			envelope.DisableSignerEmailNotifications = false;

			envelope.Recipients = new[]
			{
				new RecipientApiModel
				{
					Name          = "Bulksign Test",
					Email         = "signer_email_address",
					Index         = 1,
					RecipientType = RecipientTypeApi.Signer,

					RemoteSignatureConfiguration = new []
					{
						new RemoteSignatureConfigurationApiModel()
						{
							RemoteSignatureName = REMOTE_SIGNATURE_NAME, 
							//set the configuration data specific to this provider
							Configuration = new Dictionary<string,string>()
							{
								{"FirstKey","11111"},
								{"SecondKey","222"}
							}
						}
					}
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
							Height        = 100,
							Width         = 250,
							PageIndex     = 1,
							Left          = 20,
							Top           = 30,
							SignatureType = SignatureTypeApi.RemoteSignatureProvider,

							//set the RemoteSignature provider identifier
							RemoteSignatureName = REMOTE_SIGNATURE_NAME,
							
							//assign the signature field to the recipient. The assignment is done by the email address
							AssignedToRecipientEmail = envelope.Recipients[0].Email
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