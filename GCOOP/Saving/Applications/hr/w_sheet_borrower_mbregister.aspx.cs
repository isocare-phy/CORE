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
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using CoreSavingLibrary.WcfNShrlon;
using DataLibrary;

namespace Saving.Applications.hr
{
    public partial class w_sheet_borrower_mbregister : PageWebSheet, WebSheet
    {

        protected DwThDate tDwMain;
        protected String postAddDistance;
        protected String postEditDistance;
        protected String postSaveEdit;
        protected String postRefresh;
        protected String postDelete;
        protected String postShowItemMain;
        protected String postSelectItemMain;
       
        //protected DwThDate tDwMain;

        public void InitJsPostBack()
        {


            postAddDistance = WebUtil.JsPostBack(this, "postAddDistance");
            postEditDistance = WebUtil.JsPostBack(this, "postEditDistance");
            postSaveEdit = WebUtil.JsPostBack(this, "postSaveEdit");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postDelete = WebUtil.JsPostBack(this, "postDelete");
            postShowItemMain = WebUtil.JsPostBack(this, "postShowItemMain");
            postSelectItemMain = WebUtil.JsPostBack(this, "postSelectItemMain");

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("reqdate", "treqdate");
            tDwMain.Add("endate", "tendate");
            tDwMain.Add("entry_date", "entry_tdate"); 
        }

        public void WebSheetLoadBegin()
        {

            this.ConnectSQLCA();
           
            LtAlert.Text = "";
            DwMain.SetTransaction(sqlca);
            DwListselect.SetTransaction(sqlca);
            DwListMemnew.SetTransaction(sqlca);


            if (!IsPostBack)
            {


             DwMain.InsertRow(0);
             DwListselect.InsertRow(0);


            }
            else
            {
                try
                {
                    this.RestoreContextDw(DwMain);

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                }
            }

            tDwMain.Eng2ThaiAllRow();


            DwListMemnew.Retrieve("1");
            DwUtil.RetrieveDDDW(DwListMemnew, "mb_status", "borrower_member.pbl", null); 
  
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postAddDistance")
            {
                AddDistance();
            }
            else if (eventArg == "postEditDistance")
            {
                EditDistance();
            }
            else if (eventArg == "postSaveEdit")
            {
                SaveEdit();
            }

            else if (eventArg == "postShowItemMain")
            {
                ShowItem();
            }
            else if (eventArg == "postSelectItemMain")
            {
                SelectItem();
            }
            else if (eventArg == "postDelete")
            {
                DeleteItem();
            }
            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
 
        }

        private void SelectItem()
        {
            tDwMain.Eng2ThaiAllRow();
            String borrower_borrowerstatus;
            borrower_borrowerstatus = DwListselect.GetItemString(1, "borrower_borrowerstatus"); 
            object[] arge = new object[1];
            arge[0] = borrower_borrowerstatus;
            
            //DwListMemnew.Retrieve("borrower_borrowerstatus");
            DwUtil.RetrieveDataWindow(DwListMemnew, "borrower_member.pbl", tDwMain, arge);
        }

        //private void RetrieveDwMain()
        //{
        //    String bor_membernumber;

        //    bor_membernumber = HfMemberNo.Value;

        //    object[] args = new object[1];
        //    args[0] = bor_membernumber;

        //    DwMain.Reset();
        //    DwUtil.RetrieveDataWindow(DwMain, "borrower_member.pbl", null, args);
        //    if (DwMain.RowCount < 1)
        //    {
        //        LtAlert.Text = "<script>Alert()</script>";
        //        return;
        //    }

        //} 

        public void SaveWebSheet()
        {
            try
            {
                tDwMain.Eng2ThaiAllRow();

                String sqlStr, seq_borrower, borrower_membername, borrower_memberjurisdiction, borrower_borrowerstatus,
                       borrower_borrwoersein, borrower_borrwereceive, chkstw, chkstg, chkstb, chkstsd, chkstnd, chkstsh,bor_membernumber;

                String treqdate, entry_id, entry_tdate, reqdate, tendate; 

                Decimal borrower_membernumber;

                try
                {
                    seq_borrower = DwMain.GetItemString(1, "seq_borrower");
                }
                catch { seq_borrower = " "; }

                borrower_membername = DwMain.GetItemString(1, "borrower_membername");

                try
                {
                    borrower_memberjurisdiction = DwMain.GetItemString(1, "borrower_memberjurisdiction");
                }
                catch (Exception ex)
                { borrower_memberjurisdiction = "  "; }

                try
                {
                    borrower_borrowerstatus = DwMain.GetItemString(1, "borrower_borrowerstatus");
                }
                catch (Exception ex)
                {
                    borrower_borrowerstatus = " ";
                }

                try
                {
                    borrower_borrwoersein = DwMain.GetItemString(1, "borrower_borrwoersein");
                }
                catch (Exception ex)
                {
                    borrower_borrwoersein = " ";
                }
                try
                {
                    chkstw = DwMain.GetItemString(1, "chkstw");
                }
                catch (Exception ex)
                {
                    chkstw = " ";
                }
                try
                {
                    chkstg = DwMain.GetItemString(1, "chkstg");
                }
                catch (Exception ex)
                {
                    chkstg = " ";
                }
                try
                {
                    chkstb = DwMain.GetItemString(1, "chkstb");
                }
                catch (Exception ex)
                {
                    chkstb = " ";
                }
                try
                {
                    chkstsd = DwMain.GetItemString(1, "chkstsd");
                }
                catch (Exception ex)
                {
                    chkstsd = " ";
                }
                try
                {
                    chkstnd = DwMain.GetItemString(1, "chkstnd");
                }
                catch (Exception ex)
                {
                    chkstnd = " ";
                }
                try
                {
                    chkstsh = DwMain.GetItemString(1, "chkstsh");
                }
                catch (Exception ex)
                {
                    chkstsh = " ";
                }

                try
                {
                    borrower_borrwereceive = DwMain.GetItemString(1, "borrower_borrwereceive"); //type เป็น date
                }
                catch (Exception ex) { borrower_borrwereceive = " "; }
                try
                {
                    treqdate = DwMain.GetItemString(1, "treqdate"); //type เป็น date
                }
                catch (Exception ex) { treqdate = "01012011"; }

                try
                {
                    entry_tdate = DwMain.GetItemString(1, "entry_tdate"); //type เป็น date
                }
                catch (Exception ex) { entry_tdate = "01012011"; }

                try
                {
                    reqdate = DwMain.GetItemString(1, "reqdate"); //type เป็น date
                }
                catch (Exception ex) { reqdate = "01012011"; }

                try
                {
                    entry_id = DwMain.GetItemString(1, "entry_id");
                }
                catch (Exception ex)
                {
                    entry_id = "1";
                }
                try
                {
                    tendate = DwMain.GetItemString(1, "tendate");
                }
                catch (Exception ex)
                {
                    tendate = "01012011";
                }

                try
                {
                    borrower_membernumber = DwMain.GetItemDecimal(1, "borrower_membernumber"); // 
                }
                catch (Exception ex)
                {
                    borrower_membernumber = 0;
                }
                try
                {
                    bor_membernumber = DwMain.GetItemString(1, "bor_membernumber"); // 
                }
                catch (Exception ex)
                {
                    bor_membernumber = "000000";
                }


                Sta ta = new Sta(sqlca.ConnectionString);
                Sdt dt = new Sdt(); 

                sqlStr = @"insert into borrower_member
(seq_borrower , borrower_membername , borrower_memberjurisdiction ,
borrower_borrowerstatus , borrower_borrwoersein , borrower_borrwereceive ,
chkstw , chkstg , chkstb , chkstsd , chkstnd , chkstsh , entry_id , entry_date , reqdate , endate , borrower_membernumber,bor_membernumber)

values('" + seq_borrower + "' , '" + borrower_membername + "' , '" + borrower_memberjurisdiction + @"',
'" + borrower_borrowerstatus + "','" + borrower_borrwoersein + "' , '" + borrower_borrwereceive + "' , '" + chkstw + "' , '" + chkstg + "' , '" + chkstb + "' , '" + chkstsd + "' , '" + chkstnd + "' , '" + chkstsh + "' , '" + entry_id + @"',
to_date( '" + entry_tdate + "','ddMMyyyy'),to_date('" + treqdate + "','ddMMyyyy' ),to_date( '" + tendate + "','ddMMyyyy' ),'" + borrower_membernumber + "','" + bor_membernumber + "') ";


                ta.Exe(sqlStr);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            DwMain.SetTransaction(sqlca);
            DwMain.UpdateData();

            try
            {
                DwUtil.UpdateDataWindow(DwMain, "borrower_member.pbl", "borrower_member");


                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            DwListMemnew.UpdateData();

        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();
            DwListselect.SaveDataCache();
            //DwMain.Reset();
            // DwListMemnew.Reset();
            //DwMain.InsertRow(1);
            // DwListMemnew.InsertRow(1);
        }


         
        private void EditDistance()
        {



        }
        private void DeleteItem()
        {


            Decimal member_mbnumber;

            member_mbnumber = DwListMemnew.GetItemDecimal(1, "member_mbnumber");



            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();


            tDwMain.Eng2ThaiAllRow();

            String sqlStr, seq_membernew, member_name, member_jurisdiction,
                   member_application;

            Decimal member_paidmonth, member_feemonth, member_zone, member_age;


            //sqlStr = @"select * from mbreqapp_bf     ";   

            try
            {
                seq_membernew = DwListMemnew.GetItemString(1, "seq_membernew");
            }
            catch (Exception ex) { seq_membernew = ""; }


            try
            {
                member_name = DwListMemnew.GetItemString(1, "member_name");
            }
            catch (Exception ex) { member_name = ""; }

            try
            {
                member_jurisdiction = DwListMemnew.GetItemString(1, "member_jurisdiction");
            }
            catch (Exception ex) { member_jurisdiction = ""; }

            try
            {
                member_mbnumber = DwListMemnew.GetItemDecimal(1, "member_mbnumber");
            }
            catch (Exception ex) { member_mbnumber = 0; }
            try
            {
                member_paidmonth = DwListMemnew.GetItemDecimal(1, "member_paidmonth");
            }
            catch (Exception ex) { member_paidmonth = 0; }
            try
            {
                member_feemonth = DwListMemnew.GetItemDecimal(1, "member_feemonth");
            }
            catch (Exception ex) { member_feemonth = 0; }
            try
            {
                member_zone = DwListMemnew.GetItemDecimal(1, "member_zone");
            }
            catch (Exception ex) { member_zone = 0; }
            try
            {
                member_age = DwListMemnew.GetItemDecimal(1, "member_age");
            }
            catch (Exception ex) { member_age = 0; }


            try
            {

                //DELETE FROM TABLE_NAME WHERE some_column=some_value
                sqlStr = @"DELETE FROM mbreqapp_bf WHERE member_mbnumber = '" + member_mbnumber + "'   ";

                ta.Exe(sqlStr);

                LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

        }
        private void Refresh()
        {

        }

        private void ShowItem()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();

            String bor_membernumber, mb_membernumber, borrower_borrowerstatus; 
            
            try
            {
                bor_membernumber = DwListMemnew.GetItemString(1, "bor_membernumber");
            }
            catch (Exception ex) { bor_membernumber = ""; }
            try
            {
                mb_membernumber = DwListMemnew.GetItemString(1, "mb_membernumber");
            }
            catch (Exception ex) { mb_membernumber = ""; }
            try
            {
                borrower_borrowerstatus = DwListMemnew.GetItemString(1, "borrower_borrowerstatus");
            }
            catch (Exception ex) { borrower_borrowerstatus = ""; }
            try
            {
                String sqlstr = @" Select * From borrower_member Where  bor_membernumber = '" + bor_membernumber + "' and borrower_borrowerstatus = '" + borrower_borrowerstatus + "'  ";

                ta.Exe(sqlstr);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            bor_membernumber = DwListMemnew.GetItemString(1, "bor_membernumber");
            borrower_borrowerstatus = DwListMemnew.GetItemString(1, "borrower_borrowerstatus");
            object[] arge = new object[2];
            arge[0] = bor_membernumber;
            arge[1] = borrower_borrowerstatus; 

            DwMain.Reset();
          
            DwUtil.RetrieveDataWindow(DwMain, "borrower_member.pbl", tDwMain, arge);
            tDwMain.Eng2ThaiAllRow();
            DwMain.SetItemString(1, "bor_membernumber", bor_membernumber);
            DwMain.SetItemString(1, "borrower_borrowerstatus", borrower_borrowerstatus);  
            //DwMain.SetFilter("member_mbnumber = '" + mb_membernumber + "'");
           
            //DwMain.InsertRow(0);
        }
        private void SaveEdit()
        {

        }
        private void AddDistance()
        {

        }

    }
}