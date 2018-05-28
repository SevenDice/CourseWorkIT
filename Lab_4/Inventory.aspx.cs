using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq;
using System.Web.UI.WebControls;


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

        protected void InventoryTable_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            Debug.WriteLine("InventoryTable_RowCancelingEdit");
            InventoryTable.EditIndex = -1;
            BindGrid();
        }

        protected void InventoryTable_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            Debug.WriteLine("InventoryTable_RowDeleting");

            if (e.Keys.Contains("IdI"))
            {
                var selectedInventoryId = (int)e.Keys["IdI"];
                Debug.WriteLine($"selectedInventoryId = {selectedInventoryId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var deletingEntity = db.Inventory.FirstOrDefault(i => i.IdI == selectedInventoryId);

                    if (deletingEntity != null)
                    {
                        db.Inventory.DeleteOnSubmit(deletingEntity);
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

        protected void InventoryTable_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            Debug.WriteLine("InventoryTable_RowUpdating");

            var rowIdx = e.RowIndex;

            if (e.Keys.Contains("IdI"))
            {
                var updatingInventoryId = (int)e.Keys["IdI"];
                Debug.WriteLine($"updatingInventoryId = {updatingInventoryId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var updatingEntity = db.Inventory.FirstOrDefault(i => i.IdI == updatingInventoryId);

                    if (updatingEntity != null)
                    {
                        var selectedRow = InventoryTable.Rows[rowIdx];

                        var iNameCtl = selectedRow.Cells[2].Controls[0] as TextBox;
                        var countCtl = selectedRow.Cells[3].Controls[0] as TextBox;
                        var priceCtl = selectedRow.Cells[4].Controls[0] as TextBox;
                        var mPriceCtl = selectedRow.Cells[5].Controls[0] as TextBox;

                        updatingEntity.IName = iNameCtl?.Text;
                        updatingEntity.Count = int.TryParse(countCtl?.Text, out int _count) ? _count : (int?)null;
                        updatingEntity.Price = double.TryParse(priceCtl?.Text, out double _price)
                            ? _price
                            : (double?)null;
                        updatingEntity.MPrice = double.TryParse(mPriceCtl?.Text, out double _mPrice)
                            ? _mPrice
                            : (double?)null;

                        try
                        {
                            db.SubmitChanges();
                            InventoryTable.EditIndex = -1;
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

        protected void InventoryTable_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            Debug.WriteLine("InventoryTable_RowEditing");
            InventoryTable.EditIndex = e.NewEditIndex;
            BindGrid();
        }
    }
}