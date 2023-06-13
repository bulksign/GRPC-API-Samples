using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class OrganizationUsers
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
			GetOrganizationUsersResult result = ChannelManager.GetClient().GetOrganizationUsers(token);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Found {result.Result.Count} users ");
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
