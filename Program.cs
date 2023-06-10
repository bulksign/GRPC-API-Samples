using Bulksign.ApiSamples;

namespace GrpcApiSamples
{
	public class Program
	{
		static void Main(string[] args)
		{
			new AddNewSignatureToDocument().RunSample();

			//add whichever sample you want to call here , for example : 
			//new  GetUserContacts().RunSample();


			Console.ReadLine();

		}
	}
}