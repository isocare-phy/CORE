using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using System.Data;

namespace Saving.Applications.mbshr.dlg.w_dlg_mbshr_mbgain_history_ctrl
{
    public partial class DsList : DataSourceRepeater
    {
        public DataSet1.MBREQGAINDETAILDataTable DATA { get; set; }

        public void InitDsList(PageWeb pw)
        {
            css1.Visible = false;

            DataSet1 ds = new DataSet1();
            this.DATA = ds.MBREQGAINDETAIL;
            this.InitDataSource(pw, Repeater1, this.DATA, "dsList");
            this.EventClicked = "OnDsListClicked";
            this.EventItemChanged = "OnDsListItemChanged";
            this.Register();
        }

        public void RetrieveList(string member_no)
        {
            string sql = @" select d.seq_no , g.gain_docno , g.member_no , d.gain_name || ' ' || d.gain_surname as gainname , g.WRITE_DATE
                            from mbreqgain g , mbreqgaindetail d
                            where
                            g.coop_id = d.coop_id and
                            g.gain_docno = d.gain_docno and
                            d.gain_chgtype = 'OLD' and
                            d.coop_id = '" + state.SsCoopControl + @"' and
                            g.member_no = '" + member_no + @"'
                            order by d.seq_no asc";
            sql = WebUtil.SQLFormat(sql);
            DataTable dt = WebUtil.Query(sql);
            this.ImportData(dt);

        }
    }
}