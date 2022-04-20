using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using System.IO;
using Xamarin.Essentials;
using InTwitter.PlatformDependencyInterface;
using System.Threading.Tasks;
using Android.Media;
using Xamarin.CommunityToolkit.UI.Views;

[assembly: Dependency(typeof(InTwitter.Droid.DependencyInterfaceImplementations.ImageSaver))]
namespace InTwitter.Droid.DependencyInterfaceImplementations
{
    public class ImageSaver : IImageSaver
    {
        public async Task<string> Save(string uri, bool toTempFolder = false)
        {
            Bitmap bit;

            if (uri.StartsWith("/"))
            {
                bit = BitmapFactory.DecodeFile(uri);
            }
            else
            {
                var handler = new ImageLoaderSourceHandler();
                bit = await handler.LoadImageAsync(ImageSource.FromUri(new Uri(uri)), null);
            }

            var filePath = string.Empty;

            if (bit != null)
            {
                var folderPath = "";

                if (toTempFolder == true)
                {
                    folderPath = Xamarin.Essentials.FileSystem.CacheDirectory;
                }
                else
                {
                    folderPath = "/storage/emulated/0/" + Android.OS.Environment.DirectoryDownloads;
                }

                filePath = System.IO.Path.Combine(folderPath, $"{Guid.NewGuid().ToString().Replace('-', 'Z')}.jpeg");

                using var stream = new FileStream(filePath, FileMode.Create);

                if (bit.Height > 720)
                {
                    double compress = (double)720 / bit.Height;
                    bit = Bitmap.CreateScaledBitmap(bit, (int)(bit.Width * compress), (int)(bit.Height * compress), false);
                }

                if (bit.Width > 1280)
                {
                    double compress = (double)1280 / bit.Width;
                    bit = Bitmap.CreateScaledBitmap(bit, (int)(bit.Width * compress), (int)(bit.Height * compress), false);
                }


                bit.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);

                stream.Close();

            }

            if (uri.StartsWith("/"))
            {
                ExifInterface exif = new ExifInterface(uri);

                var orientation = exif.GetAttribute(ExifInterface.TagOrientation);

                exif = new ExifInterface(filePath);
                exif.SetAttribute(ExifInterface.TagOrientation, orientation);
                exif.SaveAttributes();
            }

            return filePath;
        }

        public async Task<string> SaveFromStreamToImage(byte[] data, int offset, int length, bool toTempFolder = false, CameraOptions cameraOptions = CameraOptions.Front)
        {
            Bitmap bit;
            Bitmap newBit;
            newBit = await BitmapFactory.DecodeByteArrayAsync(data, 0, data.Length);

            Matrix matrix = new Matrix();

            if (cameraOptions == CameraOptions.Default || cameraOptions == CameraOptions.Back)
            {
                matrix.PostRotate(90);
            }
            else if (cameraOptions == CameraOptions.Front)
            {
                matrix.PostRotate(-90);
                matrix.PreScale(1, -1);
            }

            bit = Bitmap.CreateBitmap(newBit, 0, 0, newBit.Width, newBit.Height, matrix, true);

            var filePath = string.Empty;

            if (bit != null)
            {
                var folderPath = "";

                if (toTempFolder == true)
                {
                    folderPath = Xamarin.Essentials.FileSystem.CacheDirectory;
                }
                else
                {
                    folderPath = "/storage/emulated/0/" + Android.OS.Environment.DirectoryDownloads;
                }

                filePath = System.IO.Path.Combine(folderPath, $"{Guid.NewGuid().ToString().Replace('-', 'Z')}.jpeg");

                var stream = new FileStream(filePath, FileMode.Create);
                if (bit.Height >= 720)
                {
                    double compress = (double)720 / bit.Height;
                    bit = Bitmap.CreateScaledBitmap(bit, (int)(bit.Width * compress), (int)(bit.Height * compress), false);
                }

                if (bit.Width >= 1280)
                {
                    double compress = (double)1280 / bit.Width;
                    bit = Bitmap.CreateScaledBitmap(bit, (int)(bit.Width * compress), (int)(bit.Height * compress), false);
                }

                bit.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);


                stream.Close();
            }

            return filePath;
        }
    }
}