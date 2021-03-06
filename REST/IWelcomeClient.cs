using System.Collections.Generic;
using AdaptiveImages.Welcome.REST.Media;

namespace AdaptiveImages.Welcome.REST
{
    public interface IWelcomeClient
    {
        MediaPageList GetAssets(int offset = 0, int pageSize = 100, params KeyValuePair<string, string>[] pairs);

        ImageItem GetAsset(string id);

        FolderPageList GetFolders(int offset = 0, int pageSize = 100, params KeyValuePair<string, string>[] pairs);

        Folder GetFolder(string id);

        RawItem GetRawItem(string id);

        VideoItem GetVideo(string id);

        ArticleItem GetArticle(string id);
    }
}