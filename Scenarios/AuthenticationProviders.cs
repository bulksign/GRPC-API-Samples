using System;
using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AuthenticationProviders
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
			GetAuthenticationProvidersResult result = ChannelManager.GetClient().GetAuthenticationProviders(token);

			if (result.IsSuccess)
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
