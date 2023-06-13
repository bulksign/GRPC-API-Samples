using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddUserContact
{
		public void RunSample()
		{
			AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit Authentication.cs and set your own API key there");
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
