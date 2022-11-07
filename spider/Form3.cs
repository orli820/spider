using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spider
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        MDAEntities1 database = new MDAEntities1();
        電影Movies P = new 電影Movies();
        電影圖片總表MovieImages R = new 電影圖片總表MovieImages();
        電影圖片MovieIImagesList R2 = new 電影圖片MovieIImagesList();
        電影導演MovieDirector D2 = new 電影導演MovieDirector();
        導演總表Director D = new 導演總表Director();

        private async void button1_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://www.imdb.com/chart/top/?ref_=nv_mv_250";
                List<string> list = new List<string>();
                List<string> listtitlech = new List<string>();

                var responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string result = await responseMessage.Content.ReadAsStringAsync();

                    HtmlParser parser = new HtmlParser();
                    var document = parser.ParseDocument(result);
                    var data = document.QuerySelectorAll("#main > div > span > div > div > div.lister > table > tbody > tr > td.titleColumn > a");
                    foreach (var item in data)
                    {
                        string a = "https://www.imdb.com" + item.GetAttribute("href");
                        string[] l = a.Split('?');
                        string link = l[0].Trim();
                        list.Add(link);
                    }
                    textBox1.Text = String.Join(",", list);
                    foreach (var x in list)
                    {
                        using (HttpClient client2 = new HttpClient())
                        {
                            string url2 = x;
                            var response = await client2.GetAsync(url2);
                            if (response.IsSuccessStatusCode)
                            {
                                string res = await response.Content.ReadAsStringAsync();
                                HtmlParser parser2 = new HtmlParser();
                                var document2 = parser2.ParseDocument(res);
                                var titlech = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > h1");
                                //中文標題
                                var chtitle = titlech.InnerHtml;
                                //英文標題
                                var titleen = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > div");
                                
                                //找時間(第三格)
                                var runningtime = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > ul > li:nth-child(3)");
                                //找時間(第二格)
                                var runningtime2 = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > ul > li:nth-child(2)");
                                //上映年分
                                var year = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > ul > li:nth-child(1) > a");
                                int runyear = Convert.ToInt32(year.InnerHtml);
                                //圖片路徑1
                                var data0 = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-7643a8e3-2.ebKPVC > div.sc-7643a8e3-3.kvnEqz > div > div.sc-e1fa24b3-1.eydBag > div > a");
                                //圖片路徑2
                                var data2 = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-7643a8e3-6.bunqBa > div.sc-7643a8e3-7.hNQeVX > div > div > a");
                                //圖片路徑3
                                var data00 = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-7643a8e3-2.ebKPVC > div.sc-7643a8e3-3.kvnEqz > div > div.sc-e1fa24b3-1.eydBag > div > a");

                                var gety = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > ul > li:nth-child(2) > a");
                                //抓分數
                                var getrate = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-db8c1937-0.eGmDjE.sc-80d4314-3.iBtAhY > div > div:nth-child(1) > a > div > div > div.sc-7ab21ed2-0.fAePGh > div.sc-7ab21ed2-2.kYEdvH > span.sc-7ab21ed2-1.jGRxWM");
                                string kk = getrate.InnerHtml;
                                decimal ratestar = Math.Round((Convert.ToDecimal(kk) / 2), 1);

                                //找導演
                                var dddddd = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-7643a8e3-2.ebKPVC > div.sc-7643a8e3-10.itwFpV > div.sc-7643a8e3-4.iAthmE > div.sc-fa02f843-0.fjLeDR > ul > li:nth-child(1) > div > ul > li > a");
                                string director = dddddd.InnerHtml;

                                //沒有原標(沒有中標)
                                if (titleen == null)
                                {
                                    string entitle = chtitle;
                                    //抓片長第三格
                                    if (runningtime != null)
                                    {
                                        string time1 = runningtime.InnerHtml;
                                        string[] gettime = time1.Split('<', '>');
                                        int h = Convert.ToInt32(gettime[0]) * 60;
                                        if (gettime.Length != 9)
                                        {
                                            int time = h;
                                            if (data2 != null)
                                            {
                                                string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }
                                            else if (data2 == null && data0 == null)
                                                textBox1.Text = chtitle + "沒有圖片";


                                        }
                                        else
                                        {
                                            int time = h + Convert.ToInt32(gettime[6]);
                                            if (data2 != null)
                                            {
                                                string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }
                                            else if (data2 == null && data0 == null)
                                                textBox1.Text = chtitle + "沒有圖片";
                                        }
                                    }
                                    //抓片長第二格
                                    else
                                    {
                                        string time1 = runningtime2.InnerHtml;
                                        string[] gettime = time1.Split('<', '>');
                                        int h = Convert.ToInt32(gettime[0]) * 60;
                                        if (gettime.Length != 9)
                                        {
                                            int time = h;
                                            if (data2 != null)
                                            {
                                                string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }
                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }
                                            else if (data2 == null && data0 == null)
                                                textBox1.Text = chtitle + "沒有圖片";


                                        }
                                        else
                                        {
                                            int time = h + Convert.ToInt32(gettime[6]);
                                            if (data2 != null)
                                            {
                                                string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }
                                            else if (data2 == null && data0 == null)
                                                textBox1.Text = chtitle + "沒有圖片";
                                        }
                                    }

                                }
                                //有原標(中英都有)
                                else
                                {
                                    var enti = titleen.InnerHtml;
                                    string[] line = enti.Split(':');
                                    string entitle = line[1].Trim();
                                    //抓片長第三格是時間
                                    if (runningtime != null)
                                    {
                                        string time1 = runningtime.InnerHtml;
                                        string[] gettime = time1.Split('<', '>');
                                        int h = Convert.ToInt32(gettime[0]) * 60;
                                        //分割成陣列看看，長度為九是片長時間
                                        //不是時間，可能是空值
                                        if (gettime.Length != 9)
                                        {
                                            int time = h;
                                            if (data2 != null)
                                            {
                                                string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }
                                            else if (data2 == null && data0 == null)
                                                textBox1.Text = chtitle + "沒有圖片";

                                        }
                                        //時間為第三格
                                        else
                                        {
                                            int time = h + Convert.ToInt32(gettime[6]);
                                            if (data2 != null)
                                            {
                                                string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }
                                            else if (data2 == null && data0 == null)
                                                textBox1.Text = chtitle + "沒有圖片";
                                        }
                                    }
                                    //抓片長第二格
                                    else
                                    {
                                        //片長沒有第二格                                        
                                        if (runningtime2 == null)
                                        {
                                            int h = 0;
                                            int time = h;
                                            if (data2 != null)
                                            {
                                                string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear, ratestar, director);
                                            }
                                            else if (data2 == null && data0 == null)
                                                textBox1.Text = chtitle + "沒有圖片";


                                        }
                                        //片長第二格顯示非片長&片長  runningtime2 != null
                                        else
                                        {
                                            string time1 = runningtime2.InnerHtml;
                                            string[] gettime = time1.Split('<', '>');
                                            if (gettime[2] == "PG")
                                            {
                                                int h = 0;
                                                int time = h;
                                                if (data2 != null)
                                                {
                                                    string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                    get(b, chtitle, entitle, time, runyear, ratestar, director);
                                                }

                                                else if (data0 != null)
                                                {
                                                    string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                    get(b, chtitle, entitle, time, runyear, ratestar, director);
                                                }
                                                else if (data2 == null && data0 == null)
                                                    textBox1.Text = chtitle + "沒有圖片";
                                            }
                                            else
                                            {
                                                int h = Convert.ToInt32(gettime[0]) * 60;
                                                if (gettime.Length != 9)
                                                {
                                                    int time = h;
                                                    if (data2 != null)
                                                    {
                                                        string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                        get(b, chtitle, entitle, time, runyear, ratestar, director);
                                                    }

                                                    else if (data0 != null)
                                                    {
                                                        string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                        get(b, chtitle, entitle, time, runyear, ratestar, director);
                                                    }
                                                    else if (data2 == null && data0 == null)
                                                        textBox1.Text = chtitle + "沒有圖片";

                                                }
                                                else
                                                {
                                                    int time = h + Convert.ToInt32(gettime[6]);
                                                    if (data2 != null)
                                                    {
                                                        string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                                        get(b, chtitle, entitle, time, runyear, ratestar, director);
                                                    }

                                                    else if (data0 != null)
                                                    {
                                                        string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                        get(b, chtitle, entitle, time, runyear,ratestar, director);
                                                    }
                                                    else if (data2 == null && data0 == null)
                                                        textBox1.Text = chtitle + "沒有圖片";
                                                }
                                            }
                                        }

                                    }
                                }




                            }
                        }
                    }

                    MessageBox.Show("done");
                }
            }
        }
        private async void get(string b, string chtitle, string entitle, int time, int runyear, decimal ratestar,string director)
        {
            using (HttpClient client3 = new HttpClient())
            {
                string url3 = b;
                var responses = await client3.GetAsync(url3);
                if (responses.IsSuccessStatusCode)
                {
                    //撿圖片
                    string ress = await responses.Content.ReadAsStringAsync();
                    HtmlParser parser3 = new HtmlParser();
                    var document3 = parser3.ParseDocument(ress);
                    var data3 = document3.QuerySelector("#__next > main > div.ipc-page-content-container.ipc-page-content-container--full.sc-6eab0fb3-0.fkOMyK > div.sc-92eff7c6-1.gHPZBs.media-viewer > div:nth-child(4) > img");
                    var data4 = document3.QuerySelector("#__next > main > div.ipc-page-content-container.ipc-page-content-container--full.sc-6eab0fb3-0.fkOMyK > div.sc-92eff7c6-1.gHPZBs.media-viewer > div.sc-7c0a9e7c-2.bkptFa > img");
                    if (data3 != null)
                    {
                        string c = data3.GetAttribute("src");
                        var q = from a in database.電影圖片總表MovieImages where a.圖片雲端ImageIMDB == c select a;
                        var q5 = from d in database.電影圖片總表MovieImages where d.圖片雲端ImageIMDB == c select d;
                        var q2 = from z in database.電影Movies where z.中文標題Title_Cht == chtitle select z;
                        var q3 = from d in database.電影Movies where d.中文標題Title_Cht == entitle select d;
                        var q4 = from f in database.電影Movies where f.英文標題Title_Eng == entitle select f;
                        var q6 = from m in database.導演總表Director where m.導演英文名字Name_Eng == director select m;
                        
                        //雲端圖片
                        if (q.Count() == 0 && q5.Count() == 0)
                        {
                            R.圖片雲端ImageIMDB = c;
                            R.電影名稱MovieName = chtitle;
                            this.database.電影圖片總表MovieImages.Add(R);
                            this.database.SaveChanges();
                        }
                        else
                            textBox2.Text += chtitle + "圖片已存過";

                        //電影
                        if (q2.Count() == 0 )
                        {
                            if (q3.Count() != 0 || q4.Count() != 0)
                            {
                                MessageBox.Show(chtitle + "/" + entitle + "可能已加入");
                                return;
                            }
                            P.中文標題Title_Cht = chtitle;
                            P.英文標題Title_Eng = entitle;
                            P.片長Runtime = time;
                            P.上映年份Release_Year = runyear;
                            P.評分Rate = ratestar;
                            this.database.電影Movies.Add(P);
                            this.database.SaveChanges();
                            textBox1.Text += chtitle + "已存入";

                            //var movie = (from a in database.電影Movies where a.中文標題Title_Cht == chtitle select a.電影編號Movie_ID).First();
                            //var img = (from a1 in database.電影圖片總表MovieImages where a1.圖片雲端ImageIMDB == c select a1.圖片編號Image_ID).First();
                            //R2.圖片編號Image_ID = img;
                            //R2.電影編號Movie_ID = movie;
                            //this.database.電影圖片MovieIImagesList.Add(R2);
                            //this.database.SaveChanges();
                            //textBox1.Text += "電影圖片加入成功";
                        }
                        else
                            textBox2.Text += chtitle + "電影已存過";

                        //導演
                        if (q6.Count() == 0)
                        {

                            D.導演英文名字Name_Eng = director;
                            this.database.導演總表Director.Add(D);
                            this.database.SaveChanges();
                            //var movie = (from a in database.電影Movies where a.中文標題Title_Cht == chtitle select a.電影編號Movie_ID).First();
                            //var j = (from d in database.導演總表Director where d.導演英文名字Name_Eng == director select d.導演編號Director_ID).First();
                            //D2.導演編號Director_ID = j;
                            //D2.電影編號Movie_ID = movie;
                            //this.database.電影導演MovieDirector.Add(D2);
                            //this.database.SaveChanges();
                        }
                        else if (q6.Count() != 0)
                        {
                            var movie = (from a in database.電影Movies where a.中文標題Title_Cht == chtitle select a.電影編號Movie_ID).First();
                            var p = (from i in q6 select i.導演編號Director_ID).First();
                            D2.導演編號Director_ID = p;
                            D2.電影編號Movie_ID = movie;
                            this.database.電影導演MovieDirector.Add(D2);
                            this.database.SaveChanges();
                        }
                    }
                    else
                    {
                        string c = data4.GetAttribute("src");
                        var q = from a in database.電影圖片總表MovieImages where a.圖片雲端ImageIMDB == c select a;
                        var q5 = from d in database.電影圖片總表MovieImages where d.圖片雲端ImageIMDB == c select d;
                        var q2 = from z in database.電影Movies where z.中文標題Title_Cht == chtitle select z;
                        var q3 = from d in database.電影Movies where d.中文標題Title_Cht == entitle select d;
                        var q4 = from f in database.電影Movies where f.英文標題Title_Eng == entitle select f;
                        var q6 = from m in database.導演總表Director where m.導演英文名字Name_Eng == director select m;
                        //圖片
                        if (q.Count() == 0 && q5.Count() == 0)
                        {
                            R.圖片雲端ImageIMDB = c;
                            R.電影名稱MovieName = chtitle;
                            this.database.電影圖片總表MovieImages.Add(R);
                            this.database.SaveChanges();
                        }
                        else
                            textBox2.Text += chtitle + "圖片已存過";

                        //電影
                        if (q2.Count() == 0 )
                        {
                            if (q3.Count() != 0 || q4.Count() != 0)
                            {
                                MessageBox.Show(chtitle + "/" + entitle + "可能已加入");
                                return;
                            }
                            P.中文標題Title_Cht = chtitle;
                            P.英文標題Title_Eng = entitle;
                            P.片長Runtime = time;
                            P.上映年份Release_Year = runyear;
                            P.評分Rate = ratestar;

                            this.database.電影Movies.Add(P);
                            this.database.SaveChanges();
                            textBox1.Text += chtitle + "已存入";

                           //var movie = (from a in database.電影Movies where a.中文標題Title_Cht == chtitle select a.電影編號Movie_ID).First();
                           // var img = (from a1 in database.電影圖片總表MovieImages where a1.圖片雲端ImageIMDB == c select a1.圖片編號Image_ID).First();
                           // R2.圖片編號Image_ID = img;
                           // R2.電影編號Movie_ID = movie;
                           // this.database.電影圖片MovieIImagesList.Add(R2);
                           // this.database.SaveChanges();
                           // textBox1.Text += "電影圖片加入成功";
                        }
                        else
                            textBox2.Text += chtitle + "電影已存過";

                        //導演
                        if (q6.Count() == 0)
                        {

                            D.導演英文名字Name_Eng = director;
                            this.database.導演總表Director.Add(D);
                            this.database.SaveChanges();
                        //    var movie = (from a in database.電影Movies where a.中文標題Title_Cht == chtitle select a.電影編號Movie_ID).First();
                        //    var j = (from d in database.導演總表Director where d.導演英文名字Name_Eng == director select d.導演編號Director_ID).First();
                        //    D2.導演編號Director_ID = j;
                        //    D2.電影編號Movie_ID = movie;
                        //    this.database.電影導演MovieDirector.Add(D2);
                        //    this.database.SaveChanges();
                        }
                        else if (q6.Count() != 0)
                        {
                            var movie = (from a in database.電影Movies where a.中文標題Title_Cht == chtitle select a.電影編號Movie_ID).First();
                            var p = (from i in q6 select i.導演編號Director_ID).First();
                            D2.導演編號Director_ID = p;
                            D2.電影編號Movie_ID = movie;
                            this.database.電影導演MovieDirector.Add(D2);
                            this.database.SaveChanges();
                        }


                    }



                }
            }
        }
    }
}
