using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WhetherSQLiteApp.Models;
using Xamarin.Forms;

namespace WhetherSQLiteApp.Services
{


        public class DataService : IDataService
        {

            //private ObservableCollection<TopHeadlines> OriginalGlobalTopHeadLines = new ObservableCollection<TopHeadlines>();
            //private ObservableCollection<Source> OriginalGlobalSources = new ObservableCollection<Source>();
            //private ObservableCollection<Everything> OriginalGlobalEverythings = new ObservableCollection<Everything>();
            //private ObservableCollection<WeatherDataSource> OriginalGlobalWeatherDataSource = new ObservableCollection<WeatherDataSource>();

            public async Task<WeatherDataSource> GetWeatherDataSourceAsync(String TextToSearch)
            {
                //if (OriginalGlobalCountry != null && OriginalGlobalCountry.Count > 0)
                //    return OriginalGlobalCountry;
           
                var result = new WeatherDataSource();
                //var resultLanguage = new List<Language>();
                try
                {
                    using (var client = new HttpClient())
                    {
                        var stringResponse = await client.GetStringAsync($"https://api.apixu.com/v1/current.json?key=9c53c92268204fa3bd103431192803&q={TextToSearch}");
                        result = JsonConvert.DeserializeObject<WeatherDataSource>(stringResponse);
                       //OriginalGlobalWeatherDataSource = new ObservableCollection<WeatherDataSource>(result);
                    };
                }
                catch (Exception ex)
               { 

                 await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                }

                return result;
            }


            //public async Task<IEnumerable<Everything>> GetEverythingsAsync(String SourceID, Int32 pageSize)
            //{

            //    //if (OriginalGlobalEverythings != null && OriginalGlobalEverythings.Count > 0)
            //    //    return OriginalGlobalEverythings;

            //    if (SourceID != null && SourceID != null)
            //    {
            //        var result = new List<Everything>();
            //        try
            //        {
            //            using (var client = new HttpClient())
            //            {
            //                var stringResponse = await client.GetStringAsync($"https://newsapi.org/v2/everything?sources={SourceID}&pageSize{pageSize}&apiKey=037ec3876d274e97ba55ecb4bbd830b9");
            //                result = JsonConvert.DeserializeObject<ResponseEverything>(stringResponse).Everything;
            //                //OriginalGlobalEverythings = new ObservableCollection<Everything>(result);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            //DisplayAlert("Alert", "You have been alerted", "OK");
            //        }
            //        return result;
            //    }
            //    else
            //    {
            //        return null;
            //    }



            //}

            //public async Task<IEnumerable<Source>> GetSourcesAsync()
            //{

            //    if (OriginalGlobalSources != null && OriginalGlobalSources.Count > 0)
            //        return OriginalGlobalSources;

            //    var result = new List<Source>();
            //    try
            //    {
            //        using (var client = new HttpClient())
            //        {
            //            var stringResponse = await client.GetStringAsync("https://newsapi.org/v2/sources?apiKey=037ec3876d274e97ba55ecb4bbd830b9");
            //            //result = JsonConvert.DeserializeObject<>(stringResponse);
            //            result = JsonConvert.DeserializeObject<ResponseSources>(stringResponse).Source;

            //            OriginalGlobalSources = new ObservableCollection<Source>(result);
            //        }
            //    }
            //    catch (Exception ex)
            //    {


            //        //DisplayAlert("Alert", "You have been alerted", "OK");
            //    }

            //    return OriginalGlobalSources;
            //}

            //public async Task<IEnumerable<TopHeadlines>> GetTopHeadlinesAsync(String SourceID, Int32 pageSize)
            //{
            //    if (SourceID != null && SourceID != null)
            //    {
            //        var result = new List<TopHeadlines>();
            //        try
            //        {
            //            using (var client = new HttpClient())
            //            {
            //                var stringResponse = await client.GetStringAsync($"https://newsapi.org/v2/top-headlines?sources={SourceID}&pageSize={pageSize}&apiKey=037ec3876d274e97ba55ecb4bbd830b9");
            //                result = JsonConvert.DeserializeObject<ResponseTopHeadLines>(stringResponse).TopHeadlines;
            //                //OriginalGlobalTopHeadLines = new ObservableCollection<TopHeadlines>(result);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            //DisplayAlert("Alert", "You have been alerted", "OK");
            //        }
            //        return result;
            //    }
            //    else
            //    {
            //        return null;
            //    }

            //}


        
    }
}
