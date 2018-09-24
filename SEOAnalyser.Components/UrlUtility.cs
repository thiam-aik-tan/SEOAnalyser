using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEOAnalyser.Components
{
    public class UrlUtility : IUrlUtility
    {
        #region Constructor

        public UrlUtility()
            : this(new[] { Uri.UriSchemeHttp, Uri.UriSchemeHttps })
        {
        }

        public UrlUtility(IEnumerable<string> schemes)
        {
            Schemes = schemes;
        }

        #endregion

        #region Properties

        public IEnumerable<string> Schemes { get; set; }

        #endregion

        #region Methods

        public bool IsValidUrl(string url)
        {
            return (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                (Schemes.Count() == 0 || Schemes.Contains(uriResult.Scheme)));
        }

        public bool TryGetContent(string url, out string htmlContent)
        {
            htmlContent = GetContent(url);

            return !string.IsNullOrEmpty(htmlContent);
        }

        public string GetContent(string url)
        {
            if (!IsValidUrl(url))
            {
                return string.Empty;
            }

            try
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);
                return doc.DocumentNode.OuterHtml;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
