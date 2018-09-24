using System.Collections.Generic;

namespace SEOAnalyser.Components
{
    public interface ISEOAnalyser
    {
        IStopWordsProvider Provider { get; set; }

        Dictionary<string, int> Analyse(string content);
    }
}