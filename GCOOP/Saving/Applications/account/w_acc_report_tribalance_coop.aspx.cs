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
using CoreSavingLibrary.WcfNAccount;
using CoreSavingLibrary.WcfNCommon;
using Sybase.DataWindow;
using System.Web.Services.Protocols;
using System.Globalization;
using Sybase.DataWindow.Web;
using System.IO;
using DataLibrary;



namespace Saving.Applications.account
{
    public partial class w_acc_report_tribalance_coop : PageWebSheet, WebSheet
    {
        //DataStore DStore;
        private n_accountClient acc;
        protected String postNewClear;
        protected String jsButtonShowData;
        protected String expExcel;
        private DwThDate tDwMain;

        private void JspostNewClear()
        {
            Dw_main.Reset();
            Dw_main.InsertRow(0);
            Dw_main1.Reset();
            Dw_main1.InsertRow(0);
        }

        public void InitJsPostBack()
        {
            postNewClear = WebUtil.JsPostBack(this, "postNewClear");
            jsButtonShowData = WebUtil.JsPostBack(this, "jsButtonShowData");
            expExcel = WebUtil.JsPostBack(this, "expExcel");
            //=========================
            tDwMain = new DwThDate(Dw_main, this);
            tDwMain.Add("start_date", "start_tdate");
            tDwMain.Add("end_date", "end_tdate");
        }

        public void WebSheetLoadBegin()
        {
            //DwUtil.RetrieveDDDW(Dw_main, "select_coop", "vc_report.pbl", null);
            try
            {
                this.ConnectSQLCA();
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Database ไม่ได้"); }

            try
            {
                acc = wcf.NAccount;
            }
            catch { LtServerMessage.Text = WebUtil.CompleteMessage("ติดต่อ Service ไม่ได้"); }

            Dw_main.SetTransaction(sqlca);
            Dw_main1.SetTransaction(sqlca);

            if (!IsPostBack)
            {
                JspostNewClear();
                detail.Visible = false;
            }
            else
            {
                detail.Visible = true;
                this.RestoreContextDw(Dw_main);
                this.RestoreContextDw(Dw_main1);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postNewClear")
            {
                JspostNewClear();
            }
            else if (eventArg == "jsButtonShowData")
            {
                JsButtonShowData();
            }
            else if (eventArg == "expExcel")
            {
                SaveFile();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            Dw_main.SaveDataCache();
            Dw_main1.SaveDataCache();
        }

        private void JsButtonShowData()
        {

            try
            {
                DateTime startDate = Dw_main.GetItemDate(1, "start_date");
                DateTime endDate = Dw_main.GetItemDate(1, "end_date");
                String year = startDate.ToString("yyyy");
                String period = startDate.ToString("MM");
                String st_date = startDate.ToString("ddMMyyyy");
                String ed_date = endDate.ToString("ddMMyyyy");

                Sta ta = new Sta(state.SsConnectionString);   //select หายอดยกมาของสมาชิกจากเดือนก่อน
                String sql = "";
                sql = @"select  count(member_no) as begin
		            from  mbmembmaster  
		            where   (coop_id =  '" + state.SsCoopControl + "') and (resign_status = 0) and (member_date < to_date('" + st_date + "','ddMMyyyy'))";
                Sdt dt = ta.Query(sql);
                String begin = dt.Rows[0]["begin"].ToString();
                int ldc_begin = Convert.ToInt32(begin);
                ta.Close();

                Sta ta2 = new Sta(state.SsConnectionString);   //select หายอดสมัครสมาชิกใหม่
                String sql2 = "";
                sql2 = @"select  count(member_no) as new
		                from  mbmembmaster  
		                where  coop_id = '" + state.SsCoopControl + "' and resign_status = 0 and member_date >= to_date('" + st_date + "','ddMMyyyy') and member_date <= to_date('" + ed_date + "','ddMMyyyy')";
                Sdt dt2 = ta2.Query(sql2);
                String member_new = dt2.Rows[0]["new"].ToString();
                int ldc_new = Convert.ToInt32(member_new);
                ta2.Close();

                Sta ta3 = new Sta(state.SsConnectionString);   //select หายอดลาออก
                String sql3 = "";
                sql3 = @"select  count(member_no) as out
		                from  mbmembmaster  
		                where  coop_id = '" + state.SsCoopControl + "' and resigncause_code <> 20 and resign_status = 1 and  resign_date >= to_date('" + st_date + "','ddMMyyyy') and  resign_date <= to_date('" + ed_date + "','ddMMyyyy')";
                Sdt dt3 = ta3.Query(sql3);
                String sign_out = dt3.Rows[0]["out"].ToString();
                int ldc_out = Convert.ToInt32(sign_out);
                ta3.Close();

                Sta ta4 = new Sta(state.SsConnectionString);   //select หายอดสมาชิกที่เป็นหนี้
                String sql4 = "";
                sql4 = @"select  count(distinct(member_no)) as shrlon
		                from  lncontmaster  
		                where  coop_id = '" + state.SsCoopControl + "' and contract_status = 1 ";
                Sdt dt4 = ta4.Query(sql4);
                String shrlon = dt4.Rows[0]["shrlon"].ToString();
                int ldc_shrlon = Convert.ToInt32(shrlon);
                ta4.Close();

                Dw_main1.Retrieve(year, period, state.SsCoopControl, ldc_begin, ldc_new, ldc_out, ldc_shrlon, endDate);
            }
            catch (Exception ex)
            {

            }
        }

        private void SaveFile()
        {
            String report_name = "รายงานกิจการของสหกรณ์";
            String acc_name = "สหกรณ์ออมทรัพย์มหาวิทยาลัยมหิดล จำกัด";
            String recv_period = Dw_main1.GetItemString(1, "compute_30");
            String shrlon = Dw_main1.GetItemString(1, "compute_31");
            String begin = Dw_main1.GetItemString(1, "compute_29");
            String member_new = Dw_main1.GetItemString(1, "compute_33");
            String member_resign = Dw_main1.GetItemString(1, "compute_34");
            String total = Dw_main1.GetItemString(1, "compute_35");
            String tribalance = Dw_main1.GetItemString(1, "compute_36");
            String group_id_old = "1";
            try
            {
                str_rptexcel astr_rptexcel = new str_rptexcel();
                astr_rptexcel.as_xmldw = Dw_main1.Describe("DataWindow.Data.XML");
                int result = wcf.NCommon.of_dwexportexcel_etn(state.SsWsPass, astr_rptexcel);
                if (astr_rptexcel.as_xmldw != "")
                {
                    String xml_detail = astr_rptexcel.as_xmldw;
                    String filename = report_name + recv_period + ".xls";
                    String path = WebUtil.GetStoreFile(state.SsApplication, "sms_excel\\" + filename);

                    int row = Dw_main1.RowCount;
                    int i = 1;
                    string a, b, c, d, e, f, g, h, k, l,m,n, p ,q, r, s, t, u, v, w;
                    string aa, bb, cc, dd, ee, ff, gg, hh, kk, ll, mm , nn, pp, qq,rr,ss,tt,uu,vv,ww;
                    StreamWriter writer = new StreamWriter(path);
                    try
                    {
                        q = report_name;
                    }
                    catch
                    {
                        q = "";
                    }
                    writer.WriteLine(q.Replace(Environment.NewLine, "<br/>"));
                    try
                    {
                        e = acc_name;
                    }
                    catch
                    {
                        e = "";
                    }
                    writer.WriteLine(e.Replace(Environment.NewLine, "<br/>"));
                    try
                    {
                        p = recv_period;
                    }
                    catch
                    {
                        p = "";
                    }
                    writer.WriteLine(p.Replace(Environment.NewLine, "<br/>"));

                    r = "จำนวนสมาชิก";
                    writer.WriteLine(r.Replace(Environment.NewLine, "<br/>"));

                    f = begin;
                    writer.WriteLine(f.Replace(Environment.NewLine, "<br/>"));

                    h = member_new;
                    writer.WriteLine(h.Replace(Environment.NewLine, "<br/>"));

                    k = member_resign;
                    writer.WriteLine(k.Replace(Environment.NewLine, "<br/>"));

                    s = total;
                    writer.WriteLine(s.Replace(Environment.NewLine, "<br/>"));

                    l = shrlon;
                    writer.WriteLine(l.Replace(Environment.NewLine, "<br/>"));

                    t = tribalance;
                    writer.WriteLine(t.Replace(Environment.NewLine, "<br/>"));

                    aa = "ชื่อบัญชี"; writer.Write(aa); writer.Write('\t');
                    bb = "ยอดยกมาจากเดือนก่อน"; writer.Write(bb); writer.Write('\t'); writer.Write('\t');
                    cc = "รายการระหว่างเดือน"; writer.Write(cc); writer.Write('\t'); writer.Write('\t');
                    dd = "ยอดคงเหลือ"; writer.Write(dd); writer.Write('\t'); writer.Write('\t');
                    ee = "เพิ่ม"; writer.WriteLine(ee.Replace(Environment.NewLine, "<br/>"));

                    ff = "ลูกหนี้"; writer.Write('\t'); writer.Write(ff); writer.Write('\t');
                    gg = "เจ้าหนี้"; writer.Write(gg); writer.Write('\t'); writer.Write(ff); writer.Write('\t');
                    writer.Write(gg); writer.Write('\t'); writer.Write(ff); writer.Write('\t'); writer.Write(gg); writer.Write('\t');
                    hh = "ลบ";
                    writer.WriteLine(hh.Replace(Environment.NewLine, "<br/>"));

                    while (i < row + 1)
                    {
                        String group_id = Dw_main1.GetItemString(i, "compute_38");
                        if (group_id_old != group_id)
                        {
                            m = Dw_main1.GetItemString(i-1, "compute_2"); writer.Write(m); writer.Write('\t');
                            kk = Dw_main1.GetItemString(i-1, "compute_10"); writer.Write(kk); writer.Write('\t');
                            ll = Dw_main1.GetItemString(i-1, "compute_11"); writer.Write(ll); writer.Write('\t');
                            mm = Dw_main1.GetItemString(i-1, "compute_12"); writer.Write(mm); writer.Write('\t');
                            nn = Dw_main1.GetItemString(i-1, "compute_13"); writer.Write(nn); writer.Write('\t');
                            pp = Dw_main1.GetItemString(i-1, "compute_14"); writer.Write(pp); writer.Write('\t');
                            qq = Dw_main1.GetItemString(i-1, "compute_15");
                            writer.WriteLine(qq.Replace(Environment.NewLine, "<br/>"));
                        }
                        group_id_old = group_id;

                        try
                        {
                            a = Dw_main1.GetItemString(i, "accountname");
                        }
                        catch
                        {
                            a = "";
                        }
                        try
                        {
                            b = Dw_main1.GetItemString(i, "begin_dr_amount");
                        }
                        catch
                        {
                            b = "";
                        }
                        try
                        {
                            c = Dw_main1.GetItemString(i, "begin_cr_amount");
                        }
                        catch
                        {
                            c = "";
                        }
                        try
                        {
                            d = Dw_main1.GetItemString(i, "dr_amount");
                        }
                        catch
                        {
                            d = "";
                        }
                        try
                        {
                            g = Dw_main1.GetItemString(i, "cr_amount");
                        }
                        catch
                        {
                            g = "";
                        }
                        try
                        {
                            u = Dw_main1.GetItemString(i, "forward_dr_amount");
                        }
                        catch
                        {
                            u = "";
                        }
                        try
                        {
                            v = Dw_main1.GetItemString(i, "forward_cr_amount");
                        }
                        catch
                        {
                            v = "";
                        }
                        try
                        {
                            w = Dw_main1.GetItemString(i, "compute_24");
                        }
                        catch
                        {
                            w = "";
                        }
                        writer.Write(a);
                        writer.Write('\t');
                        writer.Write(b);
                        writer.Write('\t');
                        writer.Write(c);
                        writer.Write('\t');
                        writer.Write(d);
                        writer.Write('\t');
                        writer.Write(g);
                        writer.Write('\t');
                        writer.Write(u);
                        writer.Write('\t');
                        writer.Write(v);
                        writer.Write('\t');
                        writer.WriteLine(w.Replace(Environment.NewLine, "<br/>"));

                        i++;
                    }
                    m = Dw_main1.GetItemString(i , "compute_2"); writer.Write(m); writer.Write('\t');
                    kk = Dw_main1.GetItemString(i , "compute_10"); writer.Write(kk); writer.Write('\t');
                    ll = Dw_main1.GetItemString(i , "compute_11"); writer.Write(ll); writer.Write('\t');
                    mm = Dw_main1.GetItemString(i , "compute_12"); writer.Write(mm); writer.Write('\t');
                    nn = Dw_main1.GetItemString(i , "compute_13"); writer.Write(nn); writer.Write('\t');
                    pp = Dw_main1.GetItemString(i , "compute_14"); writer.Write(pp); writer.Write('\t');
                    qq = Dw_main1.GetItemString(i , "compute_15");
                    writer.WriteLine(qq.Replace(Environment.NewLine, "<br/>"));

                    n = "รวมทั้งหมด"; writer.Write(n); writer.Write('\t');
                    rr = Dw_main1.GetItemString(1, "compute_17"); writer.Write(rr); writer.Write('\t');
                    ss = Dw_main1.GetItemString(1, "compute_18"); writer.Write(ss); writer.Write('\t');
                    tt = Dw_main1.GetItemString(1, "compute_19"); writer.Write(tt); writer.Write('\t');
                    uu = Dw_main1.GetItemString(1, "compute_20"); writer.Write(uu); writer.Write('\t');
                    vv = Dw_main1.GetItemString(1, "compute_21"); writer.Write(vv); writer.Write('\t');
                    ww = Dw_main1.GetItemString(1, "compute_22"); writer.Write(ww); writer.Write('\t');
                        //try
                        //{
                        //    n = DStore.GetItemString(1, "compute_15");
                        //}
                        //catch
                        //{
                        //    n = "";
                        //}
                        //writer.WriteLine(n.Replace(Environment.NewLine, "<br/>"));
                    writer.Close();
                    JspostNewClear();
                    string path2 = WebUtil.CreateLinkDownload(state.SsApplication, "sms_excel/" + filename);
                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลสำเร็จ คุณสามารถดาวน์โหลดไฟล์ได้ที่นี่  <a href=\"" + path2 + "\" target='_blank'>" + filename + "</a>");
                }
                else
                {
                    //   HdExportFile.Value = "";
                    LtServerMessage.Text = WebUtil.ErrorMessage("ไม่พบข้อมูล กรุณาตรวจสอบใหม่");
                    JspostNewClear();
                }

            }
            catch (SoapException ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("เกิดข้อผิดพลาด " + WebUtil.SoapMessage(ex));
            }
        }
    }
}

