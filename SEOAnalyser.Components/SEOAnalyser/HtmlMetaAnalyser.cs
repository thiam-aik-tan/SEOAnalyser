using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SEOAnalyser.Components
{
    public class HtmlMetaAnalyser : TextAnalyser, ISEOAnalyser
    {
        #region Constructor

        public HtmlMetaAnalyser()
        {
        }

        public HtmlMetaAnalyser(IStopWordsProvider provider)
            : base(provider)
        {
        }

        #endregion

        #region Methods

        public override Dictionary<string, int> Analyse(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var metas = doc.DocumentNode.SelectNodes("//meta");
            var contents = (metas == null) ? Enumerable.Empty<string>() : metas.Select(node => node.GetAttributeValue("Content", ""));

            return base.Analyse(string.Join(" ", contents));
        }

        #endregion
    }
}
