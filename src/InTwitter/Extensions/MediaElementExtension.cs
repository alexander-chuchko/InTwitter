using System;
using System.Collections.Generic;
using System.Text;
using InTwitter.Models;

namespace InTwitter.Extensions
{
    public static class MediaElementExtension
    {
        public static MediaSourceViewModel GetViewModel(this TweetMediaSource mediaSource)
        {
            MediaSourceViewModel mediaVM = new MediaSourceViewModel()
            {
                Id = mediaSource.Id,
                TweetId = mediaSource.TweetId,
                MediaType = mediaSource.MediaType,
                MediaSource = mediaSource.MediaSource,
                IsPlay = false,
            };

            return mediaVM;
        }

        public static TweetMediaSource ToTweetMediaSource(this MediaSourceViewModel mediaVM)
        {
            TweetMediaSource media = new TweetMediaSource()
            {
                TweetId = mediaVM.TweetId,
                MediaType = mediaVM.MediaType,
                MediaSource = mediaVM.MediaSource,
            };

            return media;
        }
    }
}
