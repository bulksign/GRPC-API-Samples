using System;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class SendEnvelopeDocumentNetworkShare
	{

		public void RunSample()
		{
			BulksignApiClient api = new BulksignApiClient();


			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit ApiKeys.cs and put your own token/email");
				return;
			}

			EnvelopeApiModel envelope = new EnvelopeApiModel();
			envelope.EnvelopeType = EnvelopeTypeApi.Serial;

			envelope.Recipients = new[]
			{
				new RecipientApiModel()
				{
					Email = "test@test.com",
					Index = 1,
					Name = "Test",
					RecipientType = RecipientTypeApi.Signer
				}
			};

			//NOTE : this oly works on the on-premise version
			envelope.Documents = new []
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
				},
			};

			BulksignResult<SendEnvelopeResultApiModel> result = api.SendEnvelope(token, envelope);

			if (result.IsSuccessful)
			{
				Console.WriteLine("Access code for recipient " + result.Response.RecipientAccess[0].RecipientEmail + " is " + result.Response.RecipientAccess[0].AccessCode);
				Console.WriteLine("Envelope id is : " + result.Response.EnvelopeId);
			}
			else
			{
				Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}" );
			}

		}

	}
}
