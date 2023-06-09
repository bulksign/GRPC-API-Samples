using System;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class AddRecipientsAndDocumentsToExistingDraft
	{
		public void RunSample()
		{
			//set your existing draftId here
			string existingDraftId = ".....";

			AuthenticationApiModel token = new ApiKeys().GetAuthentication();

			if (string.IsNullOrEmpty(token.Key))
			{
				Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
				return;
			}


			BulksignApiClient api = new BulksignApiClient();


			api.AddDocumentsRecipientsToDraft(token, new AddDocumentsOrRecipientsToDraftApiModel()
			{
				DraftId = existingDraftId,
				Recipients = new[]
				{
					new RecipientApiModel()
					{
						Email = "test.recipient@test.com", 
						Name = "Recipient Name", 
						Index = 3,
						RecipientType = RecipientTypeApi.Signer
					}
				},
				Documents = new []
				{
					new DocumentApiModel()
					{
						Index = 1,
						FileName = "myfile.pdf",
						FileContentByteArray = new FileContentByteArray()
						{
							ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf")
						}
					}
				}
			});
		}
	}
}