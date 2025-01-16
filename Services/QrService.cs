using System.Drawing;
using Net.Codecrete.QrCodeGenerator;

namespace pWallet.Services;


public class QrService
{
	public string? GenerateQrCodeBase64(string content, int scale = 10, int margin = 4)
	{
		// Generate the QR code
		var qrCode = QrCode.EncodeText(content, QrCode.Ecc.Medium);

		// Render it as a PNG image
		using var memoryStream = new MemoryStream();
		qrCode.ToPng(memoryStream, scale, margin);

		// Convert to Base64 string
		return Convert.ToBase64String(memoryStream.ToArray());
	}
}

public static class QrCodeExtensions
{
	public static void ToPng(this QrCode qrCode, Stream stream, int scale, int margin)
	{
		int size = qrCode.Size;
		int imageSize = (size + 2 * margin) * scale;

		using var bitmap = new System.Drawing.Bitmap(imageSize, imageSize);
		using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
		{
			graphics.Clear(System.Drawing.Color.White);

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (qrCode.GetModule(x, y))
					{
						graphics.FillRectangle(System.Drawing.Brushes.Black,
							(x + margin) * scale,
							(y + margin) * scale,
							scale,
							scale);
					}
				}
			}
		}

		bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
	}
}
