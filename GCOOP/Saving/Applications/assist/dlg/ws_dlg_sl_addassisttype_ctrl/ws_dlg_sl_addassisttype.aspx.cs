using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CoreSavingLibrary.WcfNShrlon;
using DataLibrary;

namespace Saving.Applications.assist.dlg.ws_dlg_sl_addassisttype_ctrl
{
    public partial class ws_dlg_sl_addassisttype : PageWebDialog, WebDialog
    {
        [JsPostBack]
        public string PostSave { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebDialogLoadBegin()
        {
            dsMain.RetriveGroup();
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "PostSave")
            {
                try
                {
                    //dsMain.DATA[0].coop_id = state.SsCoopControl;
                 
                    string ls_asscode = dsMain.DATA[0].assisttype_code;
                    string ls_assdesc = dsMain.DATA[0].assisttype_desc;
                    string ls_assgroup = dsMain.DATA[0].assisttype_group;
                    int li_process = dsMain.DATA[0].process_flag;
                    int li_calculate = dsMain.DATA[0].calculate_flag;
                    int li_family = dsMain.DATA[0].family_flag;
                    int li_membtype = dsMain.DATA[0].membtype_flag;
                    int li_stm = dsMain.DATA[0].stm_flag;

                    if (ls_asscode.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกรหัสสวัสดิการ');", true); return;
                    }
                    if (ls_assgroup.Trim() == "" || ls_assgroup == "00")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณาเลือกกลุ่มสวัสดิการ');", true); return;
                    }

                    string sql = @"select * from assucfassisttype where coop_id = {0} and assisttype_code= {1}";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, ls_asscode);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.GetRowCount() > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('มีรหัสสวัสดิการนี้แล้ว กรุณาตรวจสอบ');", true); return;
                    }

                    string sqlStr = @"insert into assucfassisttype 
                                    (coop_id            , assisttype_code   , assisttype_desc   , stm_flag
                                    , assisttype_group  , membtype_flag     , calculate_flag    , family_flag
                                    , process_flag)
                                    values
                                     ({0}               , {1}               , {2}               , {3}
                                    , {4}               , {5}               , {6}               , {7}
                                    , {8})";
                    sqlStr = WebUtil.SQLFormat(sqlStr
                            , state.SsCoopId, ls_asscode, ls_assdesc, li_stm
                            , ls_assgroup, li_membtype, li_calculate, li_family
                            , li_process);
                    WebUtil.ExeSQL(sqlStr);

                    //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('บันทึกสำเร็จ');", true);
                    this.SetOnLoadedScript("parent.GetValueFromDlg(" + ls_asscode + ");");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ " + ex);
                }
            }

        }

        public void WebDialogLoadEnd()
        {

        }
    }
}