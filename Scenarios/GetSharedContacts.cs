using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetSharedContacts
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
			GetSharedContactsResult result = ChannelManager.GetClient().GetSharedContacts(token);

			if (result.IsSuccessful)
			{
				Console.WriteLine($" {result.Result.Count} contacts found ");
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
