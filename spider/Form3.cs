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
        電影圖片總表MovieImages P = new 電影圖片總表MovieImages();
        電影Movies m = new 電影Movies();


        private async void button1_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://www.imdb.com/chart/top/?ref_=nv_mv_250";
                List<string> list = new List<string>();
                List<string> list2 = new List<string>();

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
                        string link = l[0].Trim() ;
                        list.Add(link);
                        textBox1.Text = String.Join(",", list);

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
                                var data2 = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-2a827f80-2.kqTacj > div.sc-2a827f80-3.dhWlsy > div > div.sc-77a2c808-1.gFDKno > div > a");
                                var data0 = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-2a827f80-6.jXSdID > div.sc-2a827f80-7.cOVoYS > div > div > a");

                                var ctitle = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > h1");
                                var etitle = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > div");
                                var show = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > ul > li:nth-child(1) > span");
                                var time= document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > ul > li:nth-child(3)");
                                var r= document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-80d4314-0.fjPRnj > div.sc-80d4314-1.fbQftq > div > ul > li:nth-child(2) > span");
                                var ratenum = document2.QuerySelector("#__next > main > div > section.ipc-page-background.ipc-page-background--base.sc-9b716f3b-0.hWwhTB > section > div:nth-child(4) > section > section > div.sc-2a827f80-2.kqTacj > div.sc-2a827f80-10.fVYbpg > div.sc-2a827f80-4.bWdgcV > div.sc-db8c1937-0.eGmDjE.sc-2a827f80-12.gOJseW > div > div:nth-child(1) > a > div > div > div.sc-7ab21ed2-0.fAePGh > div.sc-7ab21ed2-2.kYEdvH > span.sc-7ab21ed2-1.jGRxWM");
                               



                                string ct = ctitle.InnerHtml;
                                
                                string et = etitle.InnerHtml;
                                string[] enti = et.Split(':');
                                string entitle = enti[1].Trim();

                                string time1 = time.InnerHtml;
                                string[] gettime = time1.Split('<','>');
                                int h = Convert.ToInt32( gettime[0])*60;
                                int t = h + Convert.ToInt32(gettime[6]);

                                int rate = 0;
                                string p = r.InnerHtml;
                                if (p == "6+"|| p == "G" || p == "")
                                { rate = 1; }                               
                                else if (p == "PG-12" || p == "PG-13"|| p == "7+" ||p == "PG" ||p== "Not Rated" || p == "13+" || p == "12+" || p == "P" || p == "Passed" || p == "Approved") 
                                {
                                    rate = 2;
                                }
                                else if (p == "R-12" || p == "R-15" || p == "U" || p == "R" || p == "GP")
                                {
                                    rate = 3;
                                }                                
                                else if(p== "R-18"|| p == "18+"|| p == "16+")
                                { 
                                    rate = 6; 
                                }

                                string rnum = ratenum.InnerHtml;
                                decimal number =Math.Round(( Convert.ToDecimal(rnum) / 2),1);


                                if (data2 != null)
                                {
                                    string b = "https://www.imdb.com" + data2.GetAttribute("href");
                                    get( b,  entitle,  ct, t,  rate,  number);
                                }

                                else
                                {
                                    string b = "https://www.imdb.com" + data0.GetAttribute("href");
                                    get(b, entitle, ct, t, rate, number);
                                }
                            }
                        }
                    }
                    MessageBox.Show("done");
                }
            }
        }
        private async void get(string b ,string entitle, string ct,int t,int rate, decimal number)
        {
            using (HttpClient client3 = new HttpClient())
            {
                string url3 = b;
                var responses = await client3.GetAsync(url3);
                if (responses.IsSuccessStatusCode)
                {
                    string ress = await responses.Content.ReadAsStringAsync();
                    HtmlParser parser3 = new HtmlParser();
                    var document3 = parser3.ParseDocument(ress);
                    var data3 = document3.QuerySelector("#__next > main > div.ipc-page-content-container.ipc-page-content-container--full.sc-6eab0fb3-0.fkOMyK > div.sc-92eff7c6-1.gHPZBs.media-viewer > div:nth-child(4) > img");

                    string c = data3.GetAttribute("src");
                    P.電影名稱MovieName = ct;
                    var q = from v in this.database.電影圖片總表MovieImages
                            where v.圖片雲端ImageIMDB == c
                            select v;
                    var q2 = from m in this.database.電影Movies
                             where m.英文標題Title_Eng == entitle
                             select m;
                    if (q == null || q2==null)
                    {
                        m.中文標題Title_Cht = ct;
                        m.英文標題Title_Eng = entitle;
                        m.片長Runtime = t;
                        m.電影分級編號Rating_ID = rate;
                        m.評分Rate = number;
                        P.圖片雲端ImageIMDB = c;

                        this.database.電影圖片總表MovieImages.Add(P);
                        this.database.SaveChanges();
                    }
                    else
                        textBox2.Text += c + "已存過";

                }
            }
        }
    }
}
