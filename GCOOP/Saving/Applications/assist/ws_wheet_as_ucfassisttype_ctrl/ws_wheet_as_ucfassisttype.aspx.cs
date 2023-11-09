using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.assist.ws_wheet_as_ucfassisttype_ctrl
{
    public partial class ws_wheet_as_ucfassisttype : PageWebSheet, WebSheet
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
                dsList.RetriveGroup();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostNewRow)
            {
                dsList.InsertAtRow(0);
                dsList.SetItem(0, dsList.DATA.COOP_IDColumn, state.SsCoopControl);//set value to primary key
                dsList.RetriveGroup();
            }
            else if (eventArg == PostDelRow)
            {
                String ls_chktype = "";
                int row = dsList.GetRowFocus();
                String ls_type = dsList.DATA[row].ASSISTTYPE_CODE;
                
                //chk ประเภทคำขอ
                string sql = @"select assisttype_code from asnreqmaster where assisttype_code={0} and coop_id={1} and req_status=1 and rownum=1";
                sql = WebUtil.SQLFormat(sql, ls_type, state.SsCoopControl);
                Sdt dt = WebUtil.QuerySdt(sql);
                //chk เงื่อนไขการจ่าย
                string sql_pay = @"select assisttype_pay from ASSUCFASSISTTYPEDET where assisttype_code={0} and coop_id={1} ";
                sql_pay = WebUtil.SQLFormat(sql_pay, ls_type, state.SsCoopControl);
                Sdt dt_pay = WebUtil.QuerySdt(sql_pay);               

                if (dt.Next())
                {
                    ls_chktype = dt.GetString("assisttype_code");
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('สวัสดิการประเภทนี้มีการขอใช้แล้วไม่สามารถลบได้');", true); return;
                }
                else if (dt_pay.Next())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('สวัสดิการประเภทนี้มีการกำหนดเงื่อนไขการจ่ายแล้วไม่สามารถลบได้');", true); return;
                }
                else
                {
                    //dsList.DeleteRow(row);
                    try
                    {
                        ls_chktype = @"delete from ASSUCFASSISTTYPE where coop_id = {0} and assisttype_code={1} ";
                        ls_chktype = WebUtil.SQLFormat(ls_chktype, state.SsCoopId, ls_type);
                        WebUtil.ExeSQL(ls_chktype);
                        dsList.Retrieve();
                        dsList.RetriveGroup();
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
            string sqlStr, ls_assiscode, ls_assisdesc, ls_chkassiscode = "", ls_assisgroup="";
            decimal dec_membflag, dec_calflag, dec_proflag;
            int li_row, li_family;
            decimal li_stmflag = 0;

            try
            {
                //for chk
                for (li_row = 0; li_row < dsList.RowCount; li_row++)
                {
                    if (dsList.DATA[li_row].ASSISTTYPE_CODE.ToString() == "") {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกรหัสประเภทสวัสดิการ');", true); return;
                    }
                    else if (dsList.DATA[li_row].ASSISTTYPE_DESC.ToString() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('กรุณากรอกคำอธิบายสวัสดิการ');", true); return;
                    }
                    else if (ls_chkassiscode.IndexOf(dsList.DATA[li_row].ASSISTTYPE_CODE.ToString()) > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('รหัสประเภทสวัสดิการซ้ำกัน กรุณาตรวจสอบ');", true); return;
                    }
                    ls_chkassiscode = ls_chkassiscode + ", " + dsList.DATA[li_row].ASSISTTYPE_CODE.ToString();                    
                }
                for (li_row = 0; li_row < dsList.RowCount; li_row++)
                {
                    ls_assiscode = dsList.DATA[li_row].ASSISTTYPE_CODE.ToString();
                    ls_assisdesc = dsList.DATA[li_row].ASSISTTYPE_DESC.ToString();
                    li_stmflag = dsList.DATA[li_row].STM_FLAG;
                    ls_assisgroup = dsList.DATA[li_row].ASSISTTYPE_GROUP;
                    dec_membflag = dsList.DATA[li_row].membtype_flag;
                    dec_calflag = dsList.DATA[li_row].calculate_flag;
                    li_family = dsList.DATA[li_row].family_flag;

                    if (li_stmflag == 0)
                    {
                        dec_proflag = 0;
                    }
                    else
                    {
                        dec_proflag = dsList.DATA[li_row].process_flag;
                    }
                    
                    //chk ประเภทสวัสดิการ
                    string sql = @"select assisttype_code from ASSUCFASSISTTYPE where assisttype_code={0} and coop_id={1}";
                    sql = WebUtil.SQLFormat(sql, ls_assiscode, state.SsCoopControl);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        sqlStr = @"update ASSUCFASSISTTYPE set 
                                assisttype_desc={0},stm_flag={1},ASSISTTYPE_GROUP={4}, membtype_flag = {5}, calculate_flag = {6}, process_flag = {7}, family_flag = {8}
                                where assisttype_code={2} and coop_id={3}
                                ";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_assisdesc, li_stmflag, ls_assiscode, state.SsCoopId, ls_assisgroup, dec_membflag, dec_calflag, dec_proflag, li_family);
                        WebUtil.ExeSQL(sqlStr);
                    }else{
                        sqlStr = @"insert into ASSUCFASSISTTYPE 
                            (assisttype_code, assisttype_desc, coop_id, stm_flag, assisttype_group, membtype_flag, calculate_flag, process_flag, family_flag)
                            values
                            ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})";
                        sqlStr = WebUtil.SQLFormat(sqlStr, ls_assiscode, ls_assisdesc, state.SsCoopId, li_stmflag, ls_assisgroup, dec_membflag, dec_calflag, dec_proflag, li_family);
                        WebUtil.ExeSQL(sqlStr);
                    }
                }
                dsList.Retrieve();
                dsList.RetriveGroup();
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเสร็จ");
            }
        }

        public void WebSheetLoadEnd()
        {
            for (int ii = 0; ii < dsList.RowCount; ii++)
            {
                if (dsList.DATA[ii].STM_FLAG == 0)
                {
                    dsList.FindDropDownList(ii, "process_flag").Enabled = false;
                }
            }
        }
    }
}