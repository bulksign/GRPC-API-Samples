using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples
{
	public class Version
	{
		public void RunSample()
		{
			VersionResult version = ChannelManager.GetClient().GetVersion(new EmptyInput());

			Console.WriteLine($"Bulksign version : {version.Version}");
		}
	}
}