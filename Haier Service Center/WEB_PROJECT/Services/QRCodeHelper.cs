using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;

public class QRCodeHelper
{

    public static byte[] GenerateQRCode(string data, int pixelSize = 20)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            byte[] qrCodeImage = qrCode.GetGraphic(pixelSize);
            return qrCodeImage;
        }

    }

}

