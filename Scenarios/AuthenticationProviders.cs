using System;
using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AuthenticationProviders
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
			GetAuthenticationProvidersResult result = ChannelManager.GetClient().GetAuthenticationProviders(token);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Found {result.Result.Count} authentication providers");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			//handle failed request here
			Console.WriteLine(ex);
		}

	}
}
