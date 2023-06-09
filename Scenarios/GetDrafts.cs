using System;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class GetDrafts
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

			BulksignResult<DraftItemResultApiModel[]> result = api.GetDrafts(token);

			//check if the result was successful

			if (result.IsSuccessful == false)
			{
				Console.WriteLine($"Request failed : RequestId {result.RequestId}, ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}
			else
			{
				Console.WriteLine(result.Response.Length + " drafts found");
			}
		}
	}
}