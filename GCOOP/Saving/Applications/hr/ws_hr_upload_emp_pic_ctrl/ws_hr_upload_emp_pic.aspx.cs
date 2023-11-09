using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Collections.Generic;

namespace Saving.Applications.hr.ws_hr_upload_emp_pic_ctrl
{
    public partial class ws_hr_upload_emp_pic : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostEmpNo { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpNo)
            {
                string emp_no = dsMain.DATA[0].EMP_NO;
                dsMain.Retrieve(emp_no);
                //dsMain.DATA[0].EMP_NO = emp_no;

                //dsMain.DATA[0]. = emp_no;


            }
        }

        public void SaveWebSheet()
        {
        }

        public void WebSheetLoadEnd()
        {
        }

        protected void Upload(object sender, EventArgs e)
        {
            string emp_no = WebUtil.MemberNoFormat(dsMain.DATA[0].EMP_NO);
            bool chk_profile = true;
            string err_mes = "";

                try //รูปโปรไฟล์สมาชิก
                {
                    if (UploadProfile.HasFile)
                    {
                        string fileNameProfile = Path.GetFileName(UploadProfile.PostedFile.FileName);
                        UploadProfile.PostedFile.SaveAs(Server.MapPath("~/ImageEmployee/profile/") + "profile_" + emp_no.Trim() + ".jpg");
                        //LtServerMessege.Text = WebUtil.CompleteMessage("บันทึกรูปโปรไฟล์สำเร็จ");
                        // Response.Redirect(state.SsUrl);
                        chk_profile = true;  
                    }

                }
                catch (Exception ex)
                {
                    chk_profile = false;
                    err_mes += "รูปโปรไฟล์เจ้าหน้าที่:" + ex.Message;
                }

                if (chk_profile)
                {
                    LtServerMessege.Text = WebUtil.CompleteMessage("บันทึกรูปสำเร็จ");
                }
                else
                {
                    LtServerMessege.Text = WebUtil.ErrorMessage(err_mes);
                }
            }
    }
}