using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Threading.Tasks;

namespace AnalyzeImageSample
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

                AnalyzeImage(apiKey, imageUrl).Wait();
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

        private static async Task AnalyzeImage(string apiKey, string imageUrl)
        {
            VisionServiceClient VisionServiceClient = new VisionServiceClient(apiKey);

            VisualFeature[] visualFeatures = new VisualFeature[]
            {
                VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color,
                VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags
            };

            AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(imageUrl, visualFeatures);
            LogAnalysisResult(analysisResult);
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }


        private static void LogAnalysisResult(AnalysisResult result)
        {
            if (result == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Log("null");
                return;
            }

            if (result.Metadata != null)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Log("Metadata");
                Log("  Image Format : " + result.Metadata.Format);
                Log("  Image Dimensions : " + result.Metadata.Width + " x " + result.Metadata.Height);
            }

            if (result.ImageType != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Log("Image Type");
                string clipArtType;
                switch (result.ImageType.ClipArtType)
                {
                    case 0:
                        clipArtType = "0 Non-clipart";
                        break;
                    case 1:
                        clipArtType = "1 ambiguous";
                        break;
                    case 2:
                        clipArtType = "2 normal-clipart";
                        break;
                    case 3:
                        clipArtType = "3 good-clipart";
                        break;
                    default:
                        clipArtType = "Unknown";
                        break;
                }
                Log("  Clip Art Type : " + clipArtType);

                string lineDrawingType;
                switch (result.ImageType.LineDrawingType)
                {
                    case 0:
                        lineDrawingType = "0 Non-LineDrawing";
                        break;
                    case 1:
                        lineDrawingType = "1 LineDrawing";
                        break;
                    default:
                        lineDrawingType = "Unknown";
                        break;
                }
                Log("  Line Drawing Type : " + lineDrawingType);
            }


            if (result.Adult != null)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Log("Adult");
                Log("  Is Adult Content : " + result.Adult.IsAdultContent);
                Log("  Adult Score : " + result.Adult.AdultScore);
                Log("  Is Racy Content : " + result.Adult.IsRacyContent);
                Log("  Racy Score : " + result.Adult.RacyScore);
            }

            if (result.Categories != null && result.Categories.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Log("Categories");
                foreach (var category in result.Categories)
                {
                    Log("   Name : " + category.Name + "; Score : " + category.Score);
                }
            }

            if (result.Faces != null && result.Faces.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Log("Faces : ");
                foreach (var face in result.Faces)
                {
                    Log("   Age : " + face.Age + "; Gender : " + face.Gender);
                }
            }

            if (result.Color != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Log("Color");
                Log("  AccentColor : " + result.Color.AccentColor);
                Log("  Dominant Color Background : " + result.Color.DominantColorBackground);
                Log("  Dominant Color Foreground : " + result.Color.DominantColorForeground);

                if (result.Color.DominantColors != null && result.Color.DominantColors.Length > 0)
                {
                    string colors = "    Dominant Colors : ";
                    foreach (var color in result.Color.DominantColors)
                    {
                        colors += color + " ";
                    }
                    Log(colors);
                }
            }

            if (result.Description != null)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Log("Description");
                foreach (var caption in result.Description.Captions)
                {
                    Log("  Caption : " + caption.Text + "; Confidence : " + caption.Confidence);
                }
                string tags = "  Tags : ";
                foreach (var tag in result.Description.Tags)
                {
                    tags += tag + ", ";
                }
                Log(tags);

            }

            if (result.Tags != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Log("Tags : ");
                foreach (var tag in result.Tags)
                {
                    Log("  Name : " + tag.Name + "; Confidence : " + tag.Confidence + "; Hint : " + tag.Hint);
                }
            }

        }
    }
}