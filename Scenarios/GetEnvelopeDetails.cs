using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetEnvelopeDetailsSample
{
	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
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

			if (result.IsSuccessful)
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
