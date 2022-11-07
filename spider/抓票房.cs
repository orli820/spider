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
    public partial class 抓票房 : Form
    {
        MDAEntities1 database = new MDAEntities1();
        電影排行MovieRank R = new 電影排行MovieRank();
        public 抓票房()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //刪囉
            var deleteOrderDetails = from d in database.電影排行MovieRank
                                     where d.排行編號Rank_ID != 0
                                     select d;
            foreach (var i in deleteOrderDetails)
            {
                database.電影排行MovieRank.Remove(i);
            }
            database.SaveChanges();

            HttpClient client = new HttpClient();
            string url = "http://app2.atmovies.com.tw/boxoffice/twweekend/";
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            List<string> list4 = new List<string>();
            List<string> list5 = new List<string>();
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                string result = await responseMessage.Content.ReadAsStringAsync();
                HtmlParser parser = new HtmlParser();
                var document = parser.ParseDocument(result);                
                //統計時間
                var data5 = document.QuerySelector("#main > div > div.\\39 u.\\31 2u\\(mobile\\).important\\(mobile\\) > div > table:nth-child(8) > tbody > tr:nth-child(1) > td:nth-child(2) > font");
                string time = data5.TextContent;
                //name
                var test = document.QuerySelectorAll("#main > div > div.\\39 u.\\31 2u\\(mobile\\).important\\(mobile\\) > div > table:nth-child(9) > tbody > tr > td.at11 > a");
                for (int i = 0; i < 15; i++)
                {
                    //都是那個劇場版
                    if (i == 11)
                    {
                        string r1 = test[11].TextContent;
                        string[] l1 = r1.Split();
                        string lo1 = l1[0] + l1[1];
                        list.Add(lo1);
                    }
                    else
                    {
                        string r = test[i].TextContent;
                        string[] l = r.Split();
                        string lo = l[0].Trim();
                        list.Add(lo);
                    }
                }
                textBox1.Text = String.Join(",", list);
                var box = document.QuerySelectorAll("#main > div > div.\\39 u.\\31 2u\\(mobile\\).important\\(mobile\\) > div > table:nth-child(9) > tbody > tr > td:nth-child(3)");
                for (int i = 0; i < 15; i += 1)
                {
                    string r = box[i].TextContent;
                    string[] l = r.Split('$');
                    string lo = l[1].Trim();
                    string g = lo + "萬";
                    list2.Add(g);
                }
                textBox1.Text = String.Join(",", list2);
                var gross = document.QuerySelectorAll("#main > div > div.\\39 u.\\31 2u\\(mobile\\).important\\(mobile\\) > div > table:nth-child(9) > tbody > tr > td:nth-child(4)");
                for (int i = 0; i < 15; i += 1)
                {
                    string r = gross[i].TextContent;
                    string[] l = r.Split('$');
                    string lo = l[1].Trim();
                    string g = lo + "萬";
                    list3.Add(g);
                }
                textBox1.Text = String.Join(",", list3);
                var week = document.QuerySelectorAll("#main > div > div.\\39 u.\\31 2u\\(mobile\\).important\\(mobile\\) > div > table:nth-child(9) > tbody > tr > td:nth-child(6)");
                for (int i = 0; i < 15; i += 1)
                {
                    string r = week[i].TextContent;
                    list4.Add(r);
                }
                textBox1.Text = String.Join(",", list4);
                var rank = document.QuerySelectorAll("#main > div > div.\\39 u.\\31 2u\\(mobile\\).important\\(mobile\\) > div > table:nth-child(9) > tbody > tr > td:nth-child(1) > b");
                for (int i = 0; i < 15; i++)
                {
                    string r = rank[i].TextContent; 
                    list5.Add(r);
                }
                textBox1.Text = String.Join(",", list5);
                for (int i = 0; i < list.Count; i++)
                {
                    R.電影排名Movie_Rank = list5[i];
                    R.電影Movie = list[i];                    
                    R.周末票房BoxOffice_Weekend = list2[i];
                    R.累積票房BoxOffice_Gross = list3[i];
                    R.周次Weeks = list4[i];
                    R.統計時間 = time;


                    database.電影排行MovieRank.Add(R);
                    database.SaveChanges();

                }
            }
            MessageBox.Show("去看資料庫");
        }
    }
}
