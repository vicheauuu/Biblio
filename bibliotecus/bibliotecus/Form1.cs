using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace bibliotecus
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlDataAdapter adapter;
        DataTable dataTable;
        public Form1()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }
        private void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";
            connection = new SqlConnection(connectionString);
        }

        string[] denumire_carte = new string[1];
        string[] autor = new string[1];

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string grupa = comboBox1.SelectedItem.ToString();
            string query = "SELECT ID,Nume,Prenume,Grupa FROM Elevi WHERE Grupa = '" + grupa + "' ";

            string connectionString = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            panel3.Hide();
            panel4.Hide();
            panel2.Show();
            panel5.Hide();
            panel6.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

            string query = "SELECT Denumire,Autor,Stock FROM Carti";

            string connectionString = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);
                    dataGridView2.DataSource = dataTable;
                }
            }

            panel2.Hide();
            panel3.Show();
            panel4.Show();
            panel5.Hide();
            panel6.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            string connStr = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();

                string denumireCarte = textBox1.Text;
                string autor = textBox2.Text;

                int cantitate = (int)numericUpDown1.Value;

                if (!string.IsNullOrWhiteSpace(denumireCarte) && !string.IsNullOrWhiteSpace(autor))
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Carti WHERE Denumire = @Denumire", conn);
                    cmd.Parameters.AddWithValue("@Denumire", denumireCarte);
                    int existaCarte = (int)cmd.ExecuteScalar();

                    if (existaCarte > 0)
                    {
                        SqlCommand updateCmd = new SqlCommand("UPDATE Carti SET Stock = Stock + @Cantitate WHERE Denumire = @Denumire", conn);
                        updateCmd.Parameters.AddWithValue("@Cantitate", cantitate);
                        updateCmd.Parameters.AddWithValue("@Denumire", denumireCarte);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand insertCmd = new SqlCommand("INSERT INTO Carti (Denumire, Autor, Stock) VALUES (@Denumire, @Autor, @Cantitate)", conn);
                        insertCmd.Parameters.AddWithValue("@Denumire", denumireCarte);
                        insertCmd.Parameters.AddWithValue("@Autor", autor);
                        insertCmd.Parameters.AddWithValue("@Cantitate", cantitate);
                        insertCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Operație finalizată cu succes!");
                }
                else
                {
                    MessageBox.Show("Completează toate câmpurile!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string connStr = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                conn.Open();

                string denumireCarte = textBox1.Text;
                string autor = textBox2.Text;
                if (!string.IsNullOrWhiteSpace(denumireCarte) && !string.IsNullOrWhiteSpace(autor))
                {
                    SqlCommand cmd = new SqlCommand("SELECT Stock FROM Carti WHERE Denumire = @Denumire AND Autor = @Autor", conn);
                    cmd.Parameters.AddWithValue("@Denumire", denumireCarte);
                    cmd.Parameters.AddWithValue("@Autor", autor);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int stock = (int)result;
                        label7.Text = $"Stock-ul: {stock}";
                    }
                    else
                    {
                        label7.Text = "Cartea nu a fost găsită în baza de date.";
                    }
                }
                else
                {
                    MessageBox.Show("Completează toate câmpurile!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {

            string denumireCarte = textBox1.Text;
            string autor = textBox2.Text;
            int cantitate = (int)numericUpDown1.Value;

            string connStr = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT Stock FROM Carti WHERE Denumire = @Denumire AND Autor = @Autor", conn);
                cmd.Parameters.AddWithValue("@Denumire", denumireCarte);
                cmd.Parameters.AddWithValue("@Autor", autor);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    int stock = (int)result;

                    int cantitateStergere = Math.Min(cantitate, stock);

                    SqlCommand updateCmd = new SqlCommand("UPDATE Carti SET Stock = Stock - @Cantitate WHERE Denumire = @Denumire AND Autor = @Autor", conn);
                    updateCmd.Parameters.AddWithValue("@Cantitate", cantitateStergere);
                    updateCmd.Parameters.AddWithValue("@Denumire", denumireCarte);
                    updateCmd.Parameters.AddWithValue("@Autor", autor);
                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("Ați șters cartea cu succes!");
                }
                else
                {
                    MessageBox.Show("Cartea nu a fost găsită în baza de date.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            panel3.Hide();
            panel4.Hide();
            panel5.Show();
            panel6.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string grupa_aleasa = comboBox2.SelectedItem.ToString();
            string nume = textBox5.Text;
            string prenume = textBox6.Text;

            string query = @"SELECT ID,Nume,Prenume FROM Elevi WHERE (Nume = '" + nume + "' and Prenume = '" + prenume + "' and Grupa = '" + grupa_aleasa + "')";
            string connectionString = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);
                   // dataGridView2.DataSource = dataTable;
                }
            }
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            string denumire_carte = textBox5.Text;
            string autor = textBox8.Text;
            string nume = textBox7.Text;
            string prenume = textBox6.Text;

            int stock = 0;

            int ID_ELEV = 0;

            string connectionString = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string q_stock = @"SELECT Stock FROM Carti where Denumire = @denumire_carte and Autor = @autor";
                using (SqlCommand command = new SqlCommand(q_stock, connection))
                {
                    command.Parameters.AddWithValue("@denumire_carte", denumire_carte);
                    command.Parameters.AddWithValue("@autor", autor);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stock = (int)reader["Stock"];
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT ID FROM Elevi WHERE nume = @nume AND prenume = @prenume";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nume", nume);
                    command.Parameters.AddWithValue("@prenume", prenume);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ID_ELEV = reader.GetInt32(0);
                        }
                    }
                }
            }
            if (stock > 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    if (textBox7.Text != "" && textBox8.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                    {
                        string query = @"INSERT INTO Datorii (ID_CARTE, ID_ELEV)
                             SELECT c.ID_CARTE, e.ID
                             FROM Carti c, Elevi e
                             WHERE c.Denumire = '" + denumire_carte + "' AND c.Autor = '" + autor + "' AND e.Nume = '" + nume + "' AND e.Prenume = '" + prenume + "'";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@denumireCarte", denumire_carte);
                            command.Parameters.AddWithValue("@autor", autor);
                            command.Parameters.AddWithValue("@nume", nume);
                            command.Parameters.AddWithValue("@prenume", prenume);

                            command.ExecuteNonQuery();
                        }

                        if (stock == 1)
                        {
                            string query2 = @"  UPDATE Carti
                                SET Stock = Stock - 1
                                WHERE ID_CARTE = (SELECT ID_CARTE FROM Carti WHERE Denumire = '" + denumire_carte + "' AND Autor = '" + autor + "') ";

                            using (SqlCommand command2 = new SqlCommand(query2, connection))
                            {
                                command2.Parameters.AddWithValue("@denumireCarte", denumire_carte);
                                command2.Parameters.AddWithValue("@autor", autor);

                                command2.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Ați atribuit cu succes cartea " + denumire_carte + " elevului: " + nume + " " + prenume + " ");
                    }
                    else MessageBox.Show("Introduceți toate câmpurile! ", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Nu există așa carte!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            string nume = textBox9.Text;
            string prenume = textBox10.Text;

            string grupa = this.comboBox3.Text;
//            MessageBox.Show(grupa);

            string connectionString = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";

            /*string query = "  SELECT Carti.Denumire AS Denumire_Carte,Carti.Autor AS Autor_Carte " +
                 "FROM Datorii " +
                 "JOIN Elevi ON Datorii.id_elev = Elevi.ID " +
                 "JOIN Carti ON Datorii.id_carte = Carti.id_carte " +
                 "WHERE Elevi.Nume = '" + nume + "' AND Elevi.Prenume = '" + prenume + "' AND Elevi.Grupa = '" + grupa_ + "'  ";*/
            string query = "  SELECT c.Denumire, c.Autor " +
                "FROM Datorii d " +
                "JOIN Elevi e ON d.id_elev = e.id " +
                "JOIN Carti c ON d.id_carte = c.id_carte " +
                "WHERE e.nume = '" + nume + " ' " +
                "AND e.prenume = '" + prenume + " ' " +
                "AND e.grupa = '" + grupa + "' " +
                "ORDER BY e.id;  ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString))
                {

                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);
                    dataGridView3.DataSource = dataTable;

                }
                connection.Close();
            }
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source = VICHEAUUU\\SQLEXPRESS; Initial Catalog = CEITI; Integrated Security = True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string nume = textBox7.Text;
                string prenume = textBox6.Text;
                string denumire_carte = textBox5.Text;
                string autor = textBox8.Text;
                string grupa = this.comboBox3.Text;

                string deleteDatorieQuery = @"DELETE FROM Datorii
                                              WHERE ID_Elev = (SELECT id_elev FROM Elevi WHERE nume = @numeElev AND prenume = @prenumeElev AND grupa = @grupaElev)
                                              AND id_carte = (SELECT id_carte FROM Carti WHERE denumire_carti = @denumireCarte AND autor = @autorCarte)";
                using (SqlCommand deleteCommand = new SqlCommand(deleteDatorieQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@numeElev", nume);
                    deleteCommand.Parameters.AddWithValue("@prenumeElev", prenume);
                    deleteCommand.Parameters.AddWithValue("@grupaElev", grupa);
                    deleteCommand.Parameters.AddWithValue("@denumireCarte", denumire_carte);
                    deleteCommand.Parameters.AddWithValue("@autorCarte", autor);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Datoria a fost ștearsă cu succes!");
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a găsit nicio datorie pentru elevul și cartea specificată.","Eroare",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }

                // Actualizare stock carte
                string updateStockQuery = @"UPDATE Carti
                                            SET stock = stock + 1
                                            WHERE denumire_carti = @denumireCarte AND autor = @autorCarte";
                using (SqlCommand updateCommand = new SqlCommand(updateStockQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@denumireCarte", denumire_carte);
                    updateCommand.Parameters.AddWithValue("@autorCarte", autor);
                }
            }
        }
    }
}
