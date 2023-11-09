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
using System.Globalization;

namespace Saving.Applications.assist.ws_as_assistbegin_ctrl
{
    public partial class ws_as_assistbegin : PageWebSheet, WebSheet
    {

        [JsPostBack]
        public String jsPostGetlist { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsList.InitDsList(this);

        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {

            }
        }

        public void CheckJsPostBack(string eventArg)
        {

            if (eventArg == jsPostGetlist)
            {
                Int32 account_year = 0;
                account_year = Convert.ToInt32(dsMain.DATA[0].assist_year) - 543;
                dsList.RetrieveList(state.SsCoopId, account_year);

            }

        }

        public void SaveWebSheet()
        {
            try
            {
                Int32 assist_year = 0;
                assist_year = Convert.ToInt32(dsMain.DATA[0].assist_year) - 543;
                String sqlStr = "";
                int row = dsList.RowCount;
                string assisttype_code = "", assisttype_desc = "";
                Decimal begin_amt = 0;
                int num = 0;

                for (int i = 0; i < row; i++)
                {
                    assisttype_code = dsList.DATA[i].ASSISTTYPE_CODE.Trim();
                    begin_amt = dsList.DATA[i].BEGIN_AMOUNT;
                    assisttype_desc = dsList.DATA[i].ASSISTTYPE_DESC.Trim();

                    num = i + 1;

                    string sql = @"select * from asssumledgeryear WHERE coop_id={0} and  assist_year={1} and assisttype_code={2}";
                    sql = WebUtil.SQLFormat(sql, state.SsCoopId, assist_year, assisttype_code);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Next())
                        {
                            string sqlupdate = @"UPDATE asssumledgeryear SET begin_amount = '" + begin_amt + "' WHERE assist_year = '" +
                                assist_year + "' and assisttype_code='" + assisttype_code + "' and coop_id = '" + state.SsCoopId + "'";
                            Sdt update = WebUtil.QuerySdt(sqlupdate);
                        }
                    }
                    else
                    {

                        sqlStr = @"INSERT INTO asssumledgeryear 
                            (coop_id,assist_year,assisttype_code, assisttype_desc,begin_amount,item_amount, forward_amount)
                            VALUES ({0},{1},{2},{3},{4},{5},{6})";
                        sqlStr = WebUtil.SQLFormat(sqlStr, state.SsCoopId, assist_year, assisttype_code,assisttype_desc, begin_amt, 0,0);
                        WebUtil.ExeSQL(sqlStr);
                    }
                }

                LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย");
                assist_year = Convert.ToInt32(dsMain.DATA[0].assist_year) - 543;
                dsList.RetrieveList(state.SsCoopId, assist_year);

            }
            catch (Exception ex)
            {
                LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเสร็จ " + ex);
            }
        }


        public void WebSheetLoadEnd()
        {

        }


    }
}