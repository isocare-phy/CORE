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
using CoreSavingLibrary.WcfAgency;
using CoreSavingLibrary.WcfCommon;
using System.Web.Services.Protocols;
using DataLibrary;
using System.Globalization;
using System.IO;

namespace Saving.Applications.agency.dlg
{
    public partial class w_dlg_ag_membdet_picture_popup : PageWebDialog,WebDialog 
    {
        //=================================
        private void JspostNewClear()
        {
            lbl_agentrequestno.Text = "";
            lbl_name.Text = "";
            lbl_memberno.Text = "";
            Img_picture.ImageUrl = Img_picture.ImageUrl = state.SsUrl + "Applications\\agency\\image\\picture\\" + "icon_guest.jpg";
            Img_signature.ImageUrl = Img_signature.ImageUrl = state.SsUrl + "Applications\\agency\\image\\signature\\" + "icon_guest.jpg";
        }
        //=================================
        #region WebDialog Members

        public void InitJsPostBack()
        {

        }

        public void WebDialogLoadBegin()
        {
            if (!IsPostBack)
            {
                this.ConnectSQLCA();
                try
                {
                    String agentrequest_no = null;
                    String seq_no = null;

                    try { agentrequest_no = Request["agentrequest_no"].Trim(); }
                    catch { agentrequest_no = null; }

                    try { seq_no = Request["seq_no"].Trim(); }
                    catch { seq_no = null; }

                    // Request ค่ามาจาก w_sheet_ag_membdet
                    if (agentrequest_no != null && seq_no != null)
                    {
                        // search หา prename_code,memb_name,memb_surname, picture_name, signature_name
                        String prename_code = null;
                        String prename_desc = null;
                        String memb_name = null; 
                        String memb_surname = null;
                        String picture_name = null;
                        String signature_name = null;
                        String member_no = null;
                      
                        Sta ta = new Sta(sqlca.ConnectionString);
                        try 
                        {
                            String sql = @"select member_no, prename_code, memb_name, memb_surname, picture_name, signature_name from agmembagent where agentrequest_no = '"+ agentrequest_no +"' and seq_no = '"+ seq_no +"'";
                            Sdt dt = ta.Query(sql);
                            if (dt.Next())
                            {
                                try { member_no = dt.GetString("member_no"); }
                                catch { }
                                try { prename_code = dt.GetString("prename_code"); }
                                catch { }
                                try { memb_name = dt.GetString("memb_name"); }
                                catch { }
                                try { memb_surname = dt.GetString("memb_surname"); }
                                catch { }
                                try { picture_name = dt.GetString("picture_name"); }
                                catch { }
                                try { signature_name = dt.GetString("signature_name"); }
                                catch { }

                                Sta ta1 = new Sta(sqlca.ConnectionString);
                                String sql1 = @"select prename_desc from mbucfprename where prename_code = '" + prename_code + "'";
                                Sdt dt1 = ta1.Query(sql1);
                                if (dt1.Next())
                                {
                                    prename_desc = dt1.GetString("prename_desc");
                                    lbl_name.Text = prename_desc +" "+ memb_name + "  " + memb_surname;
                                    lbl_memberno.Text = member_no;
                                    lbl_agentrequestno.Text = agentrequest_no;

                                    if (picture_name != null || picture_name == "")
                                    {
                                        Img_picture.Width = 300;
                                        Img_picture.Height = 300;
                                        Img_picture.ImageUrl = picture_name;
                                    }

                                    if (signature_name != null || signature_name == "")
                                    {
                                        Img_signature.Width = 300;
                                        Img_signature.Height = 300;
                                        Img_signature.ImageUrl = signature_name;
                                    }
                                    ta.Close();
                                    ta1.Close();
                                }
                            }
                        }
                        catch(Exception ex) { }
                    }
                    else 
                    {
                        JspostNewClear();
                    }
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage(ex.Message);
                }
            }
            else
            {
                
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
        }

        public void WebDialogLoadEnd()
        {
        }

        #endregion
    }
}
