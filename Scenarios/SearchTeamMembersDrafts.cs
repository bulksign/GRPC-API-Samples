using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples
{
	public class SearchTeamMembersDrafts
	{
		public void RunSample()
		{
			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
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