using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ODA_Viewer
{
    public partial class ThumbnailViewer : Form
    {
        public ThumbnailViewer()
        {
            InitializeComponent();
            InitModal();
        }

        public void InitModal()
        {
            listView1.BeginUpdate();
            listView1.Groups.Clear();
            listView1.Items.Clear();
            label1.Text = ThumbnailCollection.Instance.GetSize().ToString();
            imageList1.ImageSize = new Size(120, 68);
            listView1.View = View.LargeIcon;

            for (int i = 0; i < ThumbnailCollection.Instance.GetSize(); i++)
            {
                Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(ThumbnailCollection.Instance.GetThumbnailWithIndex(i).Image);
                imageList1.Images.Add(i.ToString(), bitmap);

            }
            listView1.LargeImageList = imageList1;
            for (int i = 0; i < this.imageList1.Images.Count; i++)
            {
                ListViewItem listViewItem = listView1.Items.Add(ThumbnailCollection.Instance.GetThumbnailWithIndex(i).FileName);
                listViewItem.ImageKey = i.ToString();
            }
            listView1.EndUpdate();
            ///object list view 시도해봄
            this.objectListView1.SetObjects(ThumbnailCollection.Instance.GetThumbnailList());
            this.objectListView1.ShowGroups = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //listView2.Groups.Clear();
            //listView2.Items.Clear();

            //ControlDispose();

            //ListViewItem lvi = new ListViewItem(); 
            //lvi.Text = "000"; 
            //lvi.SubItems.Add("111");
            //lvi.SubItems.Add("222");
            //lvi.SubItems.Add("333");

            //listView2.Items.Add(lvi);


            //Button bt = new Button();
            //bt.Text = "In Button1";
            //bt.Click += new EventHandler(bt_Click);
            ////bt.Parent = listView2; //서브 아이템 얻어오기 
            //ListViewItem ps2 = null;
            //ps2 = listView2.Items[0]; //그리기 
            //Rectangle rt2 = new Rectangle();
            //rt2 = ps2.Bounds;
            //bt.SetBounds(rt2.X, rt2.Y, rt2.Width, rt2.Height + 6);
            //ltControl.Add(bt);

            //Button bt2 = new Button(); 
            //bt2.Text = "In Button2";
            //bt2.Click += new EventHandler(bt_Click);
            //bt2.Parent = listView2; //서브 아이템 얻어오기
            //ListViewItem.ListViewSubItem ps3 = null;
            //ps3 = listView2.Items[0].SubItems[2]; //그리기
            //Rectangle rt3 = new Rectangle(); rt3 = ps3.Bounds;
            //bt2.SetBounds(rt3.X, rt3.Y, rt3.Width, rt3.Height+6); 
            //ltControl.Add(bt2); 

            //TextBox tb = new TextBox();
            //tb.Text = "In TextBox"; 
            //tb.Parent = listView2; //서브 아이템 얻어오기
            //ListViewItem.ListViewSubItem ps4 = null; 
            //ps4 = listView2.Items[0].SubItems[3]; //그리기 
            //Rectangle rt4 = new Rectangle(); rt4 = ps4.Bounds;
            //tb.SetBounds(rt4.X, rt4.Y, rt4.Width, rt4.Height); 
            //ltControl.Add(tb);

        }
        void bt_Click(object s, EventArgs e)
        {
            Button bt = s as Button; 
            MessageBox.Show(bt.Text); 
        }


        //컨트롤 해제 변수... 
        List<Control> ltControl = new List<Control>();

        void ControlDispose() 
        { 
            for (int iCount = 0; iCount < ltControl.Count; iCount++) 
            {
                ltControl[iCount].Dispose(); 
            } 
            if (ltControl.Count > 0) 
            {
                ltControl.Clear();
            } 
        }

    }
}
