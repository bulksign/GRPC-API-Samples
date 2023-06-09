using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples
{
	public class GetEnvelopesToSign
	{
		public void RunSample()
		{
			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}
				
			GetEnvelopesToSignResult result = ChannelManager.GetClient().GetEnvelopesToSign(token);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Found {result.Result.Count} envelopes to sign");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
	}
}