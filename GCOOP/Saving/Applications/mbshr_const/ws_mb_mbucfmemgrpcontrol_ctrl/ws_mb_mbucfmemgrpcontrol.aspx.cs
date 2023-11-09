using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using CoreSavingLibrary;

namespace Saving.Applications.mbshr_const.ws_mb_mbucfmemgrpcontrol_ctrl
{
    public partial class ws_mb_mbucfmemgrpcontrol : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String PostDeleteRow { get; set; }
        [JsPostBack]
        public String PostGroupControl { get; set; }
        [JsPostBack]
        public String RefreshSheet { get; set; }

        public void InitJsPostBack()
        {

            dsSearch.InitDsSearch(this);
            dsList.InitDsList(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsSearch.DdGroupControl();
                dsList.RetrieveList();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostDeleteRow)
            {
                int ls_row = dsList.GetRowFocus();
                string ls_membgroup_code = dsList.DATA[ls_row].MEMBGROUP_CONTROL;

                ExecuteDataSource exed1 = new ExecuteDataSource(this);

                string sql = "delete from mbucfmembgroup where coop_id='" + state.SsCoopControl + "' and membgroup_code='" + ls_membgroup_code + "'";
                exed1.SQL.Add(sql);
                exed1.Execute();

                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                dsList.RetrieveList();
            }
            else if (eventArg == PostGroupControl)
            {
                dsList.RetrieveList();
            }
            else if (eventArg == RefreshSheet)
            {
                dsList.RetrieveList();
            }
        }

        public void SaveWebSheet()
        {
            try
            {
                
                string sql = "delete from mbucfmembgroupcontrol";
                Sdt dt = WebUtil.QuerySdt(sql);
                //exed1.SQL.Add(sql);
                ExecuteDataSource exed1 = new ExecuteDataSource(this);
                exed1.AddRepeater(dsList);
                exed1.Execute();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
        }

        public void WebSheetLoadEnd()
        {

        }


    }
}