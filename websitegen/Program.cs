using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using websitegen.Utils;

namespace websitegen
{
    class Program
    {
        // G:\VisualStudio\mark\websitegen\bin\Debug\
        static string startupPath = AppDomain.CurrentDomain.BaseDirectory;
        // G:\VisualStudio\mark\websitegen\bin\Debug
        static string currentDir = Environment.CurrentDirectory;

        static ConfigUtils configUtils;
        static string baseUrl;
        static string title;

        static string style;

        static void Main(string[] args)
        {
            Console.WriteLine("startupPath=" + startupPath);
            Console.WriteLine("currentDir=" + currentDir);

            // if testing
            if (currentDir.EndsWith("bin\\Debug"))
            {
                Console.WriteLine("testing, changing currentDir to blog\n");
                currentDir = currentDir + "/blog";
            }
            
            // read config file
            if (!File.Exists(currentDir + "/websitegen.config"))
                File.WriteAllText(currentDir + "/websitegen.config", Properties.Resources.websitegen);
            configUtils = new ConfigUtils(currentDir + "/websitegen.config");
            baseUrl = configUtils.get("baseUrl");
            title = configUtils.get("title");

            // read style.css
            if (!File.Exists(currentDir + "/style.css"))
            {
                File.WriteAllText(currentDir + "/style.css", Properties.Resources.style);
                style = Properties.Resources.style;
            }
            else
            {
                style = File.ReadAllText(currentDir + "/style.css");
            }

            // Get all categories
            DirectoryInfo directory = new DirectoryInfo(currentDir + "/src");
            DirectoryInfo[] categorieList = directory.GetDirectories();
            StringBuilder readme = new StringBuilder();

            foreach (DirectoryInfo category in categorieList)
            {
                //Console.WriteLine("\n" + category.Name + "\n");
                readme.Append("## " + category.Name + "\n");

                // Get all markdown files in current category
                FileInfo[] mdFileList =  category.GetFiles();
                foreach (FileInfo mdFile in mdFileList)
                {
                    //Console.WriteLine(mdFile.Name);
                    if (mdFile.Name.EndsWith(".md") || mdFile.Name.EndsWith(".markdown"))
                    {
                        string fileNameWithoutExt = mdFile.Name.Substring(0, mdFile.Name.LastIndexOf("."));
                        Console.WriteLine(mdFile.Name);
                        readme.Append("* [" + fileNameWithoutExt + "](./" + category + "/" + mdFile.Name + ")\n");

                        // read from file
                        string mdText = File.ReadAllText(mdFile.FullName);

                        // convert to html
                        string htmlBody = HtmlGenerator.MarkdownToHtml(mdText, baseUrl + "/src/" + category);
                        string html = "<html>\n<head>\n" + "<meta charset=\"utf-8\">\n"
                            + "<title>" + fileNameWithoutExt + "</title>\n"
                            + "<style>\n" + style + "\n</style>\n"
                            + "</head>\n"
                            + "<body id=\"body\" style=\"background: white; margin-left: 50px\">\n"
                            + htmlBody
                            + "\n</body></html>";
                        //Console.WriteLine(html);

                        // write to file
                        string toDir = currentDir + "/website/" + category;
                        if (!Directory.Exists(toDir))
                            Directory.CreateDirectory(toDir);
                        File.WriteAllText(toDir + "/" + fileNameWithoutExt + ".html", html);
                    }
                }

                readme.AppendLine();
            }

            // generate README.md
            string readmeText = readme.ToString();
            //Console.WriteLine(readmeText);
            File.WriteAllText(currentDir + "/src/README.md", readmeText);

            // generate index.html
            string indexHtmlBody = HtmlGenerator.MarkdownToHtml(readmeText, "website", true, true);
            string indexHtml = "<html>\n<head>\n" + "<meta charset=\"utf-8\">"
                            + "<title>" + title + "</title>"
                            + "<style>\n" + style + "\n</style>\n"
                            + "</head>\n"
                            + "<body id=\"body\" style=\"background: white; margin-left: 50px\">\n"
                            + indexHtmlBody
                            + "\n</body></html>";
            //Console.WriteLine(indexHtml);
            File.WriteAllText(currentDir + "/index.html", indexHtml);
        }
    }
}
