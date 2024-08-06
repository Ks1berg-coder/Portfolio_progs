using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diploma
{
    public partial class Авторизация : Form
    {
        public Авторизация()
        {
            InitializeComponent();
        }
        private string getLogin()
        {
            token.login = textBox1.Text;
            return token.login;
        }
        private string getPassword()
        {
            return textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = getLogin();
            string password = getPassword();
            db.conn.Open();
            SqlDataReader dataReader = new SqlCommand($"SELECT login,[password],[id_role] FROM Users WHERE Users.login = '{login}'", db.conn).ExecuteReader();
            try
            {
                dataReader.Read();
                // Проверка пороля
                if (!(dataReader.GetString(1).Equals(password)))
                {
                    dataReader.Close();
                    throw new Exception("wrong pass");
                }
                token.id_role = Convert.ToInt32(dataReader.GetValue(2));
                db.conn.Close();
                this.Hide();
                new  ГлавноеМеню().ShowDialog();
                this.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Данные введены неверно!");
                db.conn.Close();
            }
        }


    }
}
