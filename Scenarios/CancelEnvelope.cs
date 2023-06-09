using System;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class CancelEnvelopes
	{
        //replace this with your own "InProgress" envelope which will be canceled 
        const string ENVELOPE_ID = "000000000000000000000000";

		public void RunSample()
		{
			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}

			BulksignApiClient api = new BulksignApiClient();

            //this works only for "InProgress" envelopes
			BulksignResult<string> result = api.CancelEnvelope(token, ENVELOPE_ID);

			if (result.IsSuccessful)
            {
				Console.WriteLine($"Envelope was canceled");
            }
			else
            {
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
            }
		}

	}
}