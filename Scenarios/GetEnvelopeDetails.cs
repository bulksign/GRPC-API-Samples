using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetEnvelopeDetailsSample
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		EnvelopeIdInput id = new EnvelopeIdInput()
		{
			Authentication = token,
			EnvelopeId = "__your_own_envelope_id__"
		};

		try
		{
			GetEnvelopeDetailsResult result = ChannelManager.GetClient().GetEnvelopeDetails(id);

			if (result.IsSuccess)
			{
				Console.WriteLine($"Envelope '{id.EnvelopeId}' has name : '{result.Result.Name}' ");
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
