using BulksignGrpc;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class GetCompletedFormFields
{
	public void RunSample()
	{
		AuthenticationApiModel token = new Authentication().GetAuthenticationModel();

		if (string.IsNullOrEmpty(token.Key))
		{
			Console.WriteLine("Please edit Authentication.cs and set your own API key there");
			return;
		}

		EnvelopeIdInput id = new EnvelopeIdInput()
		{
			Authentication = token,
			EnvelopeId = "_enter_your_own_envelope_id__"
		};

		GetCompletedFormFieldsResult result;

		try
		{
			result = ChannelManager.GetClient().GetCompletedFormFields(id);
		}
		catch (Exception ex)
		{
			//handle failed request here
			return;
		}

		if (!result.IsSuccess)
		{
			Console.WriteLine($"The request failed, error code :  {result.ErrorCode}, message : {result.ErrorMessage}");
			return;
		}

		foreach (RecipientFormFillApiModel model in result.Result)
		{

			Console.WriteLine($"Processing form fields for recipient {model.RecipientEmail}");

			/*
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
			*/
		}
	}
}
		


