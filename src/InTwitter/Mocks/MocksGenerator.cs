using System;
using System.Collections.Generic;
using System.Linq;
using InTwitter.Models;
using InTwitter.Enums;
using InTwitter.Models.User;
using InTwitter.Models.Tweet;
using InTwitter.Models.Notification;
using System.Threading.Tasks;
using InTwitter.Models.Icon;
using Xamarin.Forms;
using InTwitter.Models.Stories;
using System.Diagnostics;

namespace InTwitter.Mocks
{
    public static class MocksGenerator
    {
        #region ---Constants---

        public const int UserAmount = 5;

        public const int MinTweetsAmount = 5;
        public const int MaxTweetAmount = 25;

        public const int LikesDensity = 100;

        public const int MaxImagesInTweet = 6;
        public const int MediaDensity = 40;

        public const string TextPlaceholder = "Lorem #ipsum #dolor sit amet, consectetur adipiscing elit. In elementum ac diam turpis. Ultricies eu vulputate ac leo egestas neque. #Lectus dolor in leo dui #id purus sit tortor. Eget vitae posuere eu, in pharetra tristique augue arcu, aliquam. Nulla bibendum justo quam sed vitae feugiat. At odio a ac scelerisque.";

        #endregion

        public static List<string> IconSources = new List<string>()
        {
            "https://images.pexels.com/photos/3912981/pexels-photo-3912981.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=150",
            "https://images.pexels.com/photos/5397723/pexels-photo-5397723.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=150",
            "https://images.pexels.com/photos/61120/pexels-photo-61120.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=150",
            "https://images.pexels.com/photos/4310461/pexels-photo-4310461.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=150",
            "https://images.pexels.com/photos/5049684/pexels-photo-5049684.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=150",
            "https://images.pexels.com/photos/5119214/pexels-photo-5119214.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=150",
            "https://images.pexels.com/photos/3907595/pexels-photo-3907595.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=150",
        };

        public static List<TweetMediaSource> ImageMediaSource;
        public static List<TweetMediaSource> GifMediaSource;
        public static List<TweetMediaSource> VideoMediaSource;

        public static List<string> HashtagSource;

        public static List<string> IdSource = new List<string>()
        {
           "a17f9781-7dc4-4524-85b7-133334130b93",
           "baddd9ae-9948-4e86-a5c4-8233ed0c9d2e",
           "a528ff6b-a51d-495e-a7a5-f339dcb6840f",
           "cbc858cd-67a7-486f-bec9-bacb763e2a4f",
           "3956049f-7ce8-4f64-ad70-15abc6546482",
        };

        public static Random Random = new Random();

        #region ---Public Static Methods---

        public static void GenerateMocks()
        {
            HashtagSource = new List<string>()
            {
                "#AMAs",
                "#blockchain",
                "#NoNuanceNovember",
            };

            ImageMediaSource = new List<TweetMediaSource>();
            GifMediaSource = new List<TweetMediaSource>();
            VideoMediaSource = new List<TweetMediaSource>();

            ImageMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://images.pexels.com/photos/7974/pexels-photo.jpg?auto=compress&cs=tinysrgb&dpr=2&w=300",
                MediaType = Enums.EMediaType.Image,
            });

            ImageMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://images.pexels.com/photos/373543/pexels-photo-373543.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=300",
                MediaType = Enums.EMediaType.Image,
            });

            ImageMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://images.pexels.com/photos/1036936/pexels-photo-1036936.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=300",
                MediaType = Enums.EMediaType.Image,
            });

            ImageMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://images.pexels.com/photos/347138/pexels-photo-347138.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=300",
                MediaType = Enums.EMediaType.Image,
            });

            ImageMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://images.pexels.com/photos/137612/pexels-photo-137612.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=300",
                MediaType = Enums.EMediaType.Image,
            });

            ImageMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://images.pexels.com/photos/3449823/pexels-photo-3449823.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=600",
                MediaType = Enums.EMediaType.Image,
            });

            ImageMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://images.pexels.com/photos/712618/pexels-photo-712618.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=600",
                MediaType = Enums.EMediaType.Image,
            });

            ImageMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://images.pexels.com/photos/376361/pexels-photo-376361.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=600",
                MediaType = Enums.EMediaType.Image,
            });

            GifMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "https://www.phdmedia.com/russia/wp-content/uploads/sites/74/2017/08/GIF-8.gif",
                MediaType = Enums.EMediaType.Gif,
            });

            VideoMediaSource.Add(new TweetMediaSource()
            {
                MediaSource = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4",
                MediaType = Enums.EMediaType.Video,
            });

            for (int i = 0; i < UserAmount; i++)
            {
                var index = Random.Next(0, IconSources.Count);
                var iconSource = IconSources[index];

                IconSources.RemoveAt(index);

                UserState.Instance.Users.Add(new User()
                {
                    Id = Guid.Parse(IdSource[i]),
                    Name = $"User{i}",
                    HashPassword = "1111",
                    Email = $"username{i}@mail.com",
                    IconSource = iconSource,
                });
            }

            foreach (var user in UserState.Instance.Users)
            {
                var tweetsAmount = Random.Next(MinTweetsAmount, MaxTweetAmount);
                var time = DateTime.Now;

                for (int i = 0; i < tweetsAmount; i++)
                {
                    time -= new TimeSpan(1, Random.Next(-59, 59), 0);

                    TweetState.Instance.Tweets.Add(new Tweet()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        CreationTime = time,
                        Text = TextPlaceholder.Substring(0, Random.Next(5, 310)) + HashtagSource[Random.Next(0, HashtagSource.Count)],
                    });
                }
            }

            var tweets = TweetState.Instance.Tweets.Where(t => t.CreationTime < DateTime.Now).OrderByDescending(t => t.CreationTime).ToList();

            AddImages(tweets[0].Id, 1);
            AddImages(tweets[1].Id, 3);
            AddImages(tweets[2].Id, 6);
            AddGif(tweets[3].Id);
            AddVideo(tweets[4].Id);

            tweets.RemoveAt(0);
            tweets.RemoveAt(0);
            tweets.RemoveAt(0);
            tweets.RemoveAt(0);
            tweets.RemoveAt(0);

            for (int i = 0; i < MediaDensity && tweets.Count != 0; i++)
            {
                var type = Random.Next(0, 10);
                EMediaType mediaType;

                switch (type)
                {
                    case 0:
                        mediaType = EMediaType.Video;
                        break;
                    case 1:
                        mediaType = EMediaType.Gif;
                        break;
                    default:
                        mediaType = EMediaType.Image;
                        break;
                }

                var index = Random.Next(0, tweets.Count);
                var tweetId = tweets[index].Id;
                tweets.RemoveAt(index);

                switch (mediaType)
                {
                    case EMediaType.Image:
                        int mediaAmount = Random.Next(1, MaxImagesInTweet + 1);
                        AddImages(tweetId, mediaAmount);
                        break;
                    case EMediaType.Gif:
                        AddGif(tweetId);
                        break;

                    case EMediaType.Video:
                        AddVideo(tweetId);
                        break;
                }
            }

            for (int i = 0; i < LikesDensity; i++)
            {
                LikeState.Instance.Likes.Add(new Like()
                {
                    UserId = UserState.Instance.Users[Random.Next(0, UserAmount)].Id,
                    TweetId = TweetState.Instance.Tweets[Random.Next(0, TweetState.Instance.Tweets.Count)].Id,
                });
            }
        }

        public static void GenerateTweet(int amount, Guid userId)
        {
            DateTime time = DateTime.Now;
            Tweet tweet;

            for (int i = 0; i < amount; i++)
            {
                time -= new TimeSpan(1, Random.Next(-59, 59), 0);

                tweet = new Tweet
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreationTime = time,
                    Text = TextPlaceholder.Substring(0, Random.Next(5, 310)) + HashtagSource[Random.Next(0, HashtagSource.Count)],
                };

                TweetState.Instance.Tweets.Add(tweet);
                AddMediaToGeneratedTweet(tweet.Id);

                time = DateTime.Now;

                for (int k = 0; k < Random.Next(0, UserAmount); k++)
                {
                    LikeState.Instance.Likes.Add(new Like()
                    {
                        UserId = UserState.Instance.Users[Random.Next(0, UserAmount)].Id,
                        TweetId = tweet.Id,
                    });

                    NotificationState.Instance.Notifications.Add(new Notification
                    {
                        Id = Guid.Empty,
                        OwnerId = userId,
                        UserId = LikeState.Instance.Likes[LikeState.Instance.Likes.Count - 1].UserId,
                        TweetId = tweet.Id,
                        CreationTime = time,
                        TweetAction = ETweetNotificationAction.Like,
                    });

                    Task.Delay(10);
                    time -= new TimeSpan(1, Random.Next(-59, 59), 0);
                }

                time = DateTime.Now;

                for (int k = 0; k < Random.Next(0, UserAmount); k++)
                {
                    MarkState.Instance.Marks.Add(new Mark()
                    {
                        UserId = UserState.Instance.Users[Random.Next(0, UserAmount)].Id,
                        TweetId = tweet.Id,
                    });

                    NotificationState.Instance.Notifications.Add(new Notification
                    {
                        Id = Guid.Empty,
                        OwnerId = userId,
                        UserId = MarkState.Instance.Marks[MarkState.Instance.Marks.Count - 1].UserId,
                        TweetId = tweet.Id,
                        CreationTime = time,
                        TweetAction = ETweetNotificationAction.Mark,
                    });

                    Task.Delay(10);
                    time -= new TimeSpan(1, Random.Next(-59, 59), 0);
                }
            }
        }

        public static void AddFollowers(Guid userId)
        {
            var users = UserState.Instance.Users.Where(u => u.Id != userId);

            foreach (var user in users)
            {
                FollowerState.Instance.Followers.Add(new Follower()
                {
                    UserWhoId = userId,
                    UserOnId = user.Id,
                });
            }
        }

        #endregion

        #region -- Private helpers --

        private static void AddImages(Guid tweetId, int count)
        {
            for (int j = 0; j < count; j++)
            {
                var source = ImageMediaSource[Random.Next(0, ImageMediaSource.Count)];
                MediaSourceState.Instance.Medias.Add(new TweetMediaSource()
                {
                    TweetId = tweetId,
                    MediaSource = source.MediaSource,
                    MediaType = source.MediaType,
                });
            }
        }

        private static void AddGif(Guid tweetId)
        {
            var source = GifMediaSource[Random.Next(0, GifMediaSource.Count)];
            MediaSourceState.Instance.Medias.Add(new TweetMediaSource()
            {
                TweetId = tweetId,
                MediaSource = source.MediaSource,
                MediaType = source.MediaType,
            });
        }

        private static void AddVideo(Guid tweetId)
        {
            var source = VideoMediaSource[Random.Next(0, VideoMediaSource.Count)];
            MediaSourceState.Instance.Medias.Add(new TweetMediaSource()
            {
                TweetId = tweetId,
                MediaSource = source.MediaSource,
                MediaType = source.MediaType,
            });
        }

        private static void AddMediaToGeneratedTweet(Guid tweetId)
        {
            switch (Random.Next(1, 5))
            {
                case 1:
                    AddImages(tweetId, Random.Next(1, 7));
                    break;
                case 2:
                    AddGif(tweetId);
                    break;
                case 3:
                    AddVideo(tweetId);
                    break;
            }
        }

        public static void GenerateUserStories(Guid currentUserId = default(Guid))
        {
            foreach (var userState in UserState.Instance.Users)
            {
                if (currentUserId == userState.Id)
                {
                    UserStoriesState.Instance.UserStories.Add(new UserStories()
                    {
                        Id = userState.Id.ToString(),
                    });
                }
                else
                {
                    UserStoriesState.Instance.UserStories.Add(new UserStories()
                    {
                        Id = userState.Id.ToString(),
                        LastUpdatedPost = DateTimeOffset.Now.AddMinutes(Random.Next(-60, 0)),
                    });
                }
            }

            var curentUser = UserStoriesState.Instance.UserStories.Where(u => u.Id == currentUserId.ToString()).FirstOrDefault();

            foreach (var userStoryState in UserStoriesState.Instance.UserStories)
            {
                if (userStoryState.Id != currentUserId.ToString())
                {
                    var numberOfMedia = Random.Next(1, ImageMediaSource.Count);

                    for (int i = 0; i < numberOfMedia; i++)
                    {
                        var index = Random.Next(0, ImageMediaSource.Count - 1);
                        MediaStoriesSourceState.Instance.MediaStoriesSources.Add(new MediaStoriesSource()
                        {
                            IsVideo = ImageMediaSource[index].MediaType == EMediaType.Image ? false : true,
                            Owner = userStoryState,
                            MediaSource = ImageMediaSource[index].MediaSource,
                            PublicationTime = DateTimeOffset.Now.AddMinutes(Random.Next(-59, 0)),
                        });
                    }

                    var numberPreviewingElement = Random.Next(0, numberOfMedia);

                    PostDataState.Instance.PostDatas.Add(new PostData()
                    {
                        Owner = curentUser,
                        LastMediaElement = 0,
                        UserId = userStoryState.Id,
                    });
                }
            }
        }

        #endregion
    }
}