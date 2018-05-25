using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public partial class Places1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));
            var placesList =
            from p in db.Places
            join c in db.Cultures on p.PCulture equals c.IdC
            join a in db.Animals on p.PAnimals equals a.IdA
            orderby p.Pname
            select new
            {
                IdP = p.IdP,
                Pname = p.Pname,
                MPrice = p.MPrice,
                PCulture = c.Cname,
                PAnimals = a.AName
            };

            var pList = placesList.ToList();

            PTable.DataSource = pList;
            PTable.DataBind();
        }

    protected void Button1_Click(object sender, EventArgs e)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var mprice = double.TryParse(TextBox2.Text, out double _mprice)
                    ? _mprice
                    : (double?) null;

                var pculture = db.Cultures.FirstOrDefault(c => c.Cname == TextBox3.Text.Trim())?.IdC;

                var panimals = db.Animals.FirstOrDefault(a => a.AName == TextBox4.Text.Trim())?.IdA;

                var place = new Places
                {
                    Pname = TextBox1.Text.Trim(),
                    MPrice = mprice,
                    PCulture = pculture,
                    PAnimals = panimals
                };

                db.Places.InsertOnSubmit(place);
                db.SubmitChanges();
            }
        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var totals = db.Places.Sum(p => p.MPrice ?? 0);

                TextBox5.Text = totals.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
