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
    public partial class 最低評分 : Form
    {
        public 最低評分()
        {
            InitializeComponent();
        }
        MDAEntities1 database = new MDAEntities1();
        電影Movies P = new 電影Movies();
        電影圖片總表MovieImages R = new 電影圖片總表MovieImages();
        private async void button1_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://www.imdb.com/calendar/?ref_=rlm&region=TW&type=MOVIE";
                List<string> list = new List<string>();
                List<string> listtitlech = new List<string>();

                var responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string result = await responseMessage.Content.ReadAsStringAsync();

                    HtmlParser parser = new HtmlParser();
                    var document = parser.ParseDocument(result);
                    var data = document.QuerySelectorAll("#__next > main > div > div.ipc-page-content-container.ipc-page-content-container--center > section > section > article > ul > li> div.ipc-metadata-list-summary-item__c > div.ipc-metadata-list-summary-item__tc > a");


                    foreach (var item in data)
                    {
                        string a = "https://www.imdb.com" + item.GetAttribute("href");
                        string[] l = a.Split('?');
                        string link = l[0].Trim();
                        list.Add(link);
                    }

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
                                                get(b, chtitle, entitle, time, runyear);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear);
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
                                                get(b, chtitle, entitle, time, runyear);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear);
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
                                                get(b, chtitle, entitle, time, runyear);
                                            }
                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear);
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
                                                get(b, chtitle, entitle, time, runyear);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear);
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
                                                get(b, chtitle, entitle, time, runyear);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear);
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
                                                get(b, chtitle, entitle, time, runyear);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear);
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
                                                get(b, chtitle, entitle, time, runyear);
                                            }

                                            else if (data0 != null)
                                            {
                                                string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                get(b, chtitle, entitle, time, runyear);
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
                                                    get(b, chtitle, entitle, time, runyear);
                                                }

                                                else if (data0 != null)
                                                {
                                                    string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                    get(b, chtitle, entitle, time, runyear);
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
                                                        get(b, chtitle, entitle, time, runyear);
                                                    }

                                                    else if (data0 != null)
                                                    {
                                                        string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                        get(b, chtitle, entitle, time, runyear);
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
                                                        get(b, chtitle, entitle, time, runyear);
                                                    }

                                                    else if (data0 != null)
                                                    {
                                                        string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                                        get(b, chtitle, entitle, time, runyear);
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

        private async void get(string b, string chtitle, string entitle, int time, int runyear)
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
                        var q2 = from z in database.電影Movies where z.中文標題Title_Cht == chtitle select z;
                        var q3 = from d in database.電影Movies where d.中文標題Title_Cht == entitle select d;
                        var q4 = from f in database.電影Movies where f.英文標題Title_Eng == entitle select f;
                        if (q.Count() == 0)
                        {
                            R.圖片雲端ImageIMDB = c;
                            R.電影名稱MovieName = chtitle;
                            this.database.電影圖片總表MovieImages.Add(R);
                            this.database.SaveChanges();
                        }
                        else

                            textBox2.Text += chtitle + "圖片已存過";
                        if (q2.Count() == 0)
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

                            this.database.電影Movies.Add(P);
                            this.database.SaveChanges();
                        }
                        else

                            textBox2.Text += chtitle + "電影已存過";
                    }
                    else
                    {
                        string c = data4.GetAttribute("src");
                        var q = from a in database.電影圖片總表MovieImages where a.圖片雲端ImageIMDB == c select a;
                        var q2 = from z in database.電影Movies where z.中文標題Title_Cht == chtitle select z;
                        var q3 = from d in database.電影Movies where d.中文標題Title_Cht == entitle select d;
                        var q4 = from f in database.電影Movies where f.英文標題Title_Eng == entitle select f;
                        if (q.Count() == 0)
                        {
                            R.圖片雲端ImageIMDB = c;
                            R.電影名稱MovieName = chtitle;
                            this.database.電影圖片總表MovieImages.Add(R);
                            this.database.SaveChanges();
                        }
                        else

                            textBox2.Text += chtitle + "圖片已存過";
                        if (q2.Count() == 0)
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

                            this.database.電影Movies.Add(P);
                            this.database.SaveChanges();
                        }
                        else

                            textBox2.Text += chtitle + "電影已存過";
                    }



                }
            }
        }
    }
}
