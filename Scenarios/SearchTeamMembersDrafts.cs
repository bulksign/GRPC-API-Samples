using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples
{
	public class SearchTeamMembersDrafts
	{
		public void RunSample()
		{
			AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit Authentication.cs and set your own API key there");
				return;
			}

			SearchTeamMembersDraftsInput search = new SearchTeamMembersDraftsInput()
			{
				Authentication = token,
				SearchTerm = "test"
			};

			try
			{
				SearchTeamMembersDraftsResult result = ChannelManager.GetClient().SearchTeamMembersDrafts(search);

				if (result.IsSuccessful)
				{
					Console.WriteLine($"Found {result.Result.Count} team member drafts");
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