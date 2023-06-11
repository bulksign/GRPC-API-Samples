using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class LicenseInformation
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
