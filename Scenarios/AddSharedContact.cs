using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddSharedContact
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
            Address = "address",
            Company = "My Company",
            Email = "email@domain.net",
            Name = "Contact Name",
            Authentication = token
        };

        try
        {
            EmptyResult result = ChannelManager.GetClient().AddSharedContact(contact);

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
            //handle failed request
            Console.WriteLine(ex.Message);
        }
    }
}