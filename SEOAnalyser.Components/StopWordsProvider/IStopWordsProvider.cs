using System.Collections.Generic;

namespace SEOAnalyser.Components
{
    public interface IStopWordsProvider
    {
        string Path { get; set; }

        IEnumerable<string> GetList();
    }
}