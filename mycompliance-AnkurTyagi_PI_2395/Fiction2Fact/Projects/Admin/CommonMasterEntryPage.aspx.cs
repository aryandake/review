using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Fiction2Fact;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using AjaxControlToolkit;

namespace Fiction2Fact.Projects.Admin
{
    public partial class Admin_CommonMasterEntryPage : System.Web.UI.Page
    {
        const string SEARCHTEXTPANEL = "TEXT";
        const string SEARCHTEXTRANGEPANEL = "TEXTRANGE";
        const string SEARCHDATEPANEL = "DATE";
        const string SEARCHDATERANGEPANEL = "DATERANGE";
        const string SEARCHDROPDOWNPANEL = "DROPDOWN";
        const string DATATYPE_STRING = "STRING";
        const string DATATYPE_NUMERIC = "NUMERIC";
        const string DATATYPE_DATE = "DATE";

        private const int Id_ColPosition = 3;
        int intMasterType;

        CommonMethods cm = new CommonMethods
            ();

        private string mstrregEmailId = ConfigurationManager.AppSettings["regEmailId"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            ImageButton1.ImageUrl = ImageButton3.ImageUrl = Global.site_url("Content/images/legacy/calendar.jpg");
            if (!Page.IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                    string username = Authentication.GetUserID(Authentication.GetUserID(Page.User.Identity.Name));
                    //Session["user"] = HRBusinessMethods.LoggedInUserInformation(username);
                }

                //Denil 29-Jun-2008.  
                ShowSave(false);
                SearchPanel.Visible = false;
                btnAdd.Visible = false;
                masterListTable.Enabled = true;
                btnExportToExcel.Visible = false;
            }
            else
            {
                hideError();
            }
            lblSelectedMasterName.Text = hfSelectedMasterName.Value;
        }

        private void LoadMasters(int intID)
        {
            DataTable dt = new DataServer().Getdata("SELECT [Name], [MasterEntryDetailsId] FROM [MasterEntryDetails] where MasterType=" + intID + " and Status = 'A' order by MasterSortOrder, Name");
            foreach (DataRow row in dt.Rows)
            {
                TableRow tblRow = new TableRow();
                TableCell cell = new TableCell();

                LinkButton lbtn = new LinkButton();
                lbtn.OnClientClick = "onMasterSelect(" + row["MasterEntryDetailsId"].ToString() + ",'" + row["Name"].ToString() + "')";
                lbtn.Text = row["Name"].ToString();
                lbtn.PostBackUrl = "#";
                lbtn.Width = 180;
                //lbtn.CssClass = "linkButton";

                //cell.BorderStyle = BorderStyle.Solid;
                cell.CssClass = "p_bg";
                cell.Controls.Add(lbtn);
                tblRow.Cells.Add(cell);
                masterListTable.Rows.Add(tblRow);
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            //Note: There are four possible values of SelectedOperation:
            //Add: When the user clicks on the Add button.
            //Edit: When the user clicks on Edit button.
            //PopulateSearchParams: When the user clicks on one of the Masters.

            base.OnPreLoad(e);
            if (!masterEntryTable.HasControls())
                try
                {
                    if (Request.QueryString["Type"] != null)
                    {
                        intMasterType = Convert.ToInt32(Request.QueryString["Type"].ToString());
                        LoadMasters(intMasterType);
                    }
                }
                catch (Exception ex)
                {
                    writeError("invalid input: " + ex.Message);
                }


            if (SelectedOperation == "Add" || SelectedOperation == "Edit")
            {
                hidePanels();
                CreateAddEditTable(SelectedMasterId);
            }
            else
            {
                masterEntryTable.Controls.Clear();
                masterEntryTable.Visible = false;
            }
            //if (SelectedOperation == "ViewDetails" && SelectedMasterId != 0)
            if (SelectedOperation == "PopulateSearchParams" && SelectedMasterId != 0)
            {
                hfFilterCondition.Value = "";
                hidePanels();
                populateSearchBy(SelectedMasterId);
                ShowAllData(SelectedMasterId, "");
                SelectedOperation = "ViewDetails";
            }
            else if (SelectedOperation == "ViewDetails" && SelectedMasterId != 0)
            {
                ShowAllData(SelectedMasterId, hfFilterCondition.Value);
            }
        }

        private int SelectedMasterId
        {
            get
            {
                if (hfSelectedMasterId.Value.ToString() == "") hfSelectedMasterId.Value = "0";
                return Convert.ToInt32(hfSelectedMasterId.Value);
            }
            set { hfSelectedMasterId.Value = value.ToString(); }
        }

        private string SelectedOperation
        {
            get
            {
                //Denil 29-Jun-2008.
                //if (hdfSelectedOperation.Value.ToString() == "") hdfSelectedOperation.Value = "ViewDetails";
                return hdfSelectedOperation.Value;
            }
            set { hdfSelectedOperation.Value = value; }
        }

        private void ShowAllData(int masterEntryDetailId, string FilterString)
        {
            string str = "Exec GetTableDataFromMaster " + masterEntryDetailId.ToString() + ", " + CommonMethods.GetSqlText(FilterString).ToString();
            DataTable dt = new DataServer().Getdata("Exec GetTableDataFromMaster " + masterEntryDetailId.ToString() + ", " + CommonMethods.GetSqlText(FilterString));

            gvAllRecords.DataSource = dt;
            gvAllRecords.PageIndex = 0;
            gvAllRecords.DataBind();

            Session["dtMasterData"] = dt;

            if ((this.gvAllRecords.Rows.Count == 0))
            {
                this.lblInfoMsg.Text = "No record found.";
                btnExportToExcel.Visible = false;
                this.lblInfoMsg.Visible = true;
            }
            else
            {
                btnExportToExcel.Visible = true;
                this.lblInfoMsg.Text = String.Empty;
                this.lblInfoMsg.Visible = false;
            }

            if (gvAllRecords.HeaderRow != null)
            {
                gvAllRecords.HeaderRow.Cells.Remove(gvAllRecords.HeaderRow.Cells[Id_ColPosition]);
                foreach (GridViewRow row in gvAllRecords.Rows)
                {
                    row.Cells.Remove(row.Cells[Id_ColPosition]);
                }
            }
            //Denil 29-Jun-2008.
            //btnAdd.Visible = true;
            ShowSave(false);
        }

        protected void gvAllRecords_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvAllRecords.PageIndex = e.NewPageIndex;
            gvAllRecords.DataSource = (DataTable)Session["dtMasterData"];
            gvAllRecords.DataBind();
            //<< Added by Amarjeet on 09-Apr-2021
            if (gvAllRecords.HeaderRow != null)
            {
                gvAllRecords.HeaderRow.Cells.Remove(gvAllRecords.HeaderRow.Cells[Id_ColPosition]);
                foreach (GridViewRow row in gvAllRecords.Rows)
                {
                    row.Cells.Remove(row.Cells[Id_ColPosition]);
                }
            }
            //>>
        }

        private void CreateAddEditTable(int masterEntryDetailId)
        {
            masterEntryTable.Visible = true;
            DataTable dt = new DataServer().Getdata("Select * From MasterEntryDetails Where MasterEntryDetailsId = "
                + masterEntryDetailId.ToString());
            if (dt.Rows.Count == 1)
            {
                DataRow row = dt.Rows[0];

                string processName = "";
                string tableName = "";
                string primaryKeyName = "";
                string parameters = "";
                //<< Added by Archana gosavi on 01-Jun-2016
                string strIsMandatory = "", strParameters = "";
                //>>
                processName = row["Name"].ToString();
                tableName = row["TableName"].ToString();
                primaryKeyName = row["PrimaryKeyName"].ToString();
                parameters = row["Parameters"].ToString();

                TableRow headerRow = new TableRow();
                TableCell cell = new TableCell();

                if (SelectedOperation == "Add")
                    cell.Text = "Add new entry";
                else
                    cell.Text = "Update entry";

                //<< Added by Archana Gosavi on 03-Aug-2017
                cell.ColumnSpan = 3;
                cell.CssClass = "contentBody";
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Font.Bold = true;
                cell.Font.Size = 15;
                //>>
                //headerRow.Cells.Add(cell);
                //masterEntryTable.Rows.Add(headerRow);

                int i = 0;
                foreach (string paramDetails in parameters.Split(';'))
                {
                    i++;
                    if (paramDetails == "") continue;
                    TableRow tblrow = new TableRow();

                    //Parameter Name
                    cell = new TableCell();
                    strParameters = GetColumnParameterValue(paramDetails, ColumnParameter.DisplayValue) + " : ";
                    //<< Added by Archana gosavi on 22-June-2016
                    bool isCompulsory = false;
                    isCompulsory = GetColumnParameterValue(paramDetails, ColumnParameter.IsCompulsory) == "0" ? false : true;
                    if (isCompulsory)
                    {
                        cell.Text = strParameters + "<span style=color:Red>*</span>";
                    }
                    else
                    {
                        cell.Text = strParameters;
                    }
                    //>>
                    cell.CssClass = "contentBody";
                    tblrow.Cells.Add(cell);


                    //Parameter Input
                    cell = new TableCell();

                    string inputType = GetColumnParameterValue(paramDetails, ColumnParameter.DataType);
                    Control inputCtrl = null;
                    //<< Addded by Archana Gosavi on 15-May-2017
                    if (inputType == "String" && paramDetails.Contains("Date"))
                    {
                        inputCtrl = new TextBox();
                        ((TextBox)inputCtrl).MaxLength = Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.Length));
                        if (Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.Length)) > 50)
                        {
                            ((TextBox)inputCtrl).TextMode = TextBoxMode.MultiLine;
                            ((TextBox)inputCtrl).Rows = 3;
                        }
                        ((TextBox)inputCtrl).Width = 300;
                        ((TextBox)inputCtrl).CssClass = "form-control mb-3";

                        //<< Added by Archana gosavi on 01-Jun-2106
                        strIsMandatory = " Please enter ";
                        //>>
                    }
                    //>>
                    else if (inputType == "String")
                    {
                        inputCtrl = new TextBox();
                        ((TextBox)inputCtrl).MaxLength = Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.Length));
                        if (Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.Length)) > 100)
                        {
                            ((TextBox)inputCtrl).TextMode = TextBoxMode.MultiLine;
                            ((TextBox)inputCtrl).Rows = 3;
                        }
                        ((TextBox)inputCtrl).Width = 300;
                        ((TextBox)inputCtrl).CssClass = "form-control mb-3";

                        //<< Added by Archana gosavi on 01-Jun-2106
                        strIsMandatory = " Please enter ";
                        //>>
                    }
                    //<< Added by Amarjeet on 16-Feb-2021
                    else if (inputType == "Textbox")
                    {
                        inputCtrl = new TextBox();
                        ((TextBox)inputCtrl).MaxLength = Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.Length));
                        //if (Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.Length)) > 150)
                        //{
                        //    ((TextBox)inputCtrl).TextMode = TextBoxMode.MultiLine;
                        //    ((TextBox)inputCtrl).Rows = 3;
                        //}
                        ((TextBox)inputCtrl).Width = 300;
                        ((TextBox)inputCtrl).CssClass = "form-control mb-3";

                        //<< Added by Archana gosavi on 01-Jun-2106
                        strIsMandatory = " Please enter ";
                        //>>
                    }
                    else if (inputType == "Number")
                    {
                        inputCtrl = new TextBox();
                        ((TextBox)inputCtrl).MaxLength = Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.Length));
                        //if (Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.Length)) > 150)
                        //{
                        //    ((TextBox)inputCtrl).TextMode = TextBoxMode.MultiLine;
                        //    ((TextBox)inputCtrl).Rows = 3;
                        //}
                        ((TextBox)inputCtrl).Width = 300;
                        ((TextBox)inputCtrl).CssClass = "form-control mb-3";

                        //<< Added by Archana gosavi on 01-Jun-2106
                        strIsMandatory = " Please enter ";
                        //>>
                    }
                    //>>
                    else if (inputType == "Option")
                    {
                        int loadDetailId = Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.LoadDetails).Trim('(', ')'));
                        string loadDetails = row["ParameterOptionDetails"].ToString().Split(';')[loadDetailId - 1];

                        //loadDetails = loadDetails.Trim('(', ')');
                        String strSQL = GetDropdownDetailsValue(loadDetails, DropdownParameterDetails.SQLQueryMember);
                        //<<Changed by Denil Shah on 27-Mar-2010 to fix "Login Failed for eComply error.
                        //SqlDataSource sds = new SqlDataSource(DataServer.Connection.ConnectionString, strSQL);
                        SqlDataSource sds = new SqlDataSource(ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString, strSQL);
                        //>>
                        sds.DataSourceMode = SqlDataSourceMode.DataReader;

                        inputCtrl = new DropDownList();
                        ((DropDownList)inputCtrl).AppendDataBoundItems = true;
                        //<< Added by Archana gosavi on 01-Jun-2106
                        ((DropDownList)inputCtrl).Items.Add(new ListItem("-- Select --", ""));
                        //>>
                        ((DropDownList)inputCtrl).DataValueField = GetDropdownDetailsValue(loadDetails, DropdownParameterDetails.ValueMember);
                        ((DropDownList)inputCtrl).DataTextField = GetDropdownDetailsValue(loadDetails, DropdownParameterDetails.DisplayMember);
                        ((DropDownList)inputCtrl).DataSource = sds;
                        ((DropDownList)inputCtrl).DataBind();
                        //((DropDownList)inputCtrl).Width = 300;
                        ((DropDownList)inputCtrl).CssClass = "form-select mb-3";

                        //<< Added by Archana gosavi on 01-Jun-2106
                        strIsMandatory = " Please select ";
                        //>>
                    }
                    else if (inputType == "MOption")
                    {
                        int loadDetailId = Convert.ToInt32(GetColumnParameterValue(paramDetails, ColumnParameter.LoadDetails).Trim('(', ')'));
                        string loadDetails = row["ParameterOptionDetails"].ToString().Split(';')[loadDetailId - 1];

                        //loadDetails = loadDetails.Trim('(', ')');

                        String strSQL = GetDropdownDetailsValue(loadDetails, DropdownParameterDetails.SQLQueryMember);
                        //<<Changed by Denil Shah on 27-Mar-2010 to fix "Login Failed for eComply error.
                        //SqlDataSource sds = new SqlDataSource(DataServer.Connection.ConnectionString, "Select * from " + 
                        //GetParameterDetailsValue(loadDetails, ParameterOptionDetails.TableName));
                        SqlDataSource sds = new SqlDataSource(ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString, "Select * from " +
                           GetParameterDetailsValue(loadDetails, ParameterOptionDetails.TableName));
                        //>>


                        sds.DataSourceMode = SqlDataSourceMode.DataReader;

                        inputCtrl = new CheckBoxList();
                        ((CheckBoxList)inputCtrl).AppendDataBoundItems = true;
                        ((CheckBoxList)inputCtrl).DataValueField = GetParameterDetailsValue(loadDetails, ParameterOptionDetails.ValueMember);
                        ((CheckBoxList)inputCtrl).DataTextField = GetParameterDetailsValue(loadDetails, ParameterOptionDetails.DisplayMember);
                        ((CheckBoxList)inputCtrl).DataSource = sds;
                        ((CheckBoxList)inputCtrl).DataBind();
                        ((CheckBoxList)inputCtrl).Width = 300;

                        //<< Added by Archana gosavi on 01-Jun-2106
                        strIsMandatory = " Please select ";
                        //>>

                    }

                    inputCtrl.ID = "inpCtrl" + i.ToString();
                    cell.Controls.Add(inputCtrl);
                    //<< Adedd by Archana Gosavi on 03-Aug-2017
                    cell.CssClass = "contentBody";
                    //>>
                    tblrow.Cells.Add(cell);

                    //Validators
                    bool isParamCompulsory = false;
                    isParamCompulsory = GetColumnParameterValue(paramDetails, ColumnParameter.IsCompulsory) == "0" ? false : true;
                    if (isParamCompulsory)
                    {
                        cell = new TableCell();
                        RequiredFieldValidator rfv = new RequiredFieldValidator();
                        rfv.ControlToValidate = inputCtrl.ID;
                        //<< Modified by Archana gosavi on 01-June-2016
                        rfv.ErrorMessage = strIsMandatory + strParameters.Remove(strParameters.LastIndexOf(':'));
                        //>>
                        //<< Added by Amarjeet on 09-Jan-2021
                        rfv.CssClass = "text-danger";
                        //>>
                        ////<< Added by Archana Gosavi on 01-June-2016
                        rfv.ValidationGroup = "Save";
                        rfv.Display = ValidatorDisplay.Dynamic;
                        cell.Controls.Add(rfv);
                        //>>

                        if (strParameters.Contains("Email"))
                        {
                            RegularExpressionValidator regex = new RegularExpressionValidator();
                            regex.ControlToValidate = inputCtrl.ID;
                            regex.ErrorMessage = " Please enter valid Email Id";
                            regex.ValidationExpression = mstrregEmailId;
                            regex.ValidationGroup = "Save";
                            regex.Display = ValidatorDisplay.Dynamic;
                            //<< Added by Amarjeet on 09-Jan-2021
                            regex.CssClass = "text-danger";
                            //>>
                            cell.Controls.Add(regex);
                        }
                        else if (strParameters.Contains("Date"))
                        {
                            RegularExpressionValidator regex = new RegularExpressionValidator();
                            regex.ControlToValidate = inputCtrl.ID;
                            regex.ErrorMessage = " Date Format has to be dd-MMM-yyyy.";
                            regex.ValidationExpression = "(^(([0-2][1-9])|(3[0-1])|(10)|(20))-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)-((19|20)[0-9][0-9])$)";
                            regex.ValidationGroup = "Save";
                            regex.Display = ValidatorDisplay.Dynamic;
                            //<< Added by Amarjeet on 09-Jan-2021
                            regex.CssClass = "text-danger";
                            //>>
                            cell.Controls.Add(regex);
                        }
                        //>>
                        cell.CssClass = "contentBody";
                        tblrow.Cells.Add(cell);
                    }

                    if (inputType == "Number")
                    {
                        F2FControls.F2FFilteredNumber F2FfNum = new F2FControls.F2FFilteredNumber();
                        F2FfNum.TargetControlID = inputCtrl.ID;
                        cell.Controls.Add(F2FfNum);
                    }

                    tblrow.EnableViewState = true;
                    tblrow.VerticalAlign = VerticalAlign.Top;
                    masterEntryTable.Rows.Add(tblrow);
                }
            }
        }

        private string GetDropdownDetailsValue(string columnDetail, DropdownParameterDetails paramName)
        {//For Dropdown population
            return columnDetail.Split('|')[(int)paramName - 1];
        }

        enum DropdownParameterDetails //For Dropdown population
        {
            SQLQueryMember = 1,
            ValueMember = 2,
            DisplayMember = 3
        }

        enum ColumnParameter
        {
            Name = 1,
            DisplayValue = 2,
            DataType = 3,
            Length = 4,
            IsCompulsory = 5,
            LoadDetails = 6
        }

        private string GetColumnParameterValue(string columnDetail, ColumnParameter paramName)
        {
            return columnDetail.Split(':')[(int)paramName - 1];
        }

        enum ParameterOptionDetails
        {
            Number = 1,
            TableName = 2,
            ValueMember = 3,
            DisplayMember = 4,
            RelationTableName = 5,
            RelationTableMasterRef = 6,
            RelationTableValueMemberRef = 7
        }

        private string GetParameterDetailsValue(string columnDetail, ParameterOptionDetails paramName)
        {
            return columnDetail.Split(',')[(int)paramName - 1];
        }

        private void ShowSave(bool show)
        {
            masterListTable.Enabled = !show;

            gvAllRecords.Visible = !show;
            SearchPanel.Visible = !show;
            btnAdd.Visible = !show;
            //btnExportToExcel.Visible = !show;

            this.SearchPanel.Visible = !show;

            btnSave.Visible = show;
            btnCancel.Visible = show;

            if (!show)
            {
                SelectedOperation = "ViewDetails";
                masterEntryTable.Controls.Clear();
                masterEntryTable.Visible = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //<< Added by Archana Gosavi on 01-June-2016
            btnExportToExcel.Visible = false;
            //>>
            ShowSave(true);
            //<< Added by Archana Gosavi on 22-June-2016 
            masterListTable.Enabled = true;
            lblInfoMsg.Text = "";
            //>>
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (!CommonCode.CheckInputValidity(this)) { return; }

            if (Page.IsValid)
            {
                if (SaveEntry(SelectedMasterId) == true)
                    ShowSave(false);
                ShowAllData(SelectedMasterId, hfFilterCondition.Value);
            }
        }

        private bool SaveEntry(int masterEntryDetailId)
        {
            try
            {
                DataServer ds = new DataServer();
                DataTable dt = ds.Getdata("Select * From MasterEntryDetails Where MasterEntryDetailsId = "
                    + masterEntryDetailId.ToString());
                //<<Added by Denil Shah on 23-Jun-2008.
                // string UserId = HRBusinessMethods.LoggedInUserInformation(Authentication.GetUserID(Page.User.Identity.Name)).UserId;
                //>>

                if (dt.Rows.Count == 1)
                {
                    DataRow row = dt.Rows[0];

                    string processName = "";
                    string tableName = "";
                    string primaryKeyName = "";
                    string parameters = "";

                    processName = row["Name"].ToString(); //Name
                    tableName = row["TableName"].ToString(); //TableName
                    primaryKeyName = row["PrimaryKeyName"].ToString(); //PrimaryKeyName
                    parameters = row["Parameters"].ToString(); //Parameters

                    string sqlString = "";
                    string sqlStringForParameters = "";
                    string paramNames = "";
                    string paramValues = "";
                    foreach (string paramDetails in parameters.Split(';'))
                    {
                        if (paramDetails.Trim() == "") continue;
                        if (GetColumnParameterValue(paramDetails, ColumnParameter.DataType) == "MOption") continue;

                        paramNames = paramNames == "" ? GetColumnParameterValue(paramDetails, ColumnParameter.Name) : paramNames + "|" + GetColumnParameterValue(paramDetails, ColumnParameter.Name);
                    }

                    int i = 0;
                    foreach (TableRow tblrow in masterEntryTable.Rows)
                    {
                        i++;
                        if (tblrow.Cells.Count == 1) continue;

                        Control inputCtrl = tblrow.Cells[1].Controls[0];
                        TextBox txt = inputCtrl as TextBox;
                        if (txt != null)
                        {
                            paramValues = paramValues == "" ? CommonMethods.GetSqlText(cm.getSanitizedString(txt.Text)) : paramValues + "|" + CommonMethods.GetSqlText(cm.getSanitizedString(txt.Text));
                            continue;
                        }

                        DropDownList ddl = inputCtrl as DropDownList;
                        if (ddl != null)
                        {
                            paramValues = paramValues == "" ? CommonMethods.GetSqlText(ddl.SelectedValue.ToString()) : paramValues + "|" + CommonMethods.GetSqlText(ddl.SelectedValue.ToString());
                            continue;
                        }

                        CheckBoxList cbl = inputCtrl as CheckBoxList;
                        if (cbl != null)
                        {
                            int loadDetailId = Convert.ToInt32(GetColumnParameterValue(parameters.Split(';')[i - 2], ColumnParameter.LoadDetails).Trim('(', ')'));
                            string loadDetails = row["ParameterOptionDetails"].ToString().Split(';')[loadDetailId - 1];
                            if (SelectedOperation == "Add")
                            {
                                foreach (ListItem item in cbl.Items)
                                {
                                    if (item.Selected == false) continue;
                                    sqlStringForParameters = sqlStringForParameters + " Insert Into "
                                        + GetParameterDetailsValue(loadDetails, ParameterOptionDetails.RelationTableName)
                                        + "(" + GetParameterDetailsValue(loadDetails, ParameterOptionDetails.RelationTableMasterRef)
                                        + "," + GetParameterDetailsValue(loadDetails, ParameterOptionDetails.RelationTableValueMemberRef)
                                        + ") Values(" + " @MasterRefId," + item.Value + ")";
                                }
                            }
                            else if (SelectedOperation == "Edit")
                            {
                                if (hfSelectedRecordId.Value != null || hfSelectedRecordId.Value != "0")
                                    sqlStringForParameters = sqlStringForParameters
                                                                + " Delete from " + GetParameterDetailsValue(loadDetails, ParameterOptionDetails.RelationTableName)
                                                                + " Where " + GetParameterDetailsValue(loadDetails, ParameterOptionDetails.RelationTableMasterRef) + " = " + hfSelectedRecordId.Value;

                                foreach (ListItem item in cbl.Items)
                                {
                                    if (item.Selected == false) continue;
                                    sqlStringForParameters = sqlStringForParameters + " Insert Into " + GetParameterDetailsValue(loadDetails, ParameterOptionDetails.RelationTableName)
                                        + "(" + GetParameterDetailsValue(loadDetails, ParameterOptionDetails.RelationTableMasterRef)
                                        + "," + GetParameterDetailsValue(loadDetails, ParameterOptionDetails.RelationTableValueMemberRef)
                                        + ") Values(" + hfSelectedRecordId.Value + "," + item.Value + ")";
                                }
                            }
                            continue;
                        }
                    }
                    if (SelectedOperation == "Add")
                    {
                        if (Convert.ToBoolean(row["IsAuditSupported"]))
                        {
                            paramNames = paramNames + "|CREATE_DT|CREATE_BY";
                            //<<Changed by Denil Shah on 23-Jun-2008.
                            //paramValues = paramValues + "|CURRENT_TIMESTAMP| " + CommonMethods.GetSqlText(Session["USER"].ToString());
                            //paramValues = paramValues + "|CURRENT_TIMESTAMP| " + CommonMethods.GetSqlText(UserId);
                            paramValues = paramValues + "|CURRENT_TIMESTAMP| " + CommonMethods.GetSqlText(Authentication.GetUserID(Page.User.Identity.Name));
                            //>>
                        }
                        sqlString = "declare @MasterRefId int " + "INSERT INTO " + tableName + "(" + paramNames.Replace("|", ",") + ") VALUES (" + paramValues.Replace("|", ",") + ")" + "\n" + "set @MasterRefId = SCOPE_IDENTITY()";
                    }
                    else if (SelectedOperation == "Edit")
                    {
                        if (Convert.ToBoolean(row["IsAuditSupported"]))
                        {
                            paramNames = paramNames + "|UPDATE_DT|UPDATE_BY";
                            //<<Changed by Denil Shah on 23-Jun-2008.
                            //paramValues = paramValues + "|CURRENT_TIMESTAMP| " + CommonMethods.GetSqlText(Session["USER"].ToString());
                            //paramValues = paramValues + "|CURRENT_TIMESTAMP| " + CommonMethods.GetSqlText(UserId);
                            paramValues = paramValues + "|CURRENT_TIMESTAMP|" + CommonMethods.GetSqlText(Authentication.GetUserID(Page.User.Identity.Name));
                            //>>
                        }
                        for (int j = 0; j < paramValues.Split('|').GetLength(0); j++)
                        {
                            if (sqlString == "")
                                sqlString = paramNames.Split('|')[j] + " = " + paramValues.Split('|')[j];
                            else
                                sqlString = sqlString + "," + paramNames.Split('|')[j] + " = " + paramValues.Split('|')[j];
                        }
                        sqlString = "UPDATE " + tableName + " SET " + sqlString + " WHERE " + primaryKeyName + "=" + hfSelectedRecordId.Value;
                    }

                    sqlString = sqlString + "  " + sqlStringForParameters;
                    int recAffected = DataServer.ExecuteSql(sqlString);
                    writeError("Record has been saved successfully.");
                    SelectedOperation = "ViewDetails";
                }
                return true;
            }
            catch (Exception ex)
            {
                writeError("In Common Masters Save:" + ex.Message);
                //Response.Write(ex);
                return false;
            }
        }

        private void writeError(string strError)
        {
            lblInfo.Text = strError;
            this.lblInfo.ForeColor = System.Drawing.Color.Red;
            lblInfo.Visible = true;
            this.lblInfo.Font.Size = 12;
        }

        private void hideError()
        {
            lblInfo.Text = "";
            lblInfo.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ShowSave(false);
            ShowAllData(SelectedMasterId, hfFilterCondition.Value);
        }

        protected void gvAllRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfSelectedRecordId.Value = gvAllRecords.SelectedValue.ToString();

            if (SelectedOperation == "Edit" || SelectedOperation == "Add")
            {
                //<< Added by Archana Gosavi on 01-June-2016
                btnExportToExcel.Visible = false;
                //>>
                StartEdit();
                ShowSave(true);
            }
            else if (SelectedOperation == "Delete")
            {
                isDeletePossible(SelectedMasterId);// new line
                                                   //Delete(SelectedMasterId);
                ShowAllData(SelectedMasterId, hfFilterCondition.Value);
            }
        }

        private void StartEdit()
        {
            DataTable dt = new DataServer().Getdata("Exec dbo.GetMasterRecordDetails "
                + SelectedMasterId.ToString() + "," + hfSelectedRecordId.Value);
            if (dt.Rows.Count != 1) return;

            int pIndex = -1;
            foreach (TableRow tblrow in masterEntryTable.Rows)
            {
                if (tblrow.Cells.Count == 1) continue;

                pIndex++;
                Control inputCtrl = tblrow.Cells[1].Controls[0];
                TextBox txt = inputCtrl as TextBox;
                if (txt != null)
                {
                    txt.Text = dt.Rows[0][pIndex].ToString();
                    //paramValues = paramValues == "" ? GetSqlText(txt.Text) : paramValues + "," + GetSqlText(txt.Text);
                }

                DropDownList ddl = inputCtrl as DropDownList;
                if (ddl != null)
                {
                    if (dt.Rows[0][pIndex].ToString() != "")
                        ddl.SelectedValue = dt.Rows[0][pIndex].ToString();
                    //paramValues = paramValues == "" ? ddl.SelectedValue.ToString() : paramValues + "," + ddl.SelectedValue.ToString();
                }

                CheckBoxList cbl = inputCtrl as CheckBoxList;
                if (cbl != null)
                {
                    if (dt.Rows[0][pIndex].ToString() != "")
                    {
                        foreach (ListItem li in cbl.Items)
                        {
                            foreach (string id in dt.Rows[0][pIndex].ToString().Split(','))
                            {
                                if (id == "") continue;
                                if (id == li.Value) li.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        #region "Delete Section"

        enum DeleteParameter
        {
            deleteTableName = 1,
            deleteColumnId = 2
        }

        private string GetDeleteColumnParameterValue(string columnDetail, DeleteParameter paramName)
        {
            return columnDetail.Split(':')[(int)paramName - 1];
        }

        private void Delete(int masterEntryDetailId)
        {
            try
            {
                DataTable dt = new DataServer().Getdata("Select DeleteParameters " +
                " From MasterEntryDetails Where MasterEntryDetailsId = " + masterEntryDetailId.ToString());

                if (dt.Rows.Count == 1)
                {
                    DataRow row = dt.Rows[0];
                    string deleteparameters = "";
                    string deleteSQL = "";
                    deleteparameters = row["DeleteParameters"].ToString(); //Parameters

                    foreach (string deleteParamDetails in deleteparameters.Split(';'))
                    {
                        if (deleteParamDetails.Trim() == "") continue;

                        else if (hfSelectedRecordId.Value != null || hfSelectedRecordId.Value != "0")
                            deleteSQL = deleteSQL
                            + " Delete from " + GetDeleteColumnParameterValue(deleteParamDetails, DeleteParameter.deleteTableName)
                            + " Where " + GetDeleteColumnParameterValue(deleteParamDetails, DeleteParameter.deleteColumnId) + " = "
                            + hfSelectedRecordId.Value;

                    }

                    int recAffected = DataServer.ExecuteSql(deleteSQL);
                    SelectedOperation = "Delete Details";
                    writeError("Record deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        #endregion

        #region "Search Section"
        private void populateSearchBy(int masterEntryDetailId)
        {
            try
            {
                this.SearchPanel.Visible = true;
                DataTable dt = new DataServer().Getdata("Select SearchParameters From MasterEntryDetails Where MasterEntryDetailsId = " + masterEntryDetailId.ToString());
                if (dt.Rows.Count == 1)
                {
                    DataRow row = dt.Rows[0];
                    string searchparameters = "";
                    searchparameters = row["SearchParameters"].ToString();
                    ddlSearchBy.Items.Clear();
                    ddlFieldType.Items.Clear();
                    ddlSearchBy.AppendDataBoundItems = true;
                    ddlFieldType.AppendDataBoundItems = true;
                    //<< Added by Archana Gosavi on 22-June-2016
                    ddlSearchBy.Items.Add(new ListItem("-- Select --", " "));
                    //>>
                    ddlSearchBy.Items.Add(new ListItem("All", "All"));
                    //<< Added by Archana Gosavi on 22-June-2016
                    ddlFieldType.Items.Add(new ListItem("-- Select --", " "));
                    ddlFieldType.Items.Add(new ListItem("None", "None"));
                    //>>
                    foreach (string searchParamDetails in searchparameters.Split(';'))
                    {
                        if (searchParamDetails.Trim() == "") continue;
                        else
                        {
                            ddlSearchBy.Items.Add(new ListItem(GetSearchParamValue(searchParamDetails, SearchParameter.SearchParamName), GetSearchParamValue(searchParamDetails, SearchParameter.ColumnName)));

                            ddlFieldType.Items.Add(new ListItem(GetSearchParamValue(searchParamDetails, SearchParameter.ControlType), searchParamDetails));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        enum SearchParameter
        {
            ColumnName = 1,
            SearchParamName = 2,
            ControlType = 3,
            ColumnType = 4,
            PopulationQuery = 5
        }

        private string GetSearchParamValue(string searchParamDetails, SearchParameter paramLocation)
        {
            return searchParamDetails.Split(':')[(int)paramLocation - 1];
        }

        protected void ddlSearchBy_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblInfoMsg.Text = "";
            //<< Added by Archana gosavi on 02-jun-2016
            string strIsMandatory = "";
            //>>
            ddlFieldType.SelectedIndex = ddlSearchBy.SelectedIndex;
            hidePanels();
            switch (ddlFieldType.SelectedItem.ToString())
            {
                case SEARCHTEXTPANEL:
                    this.TextPanel.Visible = true;
                    this.txtSearchText.Text = String.Empty;
                    lblTextSearchLabel.Text = (ddlSearchBy.SelectedItem.ToString() + ": ");
                    //<< Added by Archana gosavi on 02-Jun-2016
                    strIsMandatory = "Please Enter ";
                    rfvSearchText.ErrorMessage = strIsMandatory + ddlSearchBy.SelectedItem.ToString();
                    //>>
                    break;
                case SEARCHTEXTRANGEPANEL:
                    this.TextRangePanel.Visible = true;
                    this.txtSearchTextRangeFrom.Text = String.Empty;
                    this.txtSearchTextRangeTo.Text = String.Empty;
                    lblTextRangeFromLabel.Text = ("Min Value of "
                                + (ddlSearchBy.SelectedItem.ToString() + ":"));
                    lblTextRangeToLabel.Text = ("Max Value of "
                                + (ddlSearchBy.SelectedItem.ToString() + ":"));
                    //<< Added by Archana gosavi on 02-Jun-2016
                    strIsMandatory = "Please Enter ";
                    rfvSearchTextRangeFrom.ErrorMessage = strIsMandatory + ddlSearchBy.SelectedItem.ToString();
                    rfvSearchTextRangeFrom.ErrorMessage = strIsMandatory + ddlSearchBy.SelectedItem.ToString();
                    //>>
                    break;
                case SEARCHDATEPANEL:
                    this.DatePanel.Visible = true;
                    this.txtSearchDate.Text = String.Empty;
                    lblDateSearchLabel.Text = (ddlSearchBy.SelectedItem.ToString() + ": ");
                    //<< Added by Archana gosavi on 02-Jun-2016
                    strIsMandatory = "Please Enter ";
                    rfvSearchDate.ErrorMessage = strIsMandatory + ddlSearchBy.SelectedItem.ToString();
                    //>>
                    break;
                case SEARCHDATERANGEPANEL:
                    this.DateRangePanel.Visible = true;
                    this.txtSearchDateRangeFrom.Text = String.Empty;
                    this.txtSearchDateRangeTo.Text = String.Empty;
                    lblDateRangeFromLabel.Text = (ddlSearchBy.SelectedItem.ToString() + " From: ");
                    lblDateRangeToLabel.Text = (ddlSearchBy.SelectedItem.ToString() + " To: ");
                    //<< Added by Archana gosavi on 02-Jun-2016
                    strIsMandatory = "Please Enter ";
                    rfvSearchDateRangeFrom.ErrorMessage = strIsMandatory + ddlSearchBy.SelectedItem.ToString();
                    rfvSearchDateRangeTo.ErrorMessage = strIsMandatory + ddlSearchBy.SelectedItem.ToString();
                    //>>
                    break;
                case SEARCHDROPDOWNPANEL:
                    populateDropdown();
                    this.DropdownPanel.Visible = true;
                    lblDropdownSearchLabel.Text = (ddlSearchBy.SelectedItem.ToString() + ": ");
                    break;
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                hfFilterCondition.Value = String.Empty;
                string strFilterExpression = String.Empty;
                //string strSelectedValue = ddlFieldType.SelectedValue.ToString();
                if ((this.ddlSearchBy.SelectedValue == "All"))
                {
                    strFilterExpression = String.Empty;
                }
                else
                {
                    string strSelectedValue = GetSearchParamValue(ddlFieldType.SelectedValue.ToString(), SearchParameter.ColumnType);
                    switch (ddlFieldType.SelectedItem.ToString())
                    {
                        case SEARCHTEXTPANEL:
                            if ((strSelectedValue == DATATYPE_STRING))
                            {
                                strFilterExpression = (this.ddlSearchBy.SelectedValue + (" like \'"
                                            + (this.txtSearchText.Text.Replace("\'", "\'\'").Replace("%", "[%]") + "%\'")));
                            }
                            else if ((strSelectedValue == DATATYPE_NUMERIC))
                            {
                                strFilterExpression = (this.ddlSearchBy.SelectedValue + (" = " + this.txtSearchText.Text));
                            }
                            break;
                        case SEARCHTEXTRANGEPANEL:
                            if ((strSelectedValue == DATATYPE_STRING))
                            {

                            }
                            else if ((strSelectedValue == DATATYPE_NUMERIC))
                            {
                                strFilterExpression = (this.ddlSearchBy.SelectedValue + (" >= "
                                            + (this.txtSearchTextRangeFrom.Text + (" and "
                                            + (this.ddlSearchBy.SelectedValue + (" <= " + this.txtSearchTextRangeTo.Text))))));
                            }
                            break;
                        case SEARCHDATEPANEL:
                            strFilterExpression = (this.ddlSearchBy.SelectedValue + (" = " + System.Convert.ToDateTime(this.txtSearchDate.Text)));
                            break;
                        case SEARCHDATERANGEPANEL:
                            strFilterExpression = (this.ddlSearchBy.SelectedValue + (" >= "
                                        + (System.Convert.ToDateTime(this.txtSearchDateRangeFrom.Text) + (" and "
                                        + (this.ddlSearchBy.SelectedValue + (" <= " + System.Convert.ToDateTime(this.txtSearchDateRangeTo.Text)))))));
                            break;
                        case SEARCHDROPDOWNPANEL:
                            strFilterExpression = (this.ddlSearchBy.SelectedValue + (" = \'"
                                        + (ddlSearchValues.SelectedValue.Replace("\'", "\'\'") + "\'")));
                            break;
                    }
                }

                if (strFilterExpression != String.Empty)
                    strFilterExpression = " where " + strFilterExpression;
                hfFilterCondition.Value = strFilterExpression;
                ShowAllData(SelectedMasterId, strFilterExpression);

            }
            catch (Exception ex)
            {
                this.lblInfoMsg.Text = ex.Message;
                this.lblInfoMsg.Visible = true;
            }
        }

        private void hidePanels()
        {
            this.TextPanel.Visible = false;
            this.TextRangePanel.Visible = false;
            this.DatePanel.Visible = false;
            this.DateRangePanel.Visible = false;
            this.DropdownPanel.Visible = false;
        }

        private void populateDropdown()
        {
            string strQuery;
            strQuery = GetSearchParamValue(ddlFieldType.SelectedValue.ToString(), SearchParameter.PopulationQuery);
            DataTable dt = new DataServer().Getdata(strQuery);
            ddlSearchValues.DataSource = dt;
            ddlSearchValues.DataTextField = "Name";
            ddlSearchValues.DataValueField = "Value";
            ddlSearchValues.DataBind();
        }

        #endregion

        #region "Export to Excel"

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExportGridViewToExcel(gvAllRecords);
        }

        private void ExportGridViewToExcel(GridView gv)
        {
            gv.AllowPaging = false;
            gv.AllowSorting = false;
            gv.Columns[1].Visible = false;
            gv.Columns[2].Visible = false;
            gv.DataSource = (DataTable)(Session["dtMasterData"]);
            gv.DataBind();
            CommonCodes.PrepareGridViewForExport(gv);
            string attachment = "attachment; filename=ExcelReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gv.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
            gv.AllowPaging = true;
            gv.AllowSorting = true;
            gv.DataBind();
        }
        
        private void isDeletePossible(int masterEntryDetailId)
        {
            try
            {
                DataTable dtRefParameters = new DataTable();
                DataRow drCount;
                bool blnFlage = true;
                DataTable dt = new DataServer().Getdata("Select ReferentialIntegrity From MasterEntryDetails Where MasterEntryDetailsId = " + masterEntryDetailId.ToString());
                if (dt.Rows.Count == 1)
                {
                    DataRow row = dt.Rows[0];
                    string strReferentialIntparameters = "";
                    string strCountSQL = "";
                    strReferentialIntparameters = row["ReferentialIntegrity"].ToString(); //Parameters

                    foreach (string strRefIntegrityDetails in strReferentialIntparameters.Split(';'))
                    {
                        if (strRefIntegrityDetails.Trim() == "") continue;

                        else if (hfSelectedRecordId.Value != null || hfSelectedRecordId.Value != "0")
                            strCountSQL = " select count(*) as cnt from " + GetRefColumnParameterValue(strRefIntegrityDetails, RefIntegrityParameter.TableName)
                            + " Where " + GetRefColumnParameterValue(strRefIntegrityDetails, RefIntegrityParameter.ColumnId) + " = "
                            + hfSelectedRecordId.Value;

                        dtRefParameters = new DataServer().Getdata(strCountSQL);
                        drCount = dtRefParameters.Rows[0];
                        SelectedOperation = "Search for Referential Integrity.";
                        if (!drCount["cnt"].ToString().Equals("0"))
                        {
                            blnFlage = false;
                            writeError("Entries are present for this record in module '" + GetRefColumnParameterValue(strRefIntegrityDetails, RefIntegrityParameter.ModuleName) + "', So record can't be deleted.");
                            return;
                        }

                    }
                    if (blnFlage)
                    {
                        Delete(SelectedMasterId);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        enum RefIntegrityParameter
        {
            TableName = 1,
            ColumnId = 2,
            ModuleName = 3
        }

        private string GetRefColumnParameterValue(string columnDetail, RefIntegrityParameter paramName)
        {
            return columnDetail.Split(':')[(int)paramName - 1];
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion

        ////<< Added by Archana gosavi on 02-Jun-2016
        //protected void gvAllRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvAllRecords.PageIndex = e.NewPageIndex;
        //    gvAllRecords.DataSource = (DataTable)Session["dtMasterData"];
        //    gvAllRecords.DataBind();
        //}
        ////>>
    }
}