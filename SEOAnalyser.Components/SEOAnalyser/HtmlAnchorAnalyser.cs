using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SEOAnalyser.Components
{
    public class HtmlAnchorAnalyser : ISEOAnalyser
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlAnchorAnalyser()
        {
        }

        #endregion

        #region Properties

        public IStopWordsProvider Provider { get; set; }

        #endregion

        #region Methods

        public Dictionary<string, int> Analyse(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var bodyNode = doc.DocumentNode.SelectSingleNode("//body");

            if (bodyNode == null){ return new Dictionary<string, int>(); }

            var anchors = bodyNode.SelectNodes("//a[@href]")
                .Select(node => node.GetAttributeValue("href", ""))
                .AsParallel()
                .Where(s =>
                {
                    return (Uri.TryCreate(s, UriKind.Absolute, out Uri uriResult) &&
                        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps));
                });

            return anchors.GroupBy(s => s).ToDictionary(s => s.Key, t => t.Count());
        }

        #endregion
    }
}
