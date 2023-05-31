using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class BuyProduct : Form
    {
        DB dB = new DB();
        List<MyList> products = new List<MyList>();
        Form _LastForm;

        private static int CountTextB { get; set; } = 1;
        private static double sum { get; set; }
        private static double lastsum { get; set; }=0;
        public static int amount { get; set; }
        public static int id_supplier { get; set; } 
        public static int id_employee { get; set; }
        public static int id_invoice { get; set; }
        public string nameSupplier { get; set; }
        public static int countprodfromlist { get; set; } = 0;

        private static string name_prodCB { get; set; }
        public BuyProduct(Form _LastForm)
        {
            InitializeComponent();
            this._LastForm = _LastForm;
        }

        private void BuyProduct_Load(object sender, EventArgs e)
        {
            #region Buttons
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            #endregion

            _SetCombobox();
            dataGridView1.Font = new Font("Century Gothic", 12, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //comboBox1.SelectedIndex = 0;
            //comboBox2.SelectedIndex = 0;
        }
        private void BuyProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        #region ComboBox

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            name_prodCB = comboBox1.Text;
            dB.openConnection();

            MySqlCommand command1 = new MySqlCommand($"Select `Prod_amount` from `product` where(Product_name = '{name_prodCB}') and State = '1'", dB.GetConnection());
            command1.ExecuteNonQuery();
            textBox2.Text = command1.ExecuteScalar().ToString();

            MySqlCommand command2 = new MySqlCommand($"Select `Prod_price` from `product` where(Product_name = '{name_prodCB}') and State = '1'", dB.GetConnection());
            command2.ExecuteNonQuery();
            textBox3.Text = command2.ExecuteScalar().ToString();
            sum = Convert.ToDouble(textBox3.Text) * Convert.ToDouble(CountTextB);
            textBox5.Text = sum.ToString() ;

            MySqlCommand command3 = new MySqlCommand($"Select `Prod_country` from `product` where(Product_name = '{name_prodCB}') and State = '1'", dB.GetConnection());
            command3.ExecuteNonQuery();
            textBox4.Text = command3.ExecuteScalar().ToString();

           

            dB.closeConnection();

        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            nameSupplier = comboBox2.Text;
            dB.openConnection();
            MySqlCommand cmd2 = new MySqlCommand($"Select `Id_supplier` from supplier where(Supl_name_firm = '{nameSupplier}')", dB.GetConnection());
            cmd2.ExecuteNonQuery();
            id_supplier = int.Parse(cmd2.ExecuteScalar().ToString());
            dB.closeConnection();
        }
        
        #endregion

        #region MyMethods

        /// <summary>
        /// Из бд в комбобокс
        /// </summary>
        public void _SetCombobox()
        {
            dB.openConnection();
            DataTable _table = new DataTable();
            MySqlDataAdapter _adapter = new MySqlDataAdapter();

            #region Combobox1
            MySqlCommand cmd = new MySqlCommand("Select `Product_name` from product where State = '1'", dB.GetConnection());
            MySqlDataReader my;
            my = cmd.ExecuteReader();
            while (my.Read())
            {
                string sname = my.GetString("Product_name");
                comboBox1.Items.Add(sname);
            }
            dB.closeConnection();
            comboBox1.SelectedIndex = 0;
            #endregion

            #region Combobox2
            dB.openConnection();
            MySqlCommand cmd2 = new MySqlCommand("Select `Supl_name_firm` from supplier where State = '1'", dB.GetConnection());
            MySqlDataReader my2;
            my2 = cmd2.ExecuteReader();
            while (my2.Read())
            {
                string sname = my2.GetString("Supl_name_firm");
                comboBox2.Items.Add(sname);
            }
            dB.closeConnection();
            comboBox2.SelectedIndex = 0;
            #endregion

            
        }

        /// <summary>
        /// Удаление из комбобос
        /// </summary>
        public void _DellCombobox()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
        }
      
        /// <summary>
        /// Добавление продуктов в список
        /// </summary>
        /// <param name="myList"></param>
        private void AddProd(MyList myList)
        {
            int _coun_mismatch = 0;

            if (products.Count == 0)
            {
                products.Add(myList);
               
            }
            else
            {
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].name == myList.name)
                    {
                        if (MessageBox.Show("Товар вже є в кошику, бажаєте замінити?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            products[i].count = myList.count;
                            products[i].price = myList.price;
                            countprodfromlist = 0;
                            amount = 0;
                            lastsum = 0;
                            CountCount();
                        }
                    }
                    else
                    {
                        _coun_mismatch++;
                        if (_coun_mismatch == products.Count)
                        {
                            products.Add(myList);
                            
                            return;
                        }
                    }
                    
                }
            }
            
        }

        /// <summary>
        /// Удаление продукта из списка
        /// </summary>
        /// <param name="name"></param>
        public  void DellProd(string name)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].name == name)
                {
                    products.Remove(products[i]);
                    countprodfromlist = 0;
                    amount =0 ;
                    lastsum = 0;
                    CountCount();
                }
            }
        }
        
        /// <summary>
        /// Подсчитывает количество едениц продукта из списка
        /// </summary>
        private void CountCount()
        {
            for(int i = countprodfromlist; i < products.Count; i++)
            {
                amount += products[i].count;
                lastsum += products[i].price;
                countprodfromlist++;
            }
        }

   
        /// <summary>
     /// Обновление данных в бд (количество)
     /// </summary>
        private void SetAmountInBD()
        {
            dB.openConnection();
            for(int i =0; i< products.Count; i++) 
            {
                MySqlCommand command = new MySqlCommand($"UPDATE `product` SET `Prod_amount` = Prod_amount +'{products[i].count}' WHERE (`Product_name` = '{products[i].name}');",dB.GetConnection());
                command.ExecuteNonQuery();       
                countprodfromlist=0;
                amount=0;
                lastsum = 0;
            }
            dB.closeConnection();
        }

        /// <summary>
        /// Список поставок
        /// </summary>
        private void AcceptanceList()
        {
            Invoice();
            dB.openConnection();
            int id_prod;
            for (int i = 0; i < products.Count; i++)
            {
                MySqlCommand cmd = new MySqlCommand($"select `Id_product` from product where `Product_name` = '{products[i].name}'",dB.GetConnection()) ;
                cmd.ExecuteNonQuery();
                id_prod = int.Parse(cmd.ExecuteScalar().ToString());

                MySqlCommand cmd1 = new MySqlCommand($"INSERT INTO `acceptance_list` (`Id_ invoice`, `Id_product`, `Amount_product`, `Purchase_price`) VALUES('{id_invoice}', '{id_prod}', '{products[i].count}', '{products[i].price}');", dB.GetConnection());
                cmd1.ExecuteNonQuery();

            }
            dB.closeConnection();
        }

        /// <summary>
        /// Накладная
        /// </summary>
        private void Invoice()
        {
            dB.openConnection();
            MySqlCommand cmd = new MySqlCommand($"SELECT Id_employee From `employee` join register using (Id_employee) where login_user = '{Form1._logi}'", dB.GetConnection());
            cmd.ExecuteNonQuery();
            id_employee = int.Parse(cmd.ExecuteScalar().ToString());

            MySqlCommand cmd1 = new MySqlCommand($"INSERT INTO `invoice` (`Id_employee`, `Id_supplier`, `acceptance _date`) VALUES ('{id_employee}', '{id_supplier}', curdate());", dB.GetConnection());
            cmd1.ExecuteNonQuery();

            MySqlCommand cmd2 = new MySqlCommand($"SELECT max(`Id_ invoice`) FROM invoice;", dB.GetConnection());
            cmd2.ExecuteNonQuery();
            id_invoice = int.Parse(cmd2.ExecuteScalar().ToString());
            dB.closeConnection();
        }

        #endregion

        #region Buttons
        
        //Кнопка купить
        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви точно бажаєте купити?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (textBox7.Text == String.Empty)
                {
                    MessageBox.Show("Заповніть кошик");
                }
                else
                {
                    AcceptanceList();
                    SetAmountInBD();
                    products.Clear();
                    dataGridView1.DataSource = null;
                    _DellCombobox();
                    _SetCombobox();
                    
                    textBox6.Text = String.Empty;
                    textBox7.Text = String.Empty;
                    
                   
                    lastsum = 0;
                    amount = 0;
                    countprodfromlist = 0;
                    //comboBox1.SelectedIndex = 0;
                    //comboBox2.SelectedIndex = 0;
                    MessageBox.Show("Ви успішно закупили товар");
                }
            }
        }
        
        //Кнопка добавить в К...
        private void button4_Click(object sender, EventArgs e)
        {
            AddProd(new MyList(name_prodCB, Convert.ToInt32(textBox1.Text), Convert.ToDouble(textBox5.Text)));
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = products;
            CountCount();
            textBox7.Text = amount.ToString();
            textBox6.Text = lastsum.ToString();
           
        }

        //Кнопка удалить с К...
        private void button6_Click(object sender, EventArgs e)
        {
            DellProd(name_prodCB);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource=products;
            if(amount ==0)
            {
                textBox7.Text = String.Empty;
                textBox6.Text = String.Empty;
            }
            else
            {
                textBox7.Text = amount.ToString();
                textBox6.Text = lastsum.ToString();
            }
           
        }

        //Кнопка назад
        private void button1_Click(object sender, EventArgs e)
        {
            _DellCombobox();
            this.Hide();
            _LastForm.Show();
        }

        //Кнопка +
        private void button2_Click(object sender, EventArgs e)
        {
            //if (CountTextB != 10)
            //{
                CountTextB += 1;
                sum += double.Parse(textBox3.Text);
                textBox5.Text = sum.ToString();
                textBox1.Text = CountTextB.ToString();
                
            
        }

        //Кнопка -
        private void button3_Click(object sender, EventArgs e)
        {
            if (CountTextB != 1)
            {
                CountTextB -= 1;
                sum -= double.Parse(textBox3.Text);
                textBox5.Text = sum.ToString();
                textBox1.Text = CountTextB.ToString();
            }
        }


        #endregion

    }

    class MyList
    {
        public string name { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public MyList(string name, int count, double price)
        {
            this.name = name;
            this.count = count;
            this.price = price;
        }
    }
    
}
