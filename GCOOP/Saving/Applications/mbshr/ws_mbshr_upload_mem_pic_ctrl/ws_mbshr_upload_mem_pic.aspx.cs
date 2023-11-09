using System;
using CoreSavingLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Collections.Generic;


namespace Saving.Applications.mbshr.ws_mbshr_upload_mem_pic_ctrl
{
    public partial class ws_mbshr_upload_mem_pic : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public string PostMembNo { get; set; }

        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
        }

        public void WebSheetLoadBegin()
        {
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostMembNo)
            {
                string memb_no = WebUtil.MemberNoFormat(dsMain.DATA[0].MEMBER_NO);
                dsMain.Retrieve(memb_no);
                dsMain.DATA[0].MEMBER_NO = memb_no;

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
            string member_no = WebUtil.MemberNoFormat(dsMain.DATA[0].MEMBER_NO);
            bool chk_profile = true;
            bool chk_signature = true;
            bool chk_dept = true;
            bool chk_dept2 = true;
            string err_mes = "";
            string extendPath = WebUtil.getFilepathExtendRoot();
            try //รูปโปรไฟล์สมาชิก
            {
                if (UploadProfile.HasFile)
                {
                    string fileNameProfile = Path.GetFileName(UploadProfile.PostedFile.FileName);
                    //UploadProfile.PostedFile.SaveAs(Server.MapPath("~/ImageMember/profile/") + "profile_" + member_no + ".jpg"); //เนื่องจากบันทึกไว้ที่ core ทำใหม่ให้บันทึกที่ extend
                    string extendPathsToupProfile = extendPath + "ImageMember\\profile\\" + "profile_" + member_no + ".jpg";
                    UploadProfile.PostedFile.SaveAs(extendPathsToupProfile);
                    chk_profile = true;
                }
            }
            catch (Exception ex)
            {
                chk_profile = false;
                err_mes += " รูปโปรไฟล์สมาชิก:" + ex.Message;
            }
            try //รูปลายเซ็นสมาชิก
            {
                if (UploadSignature.HasFile)
                {
                    string fileNameSignature = Path.GetFileName(UploadSignature.PostedFile.FileName);
                    //UploadSignature.PostedFile.SaveAs(Server.MapPath("~/ImageMember/signature/") + "signature_" + member_no + ".jpg"); //เนื่องจากบันทึกไว้ที่ core ทำใหม่ให้บันทึกที่ extend
                    string extendPathsToupSign = extendPath + "ImageMember\\signature\\" + "signature_" + member_no + ".jpg";
                    UploadSignature.PostedFile.SaveAs(extendPathsToupSign);
                    chk_signature = true;
                }
            }
            catch (Exception ex)
            {
                chk_signature = false;
                err_mes += " รูปลายเซ็นสมาชิก:" + ex.Message;
            }
            try //รูปลายเซ็นบัญชีเงินฝากรูปที่ 1
            {
                if (UploadDept.HasFile)
                {
                    string fileNameDept = Path.GetFileName(UploadDept.PostedFile.FileName);
                    string dept_acc = DeptAcc.Text;

                    if ( state.SsCoopControl != "028001" )//by num ราชบุรีไม่ต้องใช่ FORMART
                    {
                        dept_acc = WebUtil.ViewAccountNoFormat(dept_acc);
                    }

                    if (dept_acc != "")
                    {
                        //UploadDept.PostedFile.SaveAs(Server.MapPath("~/ImageMember/dept/") + "d" + dept_acc + "_1.bmp"); //เนื่องจากบันทึกไว้ที่ core ทำใหม่ให้บันทึกที่ extend
                        string extendPathsToupDep1 = extendPath + "ImageMember\\dept\\" + "d" + dept_acc + "_1.bmp";
                        UploadDept.PostedFile.SaveAs(extendPathsToupDep1);
                        chk_dept = true;
                    }
                }
            }
            catch (Exception ex)
            {
                chk_dept = false;
                err_mes += " รูปลายเซ็นบัญชีเงินฝากรูปที่ 1:" + ex.Message;
            }

            try //รูปลายเซ็นบัญชีเงินฝากรูปที่ 2
            {
                if (UploadDept_2.HasFile)
                {
                    string fileNameDept = Path.GetFileName(UploadDept_2.PostedFile.FileName);
                    string dept_acc = DeptAcc.Text;

                    if (state.SsCoopControl != "028001")//by num ราชบุรีไม่ต้องใช่ FORMART
                    {
                        dept_acc = WebUtil.ViewAccountNoFormat(dept_acc);
                    }

                    if (dept_acc != "")
                    {
                        //UploadDept_2.PostedFile.SaveAs(Server.MapPath("~/ImageMember/dept/") + "d" + dept_acc + "_2.bmp"); //เนื่องจากบันทึกไว้ที่ core ทำใหม่ให้บันทึกที่ extend
                        string extendPathsToupDep2 = extendPath + "ImageMember\\dept\\" + "d" + dept_acc + "_2.bmp";
                        UploadDept_2.PostedFile.SaveAs(extendPathsToupDep2);
                        chk_dept2 = true;
                    }
                }
            }
            catch (Exception ex)
            {
                chk_dept2 = false;
                err_mes += " รูปลายเซ็นบัญชีเงินฝากรูปที่ 2:" + ex.Message;
            }

            if (chk_profile && chk_signature && chk_dept && chk_dept2)
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