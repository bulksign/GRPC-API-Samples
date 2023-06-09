using System;
using System.IO;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class OverwriteFormFieldValues
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

			EnvelopeApiModel envelope = new EnvelopeApiModel();
			envelope.EnvelopeType    = EnvelopeTypeApi.Serial;
			envelope.DaysUntilExpire = 10;
			envelope.EmailMessage    = "Please sign this document";
			envelope.EmailSubject    = "Please Bulksign this document";
			envelope.Name            = "Test envelope";

			envelope.Recipients = new[]
			{
				new RecipientApiModel()
				{
					Name = "Bulksign Test",
					Email = "enter_your_email_address",
					Index = 1,
					RecipientType = RecipientTypeApi.Signer
				}
			};

			envelope.Documents = new[]
			{
				new DocumentApiModel()
				{
					Index = 2,
					FileName = "forms.pdf",
					FileContentByteArray = new FileContentByteArray()
					{
						ContentBytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\forms.pdf")
					},

					OverwriteValues = new []
					{
						//overwrite textbox value 
						new OverwriteFieldValueApiModel()
						{
							FieldName = "Text1",
							FieldValue = "This is a test text"
						},
						//select a specific radio button 
						new OverwriteFieldValueApiModel()
						{
							FieldName = "Group3",
							FieldValue = "Choice2"
						},

						//select a checkbox 
						new OverwriteFieldValueApiModel()
						{
							FieldName = "Check Box2",
							FieldValue = "True"
						},

						//selected value in combobox
						new OverwriteFieldValueApiModel()
						{
							FieldName = "Dropdown5",
							FieldValue = "Item3"
						},

						//selected value in combobox
						new OverwriteFieldValueApiModel()
						{
							FieldName = "List Box4",
							FieldValue = "Item2"
						}

					}
				}
			};


			BulksignResult<SendEnvelopeResultApiModel> result = api.SendEnvelope(token, envelope);

			if (result.IsSuccessful)
			{
				Console.WriteLine("Access code for recipient " + result.Response.RecipientAccess[0].RecipientEmail + " is " + result.Response.RecipientAccess[0].AccessCode);
				Console.WriteLine("Envelope id is : " + result.Response.EnvelopeId);
			}
			else
			{
				Console.WriteLine($"Request failed : ErrorCode '{result.ErrorCode}' , Message {result.ErrorMessage}");
			}

		}
	}
}