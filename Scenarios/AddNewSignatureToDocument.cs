using Bulksign.Api;
using Google.Protobuf.Collections;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddNewSignatureToDocument
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
			envelope.EnvelopeType                    = EnvelopeTypeApi.Serial;
			envelope.DaysUntilExpire                 = 10;
			envelope.DisableSignerEmailNotifications = false;


			envelope.Recipients.Add(new List<RecipientApiModel>()
			{
				new RecipientApiModel()
				{
					Name = "Bulksign Test",
					Email = "contact@bulksign.com",
					Index = 1,
					RecipientType = RecipientTypeApi.Signer
				}
			});

		
			envelope.Documents.Add(new RepeatedField<DocumentApiModel>()
			{
				new DocumentApiModel()
				{
					Index = 1,
					FileName = "singlepage.pdf",
					FileContentByteArray = new FileContentByteArray()
					{
						ContentBytes = ConversionUtilities.ConvertToByteString(File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\singlepage.pdf"))
					},
					
					NewSignatures = 
					{

						//see https://bulksign.com/docs/howdoi.htm#is-there-a-easy-way-to-determine-the-position-top-and-left-of-the-new-form-fields-on-the-page-
						//about how to set the signature field at a fixed position

						new NewSignatureApiModel()
						{
							//width,height, left and top values are in pixels
							Height = 100,
							Width = 250,
							PageIndex = 1,
							Left = 20,
							Top = 30,
							SignatureType = SignatureTypeApi.ClickToSign,
							//assign the signature field to the recipient. The assignment is done by the email address
							AssignedToRecipientEmail = envelope.Recipients[0].Email
						}
					}
				}, 
			});

			try
			{
				SendEnvelopeResult result = ChannelManager.GetClient().SendEnvelope(envelope);

				if (result.IsSuccessful)
				{
					Console.WriteLine("Access code for recipient " + result.Result.RecipientAccess[0].RecipientEmail + " is " + result.Result.RecipientAccess[0].AccessCode);
					Console.WriteLine("EnvelopeId is : " + result.Result.EnvelopeId);
				}
				else
				{
					Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
				}
			}
			catch(Exception ex)
			{
				//handle failed request	
				Console.WriteLine(ex.Message);
			}

		}
}
