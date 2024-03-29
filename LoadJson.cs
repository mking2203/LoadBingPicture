﻿//
// Mark König, 03/2022
//

using System;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace LoadBingPicture
{
    public class LoadJson
    {
        public class BingPicture
        {
            public string baseurl;
            public string title;
            public string description;
            public string copyright;
            public string thumb;
        }
        public BingPicture[] BingPictures;

        public LoadJson()
        { }

        public bool DownloadJson(string path, string culture)
        {
            BingPictures = new BingPicture[7];

            try
            {
                WebClient client = new WebClient();
                var webData = client.DownloadData("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=7&mkt=" + culture);
                string webString = Encoding.UTF8.GetString(webData);

                // save JSON
                using (StreamWriter writetext = new StreamWriter(Path.Combine(path, "bing.json")))
                {
                    writetext.Write(webString);
                }

                dynamic stuff = JsonConvert.DeserializeObject(webString);

                for (int i = 0; i < 7; i++)
                {
                    BingPictures[i] = new BingPicture();
                    BingPictures[i].baseurl = "https://www.bing.com" + (string)stuff.images[i].urlbase;
                    BingPictures[i].title = (string)stuff.images[i].title;

                    // split descripton / copyright
                    string desc = (string)stuff.images[i].copyright;
                    int x = desc.LastIndexOf("(");

                    if (x > 0)
                    {
                        BingPictures[i].description = desc.Substring(0, x - 1);
                        BingPictures[i].copyright = desc.Substring(x + 1, desc.Length - x - 2);
                    }
                    else
                    {
                        BingPictures[i].description = desc;
                        BingPictures[i].copyright = string.Empty; ;
                    }
                }
            }
            catch
            {
                // somthing wrong?
                return false;
            }

            // fished OK
            return true;
        }

    }
}
