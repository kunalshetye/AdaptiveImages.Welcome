using System;
using AdaptiveImages.Providers;
using EPiServer;
using EPiServer.Framework.Cache;

namespace Adaptive.Features.AdaptiveImageProviders.Welcome
{
    public class ImageProviderAssetCache
    {
        private static void VerifyParameters(string imageId, string providerName)
        {
            if (string.IsNullOrWhiteSpace(imageId))
                throw new ArgumentNullException(nameof(imageId));
            if (string.IsNullOrWhiteSpace(providerName))
                throw new ArgumentNullException(nameof(providerName));
        }

        /// <summary>
        /// Constructs a cache key used to store/retrieve image provider assets.
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        private static string GetAssetCacheKey(string imageId, string providerName)
        {
            ImageProviderAssetCache.VerifyParameters(imageId, providerName);
            return "asset_" + providerName + "_" + imageId;
        }

        /// <summary>Retrieves an asset from cache.</summary>
        /// <param name="imageId"></param>
        /// <param name="providerName"></param>
        /// <returns>Null if asset was not in cache.</returns>
        public static IImageProviderAsset? GetAssetFromCache(
            string imageId,
            string providerName)
        {
            ImageProviderAssetCache.VerifyParameters(imageId, providerName);
            return CacheManager.Get(ImageProviderAssetCache.GetAssetCacheKey(imageId, providerName)) as
                IImageProviderAsset;
        }

        /// <summary>Adds an asset to the cache.</summary>
        /// <param name="asset"></param>
        /// <exception cref="T:System.ArgumentException"></exception>
        public static void AddAssetToCache(IImageProviderAsset asset)
        {
            if (string.IsNullOrWhiteSpace(asset.Id) || string.IsNullOrWhiteSpace(asset.Provider))
                throw new ArgumentException("Asset must have an image ID and provider name", nameof(asset));
            CacheManager.Insert(ImageProviderAssetCache.GetAssetCacheKey(asset.Id, asset.Provider), (object) asset,
                new CacheEvictionPolicy(TimeSpan.FromSeconds(30.0), CacheTimeoutType.Absolute));
        }
    }
}