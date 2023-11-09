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
    public partial class w_sheet_hr_member_proposedregister : PageWebSheet, WebSheet
    {
        
        protected DwThDate tDwMain;
        protected String postAddDistance;
        protected String postEditDistance;
        protected String postSaveEdit;
        protected String postRefresh;
        protected String postDelete;
        protected String postShowItemMain;
        protected String postSearchItemMain; 
        protected String postDeleteRow;

        //protected DwThDate tDwMain;

        public void InitJsPostBack()
        {


            postAddDistance = WebUtil.JsPostBack(this, "postAddDistance");
            postEditDistance = WebUtil.JsPostBack(this, "postEditDistance");
            postSaveEdit = WebUtil.JsPostBack(this, "postSaveEdit");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
            postDelete = WebUtil.JsPostBack(this, "postDelete");
            postShowItemMain = WebUtil.JsPostBack(this, "postShowItemMain");
            postDeleteRow = WebUtil.JsPostBack(this, "postDeleteRow");
            postSearchItemMain = WebUtil.JsPostBack(this, "postSearchItemMain");
            

            tDwMain = new DwThDate(DwMain, this);
            tDwMain.Add("member_reqdate", "member_treqdate");
            tDwMain.Add("member_application", "member_tapplication");
            tDwMain.Add("entry_date", "entry_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();

            DwMain.SetTransaction(sqlca);
            //DwListMemnew.SetTransaction(sqlca);


            if (!IsPostBack)
            {

                DwMain.InsertRow(0);

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
            else if (eventArg == "postSearchItemMain")
            {
                SearchItemMain();
            }
            else if (eventArg == "postDeleteRow")
            {
                DeleteRow();
            }

            else if (eventArg == "postRefresh")
            {
                Refresh();
            }

        }

        private void SearchItemMain() 
        {
            String seq_empid = null;
            

            seq_empid = DwMain.GetItemString(1, "seq_empid");

            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                String sql = @"select  seq_empid from  mbreqappmember_bf
                               where  
                               seq_empid = seq_empid   and seq_empid = '" + seq_empid + "'  "; 
                Sdt dt = ta.Query(sql);
                if (dt.Next())
                {
                    seq_empid = dt.GetString("seq_empid");
                    DwMain.SetItemString(1, "seq_empid", seq_empid);  
                    ShowItem();
                }
                else
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูลเจ้าหน้าที่รายนี้ กรุณากรอกข้อมูลใหม่");
                   //JspostNewClear();
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                //String err = ex.ToString();
            }

            ta.Close();
        }
         
        private void DeleteRow() 
        {
            String seq_empid;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {

                seq_empid = HdfEmplid.Value.Trim();//DwMain.GetItemString(1, "emplid").Trim();
                //String emplcode = DwMain.GetItemString(1, "emplcode").Trim();
                String sql = @"Delete FROM mbreqappmember_bf where seq_empid = '" + seq_empid + "'";
                try
                { 
                    ta.Exe(sql);
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลเรียบร้อยแล้ว");
                    //JspostNewClear(); เอาไว้ Reset ค่าใหม่
                    //JspostGetMember();
                    HdfEmplid.Value = "";
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }

            ta.Close();
            DwMain.Reset();
            DwMain.InsertRow(0);

        }
         
        public void SaveWebSheet()
        {
            try
            {
                tDwMain.Eng2ThaiAllRow();

                    String sqlStr,seq_empid,  member_name,
                           member_firstname = "null",
                           member_lastname = "null",
                           member_jurisdiction, entry_id, 
                           member_mbnumber,member_zone,
                           member_empcard,member_reqeat,
                           seq_membernew ;

                    String member_tapplication, member_treqdate, entry_tdate; 

                    Decimal member_paidmonth,
                            member_feemonth, member_citycard,
                            member_age;

                    //PK คือ seq_empid 
                    //ที่มีค่าเป็ฯ Date (member_repeat , entry_date , member_application)

                    seq_empid = DwMain.GetItemString(1, "seq_empid");
                    try
                    {
                        seq_empid = DwMain.GetItemString(1, "seq_empid"); 
                    }
                    catch (Exception ex)
                    {
                        seq_empid = ""; 
                    }
                    member_name = DwMain.GetItemString(1, "member_name");
                    try
                    {
                        member_firstname = DwMain.GetItemString(1, "member_firstname");
                    }
                    catch (Exception ex)
                    {
                        member_firstname = "1";
                    }

                    try
                    {
                        member_lastname = DwMain.GetItemString(1, "member_lastname");
                    }
                    catch (Exception ex)
                    {
                        member_lastname = "1";
                    }

                    try
                    {
                        member_jurisdiction = DwMain.GetItemString(1, "member_jurisdiction");
                    }
                    catch (Exception ex)
                    {
                        member_jurisdiction = "00000";
                    }

                    try
                    {
                        member_zone = DwMain.GetItemString(1, "member_zone"); // อาจถึงเขต
                    }
                    catch (Exception ex)
                    {
                        member_zone = "0";
                    }

                    try
                    {
                        member_age = DwMain.GetItemDecimal(1, "member_age");
                    }
                    catch (Exception ex)
                    {
                        member_age = 0;
                    }
                    try
                    {
                        member_citycard = DwMain.GetItemDecimal(1, "member_citycard");
                    }
                    catch (Exception ex)
                    {
                        member_citycard = 0;
                    }

                    try
                    {
                        member_empcard = DwMain.GetItemString(1, "member_empcard");
                    }
                    catch (Exception ex)
                    {
                        member_empcard = "0000000000000";
                    }
                    try
                    {
                        member_reqeat = DwMain.GetItemString(1, "member_reqeat");
                    }
                    catch (Exception ex)
                    {
                        member_reqeat = "1";
                    }
                    try
                    {
                        member_paidmonth = DwMain.GetItemDecimal(1, "member_paidmonth");
                    }
                    catch (Exception ex)
                    {
                        member_paidmonth = 0;
                    }
                    try
                    {
                        member_feemonth = DwMain.GetItemDecimal(1, "member_feemonth");
                    }
                    catch (Exception ex)
                    {
                        member_feemonth = 0;
                    }
                    try
                    {
                        member_treqdate = DwMain.GetItemString(1, "member_treqdate"); //type เป็น date
                    }
                    catch (Exception ex) { member_treqdate = "01012011"; } 

                    try
                    {
                        member_tapplication = DwMain.GetItemString(1, "member_tapplication"); //type เป็น date
                    }
                    catch (Exception ex) { member_tapplication = "01012011"; }

                    try
                    {
                        entry_tdate = DwMain.GetItemString(1, "entry_tdate"); //type เป็น date
                    }
                    catch (Exception ex) { entry_tdate = "01012011"; }



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
                        member_mbnumber = DwMain.GetItemString(1, "member_mbnumber");
                    }
                    catch (Exception ex)
                    {
                        member_mbnumber = "";
                    }
                   
                    try
                    {
                        seq_membernew = DwMain.GetItemString(1, "seq_membernew");
                    }
                    catch (Exception ex)
                    {
                        seq_membernew = "BF00000";
                    }

                    Sta ta = new Sta(sqlca.ConnectionString);
                    Sdt dt = new Sdt();

                    sqlStr = @"insert into mbreqappmember_bf(seq_membernew,seq_empid,member_name,member_firstname,member_lastname,
                           member_jurisdiction,member_zone,member_age,member_citycard,member_empcard,member_reqeat,
                           member_paidmonth,member_feemonth,member_reqdate,member_application,entry_date,entry_id,member_mbnumber)

values('"+seq_membernew+"','"+seq_empid+"','" + member_name + "','" + member_firstname + "','" + member_lastname + @"',
        '" + member_jurisdiction + "','" + member_zone + "','" + member_age + "','" + member_citycard + @"',
        '" + member_empcard + "','" + member_reqeat + "','" + member_paidmonth + "','" + member_feemonth + @"',
       to_date( '" + member_treqdate + "','ddMMyyyy'),to_date('" + member_tapplication + "','ddMMyyyy' ),to_date( '" + entry_tdate + "','ddMMyyyy' ),'" + entry_id + @"',
        '" + member_mbnumber + "') ";


                    ta.Exe(sqlStr);

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
                }
            
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            DwMain.SetTransaction(sqlca);
            DwMain.UpdateData();
            DwMain.Reset();
            DwMain.InsertRow(0);
            try
            {
               // DwUtil.UpdateDataWindow(DwMain,"hr_member_reqproposedregister.pbl", "mbreqappmember_bf");
                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกเรียบร้อย");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }



        }

        public void WebSheetLoadEnd()
        {
            tDwMain.Eng2ThaiAllRow();
            DwMain.SaveDataCache();

        }


        private void EditDistance()
        {



        }
        
        private void Refresh()
        {
            DwMain.Reset();
            DwMain.InsertRow(0);
            DwMain.Retrieve();

        }

        private void ShowItem()
        {
            Sta ta = new Sta(sqlca.ConnectionString);
            Sdt dt = new Sdt();
            String as_seq_empid, seq_empid;
            try
            {
                seq_empid = DwMain.GetItemString(1, "seq_empid");
            }
            catch (Exception ex) { seq_empid = ""; }
            try
            {
                as_seq_empid = DwMain.GetItemString(1, "as_seq_empid");
            }
            catch (Exception ex) { as_seq_empid = ""; }
            try
            {
                String sqlstr = @" Select * From mbreqappmember_bf Where  seq_empid = '" + seq_empid + "'";

                ta.Exe(sqlstr);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }

            seq_empid = DwMain.GetItemString(1, "seq_empid");
            try
            { DwMain.Retrieve(seq_empid); }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); } 
            tDwMain.Eng2ThaiAllRow();
            DwMain.SetItemString(1, "seq_empid", seq_empid);

        }

        private void SaveEdit()
        {

        }
        private void AddDistance()
        {

        }



    }
}