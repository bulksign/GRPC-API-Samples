using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class PrepareSendEnvelopeWithTags
{
	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
			return;
		}

		FileInput firstFile = new FileInput
		{
			Filename = "bulksign_test_Sample.odt",
			FileContent = ConversionUtilities.ConvertToByteString(File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_advanced_tags.odt"))
		};

		PrepareEnvelopeApiModelInput prepare = new PrepareEnvelopeApiModelInput();
		prepare.Authentication = token;

		//flag that determines if the PDF documents should be parsed for tags
		prepare.DocumentParseOptions = new DocumentParseOptionApiModel
		{
			ParseTags = true,
			DeleteTagText = true
		};

		prepare.Files.Add(firstFile);

		PrepareSendEnvelopeResult result = null;

		try
		{
			result = ChannelManager.GetClient().PrepareSendEnvelope(prepare);
		}
		catch (Exception ex)
		{
			//handle failed request
			Console.WriteLine(ex.Message);
			return;
		}


		if (!result.IsSuccessful)
		{
			Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			return;
		}


		//the model will include 2 placeholder recipients because we have 2 tags in the documents with different indexes
		EnvelopeApiModelInput model = result.Result;

		//now change the email placeholder with the real recipient email address
		model.Recipients[0].Email = "test@email.com";
		model.Recipients[0].Name = "First Recipient";

		model.Recipients[0].Email = "second@email.com";
		model.Recipients[0].Name = "Second Recipient";

		//now re-assign the form fields, we already know the tag identifiers from the file

		List<AssignmentApiModel> assignments = new List<AssignmentApiModel>();

		assignments.Add(new AssignmentApiModel
		{
			AssignedToRecipientEmail = model.Recipients[0].Email,
			Signatures = {
					new SignatureAssignmentApiModel
					{
						FieldId       = "sigFieldSender",
						SignatureType = SignatureTypeApi.DrawTypeToSign
					}
				}
		});

		assignments.Add(new AssignmentApiModel
		{
			AssignedToRecipientEmail = model.Recipients[1].Email,
			Signatures = {
					new SignatureAssignmentApiModel
					{
						FieldId       = "sigFieldCustomer",
						SignatureType = SignatureTypeApi.ClickToSign
					}
				}
		});

		model.Documents[0].FieldAssignments.AddRange(assignments.ToArray());

		try
		{

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
