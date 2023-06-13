using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class LicenseInformation
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
			GetLicenseResult result = ChannelManager.GetClient().GetLicense(token);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"{result.Result.EnvelopesTotal - result.Result.EnvelopesUsed} remaining envelopes");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			//handle failed requests
			Console.WriteLine(ex.Message);
		}
	}
}
