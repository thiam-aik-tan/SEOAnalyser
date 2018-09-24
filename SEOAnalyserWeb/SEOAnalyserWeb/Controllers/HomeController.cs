using SEOAnalyser.Components;
using SEOAnalyserWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SEOAnalyserWeb.Controllers
{
    public class HomeController : Controller
    {
        #region Constructor

        public HomeController()
            : this(new UrlUtility(), new StopWordsProvider(), new TextAnalyser(), new HtmlBodyAnalyser(), new HtmlMetaAnalyser(), new HtmlAnchorAnalyser())
        {
        }
        
        public HomeController(IUrlUtility utility, IStopWordsProvider provider, ISEOAnalyser textAnalyser, ISEOAnalyser bodyAnalyser, ISEOAnalyser metaAnalyser, ISEOAnalyser linkAnalyser)
        {
            Utility = utility;

            Provider = provider;
            Provider.Path = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["StopWordsFileLocation"]);

            TextAnalyser = textAnalyser;
            BodyAnalyser = bodyAnalyser;
            MetaAnalyser = metaAnalyser;
            LinkAnalyser = linkAnalyser;

            TextAnalyser.Provider = BodyAnalyser.Provider = MetaAnalyser.Provider = provider;
        }

        #endregion

        #region Properties

        private IUrlUtility Utility { get; set; }
        private IStopWordsProvider Provider { get; set; }
        private ISEOAnalyser TextAnalyser { get; set; }
        private ISEOAnalyser BodyAnalyser { get; set; }
        private ISEOAnalyser MetaAnalyser { get; set; }
        private ISEOAnalyser LinkAnalyser { get; set; }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AnalysisModel model)
        {
            if (!ModelState.IsValid) { return View(); }

            
            var content = string.Empty;
            if (!Utility.IsValidUrl(model.Text))
            {
                return View("Result", new ResultModel
                {
                    WordOccurenceOnText = TextAnalyser.Analyse(model.Text)
                });
            }
            
            if (!Utility.TryGetContent(model.Text, out string htmlContent))
            {
                ModelState.AddModelError("", "The Url is invalid.");
                return View();
            }

            return View("Result", new ResultModel
            {
                WordOccurenceOnPage = model.WordOccurenceOnPage ? BodyAnalyser.Analyse(htmlContent) : new Dictionary<string, int>(),
                WordOccurenceOnMeta = model.WordOccurenceOnMeta ? MetaAnalyser.Analyse(htmlContent) : new Dictionary<string, int>(),
                ExternalLinkOccurence = model.ExternalLinkOccurence ? LinkAnalyser.Analyse(htmlContent) : new Dictionary<string, int>()
            });
        }

        public ActionResult Result()
        {
            return View();
        }

        #endregion
    }
}