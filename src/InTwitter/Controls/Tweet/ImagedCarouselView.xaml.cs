using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InTwitter.Models;
using FFImageLoading.Helpers.Exif;

namespace InTwitter.Controls.Tweet
{
    public partial class ImagedCarouselView
    {
        public ImagedCarouselView()
        {
            InitializeComponent();
        }

        private Dictionary<object, Size> _sizes = new Dictionary<object, Size>();
        private Dictionary<object, CachedImage> _images = new Dictionary<object, CachedImage>();

        private void CachedImage_Success(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            if (_sizes?.ContainsKey((sender as CachedImage).BindingContext) == false && (sender as CachedImage)?.BindingContext != null)
            {
                var orientationValue = e.ImageInformation.Exif.FirstOrDefault(e => e.Tags.Any(t => t.Name == "Orientation"))?.Tags.FirstOrDefault(t => t.Name == "Orientation")?.Value;

                ExifOrientation orientation = ExifOrientation.ORIENTATION_UNDEFINED;

                if (int.TryParse(orientationValue, out int result))
                {
                    orientation = (ExifOrientation)result;
                }

                if (orientation == ExifOrientation.ORIENTATION_ROTATE_90 || orientation == ExifOrientation.ORIENTATION_ROTATE_270)
                {
                    _sizes?.Add((sender as CachedImage).BindingContext, new Size(e.ImageInformation.OriginalHeight, e.ImageInformation.OriginalWidth));
                }
                else
                {
                    _sizes?.Add((sender as CachedImage).BindingContext, new Size(e.ImageInformation.OriginalWidth, e.ImageInformation.OriginalHeight));
                }

                _images?.Add((sender as CachedImage).BindingContext, sender as CachedImage);
                ChangeSize((sender as CachedImage).BindingContext);
            }
        }

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            ChangeSize(e.CurrentItem);
        }

        private async void ChangeSize(object item)
        {
            if (item != null && this.CurrentItem == item && _sizes?.ContainsKey(item) == true)
            {
                var size = _sizes[item];

                var deviceWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
                var needHeight = deviceWidth / (size.Width * 1.0 / size.Height);
                this.HeightRequest = needHeight;
                await Task.Delay(20);
                _images[item].ReloadImage();
            }
        }
    }
}