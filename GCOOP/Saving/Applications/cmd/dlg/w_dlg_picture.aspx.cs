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

namespace Saving.Applications.cmd.dlg
{
    public partial class w_dlg_picture : System.Web.UI.Page
    {
        private DwTrans SQLCA;
        private WebState state;
        private String product_id;
        private String picture_field;
        //PROTECTED



        protected void Page_Load(object sender, EventArgs e)
        {
            state = new WebState(Session, Request);
            SQLCA = new DwTrans();
           
            try
            {
                product_id = Request["product_id"];
                picture_field = Request["picture_field"];


                if (product_id != "" && picture_field != "")
                {
                    String imageUrl = state.SsUrl + "Applications/cmd/picture/" + product_id + picture_field + ".jpg";
                    Image1.ImageUrl = imageUrl;
                    Label1.Text = "รูปภาพครุภัณฑ์เลขที่ " + product_id + "/" + picture_field;
                }
                else
                {
                    Image1.ImageUrl = "";
                    Label1.Text = "";
                }
                TextBox1.Text = product_id;
            }
            catch
            {
                product_id = "";
                picture_field = "";
            }

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //if (product_id != "" && picture_field != "")
            //{
            //    String imageUrl = state.SsUrl + "Applications/cmd/picture/" + product_id + picture_field + ".jpg";
            //    Image1.ImageUrl = imageUrl;
            //    Label1.Text = "รูปภาพครุภัณฑ์เลขที่ " + product_id + "/" + picture_field;
            //}
            //else
            //{
            //    Image1.ImageUrl = "";
            //    Label1.Text = "";
            //}
            try
            {
                SQLCA.Disconnect();
            }
            catch { }
        }


        protected void Save_Click(object sender, EventArgs e)
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
                    Label2.Text = "ไม่สามารถ อัปโหลดรูปได้  <br />";
                }
            }
            else
            {
                if (FileUpload.PostedFile == null)
                {
                    Label2.Text = "ไม่พบรูป <br />";
                }
                else
                {
                    Label2.Text = FileUpload.PostedFile.ToString();
                }
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
        }
    }
}
