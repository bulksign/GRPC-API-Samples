using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class CancelEnvelopes
{
	//replace this with your own "InProgress" envelope which will be canceled 
	const string ENVELOPE_ID = "000000000000000000000000";

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
			EnvelopeId = ENVELOPE_ID
		};

		try
		{
			//this works only for "InProgress" envelopes
			EmptyResult result = ChannelManager.GetClient().CancelEnvelope(id);

			if (result.IsSuccess)
			{
				Console.WriteLine($"Envelope was canceled");
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
