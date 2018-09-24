# SEOAnalyser
Implement simplified SEO Analyser: web application that performs a simple SEO analysis of the text.

# Installation
1. Download the solution to the local repository.
2. Open the solution using Visual Studion 2017.
3. Compile and build the solution.
4. Right-click [SEOAnalyserWeb] project and select "Publish" to a specified folder, for example: C:\SEOAnalyserWeb.
5. Launch IIS and create a new Website pointing to C:\SEOAnalyserWeb.  Make sure the application pool is using .NET Framework v4.0 or above.

# User Guide
1. Launch the website by specifying http://localhost/ at the browser address.
2. Once SEO Analyser page load successfully, select [Analysis Options] checkboxes as below:
   a) Number Of Stop-Word Occurences On The Page
   b) Number of Stop-Word Occurences In Meta Tags
   c) Number Of External Links In The Text
3. Then, enter the text to analyse or a website url for analysis.
4. Lastly, Click 'Analyse' button to run SEO Analyser.
