using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddSharedContact
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