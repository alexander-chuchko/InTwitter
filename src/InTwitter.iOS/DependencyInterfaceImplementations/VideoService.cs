using AVFoundation;
using CoreMedia;
using Foundation;
using InTwitter.PlatformDependencyInterface;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

[assembly: Dependency(typeof(InTwitter.iOS.DependencyInterfaceImplementations.VideoService))]
namespace InTwitter.iOS.DependencyInterfaceImplementations
{
    public class VideoService : IVideoService
    {
        public async Task<double> GetVideoLengthAsync(string path)
        {
            MediaElement mediaElement = new MediaElement();
            mediaElement.Source = path;
            var res = mediaElement.Duration;
            NSUrl inputFileUrl = new NSUrl(path, false);
            await Task.Delay(100);
            AVAsset avasset = AVAsset.FromUrl(inputFileUrl);
            await Task.Delay(100);
            double length = avasset.Duration.Seconds;

            return length;
        }

        public async Task<bool> TrimOrRotateAsync(string inputPath, string outputPath, int startMS, int lengthMS = 0, CameraOptions cameraOptions = CameraOptions.Default)
        {
            bool didOperationSucceed = false;

            try
            {
                NSUrl inputFileUrl = new NSUrl(inputPath, false);
                AVAsset videoAsset = AVAsset.FromUrl(inputFileUrl);

                var compatiblePresets = AVAssetExportSession.ExportPresetsCompatibleWithAsset(videoAsset).ToList();
                var preset = "";

                if (compatiblePresets.Contains("AVAssetExportPresetLowQuality"))
                {
                    preset = "AVAssetExportPresetLowQuality";
                }
                else
                {
                    preset = compatiblePresets.FirstOrDefault();
                }

                AVAssetExportSession exportSession = new AVAssetExportSession(videoAsset, preset);
                exportSession.OutputUrl = NSUrl.FromFilename(outputPath);
                exportSession.OutputFileType = AVFileType.QuickTimeMovie;

                CMTime start = CMTime.FromSeconds(startMS / 1000, videoAsset.Duration.TimeScale);
                CMTime duration = CMTime.FromSeconds(lengthMS / 1000, videoAsset.Duration.TimeScale);
                CMTimeRange range = new CMTimeRange();
                range.Start = start;
                range.Duration = duration;
                exportSession.TimeRange = range;

                exportSession.OutputFileType = AVFileType.QuickTimeMovie;
                await exportSession.ExportTaskAsync();

                if (exportSession.Status != AVAssetExportSessionStatus.Completed)
                {
                    Console.WriteLine("Log starts...");
                    Console.WriteLine(exportSession.Status.ToString());
                    Console.WriteLine(exportSession.Error);
                }

                didOperationSucceed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong in { nameof(TrimOrRotateAsync)}");
            }

            return didOperationSucceed;
        }
    }
}