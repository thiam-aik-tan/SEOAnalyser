using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEOAnalyserWeb.Models
{
    public class AnalysisModel
    {
        public bool WordOccurenceOnPage { get; set; }


        public bool WordOccurenceOnMeta { get; set; }


        public bool ExternalLinkOccurence { get; set; }

        [Required(ErrorMessage = "Text or URL is required.")]
        public string Text { get; set; }
    }
}