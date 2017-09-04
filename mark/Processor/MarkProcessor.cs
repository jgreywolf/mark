using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdig;
using System.Windows.Forms;
using Markdig.Syntax;
using System.IO;
using Markdig.Syntax.Inlines;
using Markdig.Renderers;
using System.Text.RegularExpressions;

namespace mark.Processor
{
    class MarkProcessor
    {
        private static readonly Regex urlRegex = new Regex(@"^(http://|https://|ftp://|file:///)", RegexOptions.IgnoreCase);
        private static MarkdownPipeline pipeline = null;
        
        public static string ToHtml(string src, string currentDir, bool transformUrl=true)
        {
            DateTime start = DateTime.Now;
            if (pipeline == null)
                pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            DateTime end = DateTime.Now;
            TimeSpan span1 = end.Subtract(start);

            start = DateTime.Now;
            var builder = new StringBuilder();
            var textwriter = new StringWriter(builder);
            var renderer = new HtmlRenderer(textwriter);
            pipeline.Setup(renderer);

            MarkdownDocument doc = Markdown.Parse(src, pipeline);
            if (transformUrl)
                TransformUrl(doc, currentDir);
            renderer.Render(doc);
            string result = builder.ToString();
            end = DateTime.Now;
            TimeSpan span2 = end.Subtract(start);
            //MessageBox.Show(span1.TotalMilliseconds + " ms\n" + span2.TotalMilliseconds + " ms");

            return result;
        }

        private static void TransformUrl(MarkdownObject markdownObject, string currentDir)
        {
            foreach (MarkdownObject child in markdownObject.Descendants())
            {
                // LinkInline can be both an image or a <a href="...">
                LinkInline link = child as LinkInline;
                if (link != null)
                {
                    string url = link.Url;
                    if (!urlRegex.IsMatch(url))
                    {
                        try
                        {
                            if (!Path.IsPathRooted(url) && currentDir != null)
                                url = Path.GetFullPath(Path.Combine(currentDir, url)).Replace('\\', '/');
                            url = "file:///" + url;
                            link.Url = url;
                        }
                        catch (Exception)
                        {
                            return;
                        } 
                    }
                }
                TransformUrl(child, currentDir);
            }
        }
    }
}
