using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddUserContact
{
		public void RunSample()
		{
			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}

			NewContactApiModelInput contact = new NewContactApiModelInput
			{
				Authentication = token,
				Address = "address",
				Company = "My Company",
				Email   = "email@domain.net",
				Name    = "Contact Name"
			};

			try
			{
				EmptyResult result = ChannelManager.GetClient().AddContact(contact);

				if (result.IsSuccessful)
				{
					Console.WriteLine("Contact was successfully added");
				}
				else
				{
					Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
				}
			}
			catch (Exception ex)
			{
				//handle the failed request
				Console.WriteLine(ex.Message);
			}
		}
}
