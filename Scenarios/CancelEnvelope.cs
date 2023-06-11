using System;
using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

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

		EnvelopeIdInput id = new EnvelopeIdInput()
		{
			Authentication = token,
			EnvelopeId = ENVELOPE_ID
		};

		try
		{
			//this works only for "InProgress" envelopes
			EmptyResult result = ChannelManager.GetClient().CancelEnvelope(id);

			if (result.IsSuccessful)
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
