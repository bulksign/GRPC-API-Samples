using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AnalyzeFileAndDynamicallyAddSignatureFields
{

	public void RunSample()
	{
		AuthenticationApiModel token = new ApiKeys().GetAuthentication();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit ApiKeys.cs and put your own token/email");
			return;
		}

		byte[] pfdContent = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\singlepage.pdf");

		AnalyzeFileResult analyzeResult = ChannelManager.GetClient().AnalyzeFile(new FileInput()
		{
			Authentication = token,
			FileContent = ConversionUtilities.ConvertToByteString(pfdContent),
			Filename = "pfdContent.pdf"
		});

		//now based on info received from AnalyzeFile, we create the EnvelopeApiModelInput

		EnvelopeApiModelInput envelope = new EnvelopeApiModelInput();
		envelope.Authentication = token;
		envelope.EnvelopeType = EnvelopeTypeApi.Serial;

		envelope.Recipients.AddRange(new[]
		{

			new RecipientApiModel()
			{
				Email = "test@test.com", //please replace this with own own email address
				Index = 1,
				Name = "Test",
				RecipientType = RecipientTypeApi.Signer
			},

			new RecipientApiModel()
			{
				Email = "other@test.com", //please replace this with own own email address
				Index = 1,
				Name = "Other",
				RecipientType = RecipientTypeApi.Signer
			}
		});


		//since we have already sent the PDF document to AnalyzeFile, for the SendEnvelope request
		//we'll just pass the file identifier returned from AnalyzeFile
		envelope.Documents.Add(

			new DocumentApiModel()
			{
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes = ConversionUtilities.ConvertToByteString(pfdContent),
				}
			}
		);

		//as an example, assign all form fields and signature fields to first recipient

		List<FormFieldResultApiModel> signatures = analyzeResult.Result.Fields
			.Where(model => model.FieldType == FormFieldTypeApi.Signature).ToList();
		List<FormFieldResultApiModel> fields = analyzeResult.Result.Fields
			.Where(model => model.FieldType != FormFieldTypeApi.Signature).ToList();

		AssignmentApiModel assignment = new AssignmentApiModel();
		assignment.AssignedToRecipientEmail = envelope.Recipients[0].Email;

		for (int i = 0; i < signatures.Count; i++)
		{
			assignment.Signatures.Add(new SignatureAssignmentApiModel()
			{
				FieldId = signatures[i].Id,
				SignatureType = SignatureTypeApi.ClickToSign
			});
		}


		for (int i = 0; i < fields.Count; i++)
		{
			assignment.Fields[i].FieldId = fields[i].Id;
		}

		envelope.Documents[0].FieldAssignments.Add(assignment);


		//now, as an example, add a signature field for the second recipient on each page of the document
		int numberPages = analyzeResult.Result.NumberOfPages;

		List<NewSignatureApiModel> newSignatures = new List<NewSignatureApiModel>();

		for (int i = 1; i <= numberPages; i++)
		{
			NewSignatureApiModel sig = new NewSignatureApiModel();
			sig.Height = 100;
			sig.Width = 300;
			sig.PageIndex = i;
			sig.Left = 10;
			sig.Top = 100;
			sig.SignatureType = SignatureTypeApi.ClickToSign;
			sig.AssignedToRecipientEmail = envelope.Recipients[1].Email;

			newSignatures.Add(sig);
		}

		envelope.Documents[0].NewSignatures.AddRange(newSignatures);

		try
		{
			SendEnvelopeResult result = ChannelManager.GetClient().SendEnvelope(envelope);

			if (result.IsSuccessful)
			{
				Console.WriteLine("Access code for recipient " + result.Result.RecipientAccess[0].RecipientEmail + " is " + result.Result.RecipientAccess[0].AccessCode);
				Console.WriteLine("Envelope id is : " + result.Result.EnvelopeId);
			}
			else
			{
				Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}
		}
		catch (Exception ex)
		{
			//handle failed request here
			Console.WriteLine(ex.Message);
		}
	}
}
	