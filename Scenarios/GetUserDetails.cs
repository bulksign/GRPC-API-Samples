using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetUserDetails
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

			GetUserDetailsResult result = ChannelManager.GetClient().GetUserDetails(token);

			if (result.IsSuccess == false)
			{
				Console.WriteLine($"Request failed : RequestId {result.RequestId}, ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}
			else
			{
				Console.WriteLine($"User name is : {result.Result.FirstName} {result.Result.LastName}");
			}

		}
		catch (Exception ex)
		{
			//handle failed request
			Console.WriteLine(ex.Message);
		}
	}
}
