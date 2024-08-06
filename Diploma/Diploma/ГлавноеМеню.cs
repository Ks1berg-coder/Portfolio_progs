using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Diploma
{
    public partial class ГлавноеМеню : Form
    {
        public ГлавноеМеню()
        {
            InitializeComponent();
            applySystemRole();
            foreach (TabPage tp in tabControl1.TabPages)
            {
                foreach (DataGridView dgv in tp.Controls.OfType<DataGridView>())
                {
                    dgv.ReadOnly = true;
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
            };
            
        }
        private void applySystemRole()
        {
            if(token.id_role != 1)
            foreach (TabPage tp in tabControl1.TabPages)
            {
                tp.Parent = null;
            };
            if(token.id_role == 2)
            {
                tab_inventory_placement.Parent = tabControl1;
                tab_log_movement.Parent = tabControl1;

            }
            if(token.id_role == 3) 
            {
                tab_inventory_arrival.Parent = tabControl1;
                tab_inventory.Parent = tabControl1;
            }
        }
        #region update_cb
        private void update_CB_building(ComboBox CB)
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
            CB.DisplayMember = "Building_Address";
            CB.ValueMember = "ID";
            CB.DataSource = ds.Tables[0];
            CB.SelectedIndex = -1;
        }

        #endregion

        #region Inventory
        private void tab_inventory_Enter(object sender, EventArgs e)
        {
            update_datagrid_inventory();
        }
        private void update_datagrid_inventory()
        {
            db.conn.Open();
            DataSet ds = new DataSet();
            new SqlDataAdapter(
                $@"SELECT [inv_num] as [Инвентарный номер],[name] as [Наименование],[description] as [Описание] from inventory",
                db.conn).Fill(ds);
            dataGrid_inventory.DataSource = ds.Tables[0];
            db.conn.Close();
        }


        #endregion

        #region log_of_movement
        private void tab_log_movement_Enter(object sender, EventArgs e)
        {
            update_datagrid_log(string.Empty);
            update_CB_building(comboBox_logMovement_building);
            comboBox_logMovement_building.SelectedIndex = -1;
        }
        private void update_datagrid_log(string where)
        {
            db.conn.Open();
            try
            {
               
                DataSet ds = new DataSet();
                new SqlDataAdapter(
                    $@"SELECT iml.[date] as [Дата],CONCAT(u.[Surname],' ',u.[Name],' ',u.[Middle_name]) as [ФИО],i.[inv_num] as [Инвентарный номер], i.name as [Имущество],
                IIF(r1.id is null, 'Приход', r1.name) as [Откуда],
                r2.name as [Куда], b.[Building_Address] as [Корпус]
                FROM [dbo].[inventory_movement_log] as iml
                JOIN inventory as i  ON i.inv_num = iml.inventory_num
                LEFT JOIN room as r1 ON r1.ID = iml.room_from
                JOIN room as r2 ON r2.ID = iml.room_to
                JOIN users as u ON u.login = iml.login
                JOIN building as b ON b.id = r2.id_building
                {where}",
                    db.conn).Fill(ds);
                dataGridView_log.DataSource = ds.Tables[0];
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
            db.conn.Close();
        }
        private void button_log_apply_filter_Click(object sender, EventArgs e)
        {
            string inventoryQuerry = textbox_log_inventory.Text;
            string inventoryNumQuerry = "";
            if (textbox_log_inventoryNUM.Text != string.Empty)
                inventoryNumQuerry = $@"AND i.inv_num LIKE '%{textbox_log_inventoryNUM.Text}%' ";
            string loginQuerry = $"AND CONCAT(u.[Surname],' ',u.[Name],' ',u.[Middle_name]) LIKE '%{textbox_log_fio.Text}%'";
            string time_since = "";
            string time_until = $"AND '{DateTime.Now}' > iml.[date]";
            if (checkbox_log_since.Checked)
                time_since = $"AND '{dateTimePicker_log_since.Value}' < iml.[date]";
            if (checkbox_log_until.Checked)
                time_until = $"AND '{dateTimePicker_log_until.Value}' > iml.[date]";
            string buildingQuerry = "";
            if (!(comboBox_logMovement_building.SelectedIndex < 0))
                buildingQuerry = $"AND b.id = {comboBox_logMovement_building.SelectedValue}";
            string FilterQuery = $" WHERE i.name LIKE '%{inventoryQuerry}%' {inventoryNumQuerry} {loginQuerry} {time_since} {time_until} {buildingQuerry}";
            update_datagrid_log(FilterQuery);
        }
        private void button_log_create_statement_Click(object sender, EventArgs e)
        {
            create_excel();
        }
        private void create_excel()
        {
            // Создаем новый экземпляр класса Excel
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            // Создаем новую книгу Excel
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);

            // Создаем новый лист Excel
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            worksheet = workbook.Sheets["Лист1"];
            worksheet = workbook.ActiveSheet;

            // Объединяем ячейки первой строки и указываем надпись "Вывод данных найденного оборудования"
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, dataGridView_log.Columns.Count + 1]].Merge();
            worksheet.Cells[1, 1].Value = "Отчёт по перемещаемому имуществу";
            worksheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            worksheet.Cells[1, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            worksheet.Cells[1, 1].WrapText = true;

            // Объединяем ячейки второй строки столбцов A, B и C и указываем надпись "Дата вывода: "
            worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 3]].Merge();
            worksheet.Range[worksheet.Cells[2, 4], worksheet.Cells[2, 8]].Merge();
            worksheet.Range[worksheet.Cells[3, 7], worksheet.Cells[3, 8]].Merge();
            worksheet.Cells[2, 4].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
            worksheet.Cells[2, 1].Value = "Дата формирования отчета: ";
            worksheet.Cells[2, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            worksheet.Cells[2, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
            worksheet.Cells[2, 4].Value = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            // Выводим названия столбцов DataGridView в строке с надписью "Расположение соответствует базе данных"
            for (int i = 1; i <= dataGridView_log.Columns.Count; i++)
            {
                worksheet.Cells[3, i ].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                worksheet.Cells[3, i ] = dataGridView_log.Columns[i - 1].HeaderText;
                worksheet.Cells[3, i ].WrapText = true;
            }

            // Выводим данные DataGridView, начиная с клетки B4
            int row = 4;
            for (int i = 0; i < dataGridView_log.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView_log.Columns.Count; j++)
                {
                    // Выводим данные в столбец B
                    worksheet.Cells[row, j + 1] = dataGridView_log.Rows[i].Cells[j].Value.ToString();
                }
                row++;
            }

            // Добавляем тонкие границы во все клетки таблицы
            Microsoft.Office.Interop.Excel.Range range = worksheet.UsedRange;
            Microsoft.Office.Interop.Excel.Borders borders = range.Borders;
            borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
            borders.Weight = 1d;
            borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

            // Сохраняем файл Excel на рабочем столе
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            workbook.SaveAs(desktopPath + "\\Отчёт по перемещаемому оборудованию.xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            // Очищаем экземпляр Excel
            excel.Quit();
            workbook = null;
            excel = null;
            MessageBox.Show("Файл Excel успешно создан на рабочем столе.", "Уведомление");
        }

        private void button_log_reset_filter_Click(object sender, EventArgs e)
        {
            checkbox_log_since.Checked = false;
            checkbox_log_until.Checked = false;
            comboBox_logMovement_building.SelectedIndex = -1;
            textbox_log_fio.Text = string.Empty;
            textbox_log_inventory.Text = string.Empty;
            update_datagrid_log(string.Empty);
        }
        #endregion

        #region add/change users
        private void tab_users_Enter(object sender, EventArgs e)
        {
            datagrid_users_update();
            update_CB_users_roles();
        }

        private void datagrid_users_update() 
        {
            try
            {
                DataSet ds = new DataSet();
                new SqlDataAdapter(
                    $@"SELECT u.login as [Логин], Concat(u.Surname,' ',u.Name,' ',u.Middle_name) as [ФИО], r.name as [Роль] 
                FROM users as u
                JOIN roles as r ON r.ID = u.id_role",
                    db.conn).Fill(ds);
                dataGridView_users.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " CERF");
            }
        }
        private void update_CB_users_roles()
        {
            db.conn.Open();
            DataSet ds = new DataSet();
            try
            {
                new SqlDataAdapter(
                $@"SELECT ID,name from [dbo].[roles]",
                db.conn).Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
            }
            db.conn.Close();
            comboBox_users_roles.DisplayMember = "name";
            comboBox_users_roles.ValueMember = "ID";
            comboBox_users_roles.DataSource = ds.Tables[0];
        }
        private bool users_data_validation()
        {
            if(textBox_users_login.Text.Length > 32 || textBox_users_login.Text.Length < 6) 
            {
                MessageBox.Show("Логин должен содержать от 6 до 32 символов");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBox_users_SurName.Text) ||
                string.IsNullOrWhiteSpace(textBox_users_Name.Text) ||
                string.IsNullOrWhiteSpace(textBox_users_MiddleName.Text))
            {
                MessageBox.Show("Заполните ФИО");
                return false;
            }
            if (textBox_users_SurName.Text.Length > 30 ||
                textBox_users_Name.Text.Length > 30 ||
                textBox_users_MiddleName.Text.Length > 30)
            {
                MessageBox.Show("Поля для ФИО не должны превышать 30 символов");
                return false;
            }
            
            return true;
        }
        private string users_data_validation_password()
        {
            string a = $@"[password] = '{textBox_users_password.Text}',";
            if (string.IsNullOrWhiteSpace(textBox_users_password.Text))
                a = "";
            return a;
        }
        private void button_add_user_Click(object sender, EventArgs e)
        {
            if(!users_data_validation())
                return;
            if (string.IsNullOrWhiteSpace(textBox_users_password.Text) ||
                textBox_users_password.Text.Length > 32)
            {
                MessageBox.Show("Пароль не должен превышать 32 символа");
                return;
            }
            db.conn.Open();
            try
            {
                new SqlDataAdapter(
               $@"INSERT INTO users([login],[password],[Surname], [Name], [Middle_name],[id_role])
                VALUES('{textBox_users_login.Text}','{textBox_users_password.Text}',
                '{textBox_users_SurName.Text},'{textBox_users_Name}', '{textBox_users_MiddleName}',
                {comboBox_users_roles.SelectedValue})",
               db.conn).Fill(new DataSet());
                
            }
            catch (Exception)
            {
                MessageBox.Show("Такой логин уже занят");
                return;
            }
            db.conn.Close();
            MessageBox.Show("Пользователь успешно добавлен!");
            datagrid_users_update();
        }
        

        private void button_change_user_Click(object sender, EventArgs e)
        {
            if (!users_data_validation())
                return;

            db.conn.Open();
            try
            {
                new SqlDataAdapter(
                    $@"UPDATE users
                    SET 
                    {users_data_validation_password()}
                    [Surname] = '{textBox_users_SurName.Text}',
                    [Name] = '{textBox_users_Name.Text}',
                    [Middle_name] = {textBox_users_MiddleName.Text},
                    [id_role] = {comboBox_users_roles.SelectedValue}
                    WHERE login = {textBox_users_login.Text}",
                    db.conn).Fill(new DataSet());
            }
            catch (Exception)
            {
                MessageBox.Show("Пользователя с таким логином не было найдено в базе данных. \n Изменение не возможно");
                return;
            }
            db.conn.Close();
            MessageBox.Show("Данные пользователя успешно изменены!");
            datagrid_users_update();
        }
        private void dataGridView_users_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            db.conn.Open();
            try
            {
                string login = dataGridView_users.SelectedRows[0].Cells[0].Value.ToString();
                SqlDataReader sdr = new SqlCommand(
                $@"SELECT u.[Surname], u.[Name], u.[Middle_name], u.[id_role]
                FROM users as u 
                WHERE u.login = '{login}'",
                db.conn).ExecuteReader();
                sdr.Read();
                textBox_users_login.Text = login;
                textBox_users_SurName.Text = sdr.GetString(0);
                textBox_users_Name.Text = sdr.GetString(1);
                textBox_users_MiddleName.Text = sdr.GetString(2);
                comboBox_users_roles.SelectedValue = sdr.GetValue(3);
                sdr.Close();
            }
            catch (Exception ex)
            {
                
            }
            db.conn.Close();
        }
        #endregion

        #region types_of_room
        private void tab_type_of_room_Enter(object sender, EventArgs e)
        {
            update_datagrid_types_of_room();
        }
        int id_type_of_room;
        private void update_datagrid_types_of_room()
        {
            DataSet ds = new DataSet();
            db.conn.Open();
            new SqlDataAdapter("SELECT id, [name] as [Наименование] from [dbo].[type_of_room]", db.conn).Fill(ds);
            dataGridView_typesOfRoom.DataSource = ds.Tables[0];
            db.conn.Close();
            dataGridView_typesOfRoom.Columns[0].Visible = false;
        }

        private void button_add_typeOfRoom_Click(object sender, EventArgs e)
        {
            new SqlCommand($@"INSERT INTO [dbo].[type_of_room](name) VALUES('{textBox_typeOfRoom_name.Text}')",db.conn).ExecuteNonQuery();
            MessageBox.Show("Новый тип помещения успешно добавлен!");
        }

        private void button_change_typeOfRoom_Click(object sender, EventArgs e)
        {
            new SqlCommand($@"UPDATE [dbo].[type_of_room] 
                            SET name = '{textBox_typeOfRoom_name.Text}'
                            WHERE id ={id_type_of_room}",db.conn).ExecuteNonQuery();
            MessageBox.Show("Тип помещения был успешно изменен!");
        }
        private void dataGridView_typesOfRoom_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                id_type_of_room = (int)dataGridView_typesOfRoom.SelectedRows[0].Cells[0].Value;
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region buildings
        private void tab_buildings_Enter(object sender, EventArgs e)
        {
            update_datagrid_buildings();
        }
        int id_buildings;
        private void update_datagrid_buildings()
        {
            DataSet ds = new DataSet();
            db.conn.Open();
            new SqlDataAdapter("SELECT [Building_Address] as [Адрес корпуса] from [dbo].[building]", db.conn).Fill(ds);
            dataGridView_buildings.DataSource = ds.Tables[0];
            db.conn.Close();
        }

        private void button_add_building_Click(object sender, EventArgs e)
        {
            new SqlCommand($@"INSERT INTO [dbo].[building]([Building_Address]) VALUES('{textBox_buildings_adressName.Text}')", db.conn).ExecuteNonQuery();
            MessageBox.Show("Корпус успешно добавлен в базу!");
        }

        private void button_change_building_Click(object sender, EventArgs e)
        {
            new SqlCommand($@"UPDATE building 
                            SET Building_Address = '{textBox_buildings_adressName.Text}'
                            WHERE id = {id_buildings}",db.conn).ExecuteNonQuery();
            MessageBox.Show("Адрес выбранного корпуса был изменен!");
        }
        private void dataGridView_buildings_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                id_buildings = (int)dataGridView_buildings.SelectedRows[0].Cells[0].Value;
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region rooms
        int id_selected_room;
        private void update_datagrid_rooms()
        {
            DataSet ds = new DataSet();
            db.conn.Open();
            new SqlDataAdapter(
                $@"SELECT r.id, r.name as [Помещение], r.level as [Этаж], t.name as [Тип помещения], b.Building_address as [Корпус] 
                FROM room as r
                join [dbo].[type_of_room] as t ON t.id = r.[type_of_room]
                join [dbo].[building] as b ON b.id = r.[id_building]",
                db.conn).Fill(ds);
            dataGridView_rooms.DataSource = ds.Tables[0];
            db.conn.Close();
            dataGridView_rooms.Columns[0].Visible = false;
        }

        private void update_CB_type_room()
        {
            db.conn.Open();
            DataSet ds = new DataSet();
            try
            {
                new SqlDataAdapter(
                $@"SELECT ID,name from [dbo].[type_of_room]",
                db.conn).Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
            }
            db.conn.Close();
            comboBox_room_typeOfRoom.DisplayMember = "name";
            comboBox_room_typeOfRoom.ValueMember = "ID";
            comboBox_room_typeOfRoom.DataSource = ds.Tables[0];
        }
        
        private void tab_room_Enter(object sender, EventArgs e)
        {
            update_CB_type_room();
            update_CB_building(comboBox_room_building);
            update_datagrid_rooms();
            comboBox_room_building.SelectedIndex = -1;
            comboBox_room_typeOfRoom.SelectedIndex= -1;
        }

        private bool room_validation()
        {

            return false;
        }
        private void button_add_room_Click(object sender, EventArgs e)
        {
            if (!room_validation())
                return;
            db.conn.Open();
            new SqlCommand(
                $"INSERT INTO [dbo].[room]([name],[level],[type_of_room],[id_building])" +
                $"VALUES('{textBox_room_name}',{comboBox_room_level.Text},{comboBox_room_typeOfRoom},{comboBox_room_building})",
                db.conn).ExecuteNonQuery();
            db.conn.Close();
            MessageBox.Show("Помещение успешно добавлено!");
        }
        private void dataGridView_rooms_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                id_selected_room = (int)dataGridView_rooms.SelectedRows[0].Cells[0].Value;
            }
            catch (Exception)
            {

            }
            
        }
        private void button_change_room_Click(object sender, EventArgs e)
        {
            if (!room_validation())
                return;
            if(!(dataGridView_rooms.SelectedRows.Count > 0))
            {
                MessageBox.Show("Выберите помещение которое хотите изменить!");
                return;
            }
            db.conn.Open();
            new SqlCommand(
                $@"UPDATE [dbo].[room] SET
                [name] = '{textBox_room_name}',
                [level] = {comboBox_room_level.Text},
                [type_of_room] = {comboBox_room_typeOfRoom},
                [id_building] = {comboBox_room_building}
                WHERE id = {id_selected_room}",
                db.conn).ExecuteNonQuery();
            db.conn.Close();
            MessageBox.Show("Помещение успешно изменено!");
        }
        #endregion

        #region inventory_placement ??

        private void update_datagrid_inventory_placement(string additional_querry)
        {
            DataSet ds = new DataSet();
            db.conn.Open();
            new SqlDataAdapter(
                $@"SELECT iml.inventory_num as [Инвентарный номер], i.name as [Имущество], r.name as [Помещение], b.[Building_Address] as [Корпус]
                FROM [dbo].[inventory_movement_log] as iml
                JOIN inventory as i ON i.inv_num = iml.inventory_num
                JOIN room as r ON r.id = iml.room_to
                JOIN building as b ON r.id_building = b.id
                WHERE iml.date=(select max(date) from inventory_movement_log where iml.inventory_num = inventory_num) 
                {additional_querry}",
                db.conn).Fill(ds);
            dataGridView_inventory_placement.DataSource = ds.Tables[0];
            db.conn.Close();
        }
        private void create_additional_querry_for_InventoryPlacement_Datagrid()
        {
            string inventoryQuerry = $"";
            string inventoryNumQuerry = "";
            string roomQuerry = "";
            string buildingQuerry = "";
            if (textBox_inventoryPlacement_invNUM.Text != string.Empty)
                inventoryNumQuerry = $@"AND i.inv_num LIKE '%{textBox_inventoryPlacement_invNUM.Text}%' ";
            
            if (!(comboBox_inventoryPlacement_building.SelectedIndex < 0))
                buildingQuerry = $"AND b.id = {comboBox_inventoryPlacement_building.SelectedValue} ";
            
            if (textBox_inventoryPlacement_inventoryName.Text != string.Empty)
                inventoryQuerry = $"AND i.name LIKE '%{textBox_inventoryPlacement_inventoryName.Text}%' ";

            if (textBox_inventoryPlacement_roomName.Text != string.Empty)
                roomQuerry = $"AND r.name LIKE '%{textBox_inventoryPlacement_inventoryName.Text}%' ";
            string FilterQuery = $"{inventoryQuerry} {inventoryNumQuerry} {buildingQuerry} {roomQuerry}";
            update_datagrid_inventory_placement(FilterQuery);
            string filter = "";
            if (!String.IsNullOrEmpty(textBox_inventoryPlacement_roomName.Text))
            {
                filter += string.Format("[Помещение] LIKE '%{0}%'", textBox_inventoryPlacement_roomName.Text);
            }
            (dataGridView_inventory_placement.DataSource as System.Data.DataTable).DefaultView.RowFilter = filter;
        }

        private void button_inventoryplacement_filterReset_Click(object sender, EventArgs e)
        {
            textBox_inventoryPlacement_inventoryName.Clear();
            comboBox_inventoryPlacement_building.SelectedIndex = -1;
            textBox_inventoryPlacement_roomName.Clear();
            update_datagrid_inventory_placement(string.Empty);
        }

        private void tab_inventory_placement_Enter(object sender, EventArgs e)
        {
            update_CB_building(comboBox_inventoryPlacement_building);
            update_datagrid_inventory_placement(string.Empty);
        }

        private void dataGridView_inventory_placement_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            new Перемещение(dataGridView_inventory_placement.SelectedRows[0].Cells[0].Value).ShowDialog();
        }

        private void textBox_inventoryPlacement_inventoryName_TextChanged(object sender, EventArgs e)
        {
            create_additional_querry_for_InventoryPlacement_Datagrid();
        }

        private void comboBox_inventoryPlacement_building_SelectedValueChanged(object sender, EventArgs e)
        {
            create_additional_querry_for_InventoryPlacement_Datagrid();
        }

        private void textBox_inventoryPlacement_roomName_TextChanged(object sender, EventArgs e)
        {
            create_additional_querry_for_InventoryPlacement_Datagrid();
        }
        private void textBox_inventoryPlacement_invNUM_TextChanged(object sender, EventArgs e)
        {
            create_additional_querry_for_InventoryPlacement_Datagrid();
        }

        #endregion

        #region arrival
        private void tab_inventory_arrival_Enter(object sender, EventArgs e)
        {
            
            try
            {
                update_CB_building(comboBox_arrival_building);
                arrival_clean_form();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        
        struct inventory_item
        {
            public string inventory_num;
            public string inventory_name;
            public string inventory_description;
            public double inventory_price;
            public int room_id;
        }
        Dictionary<string,inventory_item> inventory_list = new Dictionary<string, inventory_item>();

        private void update_CB_arrival_Room()
        {
            db.conn.Open();
            DataSet ds = new DataSet();
            try
            {
                new SqlDataAdapter(
                $@"SELECT ID, [name] from [dbo].[room] where id_building = {comboBox_arrival_building.SelectedValue}",
                db.conn).Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
            }
            db.conn.Close();
            comboBox_arrival_room.DataSource = ds.Tables[0];
            comboBox_arrival_room.DisplayMember = "Name";
            comboBox_arrival_room.ValueMember = "ID";
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {

        }
        private void arrival_add_To_List_and_ListBox()
        {
            inventory_item ii = new inventory_item();
            ii.inventory_num = textBox_arrival_invNum.Text;
            ii.inventory_name = textBox_arrival_invName.Text;
            ii.inventory_price = (double)numericUpDown_arrival_invPrice.Value;
            ii.inventory_description = textBox_arrival_invDescr.Text;
            ii.room_id = (int)comboBox_arrival_building.SelectedValue;
            inventory_list.Add(ii.inventory_num, ii);
            listBox1.Items.Add(ii.inventory_num);
        }
        private void button_add_to_Arrival_List_Click(object sender, EventArgs e)
        {
            arrival_add_To_List_and_ListBox();
        }

        private void button_change_in_arrival_list_Click(object sender, EventArgs e)
        {
            string key = listBox1.SelectedItem.ToString();
            inventory_list.Remove(key);
            listBox1.Items.Remove(key);
            arrival_add_To_List_and_ListBox();

            MessageBox.Show(key);
        }

        private void button_delete_from_arrival_list_Click(object sender, EventArgs e)
        {
            arrival_clean_form();

        }
        private void arrival_write_data_in_DataBase(int arrival_id, string key, DateTime date)
        {
            // create [dbo].[inventory]
            new SqlCommand(
                $@"INSERT INTO [dbo].[inventory]([inv_num],[name],[description])
                VALUES('{key}','{inventory_list[key].inventory_name}','{inventory_list[key].inventory_description}')",
                db.conn).ExecuteNonQuery();
            // create [dbo].[list_of_arrival]
            new SqlCommand(
                $@"INSERT INTO [dbo].[list_of_arrival]([inventory_num],[arrival_id],[room_id],[price]) 
                VALUES('{key}',{arrival_id},{inventory_list[key].room_id},{inventory_list[key].inventory_price})",
                db.conn).ExecuteNonQuery();
            // create [dbo].[inventory_movement_log]
            new SqlCommand(
                $@"INSERT INTO [dbo].[inventory_movement_log]([date],[login],[inventory_num],[room_to]) 
                VALUES('{date}','{token.login}','{key}',{inventory_list[key].room_id})",
                db.conn).ExecuteNonQuery();
        }
        private void button_create_arrival_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = DateTime.Now;
                db.conn.Open();
                // create [dbo].[inventory_arrival]
                new SqlCommand(
                    $@"INSERT INTO [dbo].[inventory_arrival](date,login) VALUES('{date}','{token.login}')",
                    db.conn).ExecuteNonQuery();

                // take last id from inventory arrival to add this in other tables
                SqlDataReader sdr_id = new SqlCommand(
                    $@"SELECT id FROM [dbo].[inventory_arrival] WHERE date = '{date}'",
                    db.conn).ExecuteReader();
                sdr_id.Read();
                int arrival_id = sdr_id.GetInt32(0);
                sdr_id.Close();
                db.conn.Close();
                foreach (string inv_item_key in inventory_list.Keys)
                {
                    db.conn.Open();
                    arrival_write_data_in_DataBase(arrival_id, inv_item_key, date);
                    db.conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + new StackFrame(0, true).GetFileLineNumber());
                return;
            }
            arrival_clean_form();
            listBox1.Items.Clear();
            inventory_list.Clear();
            MessageBox.Show("Приход оформлен успешно!");
        }
        private void arrival_clean_form()
        {
            try
            {
                foreach (Control item in tab_inventory_arrival.Controls)
                {
                    if (item.GetType() == typeof(System.Windows.Forms.TextBox) || item.GetType() == typeof(NumericUpDown))
                    {
                        item.Text = "";
                    }
                }
                comboBox_arrival_building.SelectedIndex = -1;
                comboBox_arrival_room.SelectedIndex = -1;
                DataSet ds = new DataSet(); ds.Tables.Add();
                comboBox_arrival_room.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void comboBox_arrival_building_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox_arrival_building.SelectedIndex != -1)
                update_CB_arrival_Room();
        }






        #endregion

        
    }
}
