using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetSigningImprints
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
			GetSignatureImprintsResult result = ChannelManager.GetClient().GetSignatureImprints(token);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Found {result.Result.Count} signature imprints");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			//handle failed request here
			Console.WriteLine(ex.Message);
		}
	}
}
