using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using InTwitter.Helpers;
using InTwitter.Models;

namespace InTwitter.Services.SearchService
{
    public class SearchService : ISearchService
    {
        public async Task<AOResult<List<PopularTheme>>> GetPopularThemeAsync()
        {
            var result = new AOResult<List<PopularTheme>>();

            var hashtags = new Dictionary<string, int>();

            foreach (var tweet in Mocks.TweetState.Instance.Tweets)
            {
                var tags = Regex.Matches(tweet.Text, @"(\#[a-zA-Z_]+\b)(?!;)");

                foreach (Match tag in tags)
                {
                    if (hashtags.ContainsKey(tag.Value))
                    {
                        hashtags[tag.Value] += 1;
                    }
                    else
                    {
                        hashtags.Add(tag.Value, 1);
                    }
                }
            }

            result.SetSuccess(hashtags.Select(item => new PopularTheme()
            {
                Theme = item.Key,
                PostsAmount = item.Value,
            }).ToList());

            await Task.Delay(300);

            return result;
        }
    }
}
