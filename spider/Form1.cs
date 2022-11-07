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
        電影排行MovieRank R = new 電影排行MovieRank();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            string url = "https://movies.yahoo.com.tw/";
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            var responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                string result = await responseMessage.Content.ReadAsStringAsync();
                HtmlParser parser = new HtmlParser();
                var document = parser.ParseDocument(result);
                var data = document.QuerySelectorAll("#list1 > ul > a > li > div");
                var data2 = document.QuerySelectorAll("#list1 > ul > a > li > span");
                var data3 = document.QuerySelectorAll("#list1 > ul > a");
               
                foreach (var item in data3)
                {
                    string a = item.GetAttribute("href");
                    list.Add(a);
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
                            var data4 = document2.QuerySelector("#content_l > div:nth-child(1) > div.l_box_inner > div > div > div.movie_intro_info_r > h3");

                            list2.Add(data4.InnerHtml);
                            textBox1.Text = String.Join(",", list2);
                        }
                    }
                }
                for (int i = 0; i < data.Length; i++)
                {
                    string rank = data[i].InnerHtml;
                    string movie = data2[i].InnerHtml;
                    string entitle = list2[i];
                    if (rank != "" && movie != "")
                    {
                        //label1.Text += rank + ",";
                        R.電影英Movie_En = entitle;
                        R.電影排名Movie_Rank = rank;
                        R.電影Movie = movie;
                        this.database.電影排行MovieRank.Add(R);
                        this.database.SaveChanges();
                    }
                }
            }
            MessageBox.Show("去看資料庫");
        }
        
    }
}

