using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public partial class Cultures1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));
            var culturesList =
            from c in db.Cultures
            orderby c.Cname
            select new
            {
                Idc = c.IdC,
                Cname = c.Cname,
                MarketPrice = c.MarketPrice,
                Count = c.Count
            };

            var cList = culturesList.ToList();

            CTable.DataSource = cList;
            CTable.DataBind();
        }
        //<Columns>
        //<asp:BoundField DataField = "IdC" HeaderText="IdC" InsertVisible="False" ReadOnly="True" SortExpression="IdC" Visible="False" />
        //<asp:BoundField DataField = "Cname" HeaderText="Наименование культуры" SortExpression="Cname" />
        //<asp:BoundField DataField = "MarketPrice" HeaderText="Цена за тонну" SortExpression="MarketPrice" />
        //<asp:BoundField DataField = "Count" HeaderText="Количество" SortExpression="Count" />
        //</Columns>
        protected void Button1_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            Cultures a = new Cultures()
            {
                Cname = TextBox1.Text,
                MarketPrice = Convert.ToDouble(TextBox3.Text),
                Count = Convert.ToInt32(TextBox2.Text)
            };

            db.Cultures.InsertOnSubmit(a);
            db.SubmitChanges();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            var sum = from w in db.Cultures select w.MarketPrice * w.Count;

            var totals = sum.Sum();

            TextBox4.Text = totals.ToString();
        }
    }
}