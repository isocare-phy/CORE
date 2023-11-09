using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using CoreSavingLibrary;

namespace Saving.Applications.mbshr.ws_mem_ucfmembgroup_ctrl.wd_mem_adjmembgroup_ctrl
{
    public partial class wd_mem_adjmembgroup : PageWebDialog, WebDialog
    {
        [JsPostBack]
        public String PostProvince { get; set; }
        [JsPostBack]
        public String PostAmphur { get; set; }
        [JsPostBack]
        public String PostSave { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                string ls_membgroup_code = Request["ls_membgroup_code"];
                dsMain.RetrieveMembGroup(ls_membgroup_code);
                string ls_province_code = dsMain.DATA[0].ADDR_PROVINCE;
                string ls_district_code = dsMain.DATA[0].ADDR_AMPHUR;
                dsMain.DdProvince();
                dsMain.DdDistrict(ls_province_code);
                dsMain.DdTambol(ls_district_code);
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostProvince)
            {
                string ls_province_code = dsMain.DATA[0].ADDR_PROVINCE;
                dsMain.DdDistrict(ls_province_code);
            }
            else if (eventArg == PostAmphur)
            {
                string ls_addr_amphur = dsMain.DATA[0].ADDR_AMPHUR;
                dsMain.DdTambol(ls_addr_amphur);
            }
            else if (eventArg == PostSave)
            {
                string ls_membgroup_code2 = Request["ls_membgroup_code"];
                ExecuteDataSource exed1 = new ExecuteDataSource(this);
                string ls_membgroup_code = dsMain.DATA[0].MEMBGROUP_CODE;
                string ls_membgroup_desc = dsMain.DATA[0].MEMBGROUP_DESC;
                string ls_membgroup_control = dsMain.DATA[0].MEMBGROUP_CONTROL;
                //decimal ls_membgroup_agentgrg = dsMain.DATA[0].GRPELECSHOW_FLAG;
                string ls_addr_no = dsMain.DATA[0].ADDR_NO;
                string ls_addr_moo = dsMain.DATA[0].ADDR_MOO;
                string ls_addr_soi = dsMain.DATA[0].ADDR_SOI;
                string ls_addr_road = dsMain.DATA[0].ADDR_ROAD;
                string ls_addr_tambol = dsMain.DATA[0].ADDR_TAMBOL;
                string ls_addr_amphur = dsMain.DATA[0].ADDR_AMPHUR;
                string ls_addr_province = dsMain.DATA[0].ADDR_PROVINCE;
                string ls_addr_postcode = dsMain.DATA[0].ADDR_POSTCODE;
                string ls_addr_phone = dsMain.DATA[0].ADDR_PHONE;
                string ls_addr_fax = dsMain.DATA[0].ADDR_FAX;
                if (ls_membgroup_code2 == "")
                {
                    try
                    {
                        string sql_insert = @"INSERT INTO mbucfmembgroup (coop_id,         membgroup_code,      membgroup_control,
                                                                          membgroup_desc,  addr_no,
                                                                          addr_moo,        addr_soi,            addr_road,
                                                                          addr_tambol,     addr_amphur,         addr_province,   
                                                                          addr_postcode,   addr_phone,          addr_fax) 
                                                          VALUES ('" + state.SsCoopId + "','" + ls_membgroup_code + "','" + ls_membgroup_control + "','"
                                                                     + ls_membgroup_desc + "','" + ls_addr_no + "','"
                                                                     + ls_addr_moo + "','" + ls_addr_soi + "','" + ls_addr_road + "','"
                                                                     + ls_addr_tambol + "','" + ls_addr_amphur + "','" + ls_addr_province + "','"
                                                                     + ls_addr_postcode + "','" + ls_addr_phone + "','" + ls_addr_fax + "')";
                        exed1.SQL.Add(sql_insert);
                        exed1.Execute();
                        dsMain.ResetRow();
                        this.SetOnLoadedScript("alert('บันทึกข้อมูลสำเร็จ'); parent.RefreshFromDlg();");

                    }
                    catch (Exception ex)
                    {
                        LtServerMessage.Text = WebUtil.ErrorMessage(ex);
                    }
                }
                else
                {
                    String sql_update = @"update mbucfmembgroup set membgroup_desc = {0},membgroup_control = {1},addr_no = {2}
                                          ,addr_moo = {3},addr_soi = {4},addr_road = {5},addr_tambol = {6},addr_amphur = {7},addr_province = {8}
                                          ,addr_postcode = {9},addr_phone = {10},addr_fax = {11} where coop_id = {12} and membgroup_code = {13} ";
                    sql_update = WebUtil.SQLFormat(sql_update, ls_membgroup_desc, ls_membgroup_control, ls_addr_no, ls_addr_moo, ls_addr_soi
                                                   , ls_addr_road, ls_addr_tambol, ls_addr_amphur, ls_addr_province, ls_addr_postcode, ls_addr_phone, ls_addr_fax, state.SsCoopId, ls_membgroup_code);
                    exed1.SQL.Add(sql_update);
                    exed1.Execute();
                    //LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลสำเร็จ");
                    dsMain.ResetRow();
                    this.SetOnLoadedScript("alert('แก้ไขข้อมูลสำเร็จ'); parent.RefreshFromDlg();");
                }
            }
        }

        public void WebDialogLoadEnd()
        {
           
        }
    }
}