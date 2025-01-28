using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class DownloadEnvelopeAttachments
{
	public void RunSample()
	{
		const string ENVELOPE_ID = "your_envelope_id";


		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		try
		{

			EnvelopeIdInput ei = new EnvelopeIdInput()
			{
				Authentication = token,
				EnvelopeId     = ENVELOPE_ID
			};

			DownloadEnvelopeCompletedAttachmentsResult  result = ChannelManager.GetClient().DownloadEnvelopeCompletedAttachments(ei);

			if (result.IsSuccess)
			{
				//the result here will by a byte[] of a zip file which contains ALL PDF attachments from all the envelope documents
				Console.WriteLine($"File size :  {result.Result.Length}");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception bex)
		{
			//handle failed request here. See
			Console.WriteLine($"Exception {bex.Message}");
		}
	}
}