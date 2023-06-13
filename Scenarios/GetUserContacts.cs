using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetUserContacts
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
			GetContactsResult result = ChannelManager.GetClient().GetContacts(token);

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
			//handle request failure
			Console.WriteLine(ex.Message);
		}
	}
}