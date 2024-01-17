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
using MySqlX.XDevAPI.Relational;
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
            /*/dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false; //toglie la possibilità di selezionare più righe insieme
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.MultiSelect = false;
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.MultiSelect = false;/*/

            button1.Enabled = false;

            
            string sql1 = "SELECT Squadra\r\nFROM (\r\n    SELECT HomeTeam AS Squadra FROM matches\r\n    UNION\r\n    SELECT AwayTeam AS Squadra FROM matches\r\n) AS Teams\r\nORDER BY Squadra;\r\n";
            comboBox1.Items.AddRange(DataTableToStringArray(query(sql1)).Distinct().ToArray());

            string sql2 = "SELECT nome AS Stadium\r\nFROM stadio\r\nORDER BY nome;\r\n";
            comboBox2.Items.AddRange(DataTableToStringArray(query(sql2)).Distinct().ToArray());
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            string valoreGiornata = numericUpDown1.Value.ToString();
            string sql1 = $"SELECT m.MatchRound, m.MatchDate, s.nome AS Stadium, m.HomeTeam, m.AwayTeam, m.Results\r\nFROM matches m\r\nJOIN stadio s ON m.Stadium = s.id\r\nWHERE m.MatchRound = {valoreGiornata};\r\n";
            
            dataGridView3.DataSource = query(sql1);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string valoreStadio = comboBox2.Text.ToString();
                string sql1 = $"SELECT m.MatchRound, m.MatchDate, s.nome AS Stadium, m.HomeTeam, m.AwayTeam, m.Results\r\nFROM matches m\r\nJOIN stadio s ON m.Stadium = s.id\r\nWHERE s.nome = '{valoreStadio}';\r\n";
                
                dataGridView2.DataSource = query(sql1);
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
                string valoreTeam = comboBox1.Text.ToString();
                string sql1 = $"SELECT m.MatchRound, m.MatchDate, s.nome AS Stadium, m.HomeTeam, m.AwayTeam, m.Results\r\nFROM matches m\r\nJOIN stadio s ON m.Stadium = s.id\r\nWHERE m.HomeTeam = '{valoreTeam}' OR m.AwayTeam = '{valoreTeam}'\r\nORDER BY m.MatchRound;;\r\n";
                
                dataGridView1.DataSource = query(sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string[] DataTableToStringArray(DataTable dt) //converte il risultato di una query in un array di stringhe
        {
            string[] result = new string[dt.Rows.Count];
            int index = 0;

            foreach (DataRow row in dt.Rows)
            {
                result[index] = string.Join(" ", row.ItemArray);
                index++;
            }

            return result;
        }

        public DataTable query(string query)
        {
            //Visualizza dati
            string ConnectionString = "server=127.0.0.1;uid=program;pwd=777;database=calcio";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            MySqlCommand cmd1 = new MySqlCommand(query, conn);

            MySqlDataReader reader = cmd1.ExecuteReader();

            DataTable dati = new DataTable();
            dati.Load(reader);

            conn.Close();

            return dati;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            string valoreGiornata = numericUpDown2.Value.ToString();
            string sql1 = $"SELECT m.MatchRound, m.MatchDate, s.nome AS Stadium, m.HomeTeam, m.AwayTeam, m.Results\r\nFROM matches m\r\nJOIN stadio s ON m.Stadium = s.id\r\nWHERE m.MatchRound = {valoreGiornata};\r\n";

            dataGridView4.DataSource = query(sql1);
        }

        private void AggiornaRisultatiPartita(string nomeStadio, string homeTeam, string awayTeam, string nuoviRisultati)
        {
            string connectionString = "server=127.0.0.1;uid=program;pwd=777;database=calcio";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Creare la query SQL per aggiornare i risultati della partita
                    string query = "UPDATE matches SET Results = @NuoviRisultati " +
                                   "WHERE Stadium = (SELECT id FROM stadio WHERE nome = @NomeStadio) " +
                                   "AND (HomeTeam = @HomeTeam OR AwayTeam = @AwayTeam)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Aggiungere i parametri per evitare SQL injection
                        command.Parameters.AddWithValue("@NomeStadio", nomeStadio);
                        command.Parameters.AddWithValue("@HomeTeam", homeTeam);
                        command.Parameters.AddWithValue("@AwayTeam", awayTeam);
                        command.Parameters.AddWithValue("@NuoviRisultati", nuoviRisultati);

                        // Eseguire la query di aggiornamento
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Risultati della partita aggiornati con successo.");
                        }
                        else
                        {
                            MessageBox.Show("Nessuna partita corrispondente trovata.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore: " + ex.Message);
                }
            }
        }
        string nomeStadio;
        string homeTeam;
        string awayTeam;
        private void button1_Click(object sender, EventArgs e)
        {
            string home = numericUpDown3.Value.ToString();
            string away = numericUpDown4.Value.ToString();
            string nuoviRisultati = $"{home} - {away}";
            AggiornaRisultatiPartita(nomeStadio, homeTeam, awayTeam, nuoviRisultati);
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            nomeStadio = null;
            homeTeam = null;
            awayTeam = null;
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button1.Enabled = true;

            nomeStadio = dataGridView4.Rows[e.RowIndex].Cells["Stadium"].Value.ToString();
            homeTeam = dataGridView4.Rows[e.RowIndex].Cells["HomeTeam"].Value.ToString();
            awayTeam = dataGridView4.Rows[e.RowIndex].Cells["AwayTeam"].Value.ToString();
        }
    }
}