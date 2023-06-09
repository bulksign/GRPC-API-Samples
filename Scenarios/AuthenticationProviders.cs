using System;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
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

			BulksignApiClient api = new BulksignApiClient();

			BulksignResult<AuthenticationProviderResultApiModel[]> result = api.GetAuthenticationProviders(token);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Found {result.Response.Length} authentication providers");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
	}
}