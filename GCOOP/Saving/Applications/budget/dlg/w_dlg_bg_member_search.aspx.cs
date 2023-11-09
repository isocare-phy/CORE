using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Saving.Applications.budget.dlg
{
    public partial class w_dlg_bg_member_search : PageWebDialog,WebDialog
    {
        private String sqlStr;
        #region WebDialog Members

        public void InitJsPostBack()
        {
        }

        public void WebDialogLoadBegin()
        {
            try
            {
                sqlStr = DwList.GetSqlSelect();
                if (!IsPostBack)
                {
                    DwMain.InsertRow(0);
                    DwUtil.RetrieveDataWindow(DwList, "budget_dlg.pbl", null, null);
                }
                else
                {
                    this.RestoreContextDw(DwMain);
                    this.RestoreContextDw(DwList);
                }
            }
            catch(Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
            DwMain.SaveDataCache();
            DwList.SaveDataCache();
        }

        #endregion

        protected void B_Search_Click(object sender, EventArgs e)
        {
            try
            {

                String memno, memname, memsurname, memgroup, temp, sqlext;
                int rc = 0;
                try
                {
                    memno = (DwMain.GetItemString(1, "member_no")).Trim();
                }
                catch { memno = ""; }
                try
                {
                    memname = (DwMain.GetItemString(1, "member_name")).Trim();
                }
                catch { memname = ""; }
                try
                {
                    memsurname = (DwMain.GetItemString(1, "member_surname")).Trim();
                }
                catch { memsurname = ""; }
                try
                {
                    memgroup = (DwMain.GetItemString(1, "member_group_no")).Trim();
                }
                catch { memgroup = ""; }
                sqlext = "";

                if (memno.Length > 0)
                {
                    sqlext = " and ( mbmembmaster.member_no like '" + memno + "%') ";
                }

                if (memname.Length > 0)
                {
                    sqlext += " and ( mbmembmaster.memb_name like '" + memname + "%') ";
                }

                if (memsurname.Length > 0)
                {
                    sqlext += " and ( mbmembmaster.memb_surname like '" + memsurname + "%') ";
                }

                if (memgroup.Length > 0)
                {
                    sqlext += " and ( mbmembmaster.membgroup_code = '" + memgroup + "') ";
                }

                temp = sqlStr + sqlext;
                DwUtil.ImportData(temp, DwList, null);
                if (rc < 1)
                {
                    LtServerMessage.Text = "ไม่พบข้อมูลที่ค้นหา";
                }
                else { LtServerMessage.Text = ""; }
            }
            catch (Exception ex)
            { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }
    }
}
