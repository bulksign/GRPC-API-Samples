using Bulksign.Api;
using Grpc.Net.Client;

namespace GrpcApiSamples;

public class ChannelManager
{
	private static GrpcChannel channel = null;

	//update this to point to the Bulksign GRPC API endpoint
	private const string ENDPOINT_URL = "http://localhost:5000";

	public static GrpcApi.GrpcApiClient GetClient()
	{
		if (channel == null)
		{
			channel = GrpcChannel.ForAddress("");
		}

		return new GrpcApi.GrpcApiClient(channel);

	}
}