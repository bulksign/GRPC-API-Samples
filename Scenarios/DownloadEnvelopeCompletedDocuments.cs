using System;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class DownloadEnvelopeCompletedDocuments
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

			BulksignResult<byte[]> result = api.DownloadEnvelopeCompletedDocuments(token, "your_envelope_id");

			if (result.IsSuccessful)
			{

				//the result here will by a byte[] of a zip file which contains all signed documents + audit trail file

				Console.WriteLine($"File size :  {result.Response.Length}");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
	}
}