using System.Collections.Generic;

namespace SEOAnalyser.Components
{
    public interface IUrlUtility
    {
        IEnumerable<string> Schemes { get; set; }

        bool IsValidUrl(string url);
        bool TryGetContent(string url, out string htmlContent);
        string GetContent(string url);
    }
}