using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetEnvelopesByStatus
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		GetEnvelopesByStatusInput gs = new GetEnvelopesByStatusInput()
		{
			Authentication = token,
			Status = EnvelopeStatusTypeApi.InProgress
		};

		try
		{
			GetEnvelopesByStatusResult result = ChannelManager.GetClient().GetEnvelopesByStatus(gs);

			if (result.IsSuccess)
			{
				Console.WriteLine($"{result.Result.Count} envelopes found");
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
