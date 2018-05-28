using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;

namespace Lab_4
{
    public partial class Cultures1 : System.Web.UI.Page
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

            Cultures a = new Cultures()
            {
                Cname = TextBox1.Text,
                MarketPrice = Convert.ToDouble(TextBox3.Text),
                Count = Convert.ToInt32(TextBox2.Text)
            };

            db.Cultures.InsertOnSubmit(a);

            try
            {
                db.SubmitChanges();
                BindGrid();
            }

            catch (Exception exception)
            {
                Debug.WriteLine($"Something wrong: {exception.Message}");
            }
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));

            var sum = from w in db.Cultures select w.MarketPrice * w.Count;

            var totals = sum.Sum();

            TextBox4.Text = totals.ToString();
        }

        public void BindGrid(string sortExpression = "", bool isSort = false)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                IQueryable<Cultures> query = db.Cultures;

                switch (sortExpression)
                {
                    case "IdC":
                        query = GetSortExpression(query, c => c.IdC, "IdC", isSort);
                        break;
                    case "Cname":
                        query = GetSortExpression(query, c => c.Cname, "Cname", isSort);
                        break;
                    case "MarketPrice":
                        query = GetSortExpression(query, c => c.MarketPrice, "MarketPrice", isSort);
                        break;
                    case "Count":
                        query = GetSortExpression(query, c => c.Count, "Count", isSort);
                        break;
                }

                var source = query
                    .Select(x => new
                    {
                        Idc = x.IdC,
                        Cname = x.Cname,
                        MarketPrice = x.MarketPrice,
                        Count = x.Count
                    })
                    .ToList(); 
                CulturesTable.DataSource = source;
                CulturesTable.DataBind();
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

        private IQueryable<Cultures> GetSortExpression<T>(IQueryable<Cultures> query, Expression<Func<Cultures, T>> sortFunc, string column, bool isSort)
        {
            return GetSortDirection(column, isSort) == "asc"
                ? query.OrderBy(sortFunc)
                : query.OrderByDescending(sortFunc);
        }

        protected void CulturesTable_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            Debug.WriteLine("CulturesTable_Sorting");
            BindGrid(e.SortExpression, true);
        }

        protected void CulturesTable_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            Debug.WriteLine("CulturesTable_RowEditing");
            CulturesTable.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void CulturesTable_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            Debug.WriteLine("CulturesTable_RowUpdating");

            var rowIdx = e.RowIndex;

            if (e.Keys.Contains("IdC"))
            {
                var updatingCultureId = (int)e.Keys["IdC"];
                Debug.WriteLine($"updatingCulturesId = {updatingCultureId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var updatingEntity = db.Cultures.FirstOrDefault(p => p.IdC == updatingCultureId);

                    if (updatingEntity != null)
                    {
                        var selectedRow = CulturesTable.Rows[rowIdx];

                        var cNameCtl = selectedRow.Cells[2].Controls[0] as TextBox;
                        var marketPriceCtl = selectedRow.Cells[3].Controls[0] as TextBox;
                        var countCtl = selectedRow.Cells[4].Controls[0] as TextBox;
                
                        updatingEntity.Cname = cNameCtl?.Text;
                        updatingEntity.MarketPrice = double.TryParse(marketPriceCtl?.Text, out double _marketPrice) ? _marketPrice : (double?)null;
                        updatingEntity.Count = int.TryParse(countCtl?.Text, out int _count)
                            ? _count
                            : (int?)null;

                        try
                        {
                            db.SubmitChanges();
                            CulturesTable.EditIndex = -1;
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

        protected void CulturesTable_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            Debug.WriteLine("CulturesTable_RowDeleting");

            if (e.Keys.Contains("IdС"))
            {
                var selectedCultureId = (int)e.Keys["IdС"];
                Debug.WriteLine($"selectedCultureId = {selectedCultureId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var deletingEntity = db.Cultures.FirstOrDefault(p => p.IdC == selectedCultureId);

                    if (deletingEntity != null)
                    {
                        db.Cultures.DeleteOnSubmit(deletingEntity);
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

        protected void CulturesTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Debug.WriteLine("CulturesTable_RowCancelingEdit");
            CulturesTable.EditIndex = -1;
            BindGrid();
        }
    }

}