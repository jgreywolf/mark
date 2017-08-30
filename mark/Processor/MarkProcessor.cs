using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdig;
using System.Windows.Forms;

namespace mark.Processor
{
    class MarkProcessor
    {
        private static MarkdownPipeline pipeline = null;

        public static void Initialize()
        {
            ToHtml("# This is for initialization");
        }

        public static String ToHtml(String src)
        {
            DateTime start = DateTime.Now;
            if (pipeline == null)
                pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            DateTime end = DateTime.Now;
            TimeSpan span1 = end.Subtract(start);
            
            start = DateTime.Now;
            String result = Markdown.ToHtml(src, pipeline);
            end = DateTime.Now;
            TimeSpan span2 = end.Subtract(start);
            //MessageBox.Show(span1.TotalMilliseconds + " ms\n" + span2.TotalMilliseconds + " ms");

            return result;
        }
    }
}
