using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2_for_admin : Form
    {
        Form _FormLogin;
        Form _Customer;
        Form _Employee;
        public static Form _Product;
        Form _Supplier;
        Form _Sales;
        
        public Form2_for_admin(Form Formadm)
        {
            InitializeComponent();
            _FormLogin = Formadm;
        }

        private void Form2_for_admin_Load(object sender, EventArgs e)
        {
           
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            
        

        }

        private void Form2_for_admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

       

       

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            _Employee ??= new EmployeeList(this);
            _Employee.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            _Product ??= new Product(this);
            _Product.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            _Supplier ??= new Supliter(this);
            _Supplier.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            _Sales ??= new Sales(this);
            _Sales.ShowDialog();        
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            _FormLogin.Show();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            _FormLogin.Show();
        }
    }
}
