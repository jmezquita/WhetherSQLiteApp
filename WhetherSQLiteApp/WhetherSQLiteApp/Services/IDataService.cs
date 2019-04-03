using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhetherSQLiteApp.Models;

namespace WhetherSQLiteApp.Services
{
    public interface IDataService
    {
        //Task<IEnumerable<RecentItems>> GetSourcesAsync();
        //Task<IEnumerable<TopHeadlines>> GetTopHeadlinesAsync(string SourceID, int pageSize);
        //Task<IEnumerable<Everything>> GetEverythingsAsync(string SourceID, int pageSize);
        Task<WeatherDataSource> GetWeatherDataSourceAsync(String TextToSearch);

    }
}
