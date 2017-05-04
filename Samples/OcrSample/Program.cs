using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OcrSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                Console.WriteLine("Enter your Cognitive Services Vision API Key:");
                var apiKey = Console.ReadLine();
                Console.WriteLine();

                Console.WriteLine("Enter the url of the image you want analyze:");
                var imageUrl = Console.ReadLine();
                Console.WriteLine();

                RunOcr(apiKey, imageUrl).Wait();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static async Task RunOcr(string apiKey, string imageUrl)
        {
            var VisionServiceClient = new VisionServiceClient(apiKey);

            var ocrResult = await VisionServiceClient.RecognizeTextAsync(imageUrl);
            LogOcrResults(ocrResult);
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }

        private static void LogOcrResults(OcrResults results)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            StringBuilder stringBuilder = new StringBuilder();

            if (results != null && results.Regions != null)
            {
                stringBuilder.Append("Text: ");
                stringBuilder.AppendLine();
                foreach (var item in results.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            stringBuilder.Append(" ");
                        }

                        stringBuilder.AppendLine();
                    }

                    stringBuilder.AppendLine();
                }
            }

            Log(stringBuilder.ToString());
        }
    }
}