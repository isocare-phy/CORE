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
using CoreSavingLibrary.WcfNCommon;
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.arc
{
    public partial class w_sheet_constant_fold : PageWebSheet, WebSheet
    {
        protected string pbl = "hr_constant.pbl";
        protected string postShowDomain;
        protected string postShowGroup;
        protected string postShowSubGroup;
        protected string postBranchshow;

        private string item_type_id, domain_id, group_id;
        public void InitJsPostBack()
        {
            postShowDomain = WebUtil.JsPostBack(this, "postShowDomain");
            postShowGroup = WebUtil.JsPostBack(this, "postShowGroup");
            postShowSubGroup = WebUtil.JsPostBack(this, "postShowSubGroup");
            postBranchshow = WebUtil.JsPostBack(this, "postBranchshow");
        }

        public void WebSheetLoadBegin()
        {
            this.ConnectSQLCA();
            if (!IsPostBack)
            {
               
                DwFolder.InsertRow(0);
                DwUtil.RetrieveDataWindow(DwFolder, pbl, null, null);
            }
            else
            {
                
                this.RestoreContextDw(DwFolder);
                //this.RestoreContextDw(dw_Group); Rm
                //this.RestoreContextDw(dw_SubGroup); Rm
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == "postShowDomain")
            {
                //ShowDomain();
            }
            else if (eventArg == "postShowGroup")
            {
                //ShowGroup();
            }
            else if (eventArg == "postShowSubGroup")
            {
                //ShowSubGroup();
            }
           
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            DwFolder.SaveDataCache();       
        }
    }
}