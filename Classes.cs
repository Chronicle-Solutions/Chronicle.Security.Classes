using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using FontAwesome.Sharp;
using Chronicle.Utils;
using Chronicle.Security.Classes.Objects;

namespace Chronicle.Security.Classes
{
    public partial class Classes : Form
    {

        public Classes()
        {
            InitializeComponent();
            populateClasses();
        }

        public async void populateClasses()
        {
            using (MySqlConnection conn = new MySqlConnection(Globals.ConnectionString))
            {
                await conn.OpenAsync();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM OPERATOR_CLASS";
                MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ListViewItem itm = new ListViewItem();
                    itm.Text = reader["operatorClassID"] as string ?? "";
                    itm.SubItems.Add(reader["classDescr"] as string ?? "");
                    listView1.Items.Add(itm);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            if (listView1.SelectedItems.Count == 0)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox1.ReadOnly = false;
                groupBox2.Text = "Create Class";
                button1.Text = "Create";
                return;
            }

            textBox1.ReadOnly = true;
            groupBox2.Text = "Update Class";
            button1.Text = "Update";

            string classID = listView1.SelectedItems[0].Text;
            using (MySqlConnection conn = new MySqlConnection(Globals.ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand()
                {
                    Connection = conn,
                    CommandText = "SELECT * FROM OPERATOR_CLASS WHERE operatorClassID=@oprClsID",
                    Parameters = { new MySqlParameter("@oprClsID", classID) },
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read()) return;
                textBox1.Text = reader["operatorClassID"] as string ?? "";
                textBox2.Text = reader["classDescr"] as string ?? "";
                reader.Close();
            }

            using (MySqlConnection conn = new MySqlConnection(Globals.ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand()
                {
                    Connection = conn,
                    CommandText = "SELECT B.operatorID, CONCAT(B.lastName, ', ', B.firstName) as 'name', A.isPrimary FROM OPERATOR_CLASS_LINK A, OPERATORS B WHERE A.operatorID = B.operatorID AND A.operatorClassID = @oprClassID",
                    Parameters = { new MySqlParameter("@oprClassID", classID) }
                };
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem itm = new ListViewItem();
                    itm.Text = reader["operatorID"] as string ?? "";
                    itm.SubItems.Add(reader["name"] as string ?? "");
                    itm.SubItems.Add((reader["isPrimary"] as bool? ?? false) ? "Yes" : "No");
                    listView2.Items.Add(itm);

                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                // Insert
                using (MySqlConnection conn = new MySqlConnection(Globals.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand()
                    {
                        Connection = conn,
                        CommandText = "INSERT INTO OPERATOR_CLASS (operatorClassID, classDescr, addedBy, addedDt, updateBy, updateDt) " +
                        "VALUES (@oprClsID, @clsDesc, @oprID, current_timestamp, @oprID, current_timestamp);",
                        Parameters = { new MySqlParameter("@oprClsID", textBox1.Text), new MySqlParameter("@clsDesc", textBox2.Text), new MySqlParameter("@oprID", Globals.OperatorID) }
                    };
                    cmd.ExecuteNonQuery();
                    ListViewItem itm = listView1.Items.Add(textBox1.Text);
                    itm.SubItems.Add(textBox2.Text);
                }
            }
            else
            {
                // Update

                using (MySqlConnection conn = new MySqlConnection(Globals.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand()
                    {
                        Connection = conn,
                        CommandText = "UPDATE OPERATOR_CLASS SET classDescr = @clsDesc, updateBy = @oprID, updateDt = current_timestamp WHERE operatorClassID = @oprClsID",
                        Parameters = { new MySqlParameter("@oprClsID", textBox1.Text), new MySqlParameter("@clsDesc", textBox2.Text), new MySqlParameter("@oprID", Globals.OperatorID) }
                    };
                    cmd.ExecuteNonQuery();
                    listView1.SelectedItems[0].SubItems[1].Text = textBox2.Text;
                }
            }
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
