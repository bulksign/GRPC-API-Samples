//using Bulksign.Api;
//using GrpcApiSamples;

//namespace Bulksign.ApiSamples;

//public class UpdateUserSettings
//{
//	public void RunSample()
//	{
//		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

//		if (string.IsNullOrEmpty(token.Key))
//		{
//			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
//			return;
//		}

//		//this will update all NON-NULL values that we send
//		BulksignResult<string> result = ChannelManager.GetClient().UpdateUserSettings(token, new UserUpdateSettingsApiModel()
//		{
//			JobTitle = "My job",
//			DefaultDraftLanguage = "en-US",
//			NotificationRecipientOpenedSignStep = true,
//			LastName = "MyLastName"
//		});

//		//check if the result was successful

//		if (result.IsSuccessful == false)
//		{
//			Console.WriteLine($"Request failed : RequestId {result.RequestId}, ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
//		}


//	}

//}

