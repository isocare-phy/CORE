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
using DataLibrary;
using Sybase.DataWindow;
using System.Web.Services.Protocols;

namespace Saving.Applications.ap_deposit
{
    public partial class w_sheet_as_balance_carried : PageWebDialog, WebDialog
    {
        protected String postAddDistance;
        protected String postEditDistance;
        protected String postSaveEdit;
        protected String postRefresh;
         

        public void InitJsPostBack()
        {
            postAddDistance = WebUtil.JsPostBack(this, "postAddDistance");
            postEditDistance = WebUtil.JsPostBack(this, "postEditDistance");
            postSaveEdit = WebUtil.JsPostBack(this, "postSaveEdit");
            postRefresh = WebUtil.JsPostBack(this, "postRefresh");
        
        }

        public void WebDialogLoadBegin()
        {
            throw new System.NotImplementedException();
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

            else if (eventArg == "postRefresh")
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            throw new NotImplementedException();
        }

        private void SaveEdit()
        {
            throw new NotImplementedException();
        }

        private void EditDistance()
        {
            throw new NotImplementedException();
        }

        private void AddDistance()
        {
            throw new NotImplementedException();
        }

        public void WebDialogLoadEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}