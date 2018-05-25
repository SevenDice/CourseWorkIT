using System;
using System.Linq;

namespace Lab_4
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Auto.GetAuth().GetKey() == false)
                Nonauto();
            Label2.Visible = false;
        }

        protected void Nonauto()
        {
            Button1.Visible = false;
            Button2.Visible = false;
            Button3.Visible = false;
            Button4.Visible = false;
            Button5.Visible = false;
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            var log = TextBox1.Text.Trim();
            var pwd = TextBox2.Text.Trim();

            if (log != "" && pwd != "")
            {
                bool isAuth;
                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    isAuth = db.Workers.Any(w => w.Login == log && w.Password == pwd);
                }

                if (isAuth)
                {
                    Button1.Visible = true;
                    Button2.Visible = true;
                    Button3.Visible = true;
                    Button4.Visible = true;
                    Button5.Visible = true;
                    Label2.Visible = false;
                    Auto.GetAuth().SetKey(true);
                }
                else
                {
                    Label2.Visible = true;
                    Nonauto();
                    Auto.GetAuth().SetKey(false);
                }
            }
        }
    }
}