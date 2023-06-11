using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

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

			EnvelopeIdInput id = new EnvelopeIdInput()
			{
				Authentication = token,
				EnvelopeId = "your_envelope_id"
			};

			try
			{
				DownloadEnvelopeCompletedDocumentsResult result = ChannelManager.GetClient().DownloadEnvelopeCompletedDocuments(id);

				if (result.IsSuccessful)
				{
					//convert the result to byte[]
					byte[] zipFile = ConversionUtilities.ConvertToByteArray(result.Result);

					Console.WriteLine($"File size :  {zipFile.Length}");
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
