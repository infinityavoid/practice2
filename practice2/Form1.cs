using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace practice2
{

    public partial class Form1 : Form
    {

        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM Session", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Session");
                dataGridView1.DataSource = dataSet.Tables["Session"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[11, i] = linkCell;
                }
                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                btn.Name = "Продажа билета";
                btn.Text = "Продать";
                btn.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btn);
                ((DataGridViewTextBoxColumn)dataGridView1.Columns[7]).MaxInputLength = 5;
                ((DataGridViewTextBoxColumn)dataGridView1.Columns[8]).MaxInputLength = 3;
                ((DataGridViewTextBoxColumn)dataGridView1.Columns[9]).MaxInputLength = 3;
                textBox1.MaxLength = 3;
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView1.Columns[10].ReadOnly = true;// viruchka
                dataGridView1.Columns[0].ReadOnly = true;//id
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Session"].Clear();
                sqlDataAdapter.Fill(dataSet, "Session");
                dataGridView1.DataSource = dataSet.Tables["Session"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[11, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\anton\source\repos\practice2\practice2\Database1.mdf;Integrated Security=True");

            sqlConnection.Open();

            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                if (e.ColumnIndex == 11)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["Session"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, "Session");
                        }
                    }
                    else if (task == "Insert")
                    {
                        if (dataGridView1.Rows != null && dataGridView1.Rows.Count != 0)
                        {
                            MessageBox.Show("Есть пустые ячейки!");

                        }
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["Session"].NewRow();
                        row["Название"] = dataGridView1.Rows[rowIndex].Cells["Название"].Value;
                        row["Жанр"] = dataGridView1.Rows[rowIndex].Cells["Жанр"].Value;
                        row["Дата"] = dataGridView1.Rows[rowIndex].Cells["Дата"].Value;
                        row["Время"] = dataGridView1.Rows[rowIndex].Cells["Время"].Value;
                        row["Возрастное ограничение"] = dataGridView1.Rows[rowIndex].Cells["Возрастное ограничение"].Value;
                        row["Продолжительность"] = dataGridView1.Rows[rowIndex].Cells["Продолжительность"].Value;
                        row["Стоимость"] = dataGridView1.Rows[rowIndex].Cells["Стоимость"].Value;
                        row["Всего билетов"] = dataGridView1.Rows[rowIndex].Cells["Всего билетов"].Value;
                        row["Осталось билетов"] = dataGridView1.Rows[rowIndex].Cells["Осталось билетов"].Value;
                        row["Выручка"] = dataGridView1.Rows[rowIndex].Cells["Выручка"].Value;
                        dataSet.Tables["Session"].Rows.Add(row);
                        dataSet.Tables["Session"].Rows.RemoveAt(dataSet.Tables["Session"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        Validate();
                        dataGridView1.EndEdit();
                        dataGridView1.Rows[e.RowIndex].Cells[11].Value = "Delete";
                        sqlDataAdapter.Update(dataSet, "Session");
                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;
                        dataSet.Tables["Session"].Rows[r]["Название"] = dataGridView1.Rows[r].Cells["Название"].Value;
                        dataSet.Tables["Session"].Rows[r]["Жанр"] = dataGridView1.Rows[r].Cells["Жанр"].Value;
                        dataSet.Tables["Session"].Rows[r]["Дата"] = dataGridView1.Rows[r].Cells["Дата"].Value;
                        dataSet.Tables["Session"].Rows[r]["Время"] = dataGridView1.Rows[r].Cells["Время"].Value;
                        dataSet.Tables["Session"].Rows[r]["Возрастное ограничение"] = dataGridView1.Rows[r].Cells["Возрастное ограничение"].Value;
                        dataSet.Tables["Session"].Rows[r]["Продолжительность"] = dataGridView1.Rows[r].Cells["Продолжительность"].Value;
                        dataSet.Tables["Session"].Rows[r]["Стоимость"] = dataGridView1.Rows[r].Cells["Стоимость"].Value;
                        dataSet.Tables["Session"].Rows[r]["Всего билетов"] = dataGridView1.Rows[r].Cells["Всего билетов"].Value;
                        dataSet.Tables["Session"].Rows[r]["Осталось билетов"] = dataGridView1.Rows[r].Cells["Осталось билетов"].Value;
                        dataSet.Tables["Session"].Rows[r]["Выручка"] = dataGridView1.Rows[r].Cells["Выручка"].Value;
                        Validate();
                        dataGridView1.EndEdit();
                        sqlDataAdapter.Update(dataSet, "Session");
                        dataGridView1.Rows[e.RowIndex].Cells[11].Value = "Delete";
                    }
                    ReloadData();
                }
                if (e.ColumnIndex == 12)
                {
                    int r = e.RowIndex;
                    if (textBox1.Text != "")
                    {
                        if (dataGridView1.Rows[r].Cells[9].Value.ToString() != "")
                        {
                            if ((int)dataGridView1.Rows[r].Cells["Осталось билетов"].Value >= int.Parse(textBox1.Text))
                            {
                                dataSet.Tables["Session"].Rows[r]["Осталось билетов"] = (int)dataGridView1.Rows[r].Cells["Осталось билетов"].Value - int.Parse(textBox1.Text);
                                sqlDataAdapter.Update(dataSet, "Session");
                                dataGridView1.Rows[e.RowIndex].Cells[11].Value = "Delete";
                                MessageBox.Show("Билет успешно продан!");
                                ReloadData();
                            }
                            else
                            {
                                MessageBox.Show("Такого кол-ва билетов сейчас нет\n" + "В данный момент билетов осталось: " + dataGridView1.Rows[r].Cells[9].Value);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ячейка оставшихся билетов не заполнена");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите кол-во билетов к продаже");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e) //добавлена строка
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[11, lastRow] = linkCell;
                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)  //изменение ячейки
        {
            int iri = e.RowIndex;
            if (e.ColumnIndex == 9)
            {
                if ((dataGridView1.Rows[iri].Cells[8].Value.ToString() != "") && (dataGridView1.Rows[iri].Cells[9].Value.ToString() != ""))
                {
                    if ((int)dataGridView1.Rows[iri].Cells[9].Value > (int)dataGridView1.Rows[iri].Cells[8].Value)
                    {
                        MessageBox.Show("Оставшихся билетов больше чем изначальных!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Введите данные в столбце Всего билетов");
                }
            }
            if (e.ColumnIndex == 3)
            {
                if (((DateTime)dataGridView1.Rows[iri].Cells[3].Value).CompareTo(DateTime.Now) == -1)
                {
                    MessageBox.Show("Указана дата прошлого времени", "Предупреждение");
                    return;
                }
            }
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[11, rowIndex] = linkCell;
                    editingRow.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) //можно все вынести в dataerror
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 7)
            {
                TextBox tb = (TextBox)e.Control;
                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 8)
            {
                TextBox tb = (TextBox)e.Control;
                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 9)
            {
                TextBox tb = (TextBox)e.Control;
                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
            }
            else
            {
                TextBox tb = (TextBox)e.Control;
                tb.KeyPress -= tb_KeyPress;
            }
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ввод в texBox только цифр и кнопки Backspace
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)//полностью работает!
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if ((dataGridView1.Rows[i].Cells[7].Value.ToString() != "") && (dataGridView1.Rows[i].Cells[8].Value.ToString() != "") && (dataGridView1.Rows[i].Cells[9].Value.ToString() != ""))
                {
                    dataSet.Tables["Session"].Rows[i]["Выручка"] = ((int)dataGridView1.Rows[i].Cells[8].Value - (int)dataGridView1.Rows[i].Cells[9].Value) * (int)dataGridView1.Rows[i].Cells[7].Value;
                    sqlDataAdapter.Update(dataSet, "Session");
                    dataGridView1.Rows[i].Cells[11].Value = "Delete";
                }
            }
            ReloadData();
            MessageBox.Show("Выручка обновлена!");
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == 3) //если изменена ячейка даты, то проверяем, правильно ли ее ввел человек
            {
                MessageBox.Show("Введите данные корректно: дд/мм/гггг");
                return;
            }
            if (e.ColumnIndex == 6)
            {
                MessageBox.Show("Введите данные корректно: чч/мм/сс");
                return;
            }
            if (e.ColumnIndex == 4)
            {
                MessageBox.Show("Введите данные корректно: чч/мм/сс");
                return;
            }

        }
        int i = 0;
        private void button1_Click(object sender, EventArgs e)// идеально работает
        {
            if (textBox2.Text != "")
            {
                for (; i < dataGridView1.RowCount; i++)
                {
                    if (i == dataGridView1.RowCount - 1)
                    {
                        i = 0;
                        if (dataGridView1.Rows[i].Cells[1].Value.ToString().Contains(textBox2.Text))
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[1];
                            i++;
                            break;
                        }
                    }
                    else
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value.ToString().Contains(textBox2.Text))
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[1];
                            i++;
                            break;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
