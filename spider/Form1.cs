using HtmlAgilityPack;
using AngleSharp;
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
    public partial class Form1 : Form
    {
        MDAEntities1 database = new MDAEntities1();
        電影排名MovieRank R = new 電影排名MovieRank();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            string url = "https://movies.yahoo.com.tw/";
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                string result = await responseMessage.Content.ReadAsStringAsync();
                HtmlParser parser = new HtmlParser();
                var document = parser.ParseDocument(result);
                var data = document.QuerySelectorAll("#list1 > ul > a > li > div");
                var data2 = document.QuerySelectorAll("#list1 > ul > a > li > span");
                for (int i = 0; i < data.Length; i++)
                {
                    string rank = data[i].InnerHtml;
                    string movie = data2[i].InnerHtml;
                    if (rank != "" && movie != "")
                    {
                        //label1.Text += rank + ",";
                        R.電影排名Movie_Rank = rank;
                        R.電影Movie = movie;
                        this.database.電影排名MovieRank.Add(R);
                        this.database.SaveChanges();
                    }                  
                }
                MessageBox.Show("去看資料庫");
            }
        }
    }
}
