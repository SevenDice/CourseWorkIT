using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            string log="";
            string pass="";
            log = TextBox1.Text.Trim();
            pass = TextBox2.Text.Trim();
            bool f1 = false;
            bool f2 = false;
            string autopas = "";
            string temp = "";
            
            if(log!="" && pass!="")
            {
                AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

                f1 = db.Workers.Any(w => w.Login == log);

                try
                { 
                    autopas = (from p in db.Workers where log == p.Login select p.Password).SingleOrDefault(); 
                }
                catch {  }
                finally
                {
                    autopas=autopas.TrimEnd();
                    if (pass == autopas)
                        f2 = true;
                }

                if (f1 == true && f2 == true)
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