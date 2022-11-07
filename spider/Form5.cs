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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace spider
{
    public partial class Form5 : Form
    {
        MDAEntities1 database = new MDAEntities1();
        電影排行MovieRank R = new 電影排行MovieRank();
        public Form5()
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
            string url = "https://www.wowscreen.com.tw/tops.php?type=1&sdate=2022-10-21&edate=2022-10-23&year=2022&month=10#tops";
            //List<string> list = new List<string>();
            //List<string> list2 = new List<string>();
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                string result = await responseMessage.Content.ReadAsStringAsync();
                HtmlParser parser = new HtmlParser();
                var document = parser.ParseDocument(result);
                //中
                var data = document.QuerySelectorAll("#container > div.pagecontent.rank-page > div.rank-tab-area > div.rank-tab-inside.clear > table > tbody > tr > td.rank-movie-name > p > a");
                //英
                var data2 = document.QuerySelectorAll("#container > div.pagecontent.rank-page > div.rank-tab-area > div.rank-tab-inside.clear > table > tbody > tr > td.rank-movie-name > a");
                //本州成長
                var data3 = document.QuerySelectorAll("#container > div.pagecontent.rank-page > div.rank-tab-area > div.rank-tab-inside.clear > table > tbody > tr > td:nth-child(4)");
                //累積
                var data4 = document.QuerySelectorAll("#container > div.pagecontent.rank-page > div.rank-tab-area > div.rank-tab-inside.clear > table > tbody > tr > td:nth-child(5)");
                //周次
                var data5 = document.QuerySelectorAll("#container > div.pagecontent.rank-page > div.rank-tab-area > div.rank-tab-inside.clear > table > tbody > tr > td:nth-child(6)");
                var data6 = document.QuerySelector("#tab1 > div.time");
                for (int i = 0; i < data.Length; i++)
                {
                    string rank = (i+1).ToString();
                    string chtitle = data[i].InnerHtml;
                    string entitle = data2[i].InnerHtml;
                    string boxoffice = data3[i].InnerHtml;
                    string gross = data4[i].InnerHtml;
                    string weekend = data5[i].InnerHtml;
                    string time = data6.InnerHtml;
                    R.電影排名Movie_Rank = rank;
                    R.電影Movie = chtitle;
                    R.電影英Movie_En = entitle;
                    R.周末票房BoxOffice_Weekend = boxoffice;
                    R.累積票房BoxOffice_Gross = gross;
                    R.周次Weeks = weekend;
                    R.統計時間 = time;


                    database.電影排行MovieRank.Add(R);
                    database.SaveChanges();
                    
                }
            }
            MessageBox.Show("去看資料庫");
        }

       
    }
}

