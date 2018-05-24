using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public class WorkerItem
    {       
        public string surn = "";
        public string nam = "";
        public string tnam = "";
        public string gender = "";
        public DateTime date;
        public WorkerItem()
        {

        }
        public WorkerItem(string sn, string n, string tn, string gen, DateTime d)
        {
            surn = sn; nam = n; tnam = tn; gender = gen; date = d;
        }
    }

    public partial class Worker : System.Web.UI.Page
    {
        string G;
        DateTime Da;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            WorkerItem newWorker = new WorkerItem(Surname.Text, Name.Text, TName.Text, G, Da);
            ListWorkers.GetRepository().AddWorker(newWorker);
        }

        protected void Gender_SelectedIndexChanged(object sender, EventArgs e)
        {
            G = Gen.SelectedItem.Text;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            Da = Calendar1.SelectedDate;
        }
    }
}