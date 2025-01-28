using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class StampSignatures
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		GrpcApi.GrpcApiClient client = ChannelManager.GetClient();

		//for this sample we require to define at least 1 signature stamp
		GetSignatureStampsResult stamps = null;

		try
		{
			stamps = client.GetSignatureStamps(token);
		}
		catch (Exception ex)
		{
			//handle failed request
			Console.WriteLine(ex.Message);
			return;
		}

		if (!stamps.IsSuccess)
		{
			Console.WriteLine($"Request failed : ErrorCode '{stamps.ErrorCode}' , Message {stamps.ErrorMessage}");
			return;
		}


		if (stamps.Result.Any() == false)
		{
			Console.WriteLine("This sample requires to have at least 1 signature stamp. ");
			return;
		}


		//load the imprints too
		GetSignatureImprintsResult imprints = client.GetSignatureImprints(token);


		EnvelopeApiModelInput envelope = new EnvelopeApiModelInput();
		envelope.Authentication                  = token;
		envelope.EnvelopeType                    = EnvelopeTypeApi.Serial;
		envelope.ExpirationDays                 = 10;
		envelope.DisableRecipientNotifications = false;

		envelope.Recipients.Add(new RecipientApiModel
		{
			Name          = "Bulksign Test",
			Email         = "contact@bulksign.com",
			Index         = 1,
			RecipientType = RecipientTypeApi.Signer
		});

		envelope.Documents.Add(new DocumentApiModel
		{
			Index    = 1,
			FileName = "singlepage.pdf",
			FileContentByteArray = new FileContentByteArray
			{
				ContentBytes = ConversionUtilities.ConvertToByteString( File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\singlepage.pdf"))
			},
			NewSignatures =
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
						StampName = stamps.Result.FirstOrDefault(),

						//apply an imprint too for this signature
						ImprintName = imprints.Result.FirstOrDefault()
					}
				}
			}
		});

		try
		{

			SendEnvelopeResult result = client.SendEnvelope(envelope);

			if (result.IsSuccess)
			{
				Console.WriteLine("Access code for recipient " + result.Result.RecipientAccess[0].RecipientEmail + " is " + result.Result.RecipientAccess[0].AccessCode);
				Console.WriteLine("EnvelopeId is : " + result.Result.EnvelopeId);
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			//handle failed request
			Console.WriteLine(ex.Message);
		}
	}
}
