using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetEnvelopesByStatus
{
	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
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

			if (result.IsSuccessful)
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
