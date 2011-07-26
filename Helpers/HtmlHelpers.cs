using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc; 

namespace BetterTaskList.Helpers
{
    public class HtmlHelpers
    {
        public static class PagingHelpers
        {
            public static string PageLinks(int currrentPage, int totalPages, Func<int, string> pageUrl, string urlSeperator = "")
            {
                StringBuilder result = new StringBuilder();
                for (int i = 1; i <= totalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a"); // Construct an <a> tag
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();
                    if (i == currrentPage)
                        tag.AddCssClass("selected");
                    result.AppendLine(tag.ToString() + urlSeperator);

                }
                return result.ToString();
            }
        }

        public static class UIHelpers
        {

            public static string HighlightKeywords(string input, string keywords)
            {
                if (input == string.Empty || keywords == string.Empty)
                {
                    return input;
                }

                string[] sKeywords = keywords.Split(' ');
                foreach (string sKeyword in sKeywords)
                {
                    try
                    {
                        input = Regex.Replace(input, sKeyword, string.Format("<span class=\"highlight\">{0}</span>", "$0"), RegexOptions.IgnoreCase);
                    }
                    catch
                    {
                        //
                    }
                }
                return input;
            }

            public static string TurnBoldKeywords(string input, string keywords)
            {
                if (input == string.Empty || keywords == string.Empty)
                {
                    return input;
                }

                string[] sKeywords = keywords.Split(' ');
                foreach (string sKeyword in sKeywords)
                {
                    try
                    {
                        input = Regex.Replace(input, sKeyword, string.Format("<b><u>{0}</u></b>", "$0"), RegexOptions.IgnoreCase);
                    }
                    catch{}
                }
                return input;
            }  
        }
       

    }
}