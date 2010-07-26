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
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (user == null)
                return;

            //Check to see we have done a change
            if (user.Forname == txtForname.Text && user.Surname == txtSurname.Text)
                return;
            //Basically - we do as MUCH validation as we can here. This way we mimimize the risk of
            //having the asynch command fail
            
            //We do it by using the query model - what else???
            
            //lets make up a fake stupid control - forname + " " + surname must be unique
            
            //when it comes to server validation: 
            //in real life we would have a unique contraint in the database to handle this 
            
            //but the point is - we wanna make sure as much as possible goes ok with asynch commands so
            //here on the client we do it also as well as we can

            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.Context.DataContext.CreateUnitOfWork(false))
            {
                Systementor.Database.Repositories.IRepository<UIClasses.Report_User> rep = uow.CreateRepository<UIClasses.Report_User>();
                int cnt = rep.Count(p => p.Surname == txtSurname.Text && p.Forname == txtForname.Text && p.User_Id != user.User_Id);
                if (cnt > 0)
                {
                    MessageBox.Show("That forname and surname is taken");
                    return;
                }
            }
            Communications.CommandBus.UserChangeName(user.User_Id, txtForname.Text, txtSurname.Text);

            //And we are gonna Change Name
            //System.Guid uid = Communications.CommandBus.UserChangeName(user.User_Id,txtForname.Text,txtSurname.Text);
            //This is a typical scenario where we never wanna get a result back
            //I mean the screen is already ok
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnLogin_Click(null, null);
            if (user == null)
            {
                Close();
            }
        }

        UIClasses.Report_User user;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            FormLogin oForm = new FormLogin();
            if (oForm.ShowDialog(this) == DialogResult.OK)
            {
                user = oForm.cbUsers.SelectedItem as UIClasses.Report_User;
                UserForm();
                IAmGoingToForm();
                IAmHostingForm();
                SignupForDinnerForm();
            }
        }
        void SignupForDinnerForm()
        {
            lvSignup.Items.Clear();
            if (user == null)
                return;

            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.Context.DataContext.CreateUnitOfWork(false))
            {
                //OK - so I wanted to use somehing like
                //foreach (UIClasses.Report_Dinner dinner in rep.Find(p => p.UsersComing.All(r => r.User_Id != user.User_Id)))
                //seems like nhibernate linq doesn't manage it correctly - or its me doing it wroing
                //nevermind - with nhibernate its so easy to do a query instead
                Systementor.Database.Repositories.IRepository<UIClasses.DinnersNotGoingTo> rep = uow.CreateRepository<UIClasses.DinnersNotGoingTo>();
                Dictionary<string,object> params1 = new Dictionary<string,object>();
                params1.Add("user_id", user.User_Id);

                foreach (UIClasses.DinnersNotGoingTo d2 in rep.ExecuteNamedQuery("q1DinnersNotGoingTo", params1))
                {
                    ListViewItem i = lvSignup.Items.Add(d2.Date.ToString());
                    i.SubItems.Add(d2.Location);
                    i.SubItems.Add(d2.Organizer_Fullname);
                    i.Tag = d2;
                }
            }
            lvSignup.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        }
        void IAmGoingToForm()
        {

            lvGoingTo.Items.Clear();
            if (user == null)
                return;

            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.Context.DataContext.CreateUnitOfWork(false))
            {
                Systementor.Database.Repositories.IRepository<UIClasses.Report_Dinner> rep = uow.CreateRepository<UIClasses.Report_Dinner>();
                foreach (UIClasses.Report_Dinner dinner in rep.Find(p => p.UsersComing.Any( r=> r.User_Id == user.User_Id)))
                {
                    ListViewItem i = lvGoingTo.Items.Add(dinner.Date.ToString());
                    i.SubItems.Add(dinner.Location);
                    i.SubItems.Add(dinner.Organizer_Fullname);
                    i.SubItems.Add(dinner.UsersComing.Count.ToString());
                    i.Tag = dinner;
                }
            }
            lvGoingTo.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        }
        void IAmHostingForm()
        {
            lvIAmHosting.Items.Clear();
            if (user == null)
                return;

            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.Context.DataContext.CreateUnitOfWork(false))
            {
                Systementor.Database.Repositories.IRepository<UIClasses.Report_Dinner> rep = uow.CreateRepository<UIClasses.Report_Dinner>();
                foreach (UIClasses.Report_Dinner dinner in rep.Find(p=>p.Organizer_User_id == user.User_Id ))
                {
                    ListViewItem i =  lvIAmHosting.Items.Add(dinner.Date.ToString());
                    i.SubItems.Add( dinner.Location );
                    i.SubItems.Add(dinner.Organizer_Fullname);
                    i.SubItems.Add(dinner.UsersComing.Count.ToString());
                    i.Tag = dinner;
                }
            }
            lvIAmHosting.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        }
        void UserForm()
        {
            if (user == null)
                return;
            //Ugly ?? this is not my point...
            txtForname.Text = user.Forname;
            txtSurname.Text = user.Surname;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (user == null)
                return;
            if (lvGoingTo.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a dinner first");
                return;
            }
            UIClasses.Report_Dinner dinner = lvGoingTo.SelectedItems[0].Tag as UIClasses.Report_Dinner;

            //What type of validation can we do at the UI
            //Of course check if user already is signed up
            //How - of course the best we can do is query our report model
            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.Context.DataContext.CreateUnitOfWork(false))
            {
                Systementor.Database.Repositories.IRepository<UIClasses.Report_Dinner> rep = uow.CreateRepository<UIClasses.Report_Dinner>();
                int nCnt = rep.Count(r => r.UsersComing.Any(us => us.User_Id == user.User_Id));
                if (nCnt == 0)
                {
                    MessageBox.Show("You dont seem to have opted in for that dinner");
                    return;
                }
            }
            Communications.CommandBus.UserOptOutForDinner(user.User_Id, dinner.Dinner_Id);
            //System.Guid uid = Communications.CommandBus.UserOptInForDinner(user.User_Id, dinner.Dinner_Id);
            MessageBox.Show("If all went ok you will get an email sent to you in a few minutes time or so...");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (user == null)
                return;
            if ( lvSignup.SelectedItems.Count == 0 )
            {
                MessageBox.Show("Select a dinner first");
                return;
            }
            UIClasses.DinnersNotGoingTo dinner = lvSignup.SelectedItems[0].Tag as UIClasses.DinnersNotGoingTo;

            //What type of validation can we do at the UI
            //Of course check if user already is signed up
            //How - of course the best we can do is query our report model
            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.Context.DataContext.CreateUnitOfWork(false))
            {
                Systementor.Database.Repositories.IRepository<UIClasses.Report_Dinner> rep = uow.CreateRepository<UIClasses.Report_Dinner>();
                int nCnt = rep.Count( r=>r.UsersComing.Any(us=>us.User_Id == user.User_Id ) );
                if (nCnt > 0)
                {
                    MessageBox.Show("You seems to have opted in already for that dinner");
                    return;
                }
            }
            Communications.CommandBus.UserOptInForDinner(user.User_Id, dinner.Dinner_Id);
            //System.Guid uid = Communications.CommandBus.UserOptInForDinner(user.User_Id, dinner.Dinner_Id);
            MessageBox.Show("If all went ok you will get an email sent to you in a few minutes time or so...");
            
            //Here are some ideas on what to do here
            //In this case we need to update the gui to somehow reflect what has happened
            //i.e the "lvSignup" row should move to the "I'm going to"
            //but there is NO guarantee that the report db has been updated 
            //we could for example 
            //a - poll and wait until the database has been updated
            //b - flag it as "booking is ongoing"
            //c - assume all goes well and move ourselves (i.e change the UI classes) 

            //all these could be altered/combined with some pub/sub where we get a message when its been updated
            //Here I go for the c) assume all goes well
            lvSignup.Items.Remove(lvSignup.SelectedItems[0]);



            using (Systementor.Database.Repositories.IUnitOfWork uow = DB.Context.DataContext.CreateUnitOfWork(false))
            {
                Systementor.Database.Repositories.IRepository<UIClasses.Report_Dinner> rep = uow.CreateRepository<UIClasses.Report_Dinner>();
                foreach (UIClasses.Report_Dinner d in rep.Find(p => p.UsersComing.Any(r => r.User_Id == user.User_Id)))
                {
                    ListViewItem i = lvGoingTo.Items.Add(d.Date.ToString());
                    i.SubItems.Add(d.Location);
                    i.SubItems.Add(d.Organizer_Fullname);
                    i.SubItems.Add(d.UsersComing.Count.ToString());
                }

            }
            
            
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem oItem in lvIAmHosting.SelectedItems)
            {
                UIClasses.Report_Dinner oDinner = oItem.Tag as UIClasses.Report_Dinner;
                FormDinner oForm = new FormDinner();
                oForm.txtDescription.Text = oDinner.Description;
                oForm.txtLocation.Text = oDinner.Location;
                oForm.dtDate.Value = oDinner.Date.Date;
                oForm.txtHour.Text = oDinner.Date.Hour.ToString();
                oForm.txtMinute.Text = oDinner.Date.Minute.ToString();
                if (oForm.ShowDialog(this) == DialogResult.OK)
                {
                    List<NServiceBus.IMessage> oList = new List<NServiceBus.IMessage>();
                    //What has changed?
                    DateTime dtNew = oForm.dtDate.Value.AddHours(Convert.ToInt32(oForm.txtHour.Text)).AddMinutes(Convert.ToInt32(oForm.txtMinute.Text)).AddSeconds(-oForm.dtDate.Value.Second);
                    if (dtNew != oDinner.Date)
                        oList.Add(new NerdCommandMessages.DinnerModifyTime(oDinner.Dinner_Id, dtNew, CommandInfrastructure.MessageLogInfo.CreateNew(Communications.CommandBus.CurrentUser)));
                    if ( oForm.txtLocation.Text != oDinner.Location )
                        oList.Add(new NerdCommandMessages.DinnerModifyLocation(oDinner.Dinner_Id, oForm.txtLocation.Text, CommandInfrastructure.MessageLogInfo.CreateNew(Communications.CommandBus.CurrentUser)));
                    if (oForm.txtDescription.Text != oDinner.Description)
                        oList.Add(new NerdCommandMessages.DinnerModifyDescription(oDinner.Dinner_Id, oForm.txtDescription.Text, CommandInfrastructure.MessageLogInfo.CreateNew(Communications.CommandBus.CurrentUser)));

                    Communications.CommandBus.Bus.Send(oList.ToArray());

                }
                



            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Create a new dinner...
            FormDinner oForm = new FormDinner();
            if (oForm.ShowDialog(this) == DialogResult.OK)
            {
                Guid id = Communications.CommandBus.DinnerCreate(oForm.dtDate.Value.AddHours(Convert.ToInt32(oForm.txtHour.Text)).AddMinutes(Convert.ToInt32(oForm.txtMinute.Text)).AddSeconds(-oForm.dtDate.Value.Second),
                    oForm.txtLocation.Text, oForm.txtDescription.Text, user.User_Id).ARId;
                MessageBox.Show(id.ToString());
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Create new user
            FormNewUser oForm = new FormNewUser();
            if (oForm.ShowDialog(this) == DialogResult.OK)
            {
                Guid id =  Communications.CommandBus.UserCreate(oForm.textBox1.Text, oForm.textBox2.Text).ARId;
                MessageBox.Show(id.ToString());
            }
        }

    }
}
