﻿using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples
{
	public class SearchEnvelopes
	{
		public void RunSample()
		{
			AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit Authentication.cs and set your own API key there");
				return;
			}
			
			SearchEnvelopesInput search = new SearchEnvelopesInput()
			{
				Authentication       = token,
				SearchTerm           = "test",
				SearchEnvelopeStatus = SearchEnvelopeStatus.Any // will search in ALL envelopes indifferent of status
			};

			try
			{

				SearchEnvelopesResult result = ChannelManager.GetClient().SearchEnvelopes(search);

				if (result.IsSuccess)
				{
					Console.WriteLine($"Found {result.Result.Count} envelopes");
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