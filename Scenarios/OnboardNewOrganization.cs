using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class OnboardNewOrganization
{
	//NOTE : The CreateOrganization API is ONLY available for the OnPremise version of Bulksign.
	//It will NOT work on SAAS version at https://bulksign.com

	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		GrpcApi.GrpcApiClient client = ChannelManager.GetClient();

		CreateOrganizationResult  result = client.CreateOrganization(new CreateOrganizationApiModelInput()
		{
			Authentication = token,
			OrganizationName = "MyOrganization",
			AdministratorEmail = "admin@email.com",
			AdministratorFirstName = "FirstName",
			AdministratorLastName = "SecondName",
			AdministratorPassword = "AdminPassword"
		});

		if (result.IsSuccessful == false)
		{
			Console.WriteLine($"Request failed, requestId {result.RequestId}, error {result.ErrorMessage} , code {result.ErrorCode}");
		}


		//make the new requests authenticated 
		AuthenticationApiModel newOrgToken = new AuthenticationApiModel()
		{
			Key = result.Result,
			UserEmail = "admin@email.com"
		};

		//update the org settings with whichever values we require.
		//Note that all properties are optional, you should only set values for the properties that you want updated and ignore the rest
		client.UpdateOrganizationSettings(new OrganizationUpdateSettingsApiModelInput()
		{
			Authentication = newOrgToken,
			EmailSenderType = EmailSenderTypeApi.Organization,

			SignatureSettings = new OrganizationSigningSettingsApiModel()
			{
				AllowRejectWithoutRejectionText = false,
				EnableLongTermValidation = true,
				ForceSignerToReadDocument = true
			}
		});

		//now add more users to this organization
		client.InviteUserToOrganization(new UserInvitationApiModelInput()
		{
			Authentication = newOrgToken,
			Email = "new.user@mycompnay.com",
			FirstName = "John",
			LastName = "JohnLastName",
			Role = UserRoleApiType.Administrator
		});

		//if we need to send email in a new language also create the email templates 

		string language = "pt-BR";

		client.AddEmailTemplate(new EmailTemplateApiModelInput()
		{
			Authentication = newOrgToken,
			Language = language,
			Templates = { 	
				new EmailTemplateDescriptorApiModel()
				{
					TemplateType = EmailTemplateTypeApi.ActivateEmail,
					Subject = "Por favor, ative sua conta",
					Body = "......."
				}
			}
		});
	}
}
