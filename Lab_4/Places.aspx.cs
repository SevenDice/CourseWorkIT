using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.UI;
using Lab_4.Infrastructure;

namespace Lab_4
{
    public partial class Places1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                BindInventory();
                BindCulture();
            }
            //AGRODataContext db = new AGRODataContext(Server.MapPath("\\"));
            //var placesList =
            //from p in db.Places
            //join c in db.Cultures on p.PCulture equals c.IdC
            //join a in db.Animals on p.PAnimals equals a.IdA
            //orderby p.Pname
            //select new
            //{
            //    IdP = p.IdP,
            //    Pname = p.Pname,
            //    MPrice = p.MPrice,
            //    PCulture = c.Cname,
            //    PAnimals = a.AName
            //};

            //var pList = placesList.ToList();

            //PTable.DataSource = pList;
            //PTable.DataBind();
        }

        private void BindCulture()
        {
            var source = GetCultureData();
            CultureDropDown.DataSource = source;
            CultureDropDown.DataTextField = "Text";
            CultureDropDown.DataValueField = "Id";
            CultureDropDown.DataBind();
        }

        protected List<DropDownData> GetCultureData()
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var source = db.Cultures.Select(i => new DropDownData
                    {
                        Id = i.IdC,
                        Text = i.Cname
                    })
                    .ToList();
                return source;
            }
        }

        private void BindInventory()
        {
            var source = GetInventoryData();

            InventoryDropDown.DataSource = source;
            InventoryDropDown.DataTextField = "Text";
            InventoryDropDown.DataValueField = "Id";
            InventoryDropDown.DataBind();
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

        protected void Button2_Click1(object sender, EventArgs e)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var totals = db.Places.Sum(p => p.MPrice ?? 0);

                TextBox5.Text = totals.ToString(CultureInfo.InvariantCulture);
            }
        }

        public void BindGrid(string sortExpression = "", bool isSort = false)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                IQueryable<PlaceAndCulture> query = db.Places
                    .Join(db.Cultures, c => c.Cultures, p => p.IdC, (p, c) => new PlaceAndCulture { Place = p, Culture = c })
                    .Join(db.Inventory, pc => pc.Place.Inv, i => i.IdI, (pc, i) => new PlaceAndCulture { Place = pc.Place, Culture = pc.Place, Inventory = i });
                //    IdP = p.IdP,
                //    Pname = p.Pname,
                //    MPrice = p.MPrice,
                //    PCulture = c.Cname,
                //    PAnimals = a.AName
                switch (sortExpression)
                {
                    case "Pname":
                        query = GetSortExpression(query, pc => pc.Place.Pname, "Pname", isSort);
                        break;
                    case "MPrice":
                        query = GetSortExpression(query, pc => pc.Place.MPrice, "MPrice", isSort);
                        break;
                    case "PCulture":
                        query = GetSortExpression(query, pc => pc.Culture.Cname, "PCulture", isSort);
                        break;
                    case "PAnimals":
                        query = GetSortExpression(query, pc => pc.Animals.AName, "PAnimals", isSort);
                        break;

                    default:
                        query = GetSortExpression(query, pc => pc.Place.IdP, "IdP", isSort);
                        break;
                }

                var source = query
                    .Select(x => new
                    {
                        Idp = x.Place.IdP,
                        Pname = x.Place.Pname,
                        MPrice = x.Place.MPrice,
                        PCulture = x.Culture.Cname,
                        PAnimals = x.Animals.AName
                    })
                    .ToList();

                PlacesTable.DataSource = source;
                PlacesTable.DataBind();
            }
        }
        protected void PlacesTable_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Debug.WriteLine("PlacesTable_RowDeleting");

            if (e.Keys.Contains("IdP"))
            {
                var selectedPlaceId = (int)e.Keys["IdP"];
                Debug.WriteLine($"selectedPlaceId = {selectedPlaceId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var deletingEntity = db.Places.FirstOrDefault(p=> p.IdP == selectedPlaceId);

                    if (deletingEntity != null)
                    {
                        db.Places.DeleteOnSubmit(deletingEntity);
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

        protected void PlacesTable_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Debug.WriteLine("PlacesTable_RowUpdating");

            var rowIdx = e.RowIndex;

            if (e.Keys.Contains("IdP"))
            {
                var updatingPlaceId = (int)e.Keys["IdP"];
                Debug.WriteLine($"updatingPlaceId = {updatingPlaceId}");

                using (var db = new AGRODataContext(Server.MapPath("\\")))
                {
                    var updatingEntity = db.Places.FirstOrDefault(p => p.IdP == updatingPlaceId);

                    if (updatingEntity != null)
                    {
                        var selectedRow = PlacesTable.Rows[rowIdx];

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
                        updatingEntity.Wage = int.TryParse(wageCtl?.Text, out int _wage) ? _wage : (int?)null;
                        updatingEntity.Inv = int.TryParse(invCtl?.SelectedValue, out int _inv)
                            ? _inv
                            : (int?)null;

                        updatingEntity.Place = int.TryParse(placeCtl?.SelectedValue, out int _place)
                            ? _place
                            : (int?)null;

                        updatingEntity.Login = loginCtl?.Text;
                        updatingEntity.Password = pwdCtl?.Text;

                        try
                        {
                            db.SubmitChanges();
                            PlacesTable.EditIndex = -1;
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

        protected void PlacesTable_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Debug.WriteLine("PlacesTable_RowEditing");
            PlacesTable.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void PlacesTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Debug.WriteLine("PlacesTable_RowCancelingEdit");
            PlacesTable.EditIndex = -1;
            BindGrid();
        }

        protected void PlacesTable_Sorting(object sender, GridViewSortEventArgs e)
        {
            Debug.WriteLine("PlacesTable_Sorting");
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

        private IQueryable<PlaceAndCulture> GetSortExpression<T>(IQueryable<PlaceAndCulture> query, Expression<Func<PlaceAndCulture, T>> sortFunc, string column, bool isSort)
        {
            return GetSortDirection(column, isSort) == "asc"
                ? query.OrderBy(sortFunc)
                : query.OrderByDescending(sortFunc);
        }
    }
}
