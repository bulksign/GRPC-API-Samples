using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class PrepareSendEnvelope
{
	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
			return;
		}

		FileInput firstFile = new FileInput()
		{
			Filename    = "bulksign_test_Sample.pdf",
			FileContent = ConversionUtilities.ConvertToByteString(File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf"))
		};

		PrepareEnvelopeApiModelInput prepare = new PrepareEnvelopeApiModelInput();
		prepare.Authentication = token;

		//flag that determines if the PDF documents should be parsed for tags
		prepare.DocumentParseOptions = new DocumentParseOptionApiModel()
		{
			ParseTags     = false,
			DeleteTagText = false
		};

		prepare.Files.Add(firstFile);

		PrepareSendEnvelopeResult result = null;

		try
		{
			result = ChannelManager.GetClient().PrepareSendEnvelope(prepare);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Request failed :  " + ex.Message);
			return;
		}

		if (result.IsSuccessful == false)
		{
			Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			return;
		}

		//we will used the model returned by the Prepare request
		EnvelopeApiModelInput model = result.Result;

		//now change the email placeholder with the real recipient email address
		model.Recipients[0].Email = "enter_recipient_email_here";
		model.Recipients[0].Name  = "RecipientName";

		//assign all form fields to the first recipient . Obviously if you have multiple recipients, assign the fields as needed
		foreach (DocumentApiModel document in model.Documents)
		{
			foreach (AssignmentApiModel assignment in document.FieldAssignments)
			{
				assignment.AssignedToRecipientEmail = model.Recipients[0].Email;
			}
		}

		try
		{
			//now send the envelope using this model
			SendEnvelopeResult rs = ChannelManager.GetClient().SendEnvelope(model);

			if (rs.IsSuccessful)
			{
				Console.WriteLine($"Envelope with id {rs.Result.EnvelopeId} was created");
			}
			else
			{
				Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}
		}
		catch (Exception ex)
		{
			//handle failed request
			Console.WriteLine(ex.Message);
		}


	}
}
