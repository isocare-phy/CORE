using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataLibrary;
using CoreSavingLibrary;
using System.Data;


namespace Saving.Applications.mbshr.ws_mem_ucfmembgroup_ctrl
{
    public partial class ws_mem_ucfmembgroup : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String GetSearch { get; set; }

        public void InitJsPostBack()
        {
            dsSearch.InitDsSearch(this);
        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                dsSearch.DdGroupControl();
                retreivedata();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == GetSearch)
            {
                string getgroup_code = dsSearch.DATA[0].MEMBGROUP_CODE;
                string getgroup_desc = dsSearch.DATA[0].MEMBGROUP_DESC;
                string sql = @"select membgroup_code,membgroup_desc,membgroup_control,grpelecshow_flag 
                                from mbucfmembgroup where membgroup_code like '%" + getgroup_code + "%' and membgroup_desc like '%"+getgroup_desc+"%' order by membgroup_code";
                sql = WebUtil.SQLFormat(sql);
                DataTable dt = WebUtil.Query(sql);
                dt.Columns.Add("membgroup_code");
                dt.Columns.Add("membgroup_desc");
                dt.Columns.Add("membgroup_control");
                dt.Columns.Add("grpelecshow_flag");
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {

        }

        public void retreivedata()
        {
            string sql = @"select membgroup_code,membgroup_desc,membgroup_control,kpgroup_code from mbucfmembgroup order by membgroup_code";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            dt.Columns.Add("membgroup_code");
            dt.Columns.Add("membgroup_desc");
            dt.Columns.Add("membgroup_control");
            dt.Columns.Add("kpgroup_code");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        public void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var btnDelete = (Button)sender;
                var row = (GridViewRow)btnDelete.NamingContainer;
                string id = row.Cells[0].Text;
                decimal check = OfcheckData(id);
                if (check == 0) //ตรวจสอบจำนวนสมาชิกที่มีในสังกัดที่จะลบ
                {
                    ExecuteDataSource exed1 = new ExecuteDataSource(this);
                    string sql1 = "delete from mbucfmembgroup where coop_id='" + state.SsCoopControl + "' and membgroup_code='" + row.Cells[0].Text.Trim() + "'";
                    exed1.SQL.Add(sql1);
                    exed1.Execute();
                    LtServerMessage.Text = WebUtil.CompleteMessage("ลบข้อมูลสำเร็จ");
                    retreivedata();
                }
                else
                {
                    this.SetOnLoadedScript("alert('สังกัดที่ต้องการลบ ไม่สามารถทำการลบได้ เนื่องจากมีข้อมูลที่เกี่ยวข้องอยู่')");
                    LtServerMessage.Text = WebUtil.ErrorMessage("สังกัด " + row.Cells[0].Text.Trim() + " ที่ต้องการลบไม่สามารถทำการลบได้ เนื่องจากมีข้อมูลที่เกี่ยวข้องอยู่");
                }
                retreivedata();

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
            }
        }

        private decimal OfcheckData(string membgroup_code)
        {
            decimal check_status = 0;
            string sql = @"select count(membgroup_code) as c 
                    from mbmembmaster where coop_id= {0} and trim(membgroup_code) = {1}";
            sql = WebUtil.SQLFormat(sql, state.SsCoopControl, membgroup_code.Trim());
            Sdt dt = WebUtil.QuerySdt(sql);
            if (dt.Next())
            {
                check_status = dt.GetDecimal("c");
            }
            return check_status;
        }

        public void btnEditing_Click(object sender, EventArgs e)
        {
            var btnEditing = (Button)sender;
            var row = (GridViewRow)btnEditing.NamingContainer;
            string id = row.Cells[0].Text;
            this.SetOnLoadedScript("Gcoop.OpenIFrame3('630', '450', 'wd_mem_adjmembgroup.aspx','?ls_membgroup_code=" + id + "');");
        }
        
    }
}
        
