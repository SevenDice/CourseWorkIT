using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Web.UI;
using Lab_4.Infrastructure;

namespace Lab_4
{
    public partial class Workers1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                BindInventory();
                BindDepartment();
            }
        }

        private void BindDepartment()
        {
                var source = GetDepartmentData();

                DepartmentDropDown.DataSource = source;
                DepartmentDropDown.DataTextField = "Text";
                DepartmentDropDown.DataValueField = "Id";
                DepartmentDropDown.DataBind();
        }

        protected List<DropDownData> GetDepartmentData()
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var source = db.Places.Select(i => new DropDownData
                    {
                        Id = i.IdP,
                        Text = i.Pname
                    })
                    .ToList();

                return source;
            }
        }

        private void BindInventory()
        {
            var source = GetInventoryData();

            InventaryDropDown.DataSource = source;
            InventaryDropDown.DataTextField = "Text";
            InventaryDropDown.DataValueField = "Id";
            InventaryDropDown.DataBind();
        }

        protected List<DropDownData> GetInventoryData()
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var source = db.Inventory.Select(i => new DropDownData
                    {
                        Id = i.IdI,
                        Text = i.IName
                    })
                    .ToList();
                return source;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var wage = double.TryParse(SalaryTbx.Text, out double _wage)
                    ? _wage
                    : (double?) null;

                var inv = int.TryParse(InventaryDropDown.SelectedValue, out int _inv)
                    ? _inv
                    : (int?) null;

                var place = int.TryParse(DepartmentDropDown.SelectedValue, out int _place)
                    ? _place
                    : (int?)null;

                var a = new Workers
                {
                    FName = LastNameTbx.Text,
                    SName = FirstNameTbx.Text,
                    TName = SurnameTbx.Text,
                    Wage = wage,
                    Inv = inv,
                    Place = place,
                    Login = LoginTbx.Text,
                    Password = PwdTbx.Text
                };

                db.Workers.InsertOnSubmit(a);

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
                var totals = db.Workers.Sum(w => w.Wage);
                TotalSalaryTbx.Text = totals.ToString();
            }
        }

        public void BindGrid(string sortExpression = "", bool isSort = false)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                IQueryable<WorkerAndPlace> query = db.Workers
                    .Join(db.Places, w => w.Place, p => p.IdP, (w, p) => new WorkerAndPlace { Worker = w, Place = p })
                    .Join(db.Inventory, wp => wp.Worker.Inv, i => i.IdI, (wp, i) => new WorkerAndPlace{Worker = wp.Worker, Place = wp.Place, Inventory = i});

                switch (sortExpression)
                {
                    case "FName":
                        query = GetSortExpression(query, wp => wp.Worker.FName, "FName", isSort);
                        break;
                    case "SName":
                        query = GetSortExpression(query, wp => wp.Worker.SName, "SName", isSort);
                        break;
                    case "TName":
                        query = GetSortExpression(query, wp => wp.Worker.TName, "TName", isSort);
                        break;
                    case "Wage":
                        query = GetSortExpression(query, wp => wp.Worker.Wage, "Wage", isSort);
                        break;
                    case "Inv":
                        query = GetSortExpression(query, wp => wp.Inventory.IName, "Inv", isSort);
                        break;
                    case "Place":
                        query = GetSortExpression(query, wp => wp.Place.Pname, "Place", isSort);
                        break;
                    case "Login":
                        query = GetSortExpression(query, wp => wp.Worker.Login, "Login", isSort);
                        break;
                    case "Password":
                        query = GetSortExpression(query, wp => wp.Worker.Password, "Password", isSort);
                        break;

                    default:
                        query = GetSortExpression(query, wp => wp.Worker.IdW, "IdW", isSort);
                        break;
                }

                var source = query
                    .Select(x => new
                    {
                        Idw = x.Worker.IdW,
                        FName = x.Worker.FName,
                        SName = x.Worker.SName,
                        TName = x.Worker.TName,
                        Wage = x.Worker.Wage,
                        Inv = x.Inventory.IName,
                        InvId = x.Inventory.IdI,
                        Place = x.Place.Pname,
                        PlaceId = x.Place.IdP,
                        Login = x.Worker.Login,
                        Password = x.Worker.Password
                    })
                    .ToList();

                WorkersTable.DataSource = source;
                WorkersTable.DataBind();
            }
        }

        protected void WorkersTable_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Debug.WriteLine("WorkersTable_RowDeleting");

            if (e.Keys.Contains("IdW"))
            {
                var selectedWorkerId = (int)e.Keys["IdW"];
                Debug.WriteLine($"selectedWorkerId = {selectedWorkerId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var deletingEntity = db.Workers.FirstOrDefault(w => w.IdW == selectedWorkerId);

                    if (deletingEntity != null)
                    {
                        db.Workers.DeleteOnSubmit(deletingEntity);
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

        protected void WorkersTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Debug.WriteLine("WorkersTable_RowUpdating");

            var rowIdx = e.RowIndex;

            if (e.Keys.Contains("IdW"))
            {
                var updatingWorkerId = (int)e.Keys["IdW"];
                Debug.WriteLine($"updatingWorkerId = {updatingWorkerId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var updatingEntity = db.Workers.FirstOrDefault(w => w.IdW == updatingWorkerId);

                    if (updatingEntity != null)
                    {
                        var selectedRow = WorkersTable.Rows[rowIdx];

                        var fNameCtl = selectedRow.Cells[2].Controls[0] as TextBox;
                        var sNameCtl = selectedRow.Cells[3].Controls[0] as TextBox;
                        var tNameCtl = selectedRow.Cells[4].Controls[0] as TextBox;
                        var wageCtl = selectedRow.Cells[5].Controls[0] as TextBox;
                        var invCtl = selectedRow.Cells[6].FindControl("InvEditor") as DropDownList;
                        var placeCtl = selectedRow.Cells[7].FindControl("PlaceEditor") as DropDownList;
                        var loginCtl = selectedRow.Cells[8].Controls[0] as TextBox;
                        var pwdCtl = selectedRow.Cells[9].Controls[0] as TextBox;

                        updatingEntity.FName = fNameCtl?.Text;
                        updatingEntity.TName = tNameCtl?.Text;
                        updatingEntity.SName = sNameCtl?.Text;
                        updatingEntity.Wage = int.TryParse(wageCtl?.Text, out int _wage) ? _wage : (int?) null;
                        updatingEntity.Inv = int.TryParse(invCtl?.SelectedValue, out int _inv) 
                            ? _inv 
                            : (int?)null;

                        updatingEntity.Place = int.TryParse(placeCtl?.SelectedValue, out int _place)
                            ? _place 
                            : (int?) null;

                        updatingEntity.Login = loginCtl?.Text;
                        updatingEntity.Password = pwdCtl?.Text;
                        
                        try
                        {
                            db.SubmitChanges();
                            WorkersTable.EditIndex = -1;
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

        protected void WorkersTable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Debug.WriteLine("WorkersTable_RowEditing");
            WorkersTable.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void WorkersTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Debug.WriteLine("WorkersTable_RowCancelingEdit");
            WorkersTable.EditIndex = -1;
            BindGrid();
        }

        protected void WorkersTable_Sorting(object sender, GridViewSortEventArgs e)
        {
            Debug.WriteLine("WorkersTable_Sorting");
            BindGrid(e.SortExpression, true);
        }

        private string GetSortDirection(string column, bool isSort)
        {
            string direction = "asc";

            if (!isSort)
                return direction;

            string previousColumnSorted = ViewState["SortColumn"] != null
                ? ViewState["SortColumn"].ToString()
                : "";

            if (previousColumnSorted == column)
                direction = ViewState["SortDirection"].ToString() == "asc"
                    ? "desc"
                    : "asc";
            else
                ViewState["SortColumn"] = column;

            ViewState["SortDirection"] = direction;

            return direction;
        }

        private IQueryable<WorkerAndPlace> GetSortExpression<T>(IQueryable<WorkerAndPlace> query, Expression<Func<WorkerAndPlace, T>> sortFunc, string column, bool isSort)
        {
            return GetSortDirection(column, isSort) == "asc"
                ? query.OrderBy(sortFunc)
                : query.OrderByDescending(sortFunc);
        }
    }
}
