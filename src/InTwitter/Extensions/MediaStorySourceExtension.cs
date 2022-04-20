using InTwitter.Models.MediaSource;
using InTwitter.Models.Stories;
using System;

namespace InTwitter.Extensions
{
    public static class MediaStorySourceExtension
    {
        public static MediaStorySource ToMediaStorySource(this MediaStoriesSource mediaStoriesSource, double durationVideo = 0.0)
        {
            MediaStorySource mediaStorySource = null;

            if (mediaStoriesSource != null)
            {
                var mediaType = mediaStoriesSource.IsVideo
                    ? Enums.EMediaType.Video
                    : Enums.EMediaType.Image;

                mediaStorySource = new MediaStorySource()
                {
                    Id = Guid.Parse(mediaStoriesSource.Id),
                    MediaSource = mediaStoriesSource.MediaSource,
                    MediaType = mediaType,
                    PublicationTime = mediaStoriesSource.PublicationTime,
                    TimeSincePublication = DateTimeOffset.Now - mediaStoriesSource.PublicationTime,
                    DurationVideo = durationVideo,
                };
            }

            return mediaStorySource;
        }
    }
}
