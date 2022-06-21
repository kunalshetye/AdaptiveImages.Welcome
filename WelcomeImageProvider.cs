using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Adaptive.Features.AdaptiveImageProviders.Welcome;
using AdaptiveImages.Providers;
using AdaptiveImages.Welcome.REST;
using AdaptiveImages.Welcome.REST.Media;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Logging;

namespace Adaptive.Features
{
    public class WelcomeImageProvider : IImageProvider
    {
        public string Name => "welcome";

        public string DisplayName => "Welcome";

        public ImageProviderMetaDataProperty MetadataProperties =>
            ImageProviderMetaDataProperty.Name | ImageProviderMetaDataProperty.Description;
        public IList<IImageProviderOption> Options => new List<IImageProviderOption>
        {
            new ImageProviderOption()
            {
                Id = "all",
                DisplayName = "All Assets",
                Capabilities = ImageProviderOptionCapability.Search | ImageProviderOptionCapability.List,
                SearchResultPageSize = 10
            },
            new ImageProviderOption()
            {
                Id = "images",
                DisplayName = "Images",
                Capabilities = ImageProviderOptionCapability.Search,
                SearchResultPageSize = 10
            },
            new ImageProviderOption()
            {
                Id = "videos",
                DisplayName = "Videos",
                Capabilities = ImageProviderOptionCapability.Search,
                SearchResultPageSize = 10
            },
            new ImageProviderOption()
            {
                Id = "Documents",
                DisplayName = "Documents",
                Capabilities = ImageProviderOptionCapability.Search,
                SearchResultPageSize = 10
            }
        };
        
        private Injected<IWelcomeClient> _welcomeClient;
        private IWelcomeClient welcomeClient => _welcomeClient.Service;
        
        private Injected<ILogger<WelcomeImageProvider>> _logger;
        private ILogger<WelcomeImageProvider> logger => _logger.Service;
        public async Task<IImageProviderAsset> GetAsync(string id)
        {
            IImageProviderAsset assetFromCache = ImageProviderAssetCache.GetAssetFromCache(id, this.Name);
            if (assetFromCache != null)
                return assetFromCache;
            var asset = welcomeClient.GetAsset(id);
            return ConvertToIImageProviderAsset(asset);
        }

        public async Task<IImageProviderAssetData> DownloadAsync(string id)
        {
            var asset = welcomeClient.GetAsset(id);
            using HttpClient client = new HttpClient();
            byte[] byteArrayAsync = await client.GetByteArrayAsync(asset.Url);
            var providerAssetData = (IImageProviderAssetData) new ImageProviderAssetData()
            {
                Bytes = byteArrayAsync,
                MimeType = asset.MimeType
            };
            return providerAssetData;
        }

        public async Task<string> GetUrlAsync(string id)
        {
            var imageProviderAsset = await GetAsync(id);
            return imageProviderAsset.Thumbnail;
        }

        public async Task<IList<IImageProviderAsset>> SearchAsync(string criteria, IEnumerable<IImageProviderOption> options = null, int? searchResultPage = null)
        {
            var getAssetParams = new List<KeyValuePair<string, string>>
            {
                new("include_subfolder_assets", "true"),
                new("type", "image")
            };
            var result = new List<IImageProviderAsset>();
            var imageAssets = welcomeClient.GetAssets((searchResultPage.Value - 1) * 10, 10, getAssetParams.ToArray());
            foreach (var asset in imageAssets.Assets)
            {
                result.Add(ConvertToIImageProviderAsset(welcomeClient.GetAsset(asset.Id)));
            }
            return imageAssets.Assets.ConvertAll(ConvertMediaItemToImageProviderAsset);
        }

        public Task<IImageProviderAsset> UploadAsync(IImageProviderAsset metadata, IImageProviderAssetData payload, IEnumerable<IImageProviderOption> options = null)
        {
            throw new NotSupportedException("Not supported by image provider '" + this.Name + "'");
        }

        public Task<IImageProviderAsset> UpdateAsync(IImageProviderAsset metadata, IEnumerable<IImageProviderOption> options = null)
        {
            throw new NotSupportedException("Not supported by image provider '" + this.Name + "'");
        }

        private IImageProviderAsset ConvertMediaItemToImageProviderAsset(MediaItem mediaItem)
        {
            return ConvertToIImageProviderAsset(welcomeClient.GetAsset(mediaItem.Id));
        }

        private IImageProviderAsset ConvertToIImageProviderAsset(ImageItem imageItem)
        {
            var thumbnailUrl = imageItem.Url;
            thumbnailUrl = WelcomeHelpers.AddQueryString(thumbnailUrl, "width", "132");
            thumbnailUrl = WelcomeHelpers.AddQueryString(thumbnailUrl, "height", "88");
            
            ImageProviderAsset imageProviderAsset = new ImageProviderAsset()
            {
                Id = imageItem.Id,
                Name = imageItem.Title,
                Width = imageItem.ImageResolution.Width,
                Height = imageItem.ImageResolution.Height,
                Provider = this.Name,
                Description = imageItem.Description,
                Thumbnail = thumbnailUrl,
                Filesize = long.Parse(imageItem.FileSize.ToString())
            };
            return imageProviderAsset;
        }
    }
}