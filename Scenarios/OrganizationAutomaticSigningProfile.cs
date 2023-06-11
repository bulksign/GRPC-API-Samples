﻿using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class OrganizationAutomaticSigningProfile
{

	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
			return;
		}

		try
		{
			GetOrganizationAutomaticSigningProfilesResult result = ChannelManager.GetClient().GetOrganizationAutomaticSigningProfiles(token);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Found {result.Result.Count} organization automatic signing profiles ");
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
