﻿using System;
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

        static string toolsDir;
        static string configDir;
        static ConfigUtils configUtils;
        static string baseUrl;
        static string title;

        static string style;
        static string header;
        static string footer;

        static void init()
        {
            Console.WriteLine("startupPath=" + startupPath);
            Console.WriteLine("currentDir=" + currentDir);

            // if testing
            if (currentDir.EndsWith("bin\\Debug"))
            {
                Console.WriteLine("testing, changing currentDir to blog\n");
                currentDir = currentDir + "/blog";
            }

            toolsDir = currentDir + "/tools";
            
            configDir = currentDir + "/config";
            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);
            else Console.WriteLine("using existing config.\n");

            // read config
            if (!File.Exists(configDir + "/websitegen.config"))
                File.WriteAllText(configDir + "/websitegen.config", Properties.Resources.websitegen);
            configUtils = new ConfigUtils(configDir + "/websitegen.config");
            baseUrl = configUtils.get("baseUrl");
            title = configUtils.get("title");


            // read style.css
            string path = configDir + "/style.css";
            if (File.Exists(path))
                style = File.ReadAllText(path);
            else
            {
                style = Properties.Resources.style;
                File.WriteAllText(path, style);
            }

            // read header.html
            path = configDir + "/header.html";
            if (File.Exists(path))
                header = File.ReadAllText(path);
            else
            {
                header = Properties.Resources.header;
                File.WriteAllText(path, header);
            }

            // read footer.html
            path = configDir + "/footer.html";
            if (File.Exists(path))
                footer = File.ReadAllText(path);
            else
            {
                footer = Properties.Resources.footer;
                File.WriteAllText(path, footer);
            }
        }

        static string renderHtml(string title, string htmlBody)
        {
            return "<html>\n<head>\n" + "<meta charset=\"utf-8\">\n"
                + "<title>" + title + "</title>\n"
                + "<style>\n" + style + "\n</style>\n"
                + "</head>\n"
                + "<body id=\"body\" style=\"background: white; margin: 0\">\n"
                + "<header>\n" + header + "\n</header>\n"
                + "<article>\n" + htmlBody + "\n</article>\n"
                + "<hr/>\n"
                + "<footer>\n" + footer + "\n</footer>\n"
                + "</body></html>";
        }

        static void Main(string[] args)
        {
            init();

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
                        readme.Append("* [" + fileNameWithoutExt + "](./" + category.Name + "/" + mdFile.Name + ")\n");

                        // read from file
                        string mdText = File.ReadAllText(mdFile.FullName);

                        // convert to html
                        string htmlBody = HtmlGenerator.MarkdownToHtml(mdText, baseUrl + "/src/" + category.Name);
                        string html = renderHtml(fileNameWithoutExt, htmlBody);
                        //Console.WriteLine(html);

                        // write to file
                        string toDir = currentDir + "/website/" + category.Name;
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
            StringBuilder indexHtmlBody = new StringBuilder();
            if (Directory.Exists(toolsDir))
            {
                DirectoryInfo toolsDirectory = new DirectoryInfo(toolsDir);
                FileInfo[] tools = toolsDirectory.GetFiles();
                if (tools != null && tools.Length > 0)
                {
                    indexHtmlBody.Append("<h2>Tools</h2>\n");
                    foreach (FileInfo toolFile in tools)
                    {
                        indexHtmlBody.Append("<ul>\n")
                            .Append("<li><a href=\"./tools/").Append(toolFile.Name).Append("\">").Append(toolFile.Name).Append("</a></li>\n")
                            .Append("</ul>\n");
                    }
                }
            }

            indexHtmlBody.Append(HtmlGenerator.MarkdownToHtml(readmeText, "website", true, true));
            string indexHtml = renderHtml(title, indexHtmlBody.ToString());
            //Console.WriteLine(indexHtml);
            File.WriteAllText(currentDir + "/index.html", indexHtml);
        }
    }
}
