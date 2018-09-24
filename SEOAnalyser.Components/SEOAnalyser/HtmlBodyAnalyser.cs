using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEOAnalyser.Components
{
    public class HtmlBodyAnalyser : TextAnalyser, ISEOAnalyser
    {
        #region Constructor

        public HtmlBodyAnalyser()
        {
        }

        public HtmlBodyAnalyser(IStopWordsProvider provider)
            : base(provider)
        {
        }

        #endregion

        #region Methods

        public override Dictionary<string, int> Analyse(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var bodyNode = doc.DocumentNode.SelectSingleNode("//body");

            if (bodyNode == null){ return new Dictionary<string, int>(); }

            var nodes = bodyNode.SelectNodes("//style|//script");
            if (nodes != null)
            {
                nodes.ToList().ForEach(node => node.Remove());
            }

            return base.Analyse(bodyNode.InnerText);
        }

        #endregion
    }
}
