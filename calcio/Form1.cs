using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace calcio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Aggiungere controlli
            //Visualizza dati
            string ConnectionString = "server=127.0.0.1;uid=program;pwd=777;database=calcio";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            string sql1 = "select * from matches;";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);

            MySqlDataReader reader = cmd1.ExecuteReader();

            DataTable dati = new DataTable();
            dati.Load(reader);

            dataGridView4.DataSource = dati;
            conn.Close();

            
            string query1 = "SELECT DISTINCT HomeTeam AS Squadra\r\nFROM matches\r\nUNION\r\nSELECT DISTINCT AwayTeam AS Squadra\r\nFROM matches;\r\n";
            
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //Aggiungere controlli
            //Visualizza dati
            string valoreGiornata = numericUpDown1.Value.ToString();
            string ConnectionString = "server=127.0.0.1;uid=program;pwd=777;database=calcio";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            string sql1 = $"SELECT MatchRound, MatchDate, Stadium, HomeTeam, AwayTeam, Results\r\nFROM matches\r\nWHERE MatchRound = {valoreGiornata};";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);

            MySqlDataReader reader = cmd1.ExecuteReader();

            DataTable dati = new DataTable();
            dati.Load(reader);

            dataGridView3.DataSource = dati;
            conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aggiungere controlli
                //Visualizza dati
                string valoreStadio = comboBox2.Text.ToString();
                string ConnectionString = "server=127.0.0.1;uid=program;pwd=777;database=calcio";
                MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string sql1 = $"SELECT m.MatchRound, m.MatchDate, s.nome AS Stadium, m.HomeTeam, m.AwayTeam, m.Results\r\nFROM matches m\r\nJOIN stadio s ON m.Stadium = s.id\r\nWHERE s.nome = '{valoreStadio}';\r\n";
                MySqlCommand cmd1 = new MySqlCommand(sql1, conn);

                MySqlDataReader reader = cmd1.ExecuteReader();

                DataTable dati = new DataTable();
                dati.Load(reader);

                dataGridView2.DataSource = dati;
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Aggiungere controlli
                //Visualizza dati
                string valoreTeam = comboBox1.Text.ToString();
                string ConnectionString = "server=127.0.0.1;uid=program;pwd=777;database=calcio";
                MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string sql1 = $"SELECT MatchRound, MatchDate, Stadium, HomeTeam, AwayTeam, Results\r\nFROM matches\r\nWHERE HomeTeam = '{valoreTeam}' OR AwayTeam = '{valoreTeam}';";
                MySqlCommand cmd1 = new MySqlCommand(sql1, conn);

                MySqlDataReader reader = cmd1.ExecuteReader();

                DataTable dati = new DataTable();
                dati.Load(reader);

                dataGridView1.DataSource = dati;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
