using AutoMapper;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using SPDC.Common;
using SPDC.Model.BindingModels;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Service.Services;
using SPDC.WebAPI.Helpers;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SPDC.WebAPI.Controllers
{

    [RoutePrefix("api/Search")]
    [EnableCorsAttribute("*", "*", "*")]
    public class FullTextSearchController : ApiControllerBase
    {
        private ICourseService _courseService;
        private ICMSContentService _cmsContentService;
        private IClientService _clientService;
        private IDocumentService _documentService;

        public FullTextSearchController(ICourseService courseService, ICMSContentService cmsContentService, IClientService clientService, IDocumentService documentService)
        {
            _courseService = courseService;
            _cmsContentService = cmsContentService;
            _clientService = clientService;
            _documentService = documentService;
        }
        // GET: FullTextSearch
        [HttpGet]
        [Route("Data")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> UpdateDataAsync()
        {
            var defaultIndex = "searchdata";

            ElasticClient client = ElasticSearchClient.GetInstance();
            List<SearchModel> dataList = new List<SearchModel>();
            var siteUrl = await _clientService.GetClientUrlByNameAsync("ApplicantPortal");
            var dataCourse = await _courseService.CloneDataToElastic();
            var dataCMS = await _cmsContentService.CloneDataToElastic();
            var dataDocument = await _documentService.GetCourseDocuments();

            dataList.AddRange(dataCourse.ToElasticSearchList(siteUrl));
            dataList.AddRange(dataCMS.ToElasticSearchList(siteUrl));
            dataList.AddRange(dataDocument.ToElasticSearchList(siteUrl));
            Log.Debug("Start Create Index");

            if (client.Indices.Exists(defaultIndex).Exists)
            {
                var deleteIndexResponse = client.Indices.Delete(defaultIndex);
                Log.Debug("Delete index" + deleteIndexResponse.ApiCall.DebugInformation);
            }

            var createIndexResponse = client.Indices.Create(defaultIndex, c => c
                .Settings(s => s
                    .Analysis(a => a
                        .Analyzers(ad => ad
                            .Custom("windows_path_hierarchy_analyzer", ca => ca
                                .Tokenizer("windows_path_hierarchy_tokenizer")
                            )
                        )
                        .Tokenizers(t => t
                            .PathHierarchy("windows_path_hierarchy_tokenizer", ph => ph
                                .Delimiter('\\')
                            )
                        )
                    )
                )
                .Map<SearchModel>(mp => mp
                    .AutoMap()
                    .Properties(ps => ps
                        .Text(s => s
                            .Name(n => n.Path)
                            .Analyzer("windows_path_hierarchy_analyzer")
                        )
                        .Object<Attachment>(a => a
                            .Name(n => n.Attachment)
                            .AutoMap()
                        )
                    )
                )
            );
            Log.Debug("Create index " + createIndexResponse.ApiCall.DebugInformation);
            Log.Debug("Create index " + createIndexResponse.ApiCall.OriginalException?.Message);

            var putPipelineResponse = client.Ingest.PutPipeline("attachments", p => p
              .Description("Document attachment pipeline")
              .Processors(pr => pr
                .Attachment<SearchModel>(a => a
                  .Field(f => f.Content)
                  .TargetField(f => f.Attachment)
                )
                .Remove<SearchModel>(r => r
                  .Field(ff => ff
                    .Field(f => f.Content)
                  )
                )
              )
            );

            Log.Debug("Create Pipeline " + putPipelineResponse.ApiCall.Success);
            Log.Debug("Create Pipeline " + putPipelineResponse.ApiCall.DebugInformation);
            Log.Debug("Create Pipeline " + putPipelineResponse.ApiCall.OriginalException?.Message);

            if (!putPipelineResponse.ApiCall.Success)
            {
                return BadRequest("Failure to Create Pipeline");
            }

            bool responseChecking = false;
            foreach (var item in dataList)
            {

                if (item.DataType.Equals("document"))
                {
                    var res = await client.IndexAsync(item, i => i
                                                      .Pipeline("attachments")
                                                      .Refresh(Refresh.WaitFor));
                    responseChecking = res.ApiCall.Success;
                    Log.Debug("Create document" + res.ApiCall.DebugInformation);
                    Log.Debug("Create document" + res.ApiCall.OriginalException?.Message);
                }
                else
                {
                    var res = await client.IndexDocumentAsync(item);
                    responseChecking = res.ApiCall.Success;
                    Log.Debug("Create record" + res.ApiCall.DebugInformation);
                    Log.Debug("Create record" + res.ApiCall.OriginalException?.Message);
                }

                if (!responseChecking)
                {
                    break;
                }
            }

            if (responseChecking)
            {
                return Ok("Data up to date");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost]
        [Route("Title")]
        public async Task<IHttpActionResult> SearchTitleAsync(FullTextSearchBindingModel model)
        {
            ElasticClient client = ElasticSearchClient.GetInstance();
            model.Keyword = Regex.Replace(model.Keyword, @"(\s+|@|&|'|\(|\)|<|>|#)", " ");
            model.Keyword = model.Keyword.Trim();

            var searchResponse = await client.SearchAsync<SearchModel>(s => s
                                             .From(model.Page * model.Records)
                                             .Size(model.Records)
                                             .Query(q => q
                                                .Bool(b => b
                                                    //.Filter(bs => bs.Wildcard(p => p.Title, model.Keyword))
                                                    //.Filter(bs => bs.Term(p => p.IsVisible, true) && bs.QueryString(p => p.Fields(f => f.Field(ff => ff.Title)).Query(model.Keyword + "*")))))
                                                    .Filter(bs => bs.Term(p => p.IsVisible, true) && bs.MatchPhrasePrefix(p => p.Field(ff => ff.Title).Query(model.Keyword)))))
                                             //.Sort(q => q.Descending(o => o.PublishDate))
                                             .Highlight(h => h
                                                .PreTags("<mark>")
                                                .PostTags("</mark>")
                                                .Fields(
                                                    f => f.Field(p => p.Title)
                                                )
                                            ));

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, new
            {
                total = searchResponse.Total,
                data = searchResponse.Documents,
                highlight = searchResponse.HitsMetadata.Hits
            }));
        }

        [HttpPost]
        [Route("Content")]
        public async Task<IHttpActionResult> SearchContentAsync(FullTextSearchBindingModel model)
        {
            ElasticClient client = ElasticSearchClient.GetInstance();
            model.Keyword = Regex.Replace(model.Keyword, @"(\s+|@|&|'|\(|\)|<|>|#)", " ");
            model.Keyword = model.Keyword.Trim();

            var searchResponse = await client.SearchAsync<SearchModel>(s => s
                                            .From(model.Page * model.Records)
                                            .Size(model.Records)
                                            .Query(q => q
                                               .Bool(b => b
                                                   //.Must(bs1 => bs1.Match(p => p.Field(f => f.Attachment.Content).Query(model.Keyword)) || bs1.Wildcard(p => p.Data, model.Keyword) || bs1.Wildcard(p => p.Title, model.Keyword))
                                                   .Filter(bs => bs.Term(p => p.IsVisible, true)
                                                        && (bs.Match(p => p.Field(f => f.Attachment.Content).Query(model.Keyword))
                                                        //|| bs.QueryString(p => p.Fields(f => f.Field(ff => ff.Title)).Query("*" + model.Keyword + "*"))
                                                        //|| bs.QueryString(p => p.Fields(f => f.Field(ff => ff.Data)).Query("*" + model.Keyword + "*")))
                                                        || bs.MatchPhrasePrefix(f => f.Field(ff => ff.Title).Query(model.Keyword))
                                                        || bs.MatchPhrasePrefix(f => f.Field(ff => ff.Data).Query(model.Keyword)))
                                                        )))
                                            //.Sort(q => q.Descending(o => o.PublishDate))
                                            .Highlight(h => h
                                               .PreTags("<mark>")
                                               .PostTags("</mark>")
                                               .Fields(
                                                   f => f.Field(p => p.Title),
                                                   f => f.Field(p => p.Data),
                                                   f => f.Field(p => p.Attachment.Content)
                                               )
                                           ));

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, new
            {
                total = searchResponse.Total,
                data = searchResponse.Documents,
                highlight = searchResponse.HitsMetadata.Hits
            }));
        }
    }
}