using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;

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

        protected void AnimalsTable_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            Debug.WriteLine("AnimalsTable_RowEditing");
            AnimalsTable.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void AnimalsTable_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            Debug.WriteLine("AnimalsTable_RowUpdating");

            var rowIdx = e.RowIndex;

            if (e.Keys.Contains("IdA"))
            {
                var updatingWorkerId = (int)e.Keys["IdA"];
                Debug.WriteLine($"updatingWorkerId = {updatingWorkerId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var updatingEntity = db.Animals.FirstOrDefault(a => a.IdA == updatingWorkerId);

                    if (updatingEntity != null)
                    {
                        var selectedRow = AnimalsTable.Rows[rowIdx];

                        var aNameCtl = selectedRow.Cells[2].Controls[0] as TextBox;
                        var countCtl = selectedRow.Cells[3].Controls[0] as TextBox;
                        var marketPriceCtl = selectedRow.Cells[4].Controls[0] as TextBox;

                        updatingEntity.AName = aNameCtl?.Text;
                        updatingEntity.Count = int.TryParse(countCtl?.Text, out int _count) ? _count : (int?)null;
                        updatingEntity.MarketPrice = double.TryParse(marketPriceCtl?.Text, out double _marketPlace)
                            ? _marketPlace
                            : (double?)null;

                        try
                        {
                            db.SubmitChanges();
                            AnimalsTable.EditIndex = -1;
                            BindGrid();
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine($"Somethig wrong: {exception.Message}");
                        }
                    }
                }
            }
        }

        protected void AnimalsTable_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            Debug.WriteLine("AnimalsTable_RowDeleting");

            if (e.Keys.Contains("IdA"))
            {
                var selectedWorkerId = (int)e.Keys["IdA"];
                Debug.WriteLine($"selectedWorkerId = {selectedWorkerId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var deletingEntity = db.Animals.FirstOrDefault(a => a.IdA == selectedWorkerId);

                    if (deletingEntity != null)
                    {
                        db.Animals.DeleteOnSubmit(deletingEntity);
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
            }
        }

        protected void AnimalsTable_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            Debug.WriteLine("AnimalsTable_RowCancelingEdit");
            AnimalsTable.EditIndex = -1;
            BindGrid();
        }
    }
}
