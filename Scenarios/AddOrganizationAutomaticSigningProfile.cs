using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddOrganizationAutomaticSigningProfile
{
		public void RunSample()
		{
			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}

			AutomaticSigningProfileApiModelInput newProfile = new AutomaticSigningProfileApiModelInput();
			newProfile.Name = "My Profile";

			//we'll use the default organization certificate 
			newProfile.CertificateTypeApi = AutomaticSigningProfileCertificateTypeApi.Default;
			
			//set the base64 encoded signature image here
			newProfile.SignatureImageBase64 = "...............";

			//if we want to use a specific imprint for the signature, we have to set the name here
			//you can call GetSignatureImprints here, see sample from SigningImprints.cs
			newProfile.SignatureImprintName = "";

			EmptyResult result = ChannelManager.GetClient().AddOrganizationAutomaticSigningProfile(newProfile);

			if (result.IsSuccessful)
			{
				Console.WriteLine($"Signing profile was successfully created ");
			}
			else
			{
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
}
