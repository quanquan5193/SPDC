using Elasticsearch.Net;
using Nest;
using SPDC.Common;
using SPDC.Model.BindingModels;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SPDC.WebAPI.Helpers
{
    public class ElasticSearchClient
    {
        private ElasticSearchClient() { }

        private static ElasticClient _instance;

        public static ElasticClient GetInstance()
        {
            if (_instance == null)
            {
                var defaultIndex = "searchdata";
                var pool = new SingleNodeConnectionPool(new System.Uri(ConfigHelper.GetByKey("ElasticPortal")));

                var settings = new ConnectionSettings(pool)
                    .DefaultIndex(defaultIndex)
                    .DisableDirectStreaming()
                    .PrettyJson()
                    .RequestTimeout(TimeSpan.FromMinutes(2))
                    .OnRequestCompleted(callDetails =>
                    {
                        if (callDetails.RequestBodyInBytes != null)
                        {
                            Log.Debug(
                                $"{callDetails.HttpMethod} {callDetails.Uri} \n" +
                                $"{Encoding.UTF8.GetString(callDetails.RequestBodyInBytes)}");
                        }
                        else
                        {
                            Log.Debug($"{callDetails.HttpMethod} {callDetails.Uri}");
                        }

                        if (callDetails.ResponseBodyInBytes != null)
                        {
                            Log.Debug($"Status: {callDetails.HttpStatusCode}\n" +
                                     $"{Encoding.UTF8.GetString(callDetails.ResponseBodyInBytes)}\n" +
                                     $"{new string('-', 30)}\n");
                        }
                        else
                        {
                            Log.Debug($"Status: {callDetails.HttpStatusCode}\n" +
                                     $"{new string('-', 30)}\n");
                        }
                    });

                _instance = new ElasticClient(settings);

            }
            return _instance;
        }
    }
}