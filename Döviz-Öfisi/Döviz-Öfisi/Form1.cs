using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;

namespace Döviz_Öfisi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
         
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-493DFJA\SQLEXPRESS;Initial Catalog=DovizKur;Integrated Security=True");
        void kasadaki()
        {
            
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from tbldoviz where IŞLEMID=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", label5.Text);
            SqlDataReader dr1 = komut.ExecuteReader();
            while (dr1.Read())
            {

                label6.Text = dr1[1].ToString();
                label7.Text = dr1[2].ToString();
                label8.Text = dr1[3].ToString();

            }
            baglanti.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dr = new SqlDataAdapter("select *from tbldoviz", baglanti);
            dr.Fill(dt);
            dataGridView1.DataSource = dt;
            

            string bugun = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldosya = new XmlDocument();
            xmldosya.Load(bugun);

            string dolaralis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            labeldolaral.Text = dolaralis.ToString();
            string dolarsatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            labeldolsat.Text = dolarsatis.ToString();
            string euroalis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            labeleuroal.Text = euroalis.ToString();
            string eurosatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            labeleurosat.Text = eurosatis.ToString();
           
        }

        private void buttondolal_Click(object sender, EventArgs e)
        {
            texkur.Text = labeldolaral.Text;
        }

        private void buttondolsat_Click(object sender, EventArgs e)
        {
            texkur.Text = labeldolsat.Text;
        }

        private void buttoneuroal_Click(object sender, EventArgs e)
        {
            texkur.Text = labeleuroal.Text;
        }

        private void buttoneurosat_Click(object sender, EventArgs e)
        {
            texkur.Text = labeleurosat.Text;
        }

        private void btnsatışyap_Click(object sender, EventArgs e)
        {
            double kur, miktar, tutar;
            kur = Convert.ToDouble(texkur.Text);
            miktar = Convert.ToDouble(texmikt.Text);
            tutar = kur * miktar;
            textutar.Text = tutar.ToString();
        }

        private void texkur_TextChanged(object sender, EventArgs e)
        {
            texkur.Text = texkur.Text.Replace(".", ",");
        }

        private void işlem2_Click(object sender, EventArgs e)
        {
            double kur = Convert.ToDouble(texkur.Text);
            int tutar = Convert.ToInt32(textutar.Text);
            int miktar = Convert.ToInt16(tutar/kur);
            texmikt.Text = miktar.ToString();
            double kalan;
            kalan = tutar -(kur*miktar);
            texkalan.Text = kalan.ToString();
        }

        private void button18_Click(object sender, EventArgs e)
        {
           double kur = Convert.ToDouble(texkur.Text);
            int miktar = Convert.ToInt16(texmikt.Text);
            int tutar = Convert.ToInt32(miktar * kur );
            int dolardeger = Convert.ToInt32(label6.Text);
            int eurodeger = Convert.ToInt32(label7.Text);
            double tldeger = Convert.ToDouble(label8.Text);
            int t = Convert.ToInt32(label5.Text);

            baglanti.Open();

            SqlCommand komut = new SqlCommand("insert into tbldoviz (DOLARMIKTAR,TLMİKTAR,EUROMİKTAR) values (@p1,@p2,@p3) ", baglanti);
            
            int dolarmiktar = dolardeger - miktar;
            double tlmiktar = tldeger + tutar;
            int euro = eurodeger;
            
            
            komut.Parameters.AddWithValue("@p1", dolarmiktar);
            komut.Parameters.AddWithValue("@p2", tlmiktar);
            komut.Parameters.AddWithValue("@p3", euro);
            komut.ExecuteNonQuery();
            baglanti.Close();
            t++;
            label5.Text = t.ToString();
            kasadaki();

        }

        private void button22_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dr = new SqlDataAdapter("select *from tbldoviz", baglanti);
            dr.Fill(dt);
            dataGridView1.DataSource = dt;
            
         }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            label5.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            label6.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            label7.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            label8.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();

        }

        private void button19_Click(object sender, EventArgs e)
        {
            double kur = Convert.ToDouble(texkur.Text);
            int miktar = Convert.ToInt16(texmikt.Text);
            int tutar = Convert.ToInt32(miktar * kur);
           

            int dolardeger = Convert.ToInt32(label6.Text);
            int eurodeger = Convert.ToInt32(label7.Text);
            double tldeger = Convert.ToDouble(label8.Text);
            int t = Convert.ToInt32(label5.Text);

            baglanti.Open();

            SqlCommand komut = new SqlCommand("insert into tbldoviz (DOLARMIKTAR,TLMİKTAR,EUROMİKTAR) values (@p1,@p2,@p3) ", baglanti);

            int dolarmiktar = dolardeger + miktar;
            double tlmiktar = tldeger - tutar;
            int euro = eurodeger;


            komut.Parameters.AddWithValue("@p1", dolarmiktar);
            komut.Parameters.AddWithValue("@p2", tlmiktar);
            komut.Parameters.AddWithValue("@p3", euro);
            komut.ExecuteNonQuery();
            baglanti.Close();
            t++;
            label5.Text = t.ToString();
            kasadaki();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            double kur = Convert.ToDouble(texkur.Text);
            int miktar = Convert.ToInt16(texmikt.Text);
            int tutar = Convert.ToInt32(miktar * kur);
            int dolardeger = Convert.ToInt32(label6.Text);
            int eurodeger = Convert.ToInt32(label7.Text);
            double tldeger = Convert.ToDouble(label8.Text);
            int t = Convert.ToInt32(label5.Text);

            baglanti.Open();

            SqlCommand komut = new SqlCommand("insert into tbldoviz (DOLARMIKTAR,TLMİKTAR,EUROMİKTAR) values (@p1,@p2,@p3) ", baglanti);

            int dolarmiktar = dolardeger ;
            double tlmiktar = tldeger + tutar;
            int euro = eurodeger-miktar;


            komut.Parameters.AddWithValue("@p1", dolarmiktar);
            komut.Parameters.AddWithValue("@p2", tlmiktar);
            komut.Parameters.AddWithValue("@p3", euro);
            komut.ExecuteNonQuery();
            baglanti.Close();
            t++;
            label5.Text = t.ToString();
            kasadaki();

        }

        private void button21_Click(object sender, EventArgs e)
        {
            double kur = Convert.ToDouble(texkur.Text);
            int miktar = Convert.ToInt16(texmikt.Text);
            int tutar = Convert.ToInt32(miktar * kur);


            int dolardeger = Convert.ToInt32(label6.Text);
            int eurodeger = Convert.ToInt32(label7.Text);
            double tldeger = Convert.ToDouble(label8.Text);
            int t = Convert.ToInt32(label5.Text);

            baglanti.Open();

            SqlCommand komut = new SqlCommand("insert into tbldoviz (DOLARMIKTAR,TLMİKTAR,EUROMİKTAR) values (@p1,@p2,@p3) ", baglanti);

            int dolarmiktar = dolardeger ;
            double tlmiktar = tldeger - tutar;
            int euro = eurodeger+miktar;


            komut.Parameters.AddWithValue("@p1", dolarmiktar);
            komut.Parameters.AddWithValue("@p2", tlmiktar);
            komut.Parameters.AddWithValue("@p3", euro);
            komut.ExecuteNonQuery();
            baglanti.Close();
            t++;
            label5.Text = t.ToString();
            kasadaki();
        }
    }
}
