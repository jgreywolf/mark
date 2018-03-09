using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace websitegen.Utils
{
    class HtmlGenerator
    {
        private static readonly Regex urlRegex = new Regex(@"^(http://|https://|ftp://|file:///)", RegexOptions.IgnoreCase);
        private static MarkdownPipeline pipeline = null;

        public static string MarkdownToHtml(string src, string srcUrl, bool transformUrl = true, bool forWebsite = false)
        {
            if (pipeline == null)
                pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            
            var builder = new StringBuilder();
            var textwriter = new StringWriter(builder);
            var renderer = new HtmlRenderer(textwriter);
            pipeline.Setup(renderer);

            MarkdownDocument doc = Markdown.Parse(src, pipeline);
            if (transformUrl)
                TransformUrl(doc, srcUrl, forWebsite);
            renderer.Render(doc);
            string result = builder.ToString();

            return result;
        }

        private static void TransformUrl(MarkdownObject markdownObject, string srcUrl, bool forWebsite)
        {
            if (!forWebsite)
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
                            link.Url = srcUrl + "/" + url;
                        }
                    }
                    TransformUrl(child, srcUrl, forWebsite);
                }
            } else
            {
                foreach (MarkdownObject child in markdownObject.Descendants())
                {
                    // LinkInline can be both an image or a <a href="...">
                    LinkInline link = child as LinkInline;
                    if (link != null)
                    {
                        string url = link.Url;
                        if (!urlRegex.IsMatch(url) && url.StartsWith("."))
                        {
                            if (url.EndsWith(".md") || url.EndsWith(".markdown"))
                            {
                                url = url.Substring(0, url.LastIndexOf(".")) + ".html";
                            }
                            link.Url = srcUrl + "/" + url;
                        }
                    }
                    TransformUrl(child, srcUrl, forWebsite);
                }
            }
        }
    }
}
