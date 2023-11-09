using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_approve : System.Web.UI.Page
    {
        private DwTrans sqlca;
        private WebState state;
        private String emplid;
        private String empledu;


        //POSTBACK
        protected String getEmplid;
        protected String saveData;
        protected String newRecord;
        protected String checkValue;
        protected String newEmpl;

        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);
            getEmplid = WebUtil.JsPostBack(this, "getEmplid");
            saveData = WebUtil.JsPostBack(this, "saveData");
            newEmpl = WebUtil.JsPostBack(this, "newEmpl");
            checkValue = WebUtil.JsPostBack(this, "checkValue");

            if (state.IsReadable)
            {
                LtServerMessage.Text = "";

                sqlca = new DwTrans();
                sqlca.Connect();
                DwMain.SetTransaction(sqlca);
                if (IsPostBack)
                {
                    DwMain.SetItemString(1, "entry_id", state.SsUsername);
                    DwMain.RestoreContext();
                    try
                    {
                        String eventArg = Request["__EVENTARGUMENT"];
                        if (eventArg == "getEmplid")
                        {
                            GetEmplid();
                        }
                        else if (eventArg == "saveData")
                        {
                            SaveDataWindow();
                        }
                        //else if(eventArg = "newRecord")
                        //{
                        //    NewRecord();
                        //}else if("checkValue")
                        //{
                        //    CheckValue();
                        //}
                    }
                    catch { }
                }
                else
                {
                    // !IsPostback
                    //DwMain.Retrieve("");
                }

                if (DwMain.RowCount < 1)
                {
                    DwMain.InsertRow(0);
                }

                //Last Init
                try
                {
                    emplid = DwMain.GetItemString(1, "emplid");
                    if (emplid == "") throw new Exception();
                }
                catch { emplid = null; }

            }
            else
            {
                LtServerMessage.Text = WebUtil.PermissionDeny(PermissType.ReadDeny);
                return;
            }

        }
        private void CheckValue()
        {
            throw new NotImplementedException();
        }

        private void NewRecord()
        {
            throw new NotImplementedException();
        }

        private void SaveDataWindow()
        {
            throw new NotImplementedException();
        }

        private void GetEmplid()
        {
            String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid");
                DwMain.Reset();
                DwMain.Retrieve(emplid);
            }
            catch
            {
            }
        }
        protected void Page_LoadComplete()
        {
            sqlca.Disconnect();
        }
    }
}
