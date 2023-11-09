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
using CoreSavingLibrary.WcfNDeposit;
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using DataLibrary;
using System.Web.Services.Protocols;
using Saving.ConstantConfig;
//using CoreSavingLibrary.WcfCommon;


namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_pensionreq : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        private DwThDate tDwCheque;
        private n_depositClient depService;
        private n_commonClient cmdService;
        private bool completeCheque;
        private string ass_code;
       
        //string yDate;
        //POSTBACK

        protected String postMemberNo;
        protected String postChangeAssist;
        protected String postChangeAmt;
        protected Sta ta;
        protected Sdt dt;

        #region WebSheet Members

        public void InitJsPostBack()
        {
            postMemberNo = WebUtil.JsPostBack(this, "postMemberNo");
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("mbmembmaster_birth_date", "birth_tdate");
            tDwMain.Add("mbmembmaster_member_date", "member_tdate");
            //------------------------------------------------------------------
            //tDwCheque = new DwThDate(DwCheque, this);
            //tDwCheque.Add("cheque_date", "cheque_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            DwMain.Reset();
            DwMain.InsertRow(0);
            

            if (!IsPostBack)
            {
                tDwMain.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(DwMain);
            }
            tDwMain.Eng2ThaiAllRow();
            //tDwDetail.Eng2ThaiAllRow();

        }

        public void CheckJsPostBack(string eventArg)
        {

            if (eventArg == "postMemberNo")
            {
                JsPostMemberNo();
            }

            else if (eventArg == "postSaveNoCheckApv")
            {
                SaveSheet();
            }

        }

        public void SaveWebSheet()
        {

            //if (!completeCheque)
            //{
            //    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกเลขที่เช็คให้ครบถ้วน!");
            //    return;
            //}

            //String slipXml = DwMain.Describe("datawindow.data.xml");
            SaveSheet();    
        }

        public void WebSheetLoadEnd()
        {
            //try { DwUtil.RetrieveDDDW(DwSelect, "assisttype_code", "kt_50bath.pbl", null); }
            //catch { }
            DwMain.SaveDataCache();
        }

        #endregion

        private void NewClear()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            //DwDeptMonth.Reset();
            //DwDeptMonth.InsertRow(0);
            DwMain.SetItemDateTime(1, "deptopen_date", state.SsWorkDate);
            tDwMain.Eng2ThaiAllRow();
            //DwGain.Reset();
            //DwCheque.Reset();
        }

        private void SaveSheet()
        {
            string docPrefix = "";
            string lastDocno = "";
            string checkMember = "";
            ass_code = DwMain.GetItemString(1, "asnucfassisttype_assisttype_code");
            string capYear = DateTime.Now.Year.ToString();
            int Ytemp = Convert.ToInt32(capYear);
            capYear = (Ytemp + 543).ToString();
            try
            {
                if (checkMem.Value == "fail")
                    throw new Exception("ไม่พบหมายเลขสมาชิกนี้");
            }
            catch
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบหมายเลขสมาชิกนี้");
            }
            // Get ค่า Assist_docno ล่าสุด
            string member_no = DwMain.GetItemString(1, "mbmembmaster_member_no");
            try
            {
                string sqlassdocno = @"select last_docno,doc_prefix from asndoccontrol where assisttype_code = '" + ass_code + "' and doc_year = '" + capYear + "'";
                dt = ta.Query(sqlassdocno);
                dt.Next();
                lastDocno = dt.GetString("last_docno");
                docPrefix = dt.GetString("doc_prefix");
            }
            catch(Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            int temp = Convert.ToInt32(lastDocno);
            temp++;
            lastDocno = temp.ToString("000000");
            
            // ตรวจสอบว่ามีการสมัครสมาชิกซ้ำหรือไม่
            try
            {
                string sqlassdocno = @"select member_no from asnreqmaster where member_no = '" + member_no + "' and assisttype_code = '" + ass_code + "'";
                dt = ta.Query(sqlassdocno);
                dt.Next();
                checkMember = dt.GetString("member_no");
                if (checkMember != "")
                {
                    throw new Exception("หมายเลขสมาชิกนี้ได้สมัครกองทุนประเภทนี้ไปแล้ว ");
                }
                //เพิ่มข้อมูลสมาชิกลงในตาราง asnreqmaster
                try
                {
                    string sqlinsert = @"insert into asnreqmaster (assist_docno , capital_year , member_no , assisttype_code , req_date) values " +
                            "('" + docPrefix + lastDocno + "','" + capYear + "','" + member_no + "','" + ass_code + "', + to_date('" + state.SsWorkDate.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'))" ;
                    ta.Exe(sqlinsert);
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }

                //อัพเดทเลข last_docno ให้เป็นปัจจุบัน
                try
                {
                    string sqlUpdate = @"update asndoccontrol set last_docno = '" + lastDocno + "' where assisttype_code = '" + ass_code + "' and doc_year = '" + capYear + "'";
                    ta.Exe(sqlUpdate);
                    LtServerMessage.Text = WebUtil.CompleteMessage("การสมัครสมาชิกสำเร็จ");
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            
        }

        //JS-POSTBACK
        private void JsPostMemberNo()
        {
            checkMem.Value = "pass";
            string membNo = int.Parse(DwMain.GetItemString(1, "mbmembmaster_member_no")).ToString("00000000");
            DateTime birthdate , memberdate;
            string name, sname, personID, tel, mobileTel, member_no;
            try
            { 

                DwMain.SetItemString(1, "mbmembmaster_member_no", membNo);
                try
                {
                    string sqlassdocno = @"select member_no, memb_name, memb_surname, card_person, birth_date, member_date, mem_tel, mem_telmobile  from mbmembmaster where member_no = '"+ membNo + "'";
                    dt = ta.Query(sqlassdocno);
                    dt.Next();
                    member_no = dt.GetString("member_no");
                    if (member_no == "")
                    {
                        checkMem.Value = "fail";
                        throw new Exception("ไม่พบข้อมูลของหมายเลขสมาชิกนี้");
                    }
                    
                    name = dt.GetString("memb_name");
                    sname = dt.GetString("memb_surname");
                    personID = dt.GetString("card_person");
                    birthdate = dt.GetDate("birth_date");
                    memberdate = dt.GetDate("member_date");
                    tel = dt.GetString("mem_tel");
                    mobileTel = dt.GetString("mem_telmobile");
        
                    DwMain.SetItemDateTime(1, "mbmembmaster_birth_date", birthdate);
                    DwMain.SetItemDateTime(1, "mbmembmaster_member_date", memberdate);
                    DwMain.SetItemString(1, "mbmembmaster_memb_name", name);
                    DwMain.SetItemString(1, "mbmembmaster_memb_surname", sname);
                    DwMain.SetItemString(1, "mbmembmaster_card_person", personID);
                    DwMain.SetItemString(1, "mbmembmaster_mem_tel", tel);
                    DwMain.SetItemString(1, "mbmembmaster_mem_telmobile", mobileTel);
                    //lastDocno = dt.GetString("last_docno");
                    //docPrefix = dt.GetString("doc_prefix");
                }
                catch(Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }

            }
            catch (Exception)
            {
                DwMain.SetItemString(1, "member_no", "CIF");
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบเลขที่สมาชิก " + membNo);
            }

        }

    }
}