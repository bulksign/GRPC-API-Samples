using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetEnvelopesToSign
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
			GetEnvelopesToSignResult result = ChannelManager.GetClient().GetEnvelopesToSign(token);

			if (result.IsSuccess)
			{
				Console.WriteLine($"Found {result.Result.Count} envelopes to sign");
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
