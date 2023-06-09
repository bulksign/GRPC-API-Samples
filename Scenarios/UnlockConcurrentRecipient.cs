using System;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class UnlockConcurrentRecipientSample
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

			UnlockConcurrentRecipientApiModel um = new UnlockConcurrentRecipientApiModel
			{
				EnvelopeId     = "000000000000000000000000",
				RecipientEmail = "email_of_recipient_which_locked_signing"
			};

			BulksignResult<string> result = api.UnlockConcurrentRecipient(token,um);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"{um.EnvelopeId} was unlocked");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
	}
}