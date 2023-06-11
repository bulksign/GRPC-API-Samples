using Bulksign.Api;
using Google.Protobuf.Collections;
using GrpcApiSamples;

namespace Bulksign.ApiSamples;

public class AddAttachment
{
    public void RunSample()
    {
        AuthenticationApiModel token = new ApiKeys().GetAuthentication();

        if (string.IsNullOrEmpty(token.Key))
        {
            Console.WriteLine("Please edit APiKeys.cs and put your own token/email");
            return;
        }

        EnvelopeApiModelInput envelope = new EnvelopeApiModelInput();
        envelope.Authentication = token;
        envelope.EnvelopeType = EnvelopeTypeApi.Serial;
        envelope.DaysUntilExpire = 10;
        envelope.DisableSignerEmailNotifications = false;

        envelope.Recipients.Add(new RepeatedField<RecipientApiModel>
        {
            new RecipientApiModel
            {
                Name = "Recipient First",
                Email = "add_email_address_here",
                Index = 1,
                RecipientType = RecipientTypeApi.Signer
            }
        });

        envelope.Documents.Add(new RepeatedField<DocumentApiModel>
        {
            new DocumentApiModel
            {
                Index = 1,
                FileName = "singlepage.pdf",
                FileContentByteArray = new FileContentByteArray
                {
                    ContentBytes = ConversionUtilities.ConvertToByteString(
                        File.ReadAllBytes(Environment.CurrentDirectory + @"\Files\bulksign_test_sample.pdf"))
                },


                NewAttachments =
                {
                    new NewAttachmentApiModel
                    {
                        AssignedToRecipientEmail = "add_email_address_here",
                        Id = "id_picture",
                        Text = "Please attach your ID scanned image"
                    },
                    new NewAttachmentApiModel
                    {
                        AssignedToRecipientEmail = "add_email_address_here",
                        Id = "id_passport",
                        Text = "Please attach an image of your passport"
                    }
                }
            }
        });

        try
        {
            SendEnvelopeResult result = ChannelManager.GetClient().SendEnvelope(envelope);

            if (result.IsSuccessful)
            {
                Console.WriteLine("Access code for recipient " + result.Result.RecipientAccess[0].RecipientEmail + " is " + result.Result.RecipientAccess[0].AccessCode);
                Console.WriteLine("EnvelopeId is : " + result.Result.EnvelopeId);
            }
            else
            {
                Console.WriteLine("ERROR : " + result.ErrorCode + " " + result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            //handle the failed request    
            Console.WriteLine(ex.Message);
        }
    }
}