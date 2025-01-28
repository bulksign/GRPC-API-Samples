using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetEnvelopeStatus
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
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

			if (result.IsSuccess)
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
