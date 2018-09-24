using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEOAnalyserWeb.Models
{
    public class ResultModel
    {
        public ResultModel()
        {
            WordOccurenceOnText = new Dictionary<string, int>();
            WordOccurenceOnPage = new Dictionary<string, int>();
            WordOccurenceOnMeta = new Dictionary<string, int>();
            ExternalLinkOccurence = new Dictionary<string, int>();
        }

        public Dictionary<string, int> WordOccurenceOnText { get; set; }

        public Dictionary<string, int> WordOccurenceOnPage { get; set; }

        public Dictionary<string, int> WordOccurenceOnMeta { get; set; }

        public Dictionary<string, int> ExternalLinkOccurence { get; set; }
    }
}