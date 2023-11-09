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
    public partial class w_sheet_kt_viewlog : PageWebSheet, WebSheet
    {
        private DwThDate tDwMain;
        //private bool completeCheque;
        private string ass_code;
        private String pblFileName = "kt_pension.pbl";
        private DwThDate thDwSlip;

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
            thDwSlip = new DwThDate(DwSlip, this);

            //------------------------------------------------------------------
            //tDwCheque = new DwThDate(DwCheque, this);
            //tDwCheque.Add("cheque_date", "cheque_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            ta = new Sta(sqlca.ConnectionString);
            dt = new Sdt();

            if (!IsPostBack)
            {
                tDwMain.Eng2ThaiAllRow();
                DwMain.Reset();
                DwMain.InsertRow(0);
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwSlip);
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

        }

        public void SaveWebSheet()
        {
           
        }

        public void WebSheetLoadEnd()
        {
            DwSlip.PageNavigationBarSettings.Visible = (DwSlip.RowCount > 10);
            DwMain.SaveDataCache();
            DwSlip.SaveDataCache();

            ta.Close();
        }

        #endregion

        private void NewClear()
        {

        }


        //JS-POSTBACK
        private void JsPostMemberNo()
        {

            string membNo = int.Parse(DwMain.GetItemString(1, "mbmembmaster_member_no")).ToString("00000000");
            ass_code = DwMain.GetItemString(1, "asnucfassisttype_assisttype_code");
            DateTime birthdate, memberdate;
            string name, sname, personID, tel, mobileTel, member_no;
            object[] argsDwMain = new object[2] { membNo, ass_code };
            try
            {

                DwMain.SetItemString(1, "mbmembmaster_member_no", membNo);
                try
                {
                    string sqlassdocno = @"select member_no, memb_name, memb_surname, card_person, birth_date, member_date, mem_tel, mem_telmobile  from mbmembmaster where member_no = '" + membNo + "'";
                    dt = ta.Query(sqlassdocno);
                    dt.Next();
                    member_no = dt.GetString("member_no");
                    if (member_no == "")
                    {
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
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }

            }
            catch (Exception)
            {
                //DwMain.SetItemString(1, "member_no", "CIF");
                LtServerMessage.Text = WebUtil.WarningMessage("ไม่พบเลขที่สมาชิก " + membNo);
            }

            try
            {
                DwSlip.Reset();
                DwUtil.RetrieveDataWindow(DwSlip, pblFileName, null , argsDwMain);
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }

    }
}