
using BulksignGrpc;
using Google.Protobuf.WellKnownTypes;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class ResetSignatureType
{

	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		try
		{
			ResetSignatureTypeApiModelInput model = new ResetSignatureTypeApiModelInput()
			{
				Authentication = token,

				EnvelopeId = "your_envelope_id",

				//the new type of signature which will be assigned to the recipient
				SignatureType = BulksignGrpc.ResetSignatureType.ClickToSign,

				//we'll specify the recipient by the index (in this sample being the first recipient)
				ByIndex = new FindRecipientByIndexApiModel()
				{
					Index = 1
				}
			};

			NumericResult result = ChannelManager.GetClient().ResetSignatureType(model);

			if (result.IsSuccess)
			{
				Console.WriteLine($"{result.Result} signature fields were changed");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception bex)
		{
			//handle failed request here
			Console.WriteLine($"Exception {bex.Message}");
		}
	}
}