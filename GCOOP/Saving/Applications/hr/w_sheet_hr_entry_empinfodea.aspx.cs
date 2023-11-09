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
using DataLibrary;
using CoreSavingLibrary.WcfNCommon;

namespace Saving.Applications.hr
{
    public partial class w_sheet_hr_entry_empinfodea : PageWebSheet, WebSheet
    {
        private String emplid;
        protected DwThDate tDwMain;
        protected DwThDate tDwDetail;

        protected String postNewClear;
        protected String postGetMember;
        protected String postAddRow;
        protected String postShowDetail;
        protected String postDeleteRow;
        protected String postSearchGetMember;
        protected String postRefresh;
        //==================
        private void JspostSearchGetMember()
        {
            String emplcode = null;
            String emplid = null;

            emplcode = DwMain.GetItemString(1, "emplcode");

            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select emplid from hrnmlemplfilemas where emplcode = '" + emplcode + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    emplid = dt.GetString("emplid");
                    DwMain.SetItemString(1, "emplid", emplid);
                    JspostGetMember();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณาเพิ่มข้อมูลเจ้าหน้าที่รหัส :" + emplcode + " ที่หน้าจอข้อมูลเจ้าหน้าที่ก่อน");
                    JspostNewClear();
                }
            }
            catch (Exception ex)
            {
                String err = ex.ToString();
            }

            ta.Close();
        }

        public void SetDwMasterEnable(int protect)
        {
            try
            {
                if (protect == 1)
                {
                    DwMain.Enabled = false;
                }
                else
                {
                    DwMain.Enabled = true;
                }
                int RowAll = int.Parse(DwMain.Describe("Datawindow.Column.Count"));
                for (int li_index = 1; li_index <= RowAll; li_index++)
                {
                    DwMain.Modify("#" + li_index.ToString() + ".protect= " + protect.ToString());
                }
            }
            catch (Exception ex)
            {
                //  LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        protected String GetYear(String doc_code)
        {
            String yy = "";
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select document_year from cmshrlondoccontrol where Document_Code = '" + doc_code + "'";
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    yy = dt.GetString("document_year");
                }
                yy = WebUtil.Mid(yy, 2, 2);
            }
            catch (Exception ex)
            {
                String err = ex.ToString();
            }

            ta.Close();
            return yy;
        }

        protected String GetDocNo(String doc_code)
        {
            n_commonClient comm = wcf.NCommon;
            String maxdoc = comm.of_getnewdocno(state.SsWsPass,state.SsCoopId, doc_code);
            return maxdoc;
        }

        private void JspostDeleteRow()
        {
            int RowAll = DwDetail.RowCount;
            String seq_no = null;
            seq_no = DwDetail.GetItemString(RowAll, "seq_no");
            if (seq_no == "" || seq_no == "Auto")
            {
                DwDetail.DeleteRow(RowAll);
                //DwList.DeleteRow(RowAll);
            }
            else
            {
                Sta ta = new Sta(sqlca.ConnectionString);
                try
                {
                    emplid = DwMain.GetItemString(1, "emplid").Trim();
                    //String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();

                    String sql = @"Delete FROM hrnmlemplfamidea where seq_no = '" + seq_no + "'";
                    try
                    {
                        ta.Exe(sql);
                        LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");
                        JspostGetMember();
                    }
                    catch
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage("ไม่สามารถลบข้อมูลรายการ " + seq_no + " ได้");
                    }
                }

                catch (Exception ex)
                {
                    String err = ex.ToString();

                }
                ta.Close();
            }
            //Retrieve ...
            JspostGetMember();
        }

        private void JspostShowDetail()
        {
            String seq_no = null;
            String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid");
            }
            catch (Exception ex)
            {
                emplid = null;
            }

            seq_no = DwDetail.GetItemString(1, "seq_no");
            DwDetail.Reset();
            DwDetail.Retrieve(emplid);
            DwDetail.SetFilter("seq_no = '" + seq_no + "'");
            DwDetail.Filter();
            DwDetail.SetItemString(1, "seq_no", seq_no);
        }

        private void JspostAddRow()
        {
            String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid");
            }
            catch { emplid = null; }

            if (emplid == null || emplid == "Auto")
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบรหัสสมาชิก กรุณากรอกรหัสมาชิกก่อน");
                JspostNewClear();
            }
            else
            {
                //DwList.InsertRow(0);
                DwDetail.Reset();
                DwDetail.InsertRow(0);
            }
        }

        private void JspostGetMember()
        {
            String seq_no = null;
            //  String emplid = null;
            try
            {
                emplid = DwMain.GetItemString(1, "emplid");
            }
            catch (Exception ex)
            {
                emplid = null;
            }

            if (emplid == "" || emplid == null)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลรหัสสมาชิกรายนี้ กรุณาเลือกรายการใหม่");
                JspostNewClear();
            }
            else
            {
                DwMain.Reset();

                DwMain.Retrieve(emplid);
                DwMain.SetItemString(1, "entry_id", state.SsUsername);
                DwMain.SetItemDate(1, "entry_date", state.SsWorkDate);
                DwMain.SetItemString(1, "branch_id", state.SsCoopId);
                tDwMain.Eng2ThaiAllRow();

                DwDetail.Reset();
                DwDetail.InsertRow(0);
                DwUtil.RetrieveDataWindow(DwDetail, "hr_master.pbl", null, emplid);
                //DwUtil.RetrieveDataWindow(DwDetail, "hr_master.pbl", null, emplid);
                if (DwDetail.RowCount > 0)
                {
                    seq_no = DwDetail.GetItemString(1, "empid");
                    DwDetail.Reset();
                    DwDetail.InsertRow(0);
                    DwUtil.RetrieveDataWindow(DwDetail, "hr_master.pbl", null, emplid);
                    DwDetail.SetItemString(1, "empid", seq_no);
                }
                else
                {
                    JspostAddRow();
                }
            }
        }
        private void JspostRefresh()
        {
            //String chk = HSchk.Value;
            //String date, date2, date3, date4;
            //int day, day2, day3, day4, month, month2, month3, month4, year, year2, year3, year4;
            //DateTime dt, dt2, dt3, dt4;
            //if(chk=="1"){
            //date = DwDetail.GetItemString(1, "socisecu_tdate");
            //day = Convert.ToInt16(date.Substring(0, 2));
            //month = Convert.ToInt16(date.Substring(3, 2));

            //year = Convert.ToInt16(date.Substring(6, 4)) - 543;
            //dt = new DateTime(year, month, day);
            //DwDetail.SetItemDateTime(1, "socisecu_date", dt);
            //}
            //else if (chk == "2")
            //{
            //    date2 = DwDetail.GetItemString(1, "socisein_tdate");
            //    day2 = Convert.ToInt16(date2.Substring(0, 2));
            //    month2 = Convert.ToInt16(date2.Substring(3, 2));
            //    year2 = Convert.ToInt16(date2.Substring(6, 4)) - 543;
            //    dt2 = new DateTime(year2, month2, day2);
            //    DwDetail.SetItemDateTime(1, "socisein_date", dt2);
            //}

            //else if (chk == "3")
            //{
            //    date3 = DwDetail.GetItemString(1, "sociseout_tdate");
            //    day3 = Convert.ToInt16(date3.Substring(0, 2));
            //    month3 = Convert.ToInt16(date3.Substring(3, 2));
            //    year3 = Convert.ToInt16(date3.Substring(6, 4)) - 543;
            //    dt3 = new DateTime(year3, month3, day3);
            //    DwDetail.SetItemDateTime(1, "sociseout_date", dt3);
            //}
            //else if (chk == "4")
            //{
            //    date4 = DwDetail.GetItemString(1, "modisala_tdate");
            //    day4 = Convert.ToInt16(date4.Substring(0, 2));
            //    month4 = Convert.ToInt16(date4.Substring(3, 2));
            //    year4 = Convert.ToInt16(date4.Substring(6, 4)) - 543;
            //    dt4 = new DateTime(year4, month4, day4);
            //    DwDetail.SetItemDateTime(1, "modisala_date", dt4);
            //}
        }
        private void JspostNewClear()
        {
            DwMain.Reset();
            DwDetail.Reset();
            DwMain.InsertRow(0);
            DwDetail.InsertRow(0);

            DwMain.SetItemString(1, "entry_id", state.SsUsername);
            DwMain.SetItemString(1, "branch_id", state.SsCoopId);
            DwMain.SetItemDateTime(1, "entry_date", state.SsWorkDate);

            DwDetail.SetItemDateTime(1, "socisecu_date", state.SsWorkDate);
            DwDetail.SetItemDateTime(1, "socisein_date", state.SsWorkDate);
            DwDetail.SetItemDateTime(1, "sociseout_date", state.SsWorkDate);
            DwDetail.SetItemDateTime(1, "modisala_date", state.SsWorkDate);

            //DwList.Reset();
            //DwDetail.Reset();
        }

        //==================
        #region WebSheet Members

        public void InitJsPostBack()
        {
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            postGetMember = WebUtil.JsPostBack(this, "postGetMember");
            postAddRow = WebUtil.JsPostBack(this, "postAddRow");
            postShowDetail = WebUtil.JsPostBack(this, "postShowDetail");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postSearchGetMember = WebUtil.JsPostBack(this, "postSearchGetMember");

            //====================================
            tDwDetail = new DwThDate(DwDetail, this);
            tDwMain = new DwThDate(DwMain, this);

            tDwDetail.Add("socisecu_date", "socisecu_tdate");
            tDwDetail.Add("socisein_date", "socisein_tdate");
            tDwDetail.Add("sociseout_date", "sociseout_tdate");
            tDwDetail.Add("modisala_date", "modisala_tdate");

            tDwMain.Add("entry_date", "entry_tdate");

            //tDwMain.Eng2ThaiAllRow();
            //tDwDetail.Eng2ThaiAllRow();
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            DwMain.SetTransaction(sqlca);
            //DwList.SetTransaction(sqlca);
            DwDetail.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                JspostNewClear();
            }
            else
            {
                this.RestoreContextDw(DwMain);
                this.RestoreContextDw(DwDetail);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "postRefresh")
            {
                JspostRefresh();
            }
            else if (eventArg == "postGetMember")
            {
                JspostGetMember();
            }
            else if (eventArg == "postAddRow")
            {
                JspostAddRow();
            }
            else if (eventArg == "postShowDetail")
            {
                JspostShowDetail();
            }
            else if (eventArg == "postDeleteRow")
            {
                JspostDeleteRow();
            }
            else if (eventArg == "postSearchGetMember")
            {
                JspostSearchGetMember();
            }
        }

        public void SaveWebSheet()
        {
            //Check values before save record
            //check new record or edit row
            String sql;
            Sta ta = new Sta(sqlca.ConnectionString);

            String seq_new = null;
            String seq_no, paymeth, branch_id, calctaxway, socisecubefoyn, socisecucalcyn, secusocubefoplace, socisecuid, socisecuhosp, provfundcalcyn, provfundacc, rmffundamnt, bankacct, perimastid, bankname,
                bankid, empltaxid, emplmariname, ltffundamnt, emplmariyear, emplmaristat, emplmilistat, emplcontact, emplmariamph
                , emplcontactrel, emplmiliexcp, emplcontacttel;
            DateTime socisecu_date = new DateTime();
            DateTime socisein_date = new DateTime();
            DateTime sociseout_date = new DateTime();
            DateTime modisala_date = new DateTime();
            decimal socisecumove, socisepercent, emplhospital, compprovident, emplprovident, incomove, probfundmove, emplsala,
                taxmove, emplchil, insurover, kbk, teacherfun, payatstation, reladdamo;
            try
            {



                // String empl_new = null;

                String empl_tmp = DwMain.GetItemString(1, "emplid").Trim();
                //  String seq_no = DwDetail.GetItemString(1, "seq_no").Trim();

                if (empl_tmp == "Auto" || empl_tmp == null)
                {

                    LtServerMessage.Text = WebUtil.ErrorMessage("ยังไม่มีข้อมูลเจ้าหน้าที่ กรุณาเลือกเจ้าหน้าที่");
                }
                else
                {

                    try { paymeth = DwDetail.GetItemString(1, "paymeth").Trim(); }
                    catch { paymeth = ""; }
                    try { seq_no = DwDetail.GetItemString(1, "empid").Trim(); }
                    catch { seq_no = ""; }
                    try { calctaxway = DwDetail.GetItemString(1, "calctaxway").Trim(); }
                    catch { calctaxway = ""; }
                    try { socisecubefoyn = DwDetail.GetItemString(1, "socisecubefoyn").Trim(); }
                    catch { socisecubefoyn = ""; }
                    try { socisecucalcyn = DwDetail.GetItemString(1, "socisecucalcyn").Trim(); }
                    catch { socisecucalcyn = ""; }
                    try { secusocubefoplace = DwDetail.GetItemString(1, "secusocubefoplace").Trim(); }
                    catch { secusocubefoplace = ""; }
                    try { socisecuid = DwDetail.GetItemString(1, "socisecuid").Trim(); }
                    catch { socisecuid = ""; }
                    //if (DwList.RowCount > 0)
                    try { socisecuhosp = DwDetail.GetItemString(1, "socisecuhosp").Trim(); }
                    catch { socisecuhosp = ""; }
                    //if (DwList.RowCount > 0)
                    try { provfundcalcyn = DwDetail.GetItemString(1, "provfundcalcyn").Trim(); }
                    catch { provfundcalcyn = ""; }
                    //if (DwList.RowCount > 0)
                    try { provfundacc = DwDetail.GetItemString(1, "provfundacc").Trim(); }
                    catch { provfundacc = ""; }
                    //if (DwList.RowCount > 0)
                    try { rmffundamnt = DwDetail.GetItemString(1, "rmffundamnt").Trim(); }
                    catch { rmffundamnt = ""; }
                    //if (DwList.RowCount > 0)
                    try { bankacct = DwDetail.GetItemString(1, "bankacct").Trim(); }
                    catch { bankacct = ""; }
                    //if (DwList.RowCount > 0)
                    try { perimastid = DwDetail.GetItemString(1, "perimastid").Trim(); }
                    catch { perimastid = ""; }
                    //if (DwList.RowCount > 0)
                    try { bankid = DwDetail.GetItemString(1, "bankid").Trim(); }
                    catch { bankid = ""; }
                    //if (DwList.RowCount > 0)
                    try { branch_id = DwDetail.GetItemString(1, "branch_id").Trim(); }
                    catch { branch_id = ""; }
                    //if (DwList.RowCount > 0)
                    try { empltaxid = DwDetail.GetItemString(1, "empltaxid").Trim(); }
                    catch { empltaxid = ""; }
                    //if (DwList.RowCount > 0)
                    try { emplmariname = DwDetail.GetItemString(1, "emplmariname").Trim(); }
                    catch { emplmariname = ""; }
                    try { ltffundamnt = DwDetail.GetItemString(1, "ltffundamnt").Trim(); }
                    catch { ltffundamnt = ""; }
                    try { emplmariyear = DwDetail.GetItemString(1, "emplmariyear").Trim(); }
                    catch { emplmariyear = ""; }
                    try { emplmaristat = DwDetail.GetItemString(1, "emplmaristat").Trim(); }
                    catch { emplmaristat = ""; }
                    try { emplmilistat = DwDetail.GetItemString(1, "emplmilistat").Trim(); }
                    catch { emplmilistat = ""; }
                    try { emplcontact = DwDetail.GetItemString(1, "emplcontact").Trim(); }
                    catch { emplcontact = ""; }
                    try { emplmariamph = DwDetail.GetItemString(1, "emplmariamph").Trim(); }
                    catch { emplmariamph = ""; }
                    try { emplcontactrel = DwDetail.GetItemString(1, "emplcontactrel").Trim(); }
                    catch { emplcontactrel = ""; }
                    try { emplmiliexcp = DwDetail.GetItemString(1, "emplmiliexcp").Trim(); }
                    catch { emplmiliexcp = ""; }
                    try { emplcontacttel = DwDetail.GetItemString(1, "emplcontacttel").Trim(); }
                    catch { emplcontacttel = ""; }
                    try { socisecu_date = DwDetail.GetItemDateTime(1, "socisecu_date"); }
                    catch { socisecu_date = state.SsWorkDate; }
                    try { socisein_date = DwDetail.GetItemDateTime(1, "socisein_date"); }
                    catch { socisein_date = state.SsWorkDate; }
                    try { sociseout_date = DwDetail.GetItemDateTime(1, "sociseout_date"); }
                    catch { sociseout_date = state.SsWorkDate; }
                    try { modisala_date = DwDetail.GetItemDateTime(1, "modisala_date"); }
                    catch { modisala_date = state.SsWorkDate; }
                    try { socisecumove = DwDetail.GetItemDecimal(1, "socisecumove"); }
                    catch { socisecumove = 0; }
                    try { socisepercent = DwDetail.GetItemDecimal(1, "socisepercent"); }
                    catch { socisepercent = 0; }
                    try { emplhospital = DwDetail.GetItemDecimal(1, "emplhospital"); }
                    catch { emplhospital = 0; }
                    try { compprovident = DwDetail.GetItemDecimal(1, "compprovident"); }
                    catch { compprovident = 0; }
                    try { emplprovident = DwDetail.GetItemDecimal(1, "emplprovident"); }
                    catch { emplprovident = 0; }
                    try { incomove = DwDetail.GetItemDecimal(1, "incomove"); }
                    catch { incomove = 0; }
                    try { probfundmove = DwDetail.GetItemDecimal(1, "probfundmove"); }
                    catch { probfundmove = 0; }
                    try { emplsala = DwDetail.GetItemDecimal(1, "emplsala"); }
                    catch { emplsala = 0; }
                    try { taxmove = DwDetail.GetItemDecimal(1, "  taxmove"); }
                    catch { taxmove = 0; }
                    try { emplchil = DwDetail.GetItemDecimal(1, "emplchil"); }
                    catch { emplchil = 0; }
                    try { insurover = DwDetail.GetItemDecimal(1, "insurover"); }
                    catch { insurover = 0; }
                    try { kbk = DwDetail.GetItemDecimal(1, "kbk"); }
                    catch { kbk = 0; }
                    try { teacherfun = DwDetail.GetItemDecimal(1, "teacherfun"); }
                    catch { teacherfun = 0; }
                    try { payatstation = DwDetail.GetItemDecimal(1, "payatstation"); }
                    catch { payatstation = 0; }
                    try { reladdamo = DwDetail.GetItemDecimal(1, "reladdamo"); }
                    catch { reladdamo = 0; }
                    try
                    {
                        sql = @"insert into HRNMLEMPLINFODEA (EMPLID,
                    paymeth,                    calctaxway,                     socisecubefoyn,                 socisecucalcyn, 
                    secusocubefoplace,          socisecuid,                     socisecuhosp,                   provfundcalcyn, 
                    provfundacc,                rmffundamnt,                    bankacct,                       perimastid, 
                    branch_id,                   bankname, empltaxid, emplmariname, ltffundamnt, emplmariyear, emplmaristat, emplmilistat, emplcontact, emplmariamph
                    , emplcontactrel, emplmiliexcp, emplcontacttel,socisecudate,socisein_date,sociseout_date,modisaladate,socisecumove, socisepercent, emplhospital, compprovident, emplprovident, incomove, probfundmove, emplsala,
                    taxmove, emplchil, insurover, kbk, teacherfun, payatstation, reladdamo) 
                                    values('" + empl_tmp + @"',
                    '" + paymeth + "',           '" + calctaxway + "',        '" + socisecubefoyn + "',       '" + socisecucalcyn + @"', 
                    '" + secusocubefoplace + "', '" + socisecuid + "',        '" + socisecuhosp + "',         '" + provfundcalcyn + @"', 
                    '" + provfundacc + "',       '" + rmffundamnt + "',       '" + bankacct + "',             '" + perimastid + @"',
                    '" + branch_id + "',          '" + bankid + "',            '" + empltaxid + "',            '" + emplmariname + @"', 
                    '" + ltffundamnt + "',       '" + emplmariyear + "',      '" + emplmaristat + "',         '" + emplmilistat + @"',
                    '" + emplcontact + "',       '" + emplmariamph + "',      '" + emplcontactrel + "',       '" + emplmiliexcp + @"',
                    '" + emplcontacttel + "',    to_date('" + socisecu_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + socisein_date.ToString("dd/MM/yyyy", WebUtil.EN) + @"','dd/mm/yyyy'),
                    to_date( '" + sociseout_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),to_date('" + modisala_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),'" + socisecumove + @"', 
                    '" + socisepercent + "',     '" + emplhospital + "',        '" + compprovident + "',        '" + emplprovident + @"', 
                    '" + incomove + "',          '" + probfundmove + "',        '" + emplsala + "',             '" + taxmove + @"', 
                    '" + emplchil + "',          '" + insurover + "',           '" + kbk + "',                  '" + teacherfun + @"', 
                    '" + payatstation + "',      '" + reladdamo + "' )";

                        ta.Exe(sql);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเสร็จเรียบร้อยแล้ว");
                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex.ToString());
                        sql = @"update  HRNMLEMPLINFODEA set paymeth ='" + paymeth + "',calctaxway='" + calctaxway + "', socisecubefoyn='" + socisecubefoyn + "',                 socisecucalcyn='" + socisecucalcyn + @"', 
                    secusocubefoplace='" + secusocubefoplace + "',          socisecuid='" + socisecuid + "',                     socisecuhosp='" + socisecuhosp + "',                   provfundcalcyn='" + provfundcalcyn + @"', 
                    provfundacc='" + provfundacc + "',                rmffundamnt='" + rmffundamnt + "',                    bankacct='" + bankacct + "',                       perimastid='" + perimastid + @"', 
                    branch_id='" + branch_id + "',                   bankname='" + bankid + "', empltaxid='" + empltaxid + "', emplmariname='" + emplmariname + "', ltffundamnt='" + ltffundamnt + "', emplmariyear='" + emplmariyear + "', emplmaristat='" + emplmaristat + "', emplmilistat='" + emplmilistat + "', emplcontact='" + emplcontact + "', emplmariamph='" + emplmariamph + @"'
                    , emplcontactrel='" + emplcontactrel + "', emplmiliexcp='" + emplmiliexcp + "', emplcontacttel='" + emplcontacttel + "',socisecudate=to_date('" + socisecu_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),socisein_date=to_date('" + socisein_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),sociseout_date=to_date('" + sociseout_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),modisaladate=to_date('" + modisala_date.ToString("dd/MM/yyyy", WebUtil.EN) + "','dd/mm/yyyy'),socisecumove='" + socisecumove + "', socisepercent='" + socisepercent + "', emplhospital='" + emplhospital + "', compprovident='" + compprovident + "', emplprovident='" + emplprovident + "', incomove='" + incomove + "', probfundmove='" + probfundmove + "', emplsala='" + emplsala + @"',
                    taxmove='" + taxmove + "', emplchil='" + emplchil + "', insurover='" + insurover + "', kbk='" + kbk + "', teacherfun='" + teacherfun + "', payatstation='" + payatstation + "', reladdamo='" + reladdamo + @"' 
                        where EMPLID = '" + empl_tmp + "'";
                        ta.Exe(sql);
                        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการแก้ไขข้อมูลเสร็จเรียบร้อยแล้ว");
                    }

                    //if (DwList.RowCount > 0)
                    //if (DwList.RowCount > 0)
                    //{
                    //    if (seq_no == "Auto" || seq_no == null)
                    //    {
                    //        String famititl = null;
                    //        String famifirsname = null;
                    //        String famirela = null;
                    //        try
                    //        {
                    //            famititl = DwDetail.GetItemString(1, "famititl");
                    //        }
                    //        catch { famititl = null; }

                    //        try
                    //        {
                    //            famifirsname = DwDetail.GetItemString(1, "famifirsname");
                    //        }
                    //        catch { famifirsname = null; }

                    //        try
                    //        {
                    //            famirela = DwDetail.GetItemString(1, "famirela");
                    //        }
                    //        catch { famirela = null; }



                    //        if (famititl == null || famifirsname == null || famirela == null)
                    //        {
                    //            LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากรอกข้อมูลให้ครบ");
                    //        }
                    //        else
                    //        {

                    //            //new record
                    //            String seq_tmp = GetDocNo("HRFAMILY");
                    //            seq_tmp = WebUtil.Right(seq_tmp, 4);
                    //            seq_new = "FA" + GetYear("HREMPLFILEMAS") + seq_tmp;
                    //            DwDetail.SetItemString(1, "seq_no", seq_new);
                    //            DwDetail.SetItemString(1, "emplid", DwMain.GetItemString(1, "emplid"));
                    //            DwDetail.SetItemString(1, "entry_id", state.SsUsername);
                    //            DwDetail.SetItemDateTime(1, "entry_date", state.SsWorkDate);
                    //            DwDetail.SetItemString(1, "branch_id", state.SsCoopId);

                    //            DwDetail.UpdateData();
                    //            LtServerMessage.Text = WebUtil.CompleteMessage("บันทีกข้อมูลเสร็จเรียบร้อยแล้ว");
                    //            JspostGetMember();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        seq_new = seq_no; ;
                    //        DwDetail.UpdateData();
                    //        LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกการแก้ไขข้อมูลเสร็จเรียบร้อยแล้ว");
                    //        JspostGetMember();
                    //    }
                    //}
                    //else
                    //{
                    //    LtServerMessage.Text = WebUtil.ErrorMessage("กรุณากดปุ่ม เพิ่มแถว");
                    //}
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        public void WebSheetLoadEnd()
        {
            if (DwMain.RowCount > 1)
            {
                DwMain.DeleteRow(DwMain.RowCount);
            }
            else
            {
                SetDwMasterEnable(0);
            }

            DwMain.SaveDataCache();
            DwDetail.SaveDataCache();
            tDwMain.Eng2ThaiAllRow();
            tDwDetail.Eng2ThaiAllRow();
        }
        #endregion
    }
}
