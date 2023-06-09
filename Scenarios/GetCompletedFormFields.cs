using System;
using Bulksign.Api;

namespace Bulksign.ApiSamples
{
	public class GetCompletedFormFields
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

			//set your own envelopeId here
			string envelopeId = "..............";

			BulksignResult<RecipientFormFillApiModel[]> formFields = api.GetCompletedFormFields(token,  envelopeId );

			if (!formFields.IsSuccessful)
			{
				Console.WriteLine($"The request failed, error code :  {formFields.ErrorCode}, message : {formFields.ErrorMessage}");
				return;
			}

			foreach (RecipientFormFillApiModel model in formFields.Response)
			{
				Console.WriteLine($"Processing form fields for recipient {model.RecipientEmail}");

				foreach (FormFillResultApiModel fieldModel in model.FormFillResult)
				{
					switch (fieldModel.FieldType)
					{
						case FormFieldTypeApi.TextBox:
							break;
						case FormFieldTypeApi.RadioButton:
							break;
						case FormFieldTypeApi.CheckBox:
							break;
						case FormFieldTypeApi.ComboBox:
							break;
						case FormFieldTypeApi.ListBox:
							break;
						case FormFieldTypeApi.Signature:
							break;
						case FormFieldTypeApi.Attachment:
							break;
						default:
							Console.WriteLine("Invalid form field type");
							break;
					}
				}
			}


		}
		
	}

}