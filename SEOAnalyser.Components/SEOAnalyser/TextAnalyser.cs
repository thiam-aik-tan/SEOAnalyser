using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SEOAnalyser.Components
{
    public class TextAnalyser : ISEOAnalyser
    {
        public TextAnalyser()
        #region Constructor

        {
        }

        public TextAnalyser(IStopWordsProvider provider)
        {
            Provider = provider;
        }

        #endregion

        #region Properties

        public IStopWordsProvider Provider { get; set; }

        #endregion

        #region Methods

        public virtual Dictionary<string, int> Analyse(string content)
        {
            var stopwords = Provider.GetList();

            var result = stopwords.AsParallel()
                .ToDictionary(s => s, t => Regex.Matches(content, string.Format(@"\b{0}\b", t), RegexOptions.IgnoreCase).Count)
                .Where(s => s.Value > 0)
                .ToDictionary(s => s.Key, t => t.Value);

            return result;
        }

        #endregion
    }
}
