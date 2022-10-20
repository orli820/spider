﻿using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Drawing.Imaging;
using Grpc.Core;
//using Grpc.Core;

namespace spider
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        MDAEntities1 database = new MDAEntities1();
        電影圖片總表MovieImages P = new 電影圖片總表MovieImages();

        private async void button1_Click(object sender, EventArgs e)
        {

            using (HttpClient client = new HttpClient())
            {
                string url = "https://www.imdb.com/chart/boxoffice/?ref_=hm_cht_sm";
                List<string> list = new List<string>();
                List<string> list2 = new List<string>();

                var responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string result = await responseMessage.Content.ReadAsStringAsync();

                    HtmlParser parser = new HtmlParser();
                    var document = parser.ParseDocument(result);
                    var data = document.QuerySelectorAll("#boxoffice > table > tbody > tr > td.titleColumn > a");
                    foreach (var item in data)
                    {
                        string a = "https://www.imdb.com" + item.GetAttribute("href");
                        //textBox1.Text += a;
                        list.Add(a);
                        textBox1.Text = String.Join(",", list);

                        foreach (var x in list)
                        {
                            using (HttpClient client2 = new HttpClient())
                            {
                                string[] arr = x.Split('?');

                                string url2 = arr[0].Trim() + '/';
                                var response = await client2.GetAsync(url2);
                                if (response.IsSuccessStatusCode)
                                {
                                    string res = await response.Content.ReadAsStringAsync();
                                    HtmlParser parser2 = new HtmlParser();
                                    var document2 = parser2.ParseDocument(res);
                                    var data2 = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-2a827f80-2.kqTacj > div.sc-2a827f80-3.dhWlsy > div > div.sc-77a2c808-1.gFDKno > div>a");

                                    string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                    list2.Add(b);
                                    //textBox2.Text = b;

                                    foreach (var z in list2)
                                    {
                                        using (HttpClient client3 = new HttpClient())
                                        {
                                            //string[] arry = x.Split('?');

                                            string url3 = z;
                                            var responses = await client3.GetAsync(url3);
                                            if (responses.IsSuccessStatusCode)
                                            {
                                                string ress = await responses.Content.ReadAsStringAsync();
                                                HtmlParser parser3 = new HtmlParser();
                                                var document3 = parser3.ParseDocument(ress);
                                                var data3 = document3.QuerySelector("#__next > main > div.ipc-page-content-container.ipc-page-content-container--full.sc-6eab0fb3-0.fkOMyK > div.sc-92eff7c6-1.gHPZBs.media-viewer > div:nth-child(4) > img");

                                                string c = data3.GetAttribute("src");
                                                P.圖片Image = c;
                                                this.database.電影圖片總表MovieImages.Add(P);
                                                this.database.SaveChanges();
                                                //using (HttpClient client4 = new HttpClient())
                                                //{
                                                //    //byte[] buffer = Encoding.ASCII.GetBytes(c);
                                                //    //using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(c)))
                                                //    //{
                                                //    //    using (Bitmap bm2 = new Bitmap(ms))
                                                //    //    {
                                                //    //        string pname = Guid.NewGuid().ToString() + ".Png";
                                                //    //        bm2.Save(Server.MapPath(@"C:\test" + pname));
                                                //    //    }
                                                //    //}


                                                //    byte[] buffer = Encoding.ASCII.GetBytes(c);
                                                //    //var o = client4.GetByteArrayAsync(c);
                                                //    //textBox2.Text = o.ToString();
                                                //    using (MemoryStream ms = new MemoryStream(buffer))
                                                //    {
                                                //        //Image yourImage = Image.FromStream(ms);
                                                //        using (Image yourImage = Image.FromStream(ms))
                                                //        {
                                                //            string pname = Guid.NewGuid().ToString() + ".Png";
                                                //            yourImage.Save(@"C:\test\" + pname, ImageFormat.Png);
                                                //        }
                                                //    }
                                                //}

                                            }
                                        }
                                    }
                                }
                            }


                        }
                    }

                }






                ////沒有用 抓整頁
                //public static string getheml(string url)
                //{
                //    string res = "";
                //    WebClient client = new WebClient();
                //    Stream stream = client.OpenRead(url);
                //    StreamReader sr = new StreamReader(stream, Encoding.Default);
                //    res = sr.ReadToEnd();
                //    sr.Close();
                //    client.Dispose();
                //    return res;

                //}

                //private void button2_Click(object sender, EventArgs e)
                //{

                //}
            }
        }
    }
}