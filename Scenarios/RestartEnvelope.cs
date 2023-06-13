using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples
{
	public class RestartEnvelopes
	{
		//replace this with your own expired envelope id 
		private const string EXPIRED_ENVELOPE_ID = "000000000000000000000000";

		public void RunSample()
		{
			AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit Authentication.cs and set your own API key there");
				return;
			}

			EnvelopeIdInput id = new EnvelopeIdInput()
			{
				Authentication = token,
				EnvelopeId     = EXPIRED_ENVELOPE_ID
			};

			try
			{
				EmptyResult result = ChannelManager.GetClient().RestartEnvelope(id);

				if (result.IsSuccessful)
				{
					Console.WriteLine("Envelope was successfully restarted");
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
}