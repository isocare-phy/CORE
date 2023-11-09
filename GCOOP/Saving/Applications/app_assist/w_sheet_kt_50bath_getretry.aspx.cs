using System;
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
using CommonLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using DBAccess;
using CommonLibrary.WsAssist;

namespace Saving.Applications.app_assist
{
    public partial class w_sheet_kt_50bath_getretry : PageWebSheet, WebSheet
    {
        protected String postGetRetry;
        protected String postRefresh;
        private Assist AssistService;
        private DwThDate thDwMain;

        public void InitJsPostBack()
        {
            postGetRetry = WebUtil.JsPostBack(this, "postGetRetry");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");

            thDwMain = new DwThDate(DwMain, this);
            thDwMain.Add("calculate_date", "calculate_tdate");
        }

        public void WebSheetLoadBegin()
        {
            try
            {
                AssistService = WsCalling.Assist;
            }
            catch
            {
                WebUtil.ErrorMessage("ไม่สามารถติดต่อกับ Webservice ได้");
            }
            if (!IsPostBack)
            {
                DwMain.InsertRow(0);
                DwMain.SetItemDateTime(1, "calculate_date", state.SsWorkDate);
                thDwMain.Eng2ThaiAllRow();

                try
                {
                    String SqlSelect = "select count(M.member_no) from mbmembmaster M , shsharemaster S , dpdeptmaster D where  M.member_status =1 and M.emp_type in ('03','04','09')  and M.member_no = S.member_no and M.member_no = D.member_no and S.sharestk_amt <> 0 and D.deptclose_status <> 1 and D.prncbal <>0 and M.member_no in (select member_no from asnreqmaster where assisttype_code = '70')";
                    Sdt dtCount = WebUtil.QuerySdt(SqlSelect);
                    if (dtCount.Next())
                    {
                        int count = dtCount.GetInt32("count(M.member_no)");
                        LtServerMessage.Text = WebUtil.WarningMessage("มีจำนวนคนทั้งหมด : " + count + " คน");
                    }
                }
                catch { }
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            switch (eventArg)
            {
                case "postGetRetry": GetRetry();
                    break;
                case "postRefresh" :
                    break;
            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
            DwMain.SaveDataCache();
        }

        public void GetRetry()
        {
            decimal select_option = 0;
            try
            {
                select_option = DwMain.GetItemDecimal(1, "select_option");
            }
            catch { }
            
            if (select_option == 1) // เพิ่มทั้งหมด
            {
                try
                {

                    string proc_date = DwMain.GetItemDateTime(1, "calculate_date").ToString("dd/MM/yyyy");

                    String UpdateSQL = @"UPDATE  asnreqmaster 
                                            SET proc_option = 'RTY', proc_date = to_date('" + proc_date + @"', 'dd/mm/yyyy'), 
                                            moneytype_code = 'CSH', expense_bank = '', expense_branch = '', 
                                            expense_accid = '', to_system = '' 
                                            WHERE assisttype_code = '70 and exists (
                                            select 1
                                            from mbmembmaster M , shsharemaster S , dpdeptmaster D 
                                            where  M.member_status = 1 and M.emp_type in ('03','04','09')  and M.member_no = asnreqmaster.member_no
                                            and M.member_no = S.member_no and M.member_no = D.member_no 
                                            and S.sharestk_amt <> 0 and D.deptclose_status <> 1 and D.prncbal <> 0 
                                            and M.member_no in (select member_no from asnreqmaster where assisttype_code = '70')
                                            )";
                    WebUtil.Query(UpdateSQL);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการเสร็จเรียบร้อย ให้ไปที่หน้าจอ จ่ายกรณีลากออก/ให้ออก/เกษียณ เพื่อทำการประมวลผล");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ เนื่องจาก : " + ex);
                }
            }
            else if (select_option == 2)//ยกเลิกทั้งหมด
            {
                try
                {
                    String UpdateSQL = @"UPDATE  asnreqmaster
                                            SET proc_option = 'NOL', 
                                            moneytype_code = 'CSH', expense_bank = '', expense_branch = '', 
                                            expense_accid = '', to_system = '' 
                                            WHERE assisttype_code = '70 and exists (
                                            select 1
                                            from mbmembmaster M , shsharemaster S , dpdeptmaster D 
                                            where  M.member_status = 1 and M.emp_type in ('03','04','09') and M.member_no = asnreqmaster.member_no
                                            and M.member_no = S.member_no and M.member_no = D.member_no 
                                            and S.sharestk_amt <> 0 and D.deptclose_status <> 1 and D.prncbal <> 0 
                                            and M.member_no in (select member_no from asnreqmaster where assisttype_code = '70')
                                            )";
                    WebUtil.Query(UpdateSQL);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ยกเลิกการรอจ่ายผู้เกษียณทั้งหมด เรียบร้อยแล้ว");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ เนื่องจาก : " + ex);
                }
            }
            else if (select_option == 3) //เพิ่มรายคน
            {
                try
                {

                    string proc_date = DwMain.GetItemDateTime(1, "calculate_date").ToString("dd/MM/yyyy");
                    string member_no = DwMain.GetItemString(1, "member_no");
                    String UpdateSQL = @"UPDATE  asnreqmaster 
                                            SET proc_option = 'RTY', proc_date = to_date('" + proc_date + @"', 'dd/mm/yyyy'), 
                                            moneytype_code = 'CSH', expense_bank = '', expense_branch = '', 
                                            expense_accid = '', to_system = '' 
                                            WHERE member_no = '"+member_no+"' and assisttype_code = '70'";
                                            
                    WebUtil.Query(UpdateSQL);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ทำรายการของหมายเลข "+member_no+" เสร็จเรียบร้อย ให้ไปที่หน้าจอ จ่ายกรณีลากออก/ให้ออก/เกษียณ เพื่อทำการประมวลผล");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ เนื่องจาก : " + ex);
                }
            }
            else if (select_option == 4) //ยกเลิกรายคน
            {
                try
                {
                    string member_no = DwMain.GetItemString(1, "member_no");
                    String UpdateSQL = @"UPDATE  asnreqmaster
                                            SET proc_option = 'NOL', 
                                            moneytype_code = 'CSH', expense_bank = '', expense_branch = '', 
                                            expense_accid = '', to_system = '' 
                                            WHERE member_no = '" + member_no + "' and assisttype_code = '70";
                    WebUtil.Query(UpdateSQL);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ยกเลิกการรอจ่ายของเลขสมาชิก "+member_no+" เรียบร้อยแล้ว");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ทำรายการไม่สำเร็จ เนื่องจาก : " + ex);
                }
            }
            else if (select_option == 0)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("มีความผิดพลาดในการเลือกหัวข้อทำรายการ");
            }
            

        }

    }
}