using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;

if (!Directory.Exists("input"))
{
    Log("Please place input files in the [bold]'input'[/] directory.");
    Log("[dim]Press Enter to exit[/]");
    Console.ReadLine();
    return;
}

if (!Directory.Exists("output"))
{
    Log("Creating directory [bold]'output'[/]");
    Directory.CreateDirectory("output");
}

var validExtensions = new[] { ".jpg", ".jpeg", ".webp", ".png" };
var files = Directory.GetFiles("input").Where(file => validExtensions.Contains(Path.GetExtension(file).ToLower())).ToList();

files.ForEach(file =>
{
    Log($"Processing [bold green]{file}[/]");
    ProcessImage(file);
});

Log("\r\n[dim]Processing complete. Press Enter to exit.[/]");
return;

static void ProcessImage(string filePath)
{
    try
    {
        var fileName = Path.GetFileName(filePath);

        var random = new Random();
        var contrastBoost = 1.5f;   // +50%
        var brightnessAdjust = 0.9f; // -10%
        var exposureMult = 1.1f; // +10%

        using (var image = Image.Load<Rgba32>(filePath))
        {
            image.Mutate(x =>
            {
                x.Brightness(brightnessAdjust);
                x.Contrast(contrastBoost);
            });

            var averageLuminance = 0f;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> rowSpan = accessor.GetRowSpan(y);
                    for (int x = 0; x < rowSpan.Length; x++)
                    {
                        var pixel = rowSpan[x];
                        float luminance = (pixel.R * 0.299f + pixel.G * 0.587f + pixel.B * 0.114f) / 255f;
                        averageLuminance += luminance;
                    }
                }

                var totalPixels = image.Width * image.Height;
                averageLuminance /= totalPixels;
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> rowSpan = accessor.GetRowSpan(y);
                    for (int x = 0; x < rowSpan.Length; x++)
                    {
                        var pixel = rowSpan[x];

                        // Simulate exposure bump (multiplicative)
                        pixel.R = ClampToByte(pixel.R * exposureMult);
                        pixel.G = ClampToByte(pixel.G * exposureMult);
                        pixel.B = ClampToByte(pixel.B * exposureMult);

                        // Increase brightness if average luminance is low
                        if (averageLuminance < 0.3f)
                        {
                            pixel.R = ClampToByte(pixel.R + 25);
                            pixel.G = ClampToByte(pixel.G + 25);
                            pixel.B = ClampToByte(pixel.B + 25);
                        }

                        // Decrease brightness if average luminance is high
                        if (averageLuminance > 0.7f)
                        {
                            pixel.R = ClampToByte(pixel.R - 25);
                            pixel.G = ClampToByte(pixel.G - 25);
                            pixel.B = ClampToByte(pixel.B - 25);
                        }

                        // Add noise 50%
                        if (random.Next(0, 2) == 1)
                        {
                            var noiseAmount = random.Next(-20, 20);

                            pixel.R = ClampToByte(pixel.R + noiseAmount);
                            pixel.G = ClampToByte(pixel.G + noiseAmount);
                            pixel.B = ClampToByte(pixel.B + noiseAmount);
                        }

                        rowSpan[x] = pixel;
                    }
                }
            });

            image.Mutate(x => x.Grayscale());
            
            image.Save($"output/{fileName}");
        }
    }
    catch (Exception ex)
    {
        Log($"Error processing [green bold]{filePath}[/]: [red]{ex.Message}[/]");
        Log("[dim]Press Enter to exit[/]");
        Console.ReadLine();
    }
}

static byte ClampToByte(float value) => (byte)Math.Clamp((int)value, 0, 255);
static void Log(string msg) {
    AnsiConsole.Markup($"{msg}\r\n");
}