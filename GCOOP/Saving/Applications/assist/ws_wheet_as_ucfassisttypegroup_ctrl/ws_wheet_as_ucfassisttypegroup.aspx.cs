using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.assist.ws_wheet_as_ucfassisttypegroup_ctrl
{
    public partial class ws_wheet_as_ucfassisttypegroup : PageWebSheet, WebSheet
    {

        [JsPostBack]
        public string PostNewRow { get; set; }
        [JsPostBack]
        public string PostDelRow { get; set; }

        public void InitJsPostBack()
        {
            dsList.InitDs(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsList.Retrieve();//show data first
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostNewRow)
            {
                dsList.InsertAtRow(0);
                dsList.SetItem(0, dsList.DATA.COOP_IDColumn, state.SsCoopControl);//set value to primary key
            }
            else if (eventArg == PostDelRow)
            {
                String ls_chktype = "";
                int row = dsList.GetRowFocus();
                String ls_type = dsList.DATA[row].ASSISTTYPE_GROUP;

                string sql_pay = @"select assisttype_group from assucfassistpaytype where assisttype_group={0} and coop_id={1} and rownum=1 ";
                sql_pay = WebUtil.SQLFormat(sql_pay, ls_type, state.SsCoopControl);
                Sdt dt_pay = WebUtil.QuerySdt(sql_pay);               

                if (dt_pay.Next())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กลุ่มสวัสดิการประเภทนี้มีการกำหนดเงื่อนไขการจ่ายแล้วไม่สามารถลบได้');", true); return;
                }
                else
                {
                    //dsList.DeleteRow(row);
                    try
                    {
                        ls_chktype = @"delete from assucfassisttypegroup where coop_id = {0} and assisttype_group={1} ";
                        ls_chktype = WebUtil.SQLFormat(ls_chktype, state.SsCoopId, ls_type);
                        WebUtil.ExeSQL(ls_chktype);
                        dsList.Retrieve();
                        LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                    }
                    catch {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ลบข้อมูลไม่สำเสร็จ");
                    }
                }
            }
        }
        
        public void SaveWebSheet()
        {
            string sqlStr, ls_assiscode, ls_assisdesc, ls_chkassiscode = "";
            int li_row;
            try
            {
                //for chk
                for (li_row = 0; li_row < dsList.RowCount; li_row++)
                {
                    if (dsList.DATA[li_row].ASSISTTYPE_GROUP.ToString() == "") {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกรหัสกลุ่มสวัสดิการ');", true); return;
                    }
                    else if (dsList.DATA[li_row].ASSISTTYPE_GROUPDESC.ToString() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกคำอธิบายกลุ่มสวัสดิการ');", true); return;
                    }
                    else if (ls_chkassiscode.IndexOf(dsList.DATA[li_row].ASSISTTYPE_GROUP.ToString()) > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('รหัสประเภทสวัสดิการซ้ำกัน กรุณาตรวจสอบ');", true); return;
                    }
                    ls_chkassiscode = ls_chkassiscode + ", " + dsList.DATA[li_row].ASSISTTYPE_GROUP.ToString();                    
                }
                for (li_row = 0; li_row < dsList.RowCount; li_row++)
                {
                    ls_assiscode = dsList.DATA[li_row].ASSISTTYPE_GROUP.ToString();
                    ls_assisdesc = dsList.DATA[li_row].ASSISTTYPE_GROUPDESC.ToString();
                    //chk ประเภทสวัสดิการ
                    string sql = @"select assisttype_group from assucfassisttypegroup where assisttype_group={0} and coop_id={1}";
                    sql = WebUtil.SQLFormat(sql, ls_assiscode, state.SsCoopControl);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        sqlStr = @"update assucfassisttypegroup set 
                                assisttype_groupdesc={0}  where assisttype_group={1} and coop_id={2}
                                ";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_assisdesc, ls_assiscode, state.SsCoopId);
                        WebUtil.ExeSQL(sqlStr);
                    }else{
                        sqlStr = @"insert into assucfassisttypegroup 
                            (coop_id, assisttype_group,assisttype_groupdesc)
                            values
                            ({0}, {1}, {2})";
                        sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, ls_assiscode, ls_assisdesc);
                        WebUtil.ExeSQL(sqlStr);
                    }
                }
                dsList.Retrieve();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเสร็จ");
            }
        }

        public void WebSheetLoadEnd()
        {

        }
    }
}