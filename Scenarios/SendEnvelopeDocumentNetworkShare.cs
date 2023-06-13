using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class SendEnvelopeDocumentNetworkShare
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
		envelope.EnvelopeType   = EnvelopeTypeApi.Serial;

		envelope.Recipients.Add(new RecipientApiModel()
		{
			Email         = "test@test.com",
			Index         = 1,
			Name          = "Test",
			RecipientType = RecipientTypeApi.Signer
		});

		//NOTE : specifying as document input a network path ONLY works on the on-premise version of Bulksign
		envelope.Documents.AddRange(new[]
		{
			new DocumentApiModel()
			{
				FileNetworkShare = new FileNetworkShare()
				{
					Path = @"\\DocumentShare\\mydocument.pdf"
				}
			},
			new DocumentApiModel()
			{
				FileNetworkShare = new FileNetworkShare()
				{
					Path = @"\\DocumentShare\\other.pdf"
				}
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
			//handle failed request
			Console.WriteLine(ex.Message);
		}

	}

}

