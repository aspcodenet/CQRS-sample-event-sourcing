using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            FillUsers();
        }
        void FillUsers()
        {
            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.Context.DataContext.CreateUnitOfWork(false))
            {
                Systementor.Database.Repositories.IRepository<UIClasses.Report_User> rep = uow.CreateRepository<UIClasses.Report_User>();
                foreach (UIClasses.Report_User user in rep.GetAll())
                {
                    cbUsers.Items.Add(user);
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cbUsers.SelectedIndex < 0)
            {
                MessageBox.Show("Select a user");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormNewUser oForm = new FormNewUser();
            if (oForm.ShowDialog(this) == DialogResult.OK)
            {
                Communications.CommandBus.UserCreate(oForm.textBox1.Text, oForm.textBox2.Text);
                //Sleep until found...
                while (true)
                {
                    FillUsers();
                    if (cbUsers.Items.Count > 0)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5000);

                }
            }
        }


    }
}
