using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class OverwriteFormFieldValues
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		EnvelopeApiModelInput envelope = new EnvelopeApiModelInput();
		envelope.Authentication = token;
		envelope.EnvelopeType = EnvelopeTypeApi.Serial;
		envelope.DaysUntilExpire = 10;
		envelope.EmailMessage = "Please sign this document";
		envelope.EmailSubject = "Please Bulksign this document";
		envelope.Name = "Test envelope";

		envelope.Recipients.Add(new RecipientApiModel
		{
			Name = "Bulksign Test",
			Email = "enter_your_email_address",
			Index = 1,
			RecipientType = RecipientTypeApi.Signer
		});

		envelope.Documents.Add(new DocumentApiModel()
		{
			Index = 2,
			FileName = "forms.pdf",
			FileContentByteArray = new FileContentByteArray()
			{
				ContentBytes = ConversionUtilities.ConvertToByteString(File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\forms.pdf"))
			},

			OverwriteValues =
				{
					//overwrite textbox value 
					new OverwriteFieldValueApiModel()
					{
						FieldName  = "Text1",
						FieldValue = "This is a test text"
					},
					//select a specific radio button 
					new OverwriteFieldValueApiModel()
					{
						FieldName  = "Group3",
						FieldValue = "Choice2"
					},

					//select a checkbox 
					new OverwriteFieldValueApiModel()
					{
						FieldName  = "Check Box2",
						FieldValue = "True"
					},

					//selected value in combobox
					new OverwriteFieldValueApiModel()
					{
						FieldName  = "Dropdown5",
						FieldValue = "Item3"
					},

					//selected value in combobox
					new OverwriteFieldValueApiModel()
					{
						FieldName  = "List Box4",
						FieldValue = "Item2"
					}

				}
		}
		);

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
			//handle failed request
			Console.WriteLine(ex.Message);
		}
	}
}
