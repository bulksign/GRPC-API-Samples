using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples
{
	public class GetSharedContacts
	{
		public void RunSample()
		{
			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}


			GetSharedContactsResult result = ChannelManager.GetClient().GetSharedContacts(token);

			if (result.IsSuccessful)
				Console.WriteLine($" {result.Result.Count} contacts found ");
			else
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
		}
	}
}