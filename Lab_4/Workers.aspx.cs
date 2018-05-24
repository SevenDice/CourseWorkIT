using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq;

namespace Lab_4
{
    public partial class Workers1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           BindGrid();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            var wage = double.TryParse(TextBox2.Text, out double _wage)
                ? _wage 
                : (double?)null;

            var inv = int.TryParse(TextBox7.Text, out int _inv)
                ? _inv
                : (int?)null;

            var place = db.Places.SingleOrDefault(p => p.Pname == TextBox8.Text)?.IdP;

            var a = new Workers()
            {
                FName = TextBox6.Text,
                SName = TextBox5.Text,
                TName = TextBox1.Text,
                Wage = wage,
                Inv = inv,
                Place = place,
                Login = TextBox3.Text,
                Password = TextBox4.Text
            };

            db.Workers.InsertOnSubmit(a);
            db.SubmitChanges();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));
            
            var totals = (from w in db.Workers select w.Wage).Sum();

            TextBox9.Text = totals.ToString();

        }

        public void BindGrid()
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));
            var workersList =
            from w in db.Workers
            join p in db.Places on w.Place equals p.IdP
            join i in db.Inventory on w.Inv equals i.IdI
            orderby w.FName
            select new
            {
                Idw = w.IdW,
                FName = w.FName,
                SName = w.SName,
                TName = w.TName,
                Wage = w.Wage,
                Inv = w.Inv,
                Place = p.Pname,
                Login = w.Login,
                Password = w.Password
            };

            var list = workersList.ToList();

            WorkersTable.DataSource = list;
            WorkersTable.DataBind();
        }

        protected void WorkersTable_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            var affectedRows = e.AffectedRows;

            var deletedValue = e.Values;
        }

        protected void WorkersTable_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            var affectedRows = e.AffectedRows;

            var newValues = e.NewValues;

            var oldValues = e.OldValues;


        }

        protected void WorkersTable_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void WorkersTable_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}
