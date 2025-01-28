
using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddRecipientsAndDocumentsToExistingDraft
{
	public void RunSample()
	{
		//set your existing draftId here for which you want to make these changes
		string existingDraftId = "..........";

		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		AddDocumentsOrRecipientsToDraftApiModelInput model = new AddDocumentsOrRecipientsToDraftApiModelInput();

		model.Authentication = token;
		model.DraftId = existingDraftId;

		//add a new signer recipient to the draft
		model.Recipients.Add(new RecipientApiModel()
			{
				Email = "test.recipient@test.com",
				Name = "Recipient Name",
				Index = 3,
				RecipientType = RecipientTypeApi.Signer
			}
		);

		//add a new PDF document to the draft
		model.Documents.Add(new DocumentApiModel()
			{
				Index = 1,
				FileName = "myfile.pdf",
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes = ConversionUtilities.ConvertToByteString(File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf"))
				}
			}
		);


		try
		{
			EmptyResult result = ChannelManager.GetClient().AddDocumentsRecipientsToDraft(model);

			if (result.IsSuccess)
			{
				Console.WriteLine("Draft was successfully updated");
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
