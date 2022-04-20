using System;
using Xamarin.Forms;
using InTwitter.PlatformDependencyInterface;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.Media;
using Java.Lang;
using Java.Nio;
using Xamarin.CommunityToolkit.UI.Views;

[assembly: Dependency(typeof(InTwitter.Droid.DependencyInterfaceImplementations.VideoService))]
namespace InTwitter.Droid.DependencyInterfaceImplementations
{
    public class VideoService : IVideoService
    {
        public async Task<double> GetVideoLengthAsync(string pathMediaSource)
        {
            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(pathMediaSource);
            var length = retriever.ExtractMetadata(MetadataKey.Duration);
            var lengthseconds = Convert.ToInt32(length) / 1000;
            TimeSpan timeSpan = TimeSpan.FromSeconds(lengthseconds);

            return timeSpan.TotalSeconds;
        }

        public async Task<bool> TrimOrRotateAsync(string inputPath, string outputPath, int startMS, int lengthMS = 0, CameraOptions cameraOptions = CameraOptions.Default)
        {
            bool didOperationSucceed = false;

            try
            {
                MediaExtractor extractor = new MediaExtractor();
                extractor.SetDataSource(inputPath);
                int trackCount = extractor.TrackCount;
                MediaMuxer muxer;
                muxer = new MediaMuxer(outputPath, MuxerOutputType.Mpeg4);
                Dictionary<int, int> indexDict = new Dictionary<int, int>(trackCount);
                int bufferSize = -1;

                for (int i = 0; i < trackCount; i++)
                {
                    MediaFormat format = extractor.GetTrackFormat(i);
                    string mime = format.GetString(MediaFormat.KeyMime);
                    bool selectCurrentTrack = false;
                    if (mime.StartsWith("audio/"))
                    {
                        selectCurrentTrack = true;
                    }
                    else if (mime.StartsWith("video/"))
                    {
                        selectCurrentTrack = true;
                    }
                    if (selectCurrentTrack)
                    {
                        extractor.SelectTrack(i);
                        int dstIndex = muxer.AddTrack(format);
                        indexDict.Add(i, dstIndex);
                        if (format.ContainsKey(MediaFormat.KeyMaxInputSize))
                        {
                            int newSize = format.GetInteger(MediaFormat.KeyMaxInputSize);
                            bufferSize = newSize > bufferSize ? newSize : bufferSize;
                        }
                    }
                }
                if (bufferSize < 0)
                {
                    bufferSize = 1337;
                }

                MediaMetadataRetriever retrieverSrc = new MediaMetadataRetriever();
                retrieverSrc.SetDataSource(inputPath);
                string degreesString = retrieverSrc.ExtractMetadata(MetadataKey.VideoRotation);
                string width = retrieverSrc.ExtractMetadata(MetadataKey.VideoWidth);
                string height = retrieverSrc.ExtractMetadata(MetadataKey.VideoHeight);

                if (degreesString != null)
                {
                    int degrees = int.Parse(degreesString);

                    if (degrees >= 0 && lengthMS != 0)
                    {
                        muxer.SetOrientationHint(degrees);
                    }
                    else
                    {
                        double lengthVideo = await GetVideoLengthAsync(inputPath);
                        lengthMS = (int)lengthVideo * 1000;
                        muxer.SetOrientationHint(cameraOptions == CameraOptions.Front ? 270 : 90);

                    }
                }

                if (startMS > 0)
                {
                    extractor.SeekTo(startMS * 1000, MediaExtractorSeekTo.ClosestSync);
                }

                int offset = 0;
                int trackIndex = -1;
                ByteBuffer dstBuf = ByteBuffer.Allocate(bufferSize);
                MediaCodec.BufferInfo bufferInfo = new MediaCodec.BufferInfo();

                try
                {
                    muxer.Start();
                    while (true)
                    {
                        bufferInfo.Offset = offset;
                        bufferInfo.Size = extractor.ReadSampleData(dstBuf, offset);
                        if (bufferInfo.Size < 0)
                        {
                            bufferInfo.Size = 0;
                            break;
                        }
                        else
                        {
                            bufferInfo.PresentationTimeUs = extractor.SampleTime;
                            if (lengthMS > 0 && bufferInfo.PresentationTimeUs > ((startMS + lengthMS - 1) * 1000))
                            {
                                Console.WriteLine("The current sample is over the trim end time.");
                                break;
                            }
                            else
                            {
                                bufferInfo.Flags = ConvertMediaExtractorSampleFlagsToMediaCodecBufferFlags(extractor.SampleFlags);
                                trackIndex = extractor.SampleTrackIndex;
                                muxer.WriteSampleData(indexDict[trackIndex], dstBuf, bufferInfo);
                                extractor.Advance();
                            }
                        }
                    }

                    muxer.Stop();
                    didOperationSucceed = true;
                    Java.IO.File file = new Java.IO.File(inputPath);
                    file.Delete();
                }
                catch (IllegalStateException e)
                {
                    Console.WriteLine("The source video file is malformed");
                }
                finally
                {
                    muxer.Release();
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return didOperationSucceed;
        }

        private MediaCodecBufferFlags ConvertMediaExtractorSampleFlagsToMediaCodecBufferFlags(MediaExtractorSampleFlags mediaExtractorSampleFlag)
        {
            switch (mediaExtractorSampleFlag)
            {
                case MediaExtractorSampleFlags.None:
                    return MediaCodecBufferFlags.None;
                case MediaExtractorSampleFlags.Encrypted:
                    return MediaCodecBufferFlags.KeyFrame;
                case MediaExtractorSampleFlags.Sync:
                    return MediaCodecBufferFlags.SyncFrame;
                default:
                    throw new NotImplementedException("ConvertMediaExtractorSampleFlagsToMediaCodecBufferFlags");
            }
        }
    }
}