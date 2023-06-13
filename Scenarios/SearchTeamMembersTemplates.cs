using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class SearchTeamMembersTemplates
{

	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		SearchTeamMembersTemplatesInput search = new SearchTeamMembersTemplatesInput()
		{
			Authentication = token,
			SearchTerm     = "test"
		};

		try
		{
			SearchTeamMembersTemplatesResult result = ChannelManager.GetClient().SearchTeamMembersTemplates(search);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Found {result.Result.Count} team member templates");
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
