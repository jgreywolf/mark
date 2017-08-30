using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdig;

namespace mark.Processor
{
    class MarkProcessor
    {
        private static MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public static String ToHtml(String src)
        {
            String result = Markdown.ToHtml(src, pipeline);
            return result;
        }
    }
}
