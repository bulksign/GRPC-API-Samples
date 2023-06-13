using Bulksign.Api;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddTextAnnotationsToDocument
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
		envelope.EnvelopeType = EnvelopeTypeApi.Serial;
		envelope.DaysUntilExpire = 10;
		envelope.DisableSignerEmailNotifications = false;
		envelope.ReminderOptions = new ReminderOptionsApiModel()
		{
			EnableReminders = true,
			RecurrentEachDays = 2
		};

		envelope.Recipients.Add(
			new RecipientApiModel()
			{
				Name = "Bulksign Test",
				Email = "contact@bulksign.com",
				Index = 1,
				RecipientType = RecipientTypeApi.Signer
			}
		);


		DocumentApiModel document = new DocumentApiModel();
		document.Index = 1;
		document.FileName = "singlepage.pdf";


		document.NewSignatures.Add(new NewSignatureApiModel()
			{
				Height = 100,
				Width = 250,
				PageIndex = 1,
				Left = 100,
				Top = 500
			}
		);


		//add new text annotations
		document.NewAnnotations.AddRange(new[]
		{
			//width,height, left and top values are in pixels
			new NewAnnotationApiModel
			{
				PageIndex = 1,
				Left = 10,
				Top = 650,
				FontSize = 28,
				Type = AnnotationTypeApi.SenderCustom,
				CustomText = "Annotation with custom text spanning multiple lines of text because the text is too long"
			},

			new NewAnnotationApiModel
			{
				PageIndex = 1,
				Left = 10,
				Top = 900,
				FontSize = 28,
				Type = AnnotationTypeApi.SenderName
			},

			new NewAnnotationApiModel
			{
				PageIndex = 1,
				Left = 10,
				Top = 940,
				FontSize = 28,
				Type = AnnotationTypeApi.SenderOrganizationName
			}

		});


		envelope.Documents.Add(

			new DocumentApiModel()
			{
				FileContentByteArray = new FileContentByteArray()
				{
					ContentBytes = ConversionUtilities.ConvertToByteString(File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\forms.pdf"))
				},
				FileName = "forms.pdf"
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
				Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			//handle failed request here
			Console.WriteLine(ex.Message);
		}
	}
}
