using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InTwitter.Models;
using InTwitter.Helpers;

namespace InTwitter.Services.SearchService
{
    public interface ISearchService
    {
        Task<AOResult<List<PopularTheme>>> GetPopularThemeAsync();
    }
}
