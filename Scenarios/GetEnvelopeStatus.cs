using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetEnvelopeStatus
{
	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
			return;
		}

		EnvelopeIdInput eid = new EnvelopeIdInput()
		{
			Authentication = token,
			EnvelopeId = "your_envelope_id"
		};

		try
		{
			GetEnvelopeStatusResult result = ChannelManager.GetClient().GetEnvelopeStatus(eid);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Envelope status is {result.Result.ToString()}");
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
