using Bulksign.Api;
using Grpc.Net.Client;

namespace GrpcApiSamples;

public class ChannelManager
{
	//single channel reused for ALL requests, see https://grpc.io/docs/guides/performance/
	private static GrpcChannel channel;

	//update this to point to the Bulksign GRPC API endpoint
	private const string ENDPOINT_URL = "http://localhost:5000";

	public static GrpcApi.GrpcApiClient GetClient()
	{
		if (channel == null)
		{
			channel = GrpcChannel.ForAddress(ENDPOINT_URL);
		}

		return new GrpcApi.GrpcApiClient(channel);
	}
}