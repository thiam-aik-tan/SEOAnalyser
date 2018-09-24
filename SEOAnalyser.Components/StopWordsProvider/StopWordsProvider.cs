using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace SEOAnalyser.Components
{
    public class StopWordsProvider : IStopWordsProvider
    {
        #region Constructor

        public StopWordsProvider()
        {
        }

        public StopWordsProvider(string path)
        {
            Path = path;
        }

        #endregion

        #region Properties

        /// <summary>
        /// File Path
        /// </summary>
        public string Path { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of stop-words
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetList()
        {
            return File.ReadLines(Path);
        }

        #endregion
    }
}
