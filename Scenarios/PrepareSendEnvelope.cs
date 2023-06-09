using System;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
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

			BulksignApiClient api = new BulksignApiClient();

			FileInput firstFile = new FileInput()
			{
				Filename = "bulksign_test_Sample.pdf",
				FileContent = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_Sample.pdf")
			};

			PrepareEnvelopeApiModel prepare = new PrepareEnvelopeApiModel();

			//flag that determines if the PDF documents should be parsed for tags
			prepare.DocumentParseOptions = new DocumentParseOptionApiModel()
			{
				ParseTags     = false,
				DeleteTagText = false
			};

			prepare.Files = new[]
			{
				firstFile
			};

			BulksignResult<EnvelopeApiModel> result = api.PrepareSendEnvelope(token, prepare);

			if (result.IsSuccessful)
			{
				EnvelopeApiModel model = result.Response;

				//now change the email placeholder with the real recipient email address
				model.Recipients[0].Email = "enter_recipient_email_here";
				model.Recipients[0].Name = "RecipientName";

				//assign all form fields to the first recipient . 
				//Obviously if you have multiple recipients, assign the fields as needed
				foreach (DocumentApiModel document in model.Documents)
				{
					foreach (AssignmentApiModel assignment in document.FieldAssignments)
					{
						assignment.AssignedToRecipientEmail = model.Recipients[0].Email;
					}
				}

				BulksignResult<SendEnvelopeResultApiModel> envelope = api.SendEnvelope(token, model);

				if (result.IsSuccessful)
				{
					Console.WriteLine($"Envelope with id {envelope.Response.EnvelopeId} was created");
				}
			}
			else
			{
				Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}

		}
	}
}