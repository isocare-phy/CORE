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
using System.Threading;

namespace Saving.Applications.arc
{
    public partial class w_sheet_constant_getting : PageWebSheet, WebSheet
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
                dw_ItemType.InsertRow(0);
                DwUtil.RetrieveDataWindow(dw_ItemType, pbl, null, null);
                //DwFolder.InsertRow(0);
                //DwUtil.RetrieveDataWindow(DwFolder, pbl, null, null);
            }
            else
            {
                this.RestoreContextDw(dw_ItemType);
                this.RestoreContextDw(dw_Domain);
                //this.RestoreContextDw(DwFolder);
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
            else if (eventArg == "postBranchshow")
            {
                Branchshow();
            }
        }

        public void SaveWebSheet()
        {

        }

        public void WebSheetLoadEnd()
        {
            dw_ItemType.SaveDataCache();
            dw_Domain.SaveDataCache();
            //DwFolder.SaveDataCache();
            //dw_Group.SaveDataCache();  Rm
            //dw_SubGroup.SaveDataCache(); Rm
        }

        private void Branchshow()
        {
            //========================================================================//
            //int row = int.Parse(HdrowItemtype.Value);
            //item_type_id = dw_ItemType.GetItemString(row, "itemtype_id");
            //DwUtil.RetrieveDataWindow(dw_Domain, pbl, null, item_type_id);
            //DwUtil.RetrieveDDDW(dw_Domain, "itemtype_id", pbl, null);
            //HdItemType.Value = item_type_id;
            //========================================================================//
            int row = int.Parse(HdrowItemtype.Value);
            domain_id = dw_ItemType.GetItemString(row, "domain_id");
            DwUtil.RetrieveDataWindow(dw_Domain, pbl, null, domain_id);
            HdItemType.Value = domain_id;

        }

        /*****
       private void ShowDomain()
       {
           int row = int.Parse(HdrowItemtype.Value);
           item_type_id = dw_ItemType.GetItemString(row, "itemtype_id");
           DwUtil.RetrieveDataWindow(dw_Domain, pbl, null, item_type_id);
           DwUtil.RetrieveDDDW(dw_Domain, "itemtype_id", pbl, null);
           //
           HdItemType.Value = item_type_id;
       }


       
         
     
       private void ShowGroup()
       {
           int row = int.Parse(HdrowDomain.Value);
           item_type_id = dw_Domain.GetItemString(row, "itemtype_id");
           domain_id = dw_Domain.GetItemString(row, "domain_id");

           object[] args = new object[2];
           args[0] = item_type_id;
           args[1] = domain_id;

           DwUtil.RetrieveDataWindow(dw_Group, pbl, null, args);
           DwUtil.RetrieveDDDW(dw_Group, "itemtype_id", pbl, null);
           DwUtil.RetrieveDDDW(dw_Group, "domain_id", pbl, null);

           //
           HdItemType.Value = item_type_id;
           HdDomain.Value = domain_id;

       }
       private void ShowSubGroup()
       {
           int row = int.Parse(HdrowGroup.Value);
           item_type_id = dw_Group.GetItemString(row, "itemtype_id");
           domain_id = dw_Group.GetItemString(row, "domain_id");
           group_id = dw_Group.GetItemString(row, "group_id");

           object[] args = new object[3];
           args[0] = item_type_id;
           args[1] = domain_id;
           args[2] = group_id;

           DwUtil.RetrieveDataWindow(dw_SubGroup, pbl, null, args);
           DwUtil.RetrieveDDDW(dw_SubGroup, "itemtype_id", pbl, null);
           DwUtil.RetrieveDDDW(dw_SubGroup, "domain_id", pbl, null);
           DwUtil.RetrieveDDDW(dw_SubGroup, "group_id", pbl, null);

           HdItemType.Value = item_type_id;
           HdDomain.Value = domain_id;
           HdGroup.Value = group_id;
       }
         
        *****/
       
    }
}