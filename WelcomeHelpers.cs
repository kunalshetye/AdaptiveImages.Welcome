using AdaptiveImages.Providers;
using AdaptiveImages.Welcome.REST;
using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adaptive.Features
{
    public static class WelcomeHelpers
    {
        public static IServiceCollection AddWelcomeAdaptiveImageProvider(
            this IServiceCollection services,
            string clientId,
            string clientSecret)
        {
            services.AddTransient<IWelcomeClient>(s => new WelcomeClient(
                s.GetService<ILogger<WelcomeClient>>(), clientId, clientSecret));
            ImageProviderFactory.Instance.Register(new WelcomeImageProvider());
            return services;
        }
        public static string AddQueryString(string url, string name, string val)
        {
            if (name.StartsWith("?") || name.StartsWith("&"))
                name = name.Remove(0, 1);
            if (!name.EndsWith("="))
                name += "=";
            int num1 = url.IndexOf('?');
            if (num1 < 0)
                return url + "?" + name + val;
            string str1 = url;
            int length1 = str1.Length;
            int startIndex = num1 + 1;
            int num2 = startIndex;
            int length2 = length1 - num2;
            string[] strArray = str1.Substring(startIndex, length2).Split('&');
            for (int index1 = 0; index1 < strArray.Length; ++index1)
            {
                if (strArray[index1].StartsWith(name))
                {
                    strArray[index1] = name + val;
                    string str2 = string.Empty;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                        str2 = index2 != 0 ? str2 + "&" + strArray[index2] : str2 + strArray[index2];
                    return url.Substring(0, num1 + 1) + str2;
                }
            }
            return url + "&" + name + val;
        }
    }
}