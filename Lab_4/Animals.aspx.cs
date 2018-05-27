using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Lab_4
{
    public partial class Animals1 : System.Web.UI.Page
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
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                Animals a = new Animals
                {
                    AName = TextBox1.Text,
                    MarketPrice = Convert.ToDouble(TextBox2.Text),
                    Count = Convert.ToInt32(TextBox3.Text)
                };

                db.Animals.InsertOnSubmit(a);

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
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var totals = db.Animals.Sum(a => a.MarketPrice * a.Count);
                TextBox4.Text = totals.ToString();
            }
        }

        public void BindGrid(string sortExpression = "", bool isSort = false)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                IQueryable<Animals> query = db.Animals;

                switch (sortExpression)
                {
                    case "IdA":
                        query = GetSortExpression(query, a => a.IdA, "IdA", isSort);
                        break;
                    case "AName":
                        query = GetSortExpression(query, a => a.AName, "AName", isSort);
                        break;
                    case "MarketPrice":
                        query = GetSortExpression(query, a => a.MarketPrice, "MarketPrice", isSort);
                        break;
                    case "Count":
                        query = GetSortExpression(query, a => a.Count, "Count", isSort);
                        break;

                    default:
                        query = GetSortExpression(query, a => a.IdA, "IdA", isSort);
                        break;
                }

                var source = query
                    .Select(x => new
                    {
                        IdA = x.IdA,
                        AName = x.AName,
                        MarketPrice = x.MarketPrice,
                        Count = x.Count
                    })
                    .ToList();

                AnimalsTable.DataSource = source;
                AnimalsTable.DataBind();
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

        private IQueryable<Animals> GetSortExpression<T>(IQueryable<Animals> query, Expression<Func<Animals, T>> sortFunc, string column, bool isSort)
        {
            return GetSortDirection(column, isSort) == "asc"
                ? query.OrderBy(sortFunc)
                : query.OrderByDescending(sortFunc);
        }

        protected void AnimalsTable_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            Debug.WriteLine("AnimalsTable_Sorting");
            BindGrid(e.SortExpression, true);
        }
    }
}
