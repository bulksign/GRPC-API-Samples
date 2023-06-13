using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class UnlockConcurrentRecipientSample
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}
		
		UnlockConcurrentRecipientApiModelInput unlock = new UnlockConcurrentRecipientApiModelInput()
		{
			Authentication = token,
			EnvelopeId     = "000000000000000000000000",
			RecipientEmail = "email_of_recipient_which_locked_signing"
		};


		try
		{
			EmptyResult result = ChannelManager.GetClient().UnlockConcurrentRecipient(unlock);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"{unlock.EnvelopeId} was unlocked");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			//handle failed request
			Console.WriteLine(ex.Message);
		}

	}
}
