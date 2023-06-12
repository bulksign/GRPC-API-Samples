using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class SearchTeamMembersEnvelopes
{
	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
			return;
		}

		SearchTeamMembersEnvelopesInput search = new SearchTeamMembersEnvelopesInput()
		{
			Authentication = token,
			SearchTerm     = "test"
		};

		try
		{
			SearchTeamMembersEnvelopesResult result = ChannelManager.GetClient().SearchTeamMembersEnvelopes(search);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Found {result.Result.Count} team member envelopes");
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
