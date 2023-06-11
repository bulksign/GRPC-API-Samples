using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetUserContacts
{
	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
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