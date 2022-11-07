using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spider
{
    public partial class 電影圖片連 : Form
    {
        public 電影圖片連()
        {
            InitializeComponent();
        }
        MDAEntities1 db = new MDAEntities1();
        電影Movies m = new 電影Movies();
        電影圖片總表MovieImages P = new 電影圖片總表MovieImages();
        電影圖片MovieIImagesList I = new 電影圖片MovieIImagesList();
        private void button1_Click(object sender, EventArgs e)
        {
            var movienem=from a in db.電影Movies 
                         join b in db.電影圖片總表MovieImages on a.中文標題Title_Cht equals b.電影名稱MovieName
                         select a.
        }
    }
}
