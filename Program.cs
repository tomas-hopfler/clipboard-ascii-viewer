using System.Drawing;
using CIH;

// load image from clipboard
Bitmap img = (Bitmap)ClipboardImageHelper.GetImageFromClipboard();
if (img == null)
{
	return;
}

// characters from lightest to darkest (customizable, array length independent)
char[] shades = { ' ', '░', '▒', '▓', '█' };

// default resolution settings
int wMinRes = 50;
int hMinRes = 50;

// calculate aspect ratio of image
float ratio = (float)img.Width / img.Height;

// calculate result resolution
int wRes = Math.Min((int)(Math.Min(wMinRes, img.Height) * ratio), Console.LargestWindowWidth/2);
int hRes = Math.Min((int)(Math.Min(hMinRes, img.Width) / ratio), Console.LargestWindowHeight);

char[,] arr = new char[wRes, hRes];

// set size of console to size of result image
Console.SetWindowSize(Math.Min(wRes*2, Console.LargestWindowWidth), hRes);

ShowImage();

void ShowImage()
{
	// check for correct image resolution
	if (img.Width >= wRes && img.Height >= hRes)
	{
		Scale();
	}

	Console.Clear();
	Console.OutputEncoding = System.Text.Encoding.UTF8;
	PrintArray();

	// close console with key press
	Console.ReadKey();
}
void Scale()
{
	// calculate iteration step for result image
	float wStep = wRes / (float)img.Width;
	float hStep = hRes / (float)img.Height;

	for (int h = 0; h < img.Height; h++)
	{
		for (int w = 0; w < img.Width; w++)
		{
			Color pixel = img.GetPixel(w, h);
			float brightness = pixel.GetBrightness();

			DrawInArray((int)(w * wStep), (int)(h * hStep), brightness);
		}
	}
}
void DrawInArray(int x, int y, float pixelBrightness)
{
	int shadesCount = shades.Length;
	int shadeIndex = (int)(pixelBrightness * shadesCount);
	// get character according to pixel brightness
	shadeIndex = Math.Clamp(shadeIndex, 0, shadesCount - 1);

	arr[x, y] = shades[shadeIndex];
}
void PrintArray()
{
	for (int y = 0; y < hRes; y++)
	{
		for (int x = 0; x < wRes-1; x++)
		{
			// print 2 characters to represent single pixel
			Console.Write(arr[x, y] + "" + arr[x, y]);
		}
		if(y != hRes-1)
		{
			Console.Write("\n");
		}
	}
}