using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public partial class Animals1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            Animals a = new Animals()
            {
                AName = TextBox1.Text,
                MarketPrice = Convert.ToDouble(TextBox2.Text),
                Count = Convert.ToInt32(TextBox3.Text)
            };

            db.Animals.InsertOnSubmit(a);
            db.SubmitChanges();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            var sum = from w in db.Animals select w.MarketPrice * w.Count;

            var totals = sum.Sum();

            TextBox4.Text = totals.ToString();
        }

        public void BindGrid()
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));
            var aList =
            from a in db.Animals
            orderby a.AName
            select new
            {
                IdA = a.IdA,
                AName = a.AName,
                MarketPrice = a.MarketPrice,
                Count = a.Count
            };
            AnimalsTable.DataSource = aList;
            AnimalsTable.DataBind();

         
        }
    }
}