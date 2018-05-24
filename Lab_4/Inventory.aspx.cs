using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public partial class Inventory1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));
            var inventoryList =
            from i in db.Inventory
            orderby i.IName
            select new
            {
                IdI = i.IdI,
                IName = i.IName,
                Price = i.Price,
                Count = i.Count,
                MPrice = i.MPrice
            };

            var iList = inventoryList.ToList();

            ITable.DataSource = iList;
            ITable.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            Inventory a = new Inventory()
            {
                IName = TextBox5.Text,
                Count = Convert.ToInt32(TextBox2.Text),
                Price = Convert.ToDouble(TextBox3.Text),
                MPrice = Convert.ToDouble(TextBox4.Text)
            };

            db.Inventory.InsertOnSubmit(a);
            db.SubmitChanges();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            var sums =
                from s in db.Inventory select s.Count * s.MPrice;

            var totals = sums.Sum();

            TextBox1.Text = totals.ToString();
        }
    }
}