using System;
using System.Collections.Generic;
using System.Linq;
using InTwitter.Models;
using InTwitter.Models.Tweet;

namespace InTwitter.Mocks
{
    public class TweetState
    {
        #region ---Public Static Properties---

        private static TweetState _instance;

        public static TweetState Instance => _instance ??= new TweetState();

        #endregion

        #region ---Constructors---

        private TweetState()
        {
            this.Tweets = new List<Tweet>();
        }

        #endregion

        #region ---Public Properties---

        public List<Tweet> Tweets { get; set; }

        #endregion

        #region ---Public Methods---

        public void IncreaseTweetVersion(Guid tweetId)
        {
            var tweet = Tweets.Where(t => t.Id == tweetId).FirstOrDefault();

            if (tweet != null)
            {
                tweet.Version++;
            }
        }

        public List<T> GetTruncatedList<T>(List<T> sourceList, uint amount = 5)
        {
            List<T> resultList = new List<T>();

            for (int i = 0; i < amount && i < sourceList.Count; i++)
            {
                resultList.Add(sourceList[i]);
            }

            return resultList;
        }

        #endregion
    }
}
