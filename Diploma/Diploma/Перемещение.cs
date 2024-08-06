using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diploma
{
    public partial class Перемещение : Form
    {
        public Перемещение(object data)
        {
            InitializeComponent();
            update_CB_building();
            update_cb_room();
            update_information_selected_inventory(data);
            comboBox1.SelectedIndex= -1;
            comboBox2.SelectedIndex= -1;
        }
        private int id_room_from;
        private void update_CB_building()
        {
            db.conn.Open();
            DataSet ds = new DataSet();
            try
            {
                new SqlDataAdapter(
                $@"SELECT ID, [Building_Address] from [dbo].[building]",
                db.conn).Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
            }
            db.conn.Close();
            comboBox1.DisplayMember = "Building_Address";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = ds.Tables[0];
        }
        private void update_cb_room()
        {
            db.conn.Open();
            DataSet ds = new DataSet();
            try
            {
                new SqlDataAdapter(
                $@"SELECT ID, [name] from [dbo].[room]
                   Where room.id_building = {comboBox1.SelectedValue}",
                db.conn).Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
            }
            db.conn.Close();
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "ID";
            comboBox2.DataSource = ds.Tables[0];
        }
        private void update_information_selected_inventory(object data)
        {
            db.conn.Open();
            SqlDataReader sdr = new SqlCommand(
                $@"SELECT iml.inventory_num, i.name, i.description, r.name, b.Building_Address, r.id 
                    FROM [dbo].[inventory_movement_log] as iml
                    JOIN inventory as i ON i.inv_num = iml.inventory_num
                    JOIN room as r ON r.id = iml.room_to
					JOIN building as b ON b.id = r.id_building
                    WHERE iml.inventory_num = '{data.ToString()}' and 
                    iml.date=
                        (select max(date) 
                        from inventory_movement_log 
                        where iml.inventory_num = inventory_num)",
                db.conn).ExecuteReader();
            sdr.Read();
            textBox_building.Text = sdr.GetValue(4).ToString();
            textBox_invDescr.Text = sdr.GetValue(2).ToString();
            textBox_invName.Text = sdr.GetValue(1).ToString();
            textBox_invNum.Text = sdr.GetValue(0).ToString();
            textBox_room.Text = sdr.GetValue(3).ToString();
            id_room_from = (int)sdr.GetValue(5);
            sdr.Close();
            db.conn.Close();
        }
        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex >= 0)
            {
                comboBox2.Enabled = true;
            try
                {
                    update_cb_room();
                }
                catch (Exception)
                {

                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex <0 || comboBox2.SelectedIndex <0)
            {
                MessageBox.Show("Выберите куда хотите переместить имущество");
                return; 
            }
            db.conn.Open();
            new SqlDataAdapter(
                            $@"INSERT INTO [dbo].[inventory_movement_log]
                                ([date],[login],[inventory_num],[room_from],[room_to])
                                VALUES('{DateTime.Now}','{token.login}','{textBox_invNum.Text}',
                                        {id_room_from},{comboBox2.SelectedValue})",
                            db.conn).
                Fill(new DataSet());
            db.conn.Close();
        }
    }
}
