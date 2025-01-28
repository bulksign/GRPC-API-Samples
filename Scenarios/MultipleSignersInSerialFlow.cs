using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples
{
	public class MultipleSignersInSerialFlow
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
			envelope.EnvelopeType                    = EnvelopeTypeApi.Serial;
			envelope.ExpirationDays                 = 10;
			envelope.DisableRecipientNotifications = false;
			envelope.Name                            = "Test envelope";

			envelope.Recipients.AddRange( new []
			{
				//the Index property will determine the order in which the recipients will sign the document(s). 

				new RecipientApiModel()
				{
					Name = "Bulksign Test",
					Email = "contact@bulksign.com",
					Index = 1,
					RecipientType = RecipientTypeApi.Signer
				},
				new RecipientApiModel()
				{
					Name = "Bulksign Test",
					Email = "contact@bulksign.com",
					Index = 2,
					RecipientType = RecipientTypeApi.Signer,
				} 
			});

			envelope.Documents.Add(new DocumentApiModel()
				{
					Index = 1,
					FileName = "test.pdf",
					FileContentByteArray = new FileContentByteArray()
					{
						ContentBytes = ConversionUtilities.ConvertToByteString( File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf"))
					}
				} 
			);

			try
			{
				SendEnvelopeResult result = ChannelManager.GetClient().SendEnvelope(envelope);

				Console.WriteLine("Api request is successful: " + result.IsSuccess);

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
				//handle failed request
				Console.WriteLine(ex.Message);
			}
		}
	}
}