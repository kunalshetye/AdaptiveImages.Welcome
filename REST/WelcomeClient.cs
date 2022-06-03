using System;
using System.Collections.Generic;
using System.Linq;
using Adaptive.Features;
using AdaptiveImages.Welcome.REST.Authorization;
using AdaptiveImages.Welcome.REST.Media;
using EPiServer.Framework.Cache;
using Microsoft.Extensions.Logging;
using Welcome.OptiDAM;

namespace AdaptiveImages.Welcome.REST
{
    public class WelcomeClient : IWelcomeClient
    {
        private readonly Uri AuthorizationUrl = new Uri(WelcomeConstants.AccountsBaseUrl);

        private readonly ILogger<WelcomeClient> _logger;
        private readonly string _clientId;
        private readonly string _clientSecret;

        private SynchronousWebClient httpClient;

        public WelcomeClient(ILogger<WelcomeClient> logger, string clientId, string clientSecret)
        {
            _logger = logger;
            _clientId = clientId;
            _clientSecret = clientSecret;
            if (httpClient == null)
            {
                httpClient = new SynchronousWebClient(logger);
            }
        }

        public MediaPageList GetAssets(int offset = 0, int pageSize = 100, params KeyValuePair<string, string>[] pairs)
        {
            var url = FormatRequestUrl(WelcomeConstants.AssetListPath);
            url = WelcomeHelpers.AddQueryString(url, "page_size", pageSize.ToString());
            if (offset > 0)
            {
                url = WelcomeHelpers.AddQueryString(url, "offset", offset.ToString());
            }
            if (pairs != null && pairs.Any())
            {
                foreach (var kv in pairs)
                {
                    url = WelcomeHelpers.AddQueryString(url, kv.Key, kv.Value);
                }
            }

            httpClient.SetAccessToken(GetToken());
            var assetList = httpClient.Get<MediaPageList>(url);
            return assetList;
        }

        public ImageItem GetAsset(string id)
        {
            var cacheKey = string.Format(WelcomeConstants.GetImagesPath, id);
            httpClient.SetAccessToken(GetToken());
            var asset = httpClient.Get<ImageItem>(FormatRequestUrl(string.Format(WelcomeConstants.GetImagesPath, id)));
            return asset;
        }

        public VideoItem GetVideo(string id)
        {
            httpClient.SetAccessToken(GetToken());
            return httpClient.Get<VideoItem>(FormatRequestUrl(string.Format(WelcomeConstants.GetVideoPath, id)));
        }

        public RawItem GetRawItem(string id)
        {
            httpClient.SetAccessToken(GetToken());
            return httpClient.Get<RawItem>(FormatRequestUrl(string.Format(WelcomeConstants.GetRawItemPath, id)));
        }

        public ArticleItem GetArticle(string id)
        {
            httpClient.SetAccessToken(GetToken());
            return httpClient.Get<ArticleItem>(FormatRequestUrl(string.Format(WelcomeConstants.GetArticlePath, id)));
        }

        public FolderPageList GetFolders(int offset = 0, int pageSize = 100, params KeyValuePair<string, string>[] pairs)
        {
            var cacheKey = $"{WelcomeConstants.FolderListPath}-{offset}-{pageSize}-{string.Join("|", pairs.Select(kvp => $"@{kvp.Key}={kvp.Value}"))}";
            var url = FormatRequestUrl(WelcomeConstants.FolderListPath);
            url = WelcomeHelpers.AddQueryString(url, "page_size", pageSize.ToString());
            if (offset > 0)
            {
                url = WelcomeHelpers.AddQueryString(url, "offset", offset.ToString());
            }
            if (pairs != null && pairs.Any())
            {
                foreach (var kv in pairs)
                {
                    url = WelcomeHelpers.AddQueryString(url, kv.Key, kv.Value);
                }
            }

            httpClient.SetAccessToken(GetToken());
            var folders = httpClient.Get<FolderPageList>(url);

            return folders;
        }

        public Folder GetFolder(string id)
        {
            httpClient.SetAccessToken(GetToken());
            return httpClient.Get<Folder>(FormatRequestUrl(string.Format(WelcomeConstants.GetFolderPath, id)));
        }

        private string GetToken()
        {
            var authorizationRequest = new AuthorizationRequest
            {
                ClientId = this._clientId,
                ClientSecret = this._clientSecret
            };
            var responseToken = httpClient.Post<AuthorizationRequest, AuthorizationResponse>(FormatRequestUrl("o/oauth2/v1/token", WelcomeConstants.AccountsBaseUrl), authorizationRequest);
            if (responseToken != null)
            {
                return responseToken.AccessToken;                
            }

            return null;
        }

        private string FormatRequestUrl(string url, string baseUrl = WelcomeConstants.APIBaseUrl) =>
            string.Concat(baseUrl, url);
    }
}