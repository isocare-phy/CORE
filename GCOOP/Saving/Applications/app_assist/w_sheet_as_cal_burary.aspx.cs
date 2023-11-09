using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Sybase.DataWindow;
using DataLibrary;
using System.Web.Services.Protocols;
//WAS0000010
namespace Saving.Applications.app_assist
{
    public partial class w_sheet_as_cal_burary : PageWebSheet, WebSheet
    {
        protected string postCalEncourage;
        protected string postCalContribute;
        protected string postCalOutStanding;
        protected string postUpdateVarOutStanding;
        protected string postUpdateVarEncourage;
        protected string postUpdateVarContribute;
        protected string postTranOutStanding;
        protected string postTranEncourage;
        protected string postTranContribute;
        protected string postCancelTranEncourage;
        protected string postCancelTranContribute;
        private DwThDate tdw_contribute, tdw_encourage, tdw_outstanding;

        public void InitJsPostBack()
        {
            postCalOutStanding = WebUtil.JsPostBack(this, "postCalOutStanding");
            postCalEncourage = WebUtil.JsPostBack(this, "postCalEncourage");
            postCalContribute = WebUtil.JsPostBack(this, "postCalContribute");
            postUpdateVarOutStanding = WebUtil.JsPostBack(this, "postUpdateVarOutStanding");
            postUpdateVarEncourage = WebUtil.JsPostBack(this, "postUpdateVarEncourage");
            postUpdateVarContribute = WebUtil.JsPostBack(this, "postUpdateVarContribute");
            postTranOutStanding = WebUtil.JsPostBack(this, "postTranOutStanding");
            postTranEncourage = WebUtil.JsPostBack(this, "postTranEncourage");
            postTranContribute = WebUtil.JsPostBack(this, "postTranContribute");
            postCancelTranEncourage = WebUtil.JsPostBack(this, "postCancelTranEncourage");
            postCancelTranContribute = WebUtil.JsPostBack(this, "postCancelTranContribute");



            tdw_contribute = new DwThDate(dw_contribute, this);
            tdw_contribute.Add("apv_date", "apv_tdate");

            tdw_encourage = new DwThDate(dw_encourage, this);
            tdw_encourage.Add("apv_date", "apv_tdate");

            tdw_outstanding = new DwThDate(dw_outstanding, this);
            tdw_outstanding.Add("apv_date", "apv_tdate");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            if (!IsPostBack)
            {
                dw_outstanding.InsertRow(0);
                dw_encourage.InsertRow(0);
                dw_contribute.InsertRow(0);
                GetConstant();
                dw_contribute.SetItemDateTime(1, "apv_date", DateTime.Today.Date);
                dw_contribute.SetItemString(1, "apv_tdate", "");
                dw_encourage.SetItemDateTime(1, "apv_date", DateTime.Today.Date);
                dw_encourage.SetItemString(1, "apv_tdate", "");
                dw_outstanding.SetItemDateTime(1, "apv_date", DateTime.Today.Date);
                dw_outstanding.SetItemString(1, "apv_tdate", "");

                tdw_contribute.Eng2ThaiAllRow();
                tdw_encourage.Eng2ThaiAllRow();
                tdw_outstanding.Eng2ThaiAllRow();
            }
            else
            {
                this.RestoreContextDw(dw_outstanding);
                this.RestoreContextDw(dw_contribute);
                this.RestoreContextDw(dw_encourage);
            }
            DwUtil.RetrieveDDDW(dw_outstanding, "capital_year", "as_public_funds.pbl", null);
            DwUtil.RetrieveDDDW(dw_encourage, "capital_year", "as_public_funds.pbl", null);
            DwUtil.RetrieveDDDW(dw_contribute, "capital_year", "as_public_funds.pbl", null);
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postCalContribute")
            {
                CalContribute();
            }
            else if (eventArg == "postCalEncourage")
            {
                CalEncourage();
            }
            else if (eventArg == "postCalOutStanding")
            {
                CalOutStanding();
            }
            else if (eventArg == "postUpdateVarOutStanding")
            {
                UpdateVarOutStanding();
            }
            else if (eventArg == "postUpdateVarEncourage")
            {
                UpdateVarEncourage();
            }
            else if (eventArg == "postUpdateVarContribute")
            {
                UpdateVarContribute();
            }
            else if (eventArg == "postTranOutStanding")
            {
                TranOutStanding();
            }
            else if (eventArg == "postTranEncourage")
            {
                TranEncourage();
            }
            else if (eventArg == "postTranContribute")
            {
                TranContribute();
            }
            else if (eventArg == "postCancelTranEncourage")
            {
                CancelTranEncourage();
            }
            else if (eventArg == "postCancelTranContribute")
            {
                CancelTranContribute();
            }
        }
        public void SaveWebSheet()
        {

        }
        public void WebSheetLoadEnd()
        {
            dw_outstanding.SaveDataCache();
            dw_encourage.SaveDataCache();
            dw_contribute.SaveDataCache();
        }
        private void CalOutStanding()
        {
            try
            {
                Sdt dt;
                Sta ta = new Sta(sqlca.ConnectionString);
                string sql;
                decimal outstandingperson_lv2 = 0, outstandingperson_lv3 = 0, outstandingperson_lv5 = 0;
                decimal capital_year = dw_outstanding.GetItemDecimal(1, "capital_year");
                int peoplelv2 = 0, peoplelv3 = 0, peoplelv5 = 0;
                //จำนวนเงินระดับม.1
                sql = sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_money_lv2'";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    outstandingperson_lv2 = Convert.ToDecimal(dt.Rows[0]["envvalue"].ToString());
                }
                //จำนวนเงินระดับม.4
                sql = sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_money_lv3'";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    outstandingperson_lv3 = Convert.ToDecimal(dt.Rows[0]["envvalue"].ToString());
                }
                //จำนวนเงินระดับม.5
                sql = sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_money_lv5'";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    outstandingperson_lv5 = Convert.ToDecimal(dt.Rows[0]["envvalue"].ToString());
                }
                //update ม.1
                sql = "select a.assist_docno from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and b.scholarship_type = 2 and a.req_status not in (-11,-8,-2,-1) and a.capital_year = '" + capital_year + "' and b.scholarship_level = 31";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string updated = "update asnreqschooldet set assist_amt = '" + outstandingperson_lv2 + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updated);
                    string updatem = "update asnreqmaster set assist_amt = '" + outstandingperson_lv2 + "', approve_amt = '" + outstandingperson_lv2 + "', req_status = 1  where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updatem);
                    peoplelv2 = peoplelv2 + 1;
                }
                //update ม.4
                sql = "select a.assist_docno from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and b.scholarship_type = 2 and a.req_status not in (-11,-8,-2,-1) and a.capital_year = '" + capital_year + "' and b.scholarship_level = 41";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string updated = "update asnreqschooldet set assist_amt = '" + outstandingperson_lv3 + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updated);
                    string updatem = "update asnreqmaster set assist_amt = '" + outstandingperson_lv3 + "', approve_amt = '" + outstandingperson_lv3 + "', req_status = 1  where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updatem);
                    peoplelv3 = peoplelv3 + 1;
                }
                //update ป.ตรีปี 1
                sql = "select a.assist_docno from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and b.scholarship_type = 2 and a.req_status not in (-11,-8,-2,-1) and a.capital_year = '" + capital_year + "' and b.scholarship_level = 71";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string updated = "update asnreqschooldet set assist_amt = '" + outstandingperson_lv5 + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updated);
                    string updatem = "update asnreqmaster set assist_amt = '" + outstandingperson_lv5 + "', approve_amt = '" + outstandingperson_lv5 + "', req_status = 1  where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updatem);
                    peoplelv5 = peoplelv5 + 1;
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการประมวลผลทุนรางวัลการศึกษาดีเด่นประจำปี " + capital_year + " เรียบร้อยแล้ว <br/>" + "จำนวนผู้รับทุนระดับประถมศึกษา (ม.1). : " + peoplelv2.ToString("#,##0") + " ราย " + "ทุนละ : " + outstandingperson_lv2.ToString("#,##0.00") + " บาท <br/>" + "จำนวนผู้รับทุนระดับมัธยมศึกษาตอนต้น (ม.4) : " + peoplelv3.ToString("#,##0") + " ราย " + "ทุนละ : " + outstandingperson_lv3.ToString("#,##0.00") + " บาท <br/>" + "จำนวนผู้รับทุนระดับมัธยมศึกษาตอนปลาย (ปริญญาตรีปี 1) : " + peoplelv5.ToString("#,##0") + " ราย " + "ทุนละ : " + outstandingperson_lv5.ToString("#,##0.00") + " บาท");
            }
            catch { }
        }
        private void CalEncourage()
        {
            try
            {
                Sdt dt;
                string sql;
                Sta ta = new Sta(sqlca.ConnectionString);
                decimal encourageperson_lv6 = 0, encourageperson_lv7 = 0;
                decimal capital_year = dw_encourage.GetItemDecimal(1, "capital_year");

                int peoplelv6 = 0, peoplelv7 = 0;
                //จำนวนเงินระดับปวส.
                sql = sql = "select envvalue from asnsenvironmentvar where envcode = 'encourage_money_lv6'";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    encourageperson_lv6 = Convert.ToDecimal(dt.Rows[0]["envvalue"].ToString());
                }
                //จำนวนเงินระดับปริญญาตรี
                sql = sql = "select envvalue from asnsenvironmentvar where envcode = 'encourage_money_lv7'";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    encourageperson_lv7 = Convert.ToDecimal(dt.Rows[0]["envvalue"].ToString());
                }
                //update ปวส.
                sql = "select a.assist_docno from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and b.scholarship_type = 1 and a.req_status not in (-11,-8,-2,-1) and a.capital_year = '" + capital_year + "' and b.level_school = 6";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string updated = "update asnreqschooldet set assist_amt = '" + encourageperson_lv6 + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updated);
                    string updatem = "update asnreqmaster set assist_amt = '" + encourageperson_lv6 + "', approve_amt = '" + encourageperson_lv6 + "', req_status = 1  where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updatem);
                    peoplelv6 = peoplelv6 + 1;
                }
                //update ปริญญาตรี
                sql = "select a.assist_docno from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and b.scholarship_type = 1 and a.req_status not in (-11,-8,-2,-1) and a.capital_year = '" + capital_year + "' and level_school = 7";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string updated = "update asnreqschooldet set assist_amt = '" + encourageperson_lv7 + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updated);
                    string updatem = "update asnreqmaster set assist_amt = '" + encourageperson_lv7 + "', approve_amt = '" + encourageperson_lv7 + "', req_status = 1  where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updatem);
                    peoplelv7 = peoplelv7 + 1;
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการประมวลผลทุนส่งเสริมการศึกษาประจำปี " + capital_year + " เรียบร้อยแล้ว <br/>" + "จำนวนผู้รับทุนระดับปวส. : " + peoplelv6.ToString("#,##0") + " ราย " + "ทุนละ : " + encourageperson_lv6.ToString("#,##0.00") + " บาท <br/>" + "จำนวนผู้รับทุนระดับปริญญาตรี : " + peoplelv7.ToString("#,##0") + " ราย " + "ทุนละ : " + encourageperson_lv7.ToString("#,##0.00") + " บาท");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        private void CalContribute()
        {
            try
            {
                Sdt dt;
                Sta ta = new Sta(sqlca.ConnectionString);
                string sql;
                int req_num = 0;
                int people = 0;
                decimal capital_year = dw_contribute.GetItemDecimal(1, "capital_year");
                double contributeperson = 0;
                sql = "select count(a.assist_docno) as reqnum from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and b.scholarship_type = 3 and a.req_status not in (-11,-8,-2,-1) and a.capital_year = '" + capital_year + "'";
                dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    req_num = Convert.ToInt32(dt.Rows[0]["reqnum"].ToString());
                }
                decimal contributeall = dw_contribute.GetItemDecimal(1, "envvalue");
                //จำนวนเงิน
                try
                {
                    contributeperson = Convert.ToDouble(contributeall) / req_num;
                }
                catch { }
                //update
                sql = "select a.assist_docno from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and b.scholarship_type = 3 and a.req_status not in (-11,-8,-2,-1) and a.capital_year = '" + capital_year + "'";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //string updated = "update asnreqschooldet set assist_amt = '" + contributeperson + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    //WebUtil.QuerySdt(updated);
                    string updatem = "update asnreqmaster set assist_amt = '" + contributeperson + "', approve_amt = '" + contributeperson + "', req_status = 1  where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updatem);
                    people = people + 1;
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ทำการประมวลผลทุนอุดหนุนการศึกษาประจำปี " + capital_year + " เรียบร้อยแล้ว " + "จำนวนผู้รับทุน : " + people.ToString("#,##0") + " ราย " + "ทุนละ : " + contributeperson.ToString("#,##0.00") + " บาท");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        private void GetConstant()
        {
            Sdt dt;
            string sql;
            //ทุนอุดหนุน
            sql = "select envvalue from asnsenvironmentvar where envcode = 'contribute_burary'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_contribute.SetItemDecimal(1, "envvalue", dt.GetDecimal("envvalue"));
            }
            //ทุนส่งเสริม
            sql = "select envvalue from asnsenvironmentvar where envcode = 'encourage_people_lv6'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_encourage.SetItemDecimal(1, "en_6", dt.GetDecimal("envvalue"));
            }
            //ทุนส่งเสริม
            sql = "select envvalue from asnsenvironmentvar where envcode = 'encourage_people_lv7'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_encourage.SetItemDecimal(1, "en_7", dt.GetDecimal("envvalue"));
            }
            //ทุนส่งเสริม
            sql = "select envvalue from asnsenvironmentvar where envcode = 'encourage_money_lv6'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_encourage.SetItemDecimal(1, "enm_6", dt.GetDecimal("envvalue"));
            }
            //ทุนส่งเสริม
            sql = "select envvalue from asnsenvironmentvar where envcode = 'encourage_money_lv7'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_encourage.SetItemDecimal(1, "enm_7", dt.GetDecimal("envvalue"));
            }
            //ทุนเรียนดี
            sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_people_lv2'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_outstanding.SetItemDecimal(1, "en_2", dt.GetDecimal("envvalue"));
            }
            //ทุนเรียนดี
            sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_people_lv3'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_outstanding.SetItemDecimal(1, "en_3", dt.GetDecimal("envvalue"));
            }
            //ทุนเรียนดี
            sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_people_lv5'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_outstanding.SetItemDecimal(1, "en_5", dt.GetDecimal("envvalue"));
            }
            //ทุนเรียนดี
            sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_money_lv2'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_outstanding.SetItemDecimal(1, "enm_2", dt.GetDecimal("envvalue"));
            }
            //ทุนเรียนดี
            sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_money_lv3'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_outstanding.SetItemDecimal(1, "enm_3", dt.GetDecimal("envvalue"));
            }
            //ทุนเรียนดี
            sql = "select envvalue from asnsenvironmentvar where envcode = 'outsatnding_money_lv5'";
            dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                dw_outstanding.SetItemDecimal(1, "enm_5", dt.GetDecimal("envvalue"));
            }
        }
        private void UpdateVarOutStanding()
        {
            string sql;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                sql = "update asnsenvironmentvar set envvalue = '" + dw_outstanding.GetItemDecimal(1, "enm_2") + "' where envcode = 'outsatnding_money_lv2'";
                ta.Exe(sql);

                sql = "update asnsenvironmentvar set envvalue = '" + dw_outstanding.GetItemDecimal(1, "enm_3") + "' where envcode = 'outsatnding_money_lv3'";
                ta.Exe(sql);

                sql = "update asnsenvironmentvar set envvalue = '" + dw_outstanding.GetItemDecimal(1, "enm_5") + "' where envcode = 'outsatnding_money_lv5'";
                ta.Exe(sql);

                sql = "update asnsenvironmentvar set envvalue = '" + dw_outstanding.GetItemDecimal(1, "en_2") + "' where envcode = 'outsatnding_people_lv2'";
                ta.Exe(sql);

                sql = "update asnsenvironmentvar set envvalue = '" + dw_outstanding.GetItemDecimal(1, "en_3") + "' where envcode = 'outsatnding_people_lv3'";
                ta.Exe(sql);

                sql = "update asnsenvironmentvar set envvalue = '" + dw_outstanding.GetItemDecimal(1, "en_5") + "' where envcode = 'outsatnding_people_lv5'";
                ta.Exe(sql);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            }
            catch { }

        }
        private void UpdateVarEncourage()
        {
            string sql;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                sql = "update asnsenvironmentvar set envvalue = '" + dw_encourage.GetItemDecimal(1, "enm_6") + "' where envcode = 'encourage_money_lv6'";
                ta.Exe(sql);

                sql = "update asnsenvironmentvar set envvalue = '" + dw_encourage.GetItemDecimal(1, "enm_7") + "' where envcode = 'encourage_money_lv7'";
                ta.Exe(sql);

                sql = "update asnsenvironmentvar set envvalue = '" + dw_encourage.GetItemDecimal(1, "en_6") + "' where envcode = 'encourage_people_lv6'";
                ta.Exe(sql);

                sql = "update asnsenvironmentvar set envvalue = '" + dw_encourage.GetItemDecimal(1, "en_7") + "' where envcode = 'encourage_people_lv7'";
                ta.Exe(sql);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            }
            catch { }
        }
        private void UpdateVarContribute()
        {
            string sql;
            Sta ta = new Sta(sqlca.ConnectionString);
            try
            {
                sql = "update asnsenvironmentvar set envvalue = '" + dw_contribute.GetItemDecimal(1, "envvalue") + "' where envcode = 'contribute_burary'";
                ta.Exe(sql);

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อยแล้ว");
            }
            catch { }
        }
        private void TranOutStanding()
        {
            try
            {
                Sdt dt, dtcheckclose;
                Sta ta = new Sta(sqlca.ConnectionString);
                string sql;
                double sumasamt = 0;
                decimal capital_year = dw_encourage.GetItemDecimal(1, "capital_year");
                sql = "select a.* from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = 10 and req_status = 1 and b. scholarship_type = 2 and a.capital_year = '" + capital_year + "' and a.approve_status <> 1 and a.assist_docno not in ('AS0000168','AS0000460','AS0004131','AS0001805')";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //check close
                    string checkclose = "select deptclose_status from dpdeptmaster where deptaccount_no = '" + dt.Rows[i]["deptaccount_no"].ToString() + "'";
                    dtcheckclose = WebUtil.QuerySdt(checkclose);
                    //update req if deptaccount_no is closed
                    try
                    {
                        if (dtcheckclose.Rows[0]["deptclose_status"].ToString() == "1")
                        {
                            string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                            ta.Exe(updatereqmaster);
                        }
                    }
                    catch
                    {
                        // โอนไม่สำเร็จ
                        string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                        ta.Exe(updatereqmaster);
                    }
                    try
                    {
                        if (dtcheckclose.Rows[0]["deptclose_status"].ToString() == "0")
                        {
                            int seq_no = 1;
                            string trandate = dw_encourage.GetItemDateTime(1, "apv_date").ToString("MM/dd/yyyy");
                            string sqlcheckseq = "select count(deptaccount_no) as depcount from dpdepttran where tran_date = to_date('" + trandate + "','MM/dd/yyyy') and deptaccount_no = '" + dt.Rows[i]["deptaccount_no"].ToString() + "' and system_code = 'TRE'";
                            Sdt dtcheckseq = WebUtil.QuerySdt(sqlcheckseq);
                            if (dtcheckseq.Next())
                            {
                                seq_no = seq_no + Convert.ToInt32(dtcheckseq.Rows[0]["depcount"].ToString());
                            }
                            //update asnreqmaster
                            string updateasnreq = "update asnreqmaster set approve_date = to_date('" + trandate + "','MM/dd/yyyy'),approve_status = 1,approve_amt = '" + dt.Rows[i]["assist_amt"].ToString() + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                            ta.Exe(updateasnreq);
                            // tran
                            string tran = "insert into dpdepttran (coop_id,deptaccount_no,memcoop_id,member_no,system_code,tran_year,tran_date,seq_no,old_balanc,old_accuint,deptitem_amt,int_amt,new_accuint,new_balanc,tran_status,dividen_amt,average_amt,branch_operate,sequest_status,sequest_amt) values ('001001','" + dt.Rows[i]["deptaccount_no"].ToString() + "','001001','" + dt.Rows[i]["member_no"].ToString() + "','TRE','" + dt.Rows[i]["capital_year"].ToString() + "',to_date('" + trandate + "','MM/dd/yyyy'),'" + seq_no + "',0,0,'" + dt.Rows[i]["assist_amt"].ToString() + "',0,0,0,0,0,0,'001',0,0)";
                            ta.Exe(tran);
                            sumasamt = sumasamt + Convert.ToDouble(dt.Rows[i]["assist_amt"].ToString());
                        }
                    }
                    catch
                    {
                        // โอนไม่สำเร็จ
                        string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                        ta.Exe(updatereqmaster);
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("โอนเงินสวัสดิการทุนการศึกษาดีเด่น ยอดเงินสุทธิ : " + sumasamt.ToString("#,##0.00") + " บาท กรุณาตรวจสอบข้อมูล");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        private void TranEncourage()
        {
            try
            {
                Sdt dt, dtcheckclose;
                Sta ta = new Sta(sqlca.ConnectionString);
                string sql;
                double sumasamt = 0;
                decimal capital_year = dw_encourage.GetItemDecimal(1, "capital_year");
                sql = "select a.* from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = 10 and req_status = 1 and b. scholarship_type = 1 and a.capital_year = '" + capital_year + "' and a.approve_status <> 1";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //check close
                    string checkclose = "select deptclose_status from dpdeptmaster where deptaccount_no = '" + dt.Rows[i]["deptaccount_no"].ToString() + "'";
                    dtcheckclose = WebUtil.QuerySdt(checkclose);
                    //update req if deptaccount_no is closed
                    try
                    {
                        if (dtcheckclose.Rows[0]["deptclose_status"].ToString() == "1")
                        {
                            string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                            ta.Exe(updatereqmaster);
                        }
                    }
                    catch
                    {
                        // โอนไม่สำเร็จ
                        string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                        ta.Exe(updatereqmaster);
                    }
                    try
                    {
                        if (dtcheckclose.Rows[0]["deptclose_status"].ToString() == "0")
                        {
                            int seq_no = 1;
                            string trandate = dw_encourage.GetItemDateTime(1, "apv_date").ToString("MM/dd/yyyy");
                            string sqlcheckseq = "select count(deptaccount_no) as depcount from dpdepttran where tran_date = to_date('" + trandate + "','MM/dd/yyyy') and deptaccount_no = '" + dt.Rows[i]["deptaccount_no"].ToString() + "' and system_code = 'TRE'";
                            Sdt dtcheckseq = WebUtil.QuerySdt(sqlcheckseq);
                            if (dtcheckseq.Next())
                            {
                                seq_no = seq_no + Convert.ToInt32(dtcheckseq.Rows[0]["depcount"].ToString());
                            }
                            //update asnreqmaster
                            string updateasnreq = "update asnreqmaster set approve_date = to_date('" + trandate + "','MM/dd/yyyy'),approve_status = 1,approve_amt = '" + dt.Rows[i]["assist_amt"].ToString() + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                            ta.Exe(updateasnreq);
                            // tran
                            string tran = "insert into dpdepttran (coop_id,deptaccount_no,memcoop_id,member_no,system_code,tran_year,tran_date,seq_no,old_balanc,old_accuint,deptitem_amt,int_amt,new_accuint,new_balanc,tran_status,dividen_amt,average_amt,branch_operate,sequest_status,sequest_amt) values ('001001','" + dt.Rows[i]["deptaccount_no"].ToString() + "','001001','" + dt.Rows[i]["member_no"].ToString() + "','TRE','" + dt.Rows[i]["capital_year"].ToString() + "',to_date('" + trandate + "','MM/dd/yyyy'),'" + seq_no + "',0,0,'" + dt.Rows[i]["assist_amt"].ToString() + "',0,0,0,0,0,0,'001',0,0)";
                            ta.Exe(tran);
                            sumasamt = sumasamt + Convert.ToDouble(dt.Rows[i]["assist_amt"].ToString());
                        }
                    }
                    catch
                    {
                        // โอนไม่สำเร็จ
                        string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                        ta.Exe(updatereqmaster);
                    }
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("โอนเงินสวัสดิการทุนส่งเสริมการศึกษาเรียบร้อยแล้ว ยอดเงินสุทธิ : " + sumasamt.ToString("#,##0.00") + " บาท กรุณาตรวจสอบข้อมูล");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        private void TranContribute()
        {
            try
            {
                Sdt dt, dtcheckclose;
                Sta ta = new Sta(sqlca.ConnectionString);
                string sql;
                double sumasamt = 0;
                decimal capital_year = dw_encourage.GetItemDecimal(1, "capital_year");
                sql = "select a.* from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = 10 and req_status = 1 and b. scholarship_type = 3 and a.capital_year = '" + capital_year + "' and a.approve_status <> 1";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //check close
                    string checkclose = "select deptclose_status from dpdeptmaster where deptaccount_no = '" + dt.Rows[i]["deptaccount_no"].ToString() + "'";
                    dtcheckclose = WebUtil.QuerySdt(checkclose);
                    //update req if deptaccount_no is closed
                    try
                    {
                        if (dtcheckclose.Rows[0]["deptclose_status"].ToString() == "1")
                        {
                            string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                            ta.Exe(updatereqmaster);
                        }
                    }
                    catch
                    {
                        // โอนไม่สำเร็จ
                        string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                        ta.Exe(updatereqmaster);
                    }
                    try
                    {
                        if (dtcheckclose.Rows[0]["deptclose_status"].ToString() == "0")
                        {
                            int seq_no = 1;
                            string trandate = dw_encourage.GetItemDateTime(1, "apv_date").ToString("MM/dd/yyyy");
                            string sqlcheckseq = "select count(deptaccount_no) as depcount from dpdepttran where tran_date = to_date('" + trandate + "','MM/dd/yyyy') and deptaccount_no = '" + dt.Rows[i]["deptaccount_no"].ToString() + "'";
                            Sdt dtcheckseq = WebUtil.QuerySdt(sqlcheckseq);
                            if (dtcheckseq.Next())
                            {
                                seq_no = seq_no + Convert.ToInt32(dtcheckseq.Rows[0]["depcount"].ToString());
                            }
                            //update asnreqmaster
                            string updateasnreq = "update asnreqmaster set approve_date = to_date('" + trandate + "','MM/dd/yyyy'),approve_status = 1,approve_amt = '" + dt.Rows[i]["assist_amt"].ToString() + "' where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                            ta.Exe(updateasnreq);
                            // tran
                            string tran = "insert into dpdepttran (coop_id,deptaccount_no,memcoop_id,member_no,system_code,tran_year,tran_date,seq_no,old_balanc,old_accuint,deptitem_amt,int_amt,new_accuint,new_balanc,tran_status,dividen_amt,average_amt,branch_operate,sequest_status,sequest_amt) values ('001001','" + dt.Rows[i]["deptaccount_no"].ToString() + "','001001','" + dt.Rows[i]["member_no"].ToString() + "','TRE','" + dt.Rows[i]["capital_year"].ToString() + "',to_date('" + trandate + "','MM/dd/yyyy'),'" + seq_no + "',0,0,'" + dt.Rows[i]["assist_amt"].ToString() + "',0,0,0,0,0,0,'001',0,0)";
                            ta.Exe(tran);
                            sumasamt = sumasamt + Convert.ToDouble(dt.Rows[i]["assist_amt"].ToString());
                        }
                    }
                    catch
                    {
                        // โอนไม่สำเร็จ
                        string updatereqmaster = "update asnreqmaster set approve_status = 7 where assist_docno = '" + dt.Rows[i]["assist_docno"] + "'";
                        ta.Exe(updatereqmaster);
                    }
                    LtServerMessage.Text = WebUtil.CompleteMessage("โอนเงินสวัสดิการทุนอุดหนุนการศึกษาเรียบร้อยแล้ว ยอดเงินสุทธิ : " + sumasamt.ToString("#,##0.00") + " บาท กรุณาตรวจสอบข้อมูล");
                }
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        private void CancelTranEncourage()
        {
            try
            {
                Sdt dt;
                Sta ta = new Sta(sqlca.ConnectionString);
                string sql;
                decimal capital_year = dw_encourage.GetItemDecimal(1, "capital_year");
                sql = "select a.* from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = 10 and req_status = 1 and b. scholarship_type = 1 and a.capital_year = '" + capital_year + "' and a.approve_status = 1";
                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string updateasnreq = "update asnreqmaster set approve_status = 8 where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updateasnreq);
                    string approvedate = Convert.ToDateTime(dt.Rows[i]["approve_date"].ToString()).ToString("dd/MM/yyyy");
                    string deldepttran = "delete from dpdepttran where deptaccount_no = '" + dt.Rows[i]["deptaccount_no"].ToString() + "' and system_code = 'TRE' and member_no = '" + dt.Rows[i]["member_no"] + "' and tran_year = '" + capital_year + "' and tran_date = to_date('" + approvedate + "','dd/MM/yyyy') and deptitem_amt = '" + dt.Rows[i]["approve_amt"].ToString() + "'";
                    ta.Exe(deldepttran);
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ยกเลิกรายการโอนเงินสวัสดิการทุนส่งเสริมการศึกษาเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
        private void CancelTranContribute()
        {
            try
            {
                Sdt dt;
                Sta ta = new Sta(sqlca.ConnectionString);
                string sql;
                decimal capital_year = dw_encourage.GetItemDecimal(1, "capital_year");
                sql = "select a.* from asnreqmaster a,asnreqschooldet b where a.assist_docno = b.assist_docno and a.capital_year = b.capital_year and a.assisttype_code = 10 and req_status = 1 and b. scholarship_type = 3 and a.capital_year = '" + capital_year + "' and a.approve_status = 1";

                dt = WebUtil.QuerySdt(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string updateasnreq = "update asnreqmaster set approve_status = 8 where assist_docno = '" + dt.Rows[i]["assist_docno"].ToString() + "'";
                    ta.Exe(updateasnreq);
                    string approvedate = Convert.ToDateTime(dt.Rows[i]["approve_date"].ToString()).ToString("dd/MM/yyyy");
                    string deldepttran = "delete from dpdepttran where deptaccount_no = '" + dt.Rows[i]["deptaccount_no"].ToString() + "' and system_code = 'TRE' and member_no = '" + dt.Rows[i]["member_no"] + "' and tran_year = '" + capital_year + "' and tran_date = to_date('" + approvedate + "','dd/MM/yyyy') and deptitem_amt = '" + dt.Rows[i]["approve_amt"].ToString() + "'";
                    ta.Exe(deldepttran);
                }
                LtServerMessage.Text = WebUtil.CompleteMessage("ยกเลิกรายการโอนเงินสวัสดิการทุนอุดหนุนการศึกษา เรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }
    }
}