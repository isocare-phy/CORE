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
using System.IO;

namespace Saving.Applications.cmd
{
    public partial class w_sheet_cmd_picture : PageWebSheet, WebSheet
    {
        private DwTrans sqlca;
        private WebState state;
        private String product_id;
        private String picture_field;
      
        protected void Button1_Click(object sender, EventArgs e)
        {
            // int userId = state.getUserId();
            if ((FileUpload.PostedFile != null) && (FileUpload.PostedFile.ContentLength > 0) && product_id != "")
            {
                try
                {
                    String filename = Path.GetFileName(FileUpload.PostedFile.FileName);
                    //String save = state.SsUrl + "Application\\cmd\\picture\\" + product_id + ".jpg";
                    String save = Server.MapPath("") + "\\picture\\" + product_id + picture_field + ".jpg";
                    FileUpload.PostedFile.SaveAs(save);

                }
                catch (Exception ex)
                {
                    ex.ToString();
                    Label2.Text = "No UpLoad picture <br />";
                }
            }
            else
            {
                if (FileUpload.PostedFile == null)
                {
                    Label2.Text = "No picture source<br />";
                }
                else
                {
                    Label2.Text = "Please login<br />";
                }
            }
        }

        protected void DeleteB_Click(object sender, EventArgs e)
        {
        }

        #region WebSheet Members

        public void InitJsPostBack()
        {
            throw new NotImplementedException();
        }

        public void WebSheetLoadBegin()
        {

            try
            {
                product_id = Request["product_id"];
                picture_field = Request["picture_field"];
            }
            catch
            {
                product_id = "";
                picture_field = "";
            }

        }

        public void CheckJsPostBack(string eventArg)
        {
            throw new NotImplementedException();
        }

        public void SaveWebSheet()
        {
            throw new NotImplementedException();
        }

        public void WebSheetLoadEnd()
        {
            if (product_id != "" && picture_field != "")
            {
                //ที่อยู่ของรูป ดึงขึ้นมาแสดง
                String imageUrl = state.SsUrl + "Applications/cmd/picture/" + product_id + picture_field + ".jpg";
                Image1.ImageUrl = imageUrl;
                Label1.Text = "รูปภาพครุภัณฑ์เลขที่ " + product_id + "/" + picture_field;
            }
            else
            {
                Image1.ImageUrl = "";
                Label1.Text = "";
            }
            try
            {
                sqlca.Disconnect();
            }
            catch { }
        }

        #endregion
    }
}
