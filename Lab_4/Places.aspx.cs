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
                BindCulture();
                BindAnimals();
                
            }
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

        private void BindAnimals()
        {
            var source = GetAnimalsData();

            AnimalsDropDown.DataSource = source;
            AnimalsDropDown.DataTextField = "Text";
            AnimalsDropDown.DataValueField = "Id";
            AnimalsDropDown.DataBind();
        }

        protected List<DropDownData> GetAnimalsData()
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var source = db.Animals.Select(i => new DropDownData
                    {
                        Id = i.IdA,
                        Text = i.AName
                    })
                    .ToList();
                return source;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (var db = new AGRODataContext(Server.MapPath("\\")))
            {
                var mPrice = double.TryParse(TextBox2.Text, out double _mPrice)
                    ? _mPrice
                    : (double?) null;

                var pCulture = int.TryParse(CultureDropDown.SelectedValue, out int _pCulture)
                    ? _pCulture
                    : (int?)null;

                var pAnimals = int.TryParse(AnimalsDropDown.SelectedValue, out int _pAnimals)
                    ? _pAnimals
                    : (int?)null;

                var place = new Places
                {
                    Pname = TextBox1.Text.Trim(),
                    MPrice = mPrice,
                    PCulture = pCulture,
                    PAnimals = pAnimals
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
                    .Join(db.Cultures, p => p.PCulture, c => c.IdC, (p, c) => new PlaceAndCulture { Place = p, Culture = c })
                    .Join(db.Animals, pc => pc.Place.PAnimals, a => a.IdA, (pc, a) => new PlaceAndCulture { Place = pc.Place, Culture = pc.Culture, Animals = a});

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
                        PCultuteId = x.Culture.IdC,
                        PAnimals = x.Animals.AName,
                        PAnimalsId = x.Animals.IdA
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

        protected void PlacesTable_RowUpdating(object sender, GridViewUpdateEventArgs e) //NEED FIX
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

                        var pNameCtl = selectedRow.Cells[2].Controls[0] as TextBox;
                        var mPriceCtl = selectedRow.Cells[3].Controls[0] as TextBox;
                        var cultureCtl = selectedRow.Cells[4].FindControl("CultureEditor") as DropDownList;
                        var animalsCtl = selectedRow.Cells[5].FindControl("AnimalsEditor") as DropDownList;

                        updatingEntity.Pname = pNameCtl?.Text;
                        updatingEntity.MPrice = int.TryParse(mPriceCtl?.Text, out int _mPrice) ? _mPrice : (int?) null;
                        updatingEntity.PCulture = int.TryParse(cultureCtl?.SelectedValue, out int _pCulture)
                            ? _pCulture 
                            : (int?)null;
                        updatingEntity.PAnimals = int.TryParse(animalsCtl?.SelectedValue, out int _pAnimals)
                            ? _pAnimals
                            : (int?)null;

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
