using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq;



namespace Lab_4
{
    public partial class Inventory1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
          
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

            try
            {
                db.SubmitChanges();
                BindGrid();
            }

            catch (Exception exception)
            {
                Debug.WriteLine($"Somethig wrong: {exception.Message}");
            }
       
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            var sums = from s in db.Inventory select s.Count * s.MPrice;

            var totals = sums.Sum();

            TextBox1.Text = totals.ToString();
        }
        public void BindGrid(string sortExpression = "", bool isSort = false)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                IQueryable<Inventory> query = db.Inventory;

                switch (sortExpression)
                {
                    case "IdI":
                        query = GetSortExpression(query, i => i.IdI, "IdI", isSort);
                        break;
                    case "IName":
                        query = GetSortExpression(query, i => i.IName, "IName", isSort);
                        break;
                    case "Price":
                        query = GetSortExpression(query, i => i.Price, "Price", isSort);
                        break;
                    case "Count":
                        query = GetSortExpression(query, i => i.Count, "Count", isSort);
                        break;
                    case "MPrice":
                        query = GetSortExpression(query, i => i.MPrice, "MPrice", isSort);
                        break;

                    default:
                        query = GetSortExpression(query, i => i.IdI, "IdI", isSort);
                        break;
                }

                var source = query
                    .Select(x => new
                    {
                        IdI = x.IdI,
                        IName = x.IName,
                        Price = x.Price,
                        Count = x.Count,
                        MPrice = x.MPrice
                    })
                    .ToList();

                InventoryTable.DataSource = source;
                InventoryTable.DataBind();
            }
        }

        private string GetSortDirection(string column, bool isSort)
        {
            string direction = "asc";

            if (!isSort)
                return direction;

            string previousColumnSorted = ViewState["SortColumn"]?.ToString() ?? "";

            if (previousColumnSorted == column)
                direction = ViewState["SortDirection"].ToString() == "asc"
                    ? "desc"
                    : "asc";
            else
                ViewState["SortColumn"] = column;

            ViewState["SortDirection"] = direction;

            return direction;
        }

        private IQueryable<Inventory> GetSortExpression<T>(IQueryable<Inventory> query, Expression<Func<Inventory, T>> sortFunc, string column, bool isSort)
        {
            return GetSortDirection(column, isSort) == "asc"
                ? query.OrderBy(sortFunc)
                : query.OrderByDescending(sortFunc);
        }

        protected void InventoryTable_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            Debug.WriteLine("InventoryTable_Sorting");
            BindGrid(e.SortExpression, true);
        }

    }

}

   