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
using CoreSavingLibrary.WcfNKeeping;
using Sybase.DataWindow;
using DataLibrary;
using System.Threading.Tasks;
using System.IO;

namespace Saving.Applications.keeping
{
    public partial class w_sheet_kp_ccl_adjust_monthly_prc : PageWebSheet, WebSheet
    {
        private DwThDate tdwmain;
        public String pbl = "kp_ccl_adjust_monthly.pbl";

        protected String runProcess;
        private int chk_error = 0;

        [JsPostBack]
        public string PostCalIntprocess { get; set; }

        public void InitJsPostBack()
        {
            tdwmain = new DwThDate(dw_main, this);
            tdwmain.Add("operate_date", "operate_tdate");
            runProcess = WebUtil.JsPostBack(this, "runProcess");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            dw_main.SetTransaction(sqlca);
            dw_detail.SetTransaction(sqlca);
            DataTable dt = new DataTable();
            if (IsPostBack)
            {
                try
                {
                    this.RestoreContextDw(dw_main);
                    this.RestoreContextDw(dw_detail);
                }
                catch { }
            }
            else {
                string sql = "select membgroup_code,membgroup_code||' : '||membgroup_desc as membgroup_desc from mbucfmembgroup where length(membgroup_code) = 6 and membgroup_desc is not null and kpgroup_code is not null order by membgroup_code";
                dt = WebUtil.QuerySdt(sql);
                this.membgroup_code.DataSource = dt;
                this.membgroup_code.DataTextField = "membgroup_desc";
                this.membgroup_code.DataValueField = "membgroup_code";
                this.membgroup_code.DataBind();
                sql = "select max(trim(recv_period)) as recv_period  from kpmastreceive";
                Sdt Sdt = WebUtil.QuerySdt(sql);
                if (Sdt.Next())
                {
                    recv_period.Text = Sdt.GetString("recv_period");
                }
                dw_main.InsertRow(0);
                dw_detail.InsertRow(0);
            }
        }

        public void CheckJsPostBack(String eventArg)
        {

        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
        }

        //JS-EVENT
        private void JsNewClear()
        {
            dw_main.Reset();
            dw_detail.Reset();
            dw_main.InsertRow(0);
            dw_detail.InsertRow(0);
            dw_main.SetItemDate(1, "operate_date", state.SsWorkDate);
            tdwmain.Eng2ThaiAllRow();
        }

        private void Save() {
            try
            {
                str_keep_adjust astr_keep_adjust = new str_keep_adjust();
                astr_keep_adjust.xml_main = dw_main.Describe("DataWindow.Data.XML");
                astr_keep_adjust.xml_detail = dw_detail.Describe("DataWindow.Data.XML");
                astr_keep_adjust.cancel_id = state.SsUsername;
                astr_keep_adjust.operate_date = state.SsWorkDate;

                int result = wcf.NKeeping.of_save_adjust_monthly(state.SsWsPass, astr_keep_adjust);
                //if (result == 1)
                //{
                //    JsNewClear();
                //}
            }
            catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message); }
        }

        protected void B_Pro_Click(object sender, EventArgs e)
        {
            CalIntprocess();
        }
        public String outputProcess = "";
        public String logsFilePath = "";

        public void CalIntprocess()
        {
            this.connectionString = state.SsConnectionString;
            string coop_id = state.SsCoopId;
            string coop_control = state.SsCoopControl;
            string entry_id = state.SsUsername;
            string str_membgroup_code = membgroup_code.SelectedValue;
            string str_recv_period = recv_period.Text;

            String log_file = "trans_log_" + coop_id.Replace(",", "-") + "_" + entry_id + ".log";
            try
            {
                String xml = "<?xml version=\"1.0\" encoding=\"UTF-16LE\" standalone=\"no\"?>" +
                            "<entry_id>" + entry_id + "</entry_id>" +
                            "<membgroup_code>" + str_membgroup_code + "</membgroup_code>" +
                            "<recv_period>" + str_recv_period + "</recv_period>" +
                            "<log_file>" + log_file + "</log_file>";
                outputProcess = WebUtil.runProcessingCappExtend(state, "runPostProcess", xml, "", "");
                logsFilePath = Environment.GetEnvironmentVariable("windir") + "\\Temp\\" + log_file;
                HdLogFile.Value = logsFilePath;
                Task.Factory.StartNew(() => this.runObjectProcessing(outputProcess, this.xmlconfig, "runPostProcess", xml, "", ""));
                File.WriteAllText(logsFilePath, "<<ประมวลเสร็จกดดูรายละเอียดที่ Log>>");
            }
            catch (Exception ex)
            {

                LtServerMessage.Text = WebUtil.ErrorMessage(ex);
            }
            finally
            {
            }
        }

        private XmlConfigService xml;

        public void runObjectProcessing(String outputProcess, XmlConfigService xmlConfig, String object_name, String criteria_xml, String criteria_xml_1, String criteria_xml_2)
        {
            this.xml = xmlConfig;
            initProcessing(outputProcess, object_name, criteria_xml, criteria_xml_1, criteria_xml_2);
            if (object_name == "runPostProcess")
            {
                WriteLine("runPostProcess");
                bool status = false;
                HdERR.Value = "";
                int allStep = 2;

                Sta taSrc = new Sta(state.SsConnectionString);
                Sta taSrcQuery = new Sta(state.SsConnectionString);
                taSrc.Transection();
                String Mode = Hdmode.Value.ToString(); // เลือก mode ได้ ใช้ if เอา

                status = runPostProcess(allStep, taSrc, taSrcQuery, Mode);

                if (status)
                {
                    taSrc.Commit();
                    taSrc.Close();
                    finishProcessMsgLog("ทำรายการสำเร็จ", allStep);
                }
                else
                {
                    taSrc.RollBack();
                    taSrc.Close();
                    finishProcessMsgLog("!!! ผิดพลาด ไม่สำเร็จขั้นตอนที่  " + HdERR.Value.ToString(), allStep);
                    LtServerMessage.Text = WebUtil.ErrorMessage(HdERR.Value.ToString());
                }
            }
            closeProcessing("Main");
        }

        private Boolean runPostProcess(int allStep, Sta taSrc, Sta taSrcQuery, String Mode)
        {
            startProcessMsgLog("กำลังดำเนินการเริ่มประมวลผล", allStep);

            Sdt dt = null;

            string entry_id = string.Empty;
            string membgroup_code = string.Empty;
            string recv_period = string.Empty;
            int row_count = 0;

            try
            {
                coop_id = state.SsCoopId;
                coop_control = state.SsCoopControl;
                entry_id = getXMLStringValue(criteria_xml, "entry_id");
                membgroup_code = getXMLStringValue(criteria_xml, "membgroup_code");
                recv_period = getXMLStringValue(criteria_xml, "recv_period");

                try
                {
                    updateProcessMsgLog("STEP " + 1 + "กำลังเตรียมข้อมูล....", 1);
                    string sql = @"select KPMASTRECEIVE.KPSLIP_NO,KPMASTRECEIVE.MEMBER_NO
                                    from KPMASTRECEIVE
                                    where RECV_PERIOD = {0} and KEEPING_STATUS = 1
                                    and trim(MEMBGROUP_CODE) = {1}";
                    sql = WebUtil.SQLFormat(sql, recv_period, membgroup_code);
                    dt = taSrcQuery.Query(sql);
                    row_count = dt.Rows.Count;
                }
                catch
                {
                    chk_error = 1;
                    HdERR.Value = chk_error.ToString();
                    return false;
                }

                try
                {
                    int die_cnt = 0;
                    while (dt.Next())
                    {
                        die_cnt++;
                        updateProcessMsgLog("STEP " + 2 + ": กำลัประมวลยกเลิก  " + die_cnt + "/" + row_count + " ราย ", 2);
                        dw_main.SetItemString(1, "memcoop_id", state.SsCoopId);
                        dw_main.SetItemString(1, "member_no", dt.GetString("member_no"));
                        dw_main.SetItemString(1, "ref_recvperiod", recv_period);
                        dw_main.SetItemString(1, "ref_slipno",  dt.GetString("kpslip_no"));
                        dw_main.SetItemString(1, "entry_id", state.SsUsername);
                        try
                        {
                            str_keep_adjust astr_keep_adjust = new str_keep_adjust();
                            astr_keep_adjust.xml_main = dw_main.Describe("DataWindow.Data.XML");
                            astr_keep_adjust.xml_detail = dw_detail.Describe("DataWindow.Data.XML");
                            int result = wcf.NKeeping.of_init_adjust_monthly(state.SsWsPass, ref astr_keep_adjust);
                            if (result == 1)
                            {
                                dw_main.Reset();
                                dw_detail.Reset();
                                DwUtil.ImportData(astr_keep_adjust.xml_main, dw_main, tdwmain, FileSaveAsType.Xml);
                                DwUtil.ImportData(astr_keep_adjust.xml_detail, dw_detail, null, FileSaveAsType.Xml);
                                dw_main.SetItemDecimal(1, "slipretall_flag", 1);
                                for (int i = 1; i <= dw_detail.RowCount; i++)
                                {
                                    dw_detail.SetItemDecimal(i, "operate_flag", 1);
                                }
                                dw_main.SetItemDate(1, "adjslip_date", state.SsWorkDate);
                                dw_main.SetItemString(1, "tofrom_accid", "11201010");
                            }
                            //Save();
                            JsNewClear();
                        }
                        catch (Exception ex)
                        {
                            LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                        }
                    }

                }catch
                {
                    chk_error = 2;
                    HdERR.Value = chk_error.ToString();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                HdERR.Value = ex.Message;
                return false;
            }
        }
    }
}
