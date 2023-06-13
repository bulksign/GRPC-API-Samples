using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class UpdateOrganizationSettings
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		OrganizationUpdateSettingsApiModelInput newSettings = new OrganizationUpdateSettingsApiModelInput();
		newSettings.Authentication = token;

		//we need to set only the values we want updated
		newSettings.Name           = "New Organization Name";

		try
		{
			EmptyResult result = ChannelManager.GetClient().UpdateOrganizationSettings(newSettings);

			if (result.IsSuccessful)
			{
				Console.WriteLine("Organization settings were successfully updated");
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

