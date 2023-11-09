using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreSavingLibrary;
using DataLibrary;

namespace Saving.Applications.hr.ws_hr_master_ctrl
{
    public partial class ws_hr_master : PageWebSheet, WebSheet
    {
        [JsPostBack]
        public String PostEmpNo { get; set; }
        [JsPostBack]
        public String PostInsertRowFamily { get; set; }
        //[JsPostBack]
        //public String PostInsertRowCureFamily { get; set; }
        [JsPostBack]
        public String PostInsertRowFamily_Hos { get; set; }
        [JsPostBack]
        public String PostInsertRowEdu { get; set; }
        [JsPostBack]
        public String PostInsertRowExperience { get; set; }
        [JsPostBack]
        public String PostInsertRowTraining { get; set; }
        [JsPostBack]
        public String PostInsertRowAssist { get; set; }
        [JsPostBack]
        public String PostDeleteFamilyRow { get; set; }
        //[JsPostBack]
        //public String PostDeleteCureFamilyRow { get; set; }
        [JsPostBack]
        public String PostDeleteFamily_HosRow { get; set; }
        [JsPostBack]
        public String PostDeleteEduRow { get; set; }
        [JsPostBack]
        public String PostDeleteExperienceRow { get; set; }
        [JsPostBack]
        public String PostDeleteTrainingRow { get; set; }
        [JsPostBack]
        public String PostDeleteAssistRow { get; set; }
        [JsPostBack]
        public String PostAdnProvince { get; set; }
        [JsPostBack]
        public String PostAdrProvince { get; set; }
        [JsPostBack]
        public String PostAdnAmphur { get; set; }
        [JsPostBack]
        public String PostAdrAmphur { get; set; }
        [JsPostBack]
        public String PostExpBank { get; set; }
        [JsPostBack]
        public String PostInsertRowGuarantee { get; set; }
        [JsPostBack]
        public String PostDeleteGaranteeRow { get; set; }
        [JsPostBack]
        public String PostInsertRowDisoffense { get; set; }
        [JsPostBack]
        public String PostDeleteDisoffenseRow { get; set; }
        [JsPostBack]
        public String displeyAddress { get; set; }

        string emptype_code_checkempnew = "";




        public void InitJsPostBack()
        {
            dsMain.InitDsMain(this);
            dsDetail.InitDsDetail(this);
            dsSalary.InitDsSalary(this);
            dsFamily.InitDsFamily(this);
            //dsCurefamily.InitDsCurefamily(this);
           // dsFamily_Hos.InitDsFamily_Hos(this);
            dsEdu.InitDsEdu(this);
            dsExperience.InitDsExperience(this);
            dsTraining.InitTraining(this);
            dsAssist.InitDsAssist(this);
            dsGuarantee.InitDsGuarantee(this);
            dsLeaves.InitDsLeave(this);
            dsMovework.InitDsMovework(this);
            dsDisoffense.InitDsDisoffense(this);
            dsLeaveout.InitDsLeaveout(this);


        }

        public void WebSheetLoadBegin()
        {
            if (!IsPostBack)
            {
                NewClear();
            }
        }

        public void CheckJsPostBack(string eventArg)
        {
            if (eventArg == PostEmpNo)
            {
                string ls_empno = dsMain.DATA[0].EMP_NO;

                String sql_emptype = @"select   
                             emptype_code from hremployee where emptype_code = '02' and emp_no = '" + ls_empno + "'";

                sql_emptype = WebUtil.SQLFormat(sql_emptype);
                Sdt dt_emptype = WebUtil.QuerySdt(sql_emptype);
                if (dt_emptype.Next())
                {
                    emptype_code_checkempnew = dt_emptype.GetString("emptype_code");
                }

                dsMain.Retrieve(ls_empno);
                dsDetail.Retrieve(ls_empno);
                dsSalary.Retrieve(ls_empno);
                dsFamily.Retrieve(ls_empno);
                //dsCurefamily.Retrieve(ls_empno);
               // dsFamily_Hos.Retrieve(ls_empno);
                dsEdu.Retrieve(ls_empno);
                dsExperience.Retrieve(ls_empno);
                dsTraining.Retrieve(ls_empno);
                dsAssist.Retrieve(ls_empno);
                dsGuarantee.RetrieveHrGuarantee(ls_empno);
                dsLeaves.RetrieveHrLeaves(ls_empno);
                dsMovework.Retrieve(ls_empno);
                dsDisoffense.Retrieve(ls_empno);
                dsLeaveout.RetrieveHrLeaveout(ls_empno);
                dsMain.DdDeptgrp();
                dsMain.DdPosition();
                dsMain.DdEmptype();
                dsMain.DdPrename();
                dsDetail.DdProvince();
                dsDetail.DdAdnAmphur(dsDetail.DATA[0].ADN_PROVINCE);
                dsDetail.DdAdrAmphur(dsDetail.DATA[0].ADR_PROVINCE);
                dsDetail.DdAdnTambol(dsDetail.DATA[0].ADN_AMPHUR);
                dsDetail.DdAdrTambol(dsDetail.DATA[0].ADR_AMPHUR);
                dsDetail.DdBloodtype();
                dsEdu.DdEducation();
                dsAssist.DdAssist();
                dsDisoffense.DdDisoffense();
            }
            else if (eventArg == PostInsertRowFamily)
            {
                dsFamily.InsertLastRow();
            }
           /* else if (eventArg == PostInsertRowCureFamily)
            {
                dsCurefamily.InsertLastRow();
            }
            else if (eventArg == PostInsertRowFamily_Hos)
            {
                dsFamily_Hos.InsertLastRow();
            }*/
            else if (eventArg == PostInsertRowEdu)
            {
                dsEdu.InsertLastRow();
                dsEdu.DdEducation();
            }
            else if (eventArg == PostInsertRowExperience)
            {
                dsExperience.InsertLastRow();
            }
            else if (eventArg == PostInsertRowTraining)
            {
                dsTraining.InsertLastRow();
            }
            else if (eventArg == PostInsertRowAssist)
            {
                dsAssist.InsertLastRow();
                dsAssist.DdAssist();
            }
            else if (eventArg == PostDeleteFamilyRow)
            {
                int row = dsFamily.GetRowFocus();
                dsFamily.DeleteRow(row);
            }
           /* else if (eventArg == PostDeleteCureFamilyRow)
            {
                int row = dsCurefamily.GetRowFocus();
                dsCurefamily.DeleteRow(row);
            }
            else if (eventArg == PostDeleteFamily_HosRow)
            {
                int row = dsFamily_Hos.GetRowFocus();
                dsFamily_Hos.DeleteRow(row);
            }*/
            else if (eventArg == PostDeleteEduRow)
            {
                int row = dsEdu.GetRowFocus();
                dsEdu.DeleteRow(row);
                dsEdu.DdEducation();
            }
            else if (eventArg == PostDeleteExperienceRow)
            {
                int row = dsExperience.GetRowFocus();
                dsExperience.DeleteRow(row);
            }
            else if (eventArg == PostDeleteTrainingRow)
            {
                int row = dsTraining.GetRowFocus();
                dsTraining.DeleteRow(row);
            }
            else if (eventArg == PostDeleteAssistRow)
            {
                int row = dsAssist.GetRowFocus();
                dsAssist.DeleteRow(row);
                dsAssist.DdAssist();
            }
            else if (eventArg == PostAdnProvince)
            {
                dsDetail.DATA[0].ADN_AMPHUR = "";
                dsDetail.DATA[0].ADN_TAMBOL = "";
                dsDetail.DATA[0].ADN_POSTCODE = "";
                dsDetail.DdAdnAmphur(dsDetail.DATA[0].ADN_PROVINCE);
            }
            else if (eventArg == PostAdrProvince)
            {
                dsDetail.DATA[0].ADR_AMPHUR = "";
                dsDetail.DATA[0].ADR_TAMBOL = "";
                dsDetail.DATA[0].ADR_POSTCODE = "";
                dsDetail.DdAdrAmphur(dsDetail.DATA[0].ADR_PROVINCE);
            }
            else if (eventArg == PostAdnAmphur)
            {
                dsDetail.DATA[0].ADN_TAMBOL = "";
                dsDetail.DdAdnTambol(dsDetail.DATA[0].ADN_AMPHUR);
                String sql = @"SELECT MBUCFDISTRICT.POSTCODE   
                    FROM MBUCFDISTRICT,  
	                    MBUCFPROVINCE         	                        
                    WHERE ( MBUCFPROVINCE.PROVINCE_CODE = MBUCFDISTRICT.PROVINCE_CODE )   
	                    and ( MBUCFDISTRICT.PROVINCE_CODE = {0} ) 
	                    and ( MBUCFDISTRICT.DISTRICT_CODE = {1} )";
                sql = WebUtil.SQLFormat(sql, dsDetail.DATA[0].ADN_PROVINCE, dsDetail.DATA[0].ADN_AMPHUR);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dsDetail.DATA[0].ADN_POSTCODE = dt.Rows[0]["postcode"].ToString();
                }
            }
            else if (eventArg == PostAdrAmphur)
            {
                dsDetail.DATA[0].ADR_TAMBOL = "";
                dsDetail.DdAdrTambol(dsDetail.DATA[0].ADR_AMPHUR);
                String sql = @"SELECT MBUCFDISTRICT.POSTCODE   
                    FROM MBUCFDISTRICT,  
	                    MBUCFPROVINCE         	                        
                    WHERE ( MBUCFPROVINCE.PROVINCE_CODE = MBUCFDISTRICT.PROVINCE_CODE )   
	                    and ( MBUCFDISTRICT.PROVINCE_CODE = {0} ) 
	                    and ( MBUCFDISTRICT.DISTRICT_CODE = {1} )";
                sql = WebUtil.SQLFormat(sql, dsDetail.DATA[0].ADR_PROVINCE, dsDetail.DATA[0].ADR_AMPHUR);
                Sdt dt = WebUtil.QuerySdt(sql);
                if (dt.Next())
                {
                    dsDetail.DATA[0].ADR_POSTCODE = dt.Rows[0]["postcode"].ToString();
                }
            }
            else if (eventArg == PostExpBank)
            {
                string salexp_bank = dsSalary.DATA[0].SALEXP_BANK;
                dsSalary.DdBranch(salexp_bank);
            }
            else if (eventArg == PostInsertRowGuarantee)
            {
                dsGuarantee.InsertLastRow();
            }
            else if (eventArg == PostDeleteGaranteeRow)
            {
                int row = dsGuarantee.GetRowFocus();
                dsGuarantee.DeleteRow(row);
            }
            else if (eventArg == PostInsertRowDisoffense)
            {
                dsDisoffense.InsertLastRow();
                dsDisoffense.DdDisoffense();
            }
            else if (eventArg == PostDeleteDisoffenseRow)
            {
                int row = dsDisoffense.GetRowFocus();
                dsDisoffense.DeleteRow(row);
                dsDisoffense.DdDisoffense();
            }
            else if (eventArg == displeyAddress)
            {

                string sql, address = "", addr_no = "", addr_moo = "", addr_village = ""
                    , addr_road = "", addr_tambol = "", addr_province = "", addr_postcode = "", addr_mobilephone = ""
                    , curraddr_no = "", curraddr_moo = "", curraddr_village = "", curraddr_road = "", curraddr_tambol = "",
                    curraddr_province = "", curraddr_postcode = "", curraddress = "";
                string EmpNo = dsMain.DATA[0].EMP_NO;
                try
                {
                    sql = @"select 
                            hr.emp_no,
                            hr.ref_membno,
                            ma.member_no,
                            ma.addr_no as addr_no,
                            ma.addr_moo as addr_moo,
                            ma.addr_village as addr_village,
                            ma.addr_road as addr_road,
                            ma.addr_mobilephone as  addr_mobilephone,
                            ucftam.tambol_desc as ta,
                            ucfpro.province_desc as pr,
                            ma.addr_postcode as addr_postcode,
                            
                            ma.curraddr_no as curraddr_no,
                            ma.curraddr_moo as curraddr_moo,
                            ma.curraddr_village as curraddr_village,
                            ma.curraddr_road as curraddr_road,
                            ucftam2.tambol_desc as ta2,
                            ucfpro2.province_desc as pr2,
                            ma.curraddr_postcode as curraddr_postcode
                         from
                            hremployee hr,
                            mbmembmaster ma,
                            mbucftambol ucftam,
                            mbucfprovince ucfpro,
                            mbucftambol ucftam2,
                            mbucfprovince ucfpro2
                         where
                            hr.emp_no = {0} and
                            hr.ref_membno=ma.member_no and
                            ma.tambol_code=ucftam.tambol_code and
                            ma.province_code=ucfpro.province_code and
                            ma.tambol_code=ucftam2.tambol_code and
                            ma.province_code=ucfpro2.province_code";
                    sql = WebUtil.SQLFormat(sql, EmpNo);
                    Sdt dt = WebUtil.QuerySdt(sql);
                    if (dt.Next())
                    {
                        addr_no = dt.GetString("addr_no");
                        addr_moo = dt.GetString("addr_moo");
                        addr_village = dt.GetString("addr_village");
                        addr_road = dt.GetString("addr_road");
                        addr_tambol = dt.GetString("ta");
                        addr_province = dt.GetString("pr");
                        addr_postcode = dt.GetString("addr_postcode");
                        addr_mobilephone = dt.GetString("addr_mobilephone");

                        curraddr_no = dt.GetString("curraddr_no");
                        curraddr_moo = dt.GetString("curraddr_moo");
                        curraddr_village = dt.GetString("curraddr_village");
                        curraddr_road = dt.GetString("curraddr_road");
                        curraddr_tambol = dt.GetString("ta2");
                        curraddr_province = dt.GetString("pr2");
                        curraddr_postcode = dt.GetString("curraddr_postcode");
                    }
                    if (addr_no != "")
                    {
                        address = "เลขที่ " + addr_no;
                    }
                    else
                    {
                        address += "";
                    }
                    if (addr_moo != "")
                    {
                        address += " หมู่ที่ " + addr_moo;
                    }
                    else
                    {
                        address += "";
                    }
                    if (addr_village != "")
                    {
                        address += " หมู่บ้าน" + addr_village;
                    }
                    else
                    {
                        address += "";
                    }
                    if (addr_road != "")
                    {
                        address += " ถ." + addr_road;
                    }
                    else
                    {
                        address += "";
                    }
                    if (addr_tambol != "")
                    {
                        address += " ตำบล" + addr_tambol;
                    }
                    else
                    {
                        address += "";
                    }
                    if (addr_province != "")
                    {
                        address += " จ." + addr_province;
                    }
                    else
                    {
                        address += "";
                    }
                    if (addr_postcode != "")
                    {
                        address += " " + addr_postcode;
                    }
                    else
                    {
                        address += "";
                    }

                    //ที่อยู่ตามทะเบียนบ้าน
                    if (curraddr_no != "")
                    {
                        curraddress = "เลขที่ " + curraddr_no;
                    }
                    else
                    {
                        curraddress += "";
                    }
                    if (curraddr_moo != "")
                    {
                        curraddress += " หมู่ที่ " + curraddr_moo;
                    }
                    else
                    {
                        curraddress += "";
                    }
                    if (curraddr_village != "")
                    {
                        curraddress += " หมู่บ้าน" + curraddr_village;
                    }
                    else
                    {
                        curraddress += "";
                    }
                    if (curraddr_road != "")
                    {
                        curraddress += " ถ." + curraddr_road;
                    }
                    else
                    {
                        curraddress += "";
                    }
                    if (curraddr_tambol != "")
                    {
                        curraddress += " ตำบล" + curraddr_tambol;
                    }
                    else
                    {
                        curraddress += "";
                    }
                    if (curraddr_province != "")
                    {
                        curraddress += " จ." + curraddr_province;
                    }
                    else
                    {
                        curraddress += "";
                    }
                    if (curraddr_postcode != "")
                    {
                        curraddress += " " + curraddr_postcode;
                    }
                    else
                    {
                        curraddress += "";
                    }

                    dsDetail.DATA[0].ADN_NO = address;
                    dsDetail.DATA[0].ADR_NO = curraddress;

                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.WarningMessage(ex.ToString());
                }

            }
        }



        public void NewClear()
        {
            dsMain.ResetRow();
            dsDetail.ResetRow();
            dsSalary.ResetRow();
            dsFamily.ResetRow();
            dsEdu.ResetRow();
            dsExperience.ResetRow();
            dsTraining.ResetRow();
            dsAssist.ResetRow();
            dsMovework.ResetRow();
            dsGuarantee.ResetRow();
            dsLeaves.ResetRow();
            dsDisoffense.ResetRow();
            //dsCurefamily.ResetRow();
            //dsFamily_Hos.ResetRow();
            dsMain.DdDeptgrp();
            dsMain.DdPosition();
            dsMain.DdEmptype();
            dsMain.DdPrename();
            dsDetail.DdProvince();
            dsSalary.DdBank();
            dsDetail.DdBloodtype();
        }

        public void SaveWebSheet()
        {
            ExecuteDataSource exe = new ExecuteDataSource(this);
            string coop_id = state.SsCoopControl, ls_empno = "";
            string fullname = dsMain.DATA[0].EMP_NAME + " " + dsMain.DATA[0].EMP_SURNAME;
            string last_documentno = "";
            if (dsMain.DATA[0].EMP_NO == "Auto")
            {
                string sex = dsMain.DATA[0].SEX;
                string type = dsMain.DATA[0].EMPTYPE_CODE;
                if (sex == "M")
                {
                    sex = "1";
                }
                else
                {
                    sex = "2";
                }

                string year = Convert.ToString(WebUtil.GetAccyear(state.SsCoopControl, state.SsWorkDate));

                if (state.SsCoopControl == "031001" || state.SsCoopControl == "030001")
                {
                    Sta ta = new Sta(state.SsConnectionString);
                    string sub_year_ch = year.Substring(2, 2);

                    try
                    {

                        last_documentno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopControl, "HREMPLFILEMAS");
                        //last_documentno = last_documentno;
                        ta.Close();
                    }
                    catch
                    {
                        ta.Close();
                    }


                }
                else // Other Site
                {

                    string sub_year_ch = year.Substring(2, 2);
                    string date_emp = sub_year_ch + "90";
                    //string max_empno = ""; string max_emp = "";
                    decimal emp = 0; decimal max_emp_d = 0;

                    String sql_maxemp = @"select to_number(nvl(max(substr(emp_no,5,1)),0)) as max_empno from hremployee where emptype_code = '02' and substr(emp_no,1,2) = '" + sub_year_ch + "'";
                    Sdt dt_maxemp = WebUtil.QuerySdt(sql_maxemp);
                    if (dt_maxemp.Next())
                    {
                        //max_empno = dt_maxemp.GetString("max_empno");
                        //max_emp = max_empno.Substring(4, 1);
                        //max_emp_d = Convert.ToDecimal(max_emp);
                        max_emp_d = dt_maxemp.GetDecimal("max_empno");
                        emp = max_emp_d + 1;
                    }

                    if (type == "02")
                    {

                        Sta ta = new Sta(state.SsConnectionString);
                        //string postNumber = "";
                        try
                        {

                            last_documentno = date_emp + emp + sex;
                            ta.Close();
                        }
                        catch
                        {
                            ta.Close();
                        }
                    }
                    else
                    {
                        Sta ta = new Sta(state.SsConnectionString);
                        //string postNumber = "";
                        try
                        {
                            //ta.AddInParameter("AVC_COOPID", state.SsCoopId, System.Data.OracleClient.OracleType.VarChar);
                            //ta.AddInParameter("AVC_DOCCODE", "HREMPLFILEMAS", System.Data.OracleClient.OracleType.VarChar);
                            //// ta.AddInParameter("AVC_DOCCODE", sex, System.Data.OracleClient.OracleType.VarChar);
                            //ta.AddReturnParameter("return_value", System.Data.OracleClient.OracleType.VarChar);
                            //ta.ExePlSql("N_PK_DOCCONTROL.OF_GETNEWDOCNO");
                            //last_documentno = ta.OutParameter("return_value").ToString();
                            last_documentno = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopControl, "HREMPLFILEMAS");
                            last_documentno = last_documentno + sex;
                            ta.Close();
                        }
                        catch
                        {
                            ta.Close();
                        }
                    }
                }
                
                dsMain.DATA[0].COOP_ID = coop_id;
                dsMain.DATA[0].EMP_NO = last_documentno;

                dsDetail.DATA[0].COOP_ID = coop_id;
                dsDetail.DATA[0].EMP_NO = last_documentno;

                dsSalary.DATA[0].COOP_ID = coop_id;
                dsSalary.DATA[0].EMP_NO = last_documentno;

                for (int i = 0; i < dsFamily.RowCount; i++)
                {
                    dsFamily.DATA[i].COOP_ID = coop_id;
                    dsFamily.DATA[i].EMP_NO = last_documentno;
                    dsFamily.DATA[i].SEQ_NO = i + 1;
                }
                /*for (int i = 0; i < dsCurefamily.RowCount; i++)
                {
                    dsCurefamily.DATA[i].COOP_ID = coop_id;
                    dsCurefamily.DATA[i].EMP_NO = last_documentno;
                    dsCurefamily.DATA[i].SEQ_NO = i + 1;
                }
                for (int i = 0; i < dsFamily_Hos.RowCount; i++)
                {
                    dsFamily_Hos.DATA[i].COOP_ID = coop_id;
                    dsFamily_Hos.DATA[i].EMP_NO = last_documentno;
                    dsFamily_Hos.DATA[i].SEQ_NO = i + 1;
                }*/
                for (int i = 0; i < dsEdu.RowCount; i++)
                {
                    dsEdu.DATA[i].COOP_ID = coop_id;
                    dsEdu.DATA[i].EMP_NO = last_documentno;
                    dsEdu.DATA[i].SEQ_NO = i + 1;
                }
                for (int i = 0; i < dsExperience.RowCount; i++)
                {
                    dsExperience.DATA[i].COOP_ID = coop_id;
                    dsExperience.DATA[i].EMP_NO = last_documentno;
                    dsExperience.DATA[i].SEQ_NO = i + 1;
                }
                for (int i = 0; i < dsTraining.RowCount; i++)
                {
                    dsTraining.DATA[i].COOP_ID = coop_id;
                    dsTraining.DATA[i].EMP_NO = last_documentno;
                    dsTraining.DATA[i].SEQ_NO = i + 1;
                }
                for (int i = 0; i < dsAssist.RowCount; i++)
                {
                    dsAssist.DATA[i].COOP_ID = coop_id;
                    dsAssist.DATA[i].EMP_NO = last_documentno;
                    dsAssist.DATA[i].SEQ_NO = i + 1;
                }
                for (int i = 0; i < dsDisoffense.RowCount; i++)
                {
                    dsDisoffense.DATA[i].COOP_ID = coop_id;
                    dsDisoffense.DATA[i].EMP_NO = last_documentno;
                    dsDisoffense.DATA[i].SEQ_NO = i + 1;
                }
                for (int i = 0; i < dsGuarantee.RowCount; i++)
                {
                    dsGuarantee.DATA[i].COOP_ID = coop_id;
                    dsGuarantee.DATA[i].EMP_NO = last_documentno;
                    dsGuarantee.DATA[i].SEQ_NO = i + 1;
                }
                try
                {
                    exe.AddFormView(dsMain, ExecuteType.Insert);
                    exe.AddFormView(dsDetail, ExecuteType.Update);
                    exe.AddFormView(dsSalary, ExecuteType.Update);
                    exe.AddRepeater(dsFamily);
                    //exe.AddRepeater(dsCurefamily);
                    //exe.AddRepeater(dsFamily_Hos);
                    exe.AddRepeater(dsEdu);
                    exe.AddRepeater(dsExperience);
                    exe.AddRepeater(dsTraining);
                    exe.AddRepeater(dsAssist);
                    exe.AddRepeater(dsDisoffense);
                    exe.AddRepeater(dsGuarantee);
                    exe.Execute();

                    String sql_empstatus = @"UPDATE HREMPLOYEE SET EMP_STATUS = 1  WHERE EMP_NO = {1}";
                    sql_empstatus = WebUtil.SQLFormat(sql_empstatus, dsDetail.DATA[0].EMP_ENAME, last_documentno);
                    Sdt dt_empstatus = WebUtil.QuerySdt(sql_empstatus);

                    LtServerMessage.Text = WebUtil.CompleteMessage("บันทึกข้อมูลเรียบร้อย" + " " + ls_empno);

                    NewClear();
                }
                catch (Exception ex)
                {
                    LtServerMessage.Text = WebUtil.ErrorMessage("บันทึกไม่สำเร็จ" + ex);
                }
            }

            else
            {

                

                ls_empno = dsMain.DATA[0].EMP_NO.Trim();
                String sql = @"UPDATE HREMPLOYEE SET EMP_ENAME = {0}  WHERE EMP_NO = {1}";
                sql = WebUtil.SQLFormat(sql, dsDetail.DATA[0].EMP_ENAME, ls_empno);
                Sdt dt = WebUtil.QuerySdt(sql);
                exe.AddFormView(dsMain, ExecuteType.Update);
                exe.AddFormView(dsDetail, ExecuteType.Update);
                exe.AddFormView(dsSalary, ExecuteType.Update);
                try
                {
                    //dsFamily
                    for (int i = 0; i < dsFamily.RowCount; i++)
                    {
                        dsFamily.DATA[i].COOP_ID = coop_id;
                        dsFamily.DATA[i].EMP_NO = ls_empno;
                        dsFamily.DATA[i].SEQ_NO = i + 1;
                    }

                    if (dsFamily.RowCount >= 0)
                    {
                        String ls_sql = ("delete hremployeefamily where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno + "'");
                        exe.SQL.Add(ls_sql);
                    }

                    for (int i = 0; i < dsFamily.RowCount; i++)
                    {
                        string sqlInsertFamily = @"insert into hremployeefamily(coop_id,
                            emp_no,   
                            seq_no,   
                            f_name,   
                            f_relation,   
                            birth_date,   
                            occupation )values({0},{1},{2},{3},{4},{5},{6})";
                        object[] argslistInsert = new object[] { coop_id,
                        dsFamily.DATA[i].EMP_NO,
                        dsFamily.DATA[i].SEQ_NO,
                        dsFamily.DATA[i].F_NAME,
                        dsFamily.DATA[i].F_RELATION,
                        dsFamily.DATA[i].BIRTH_DATE,
                        dsFamily.DATA[i].OCCUPATION };
                        sqlInsertFamily = WebUtil.SQLFormat(sqlInsertFamily, argslistInsert);
                        exe.SQL.Add(sqlInsertFamily);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hremployeefamily " + ex); }

                /*try
                {
                    //dsFamily_Hos
                    for (int i = 0; i < dsFamily_Hos.RowCount; i++)
                    {
                        dsFamily_Hos.DATA[i].COOP_ID = coop_id;
                        dsFamily_Hos.DATA[i].EMP_NO = ls_empno;
                        dsFamily_Hos.DATA[i].SEQ_NO = i + 1;
                    }

                    if (dsFamily_Hos.RowCount >= 0)
                    {
                        String ls_sql = ("delete hremployeeassist where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno.Trim() + "' and assist_code = '01'");
                        exe.SQL.Add(ls_sql);
                    }

                    for (int i = 0; i < dsFamily_Hos.RowCount; i++)
                    {
                        string sqlInsertFamily_Hos = @"insert into hremployeeassist(coop_id,
                            emp_no,   
                            seq_no,   
                            assist_name,
                            assist_state,
                            assist_hosname,
                            assist_posit,
                            assist_amp,
                            assist_sdate,
                            assist_amt,
                            assist_minamt,
                            assist_code
                             )values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'01')";
                        object[] argslistInsert = new object[] { coop_id,
                        dsFamily_Hos.DATA[i].EMP_NO,
                        dsFamily_Hos.DATA[i].SEQ_NO,
                        dsFamily_Hos.DATA[i].ASSIST_NAME,
                        dsFamily_Hos.DATA[i].ASSIST_STATE,
                        dsFamily_Hos.DATA[i].ASSIST_HOSNAME,
                        dsFamily_Hos.DATA[i].ASSIST_POSIT,
                        dsFamily_Hos.DATA[i].ASSIST_AMP,
                        dsFamily_Hos.DATA[i].ASSIST_SDATE,
                        dsFamily_Hos.DATA[i].ASSIST_AMT,
                        dsFamily_Hos.DATA[i].ASSIST_MINAMT };
                        sqlInsertFamily_Hos = WebUtil.SQLFormat(sqlInsertFamily_Hos, argslistInsert);
                        exe.SQL.Add(sqlInsertFamily_Hos);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hremployeeassist " + ex); }

                try
                {
                    //dsCurefamily
                    for (int i = 0; i < dsCurefamily.RowCount; i++)
                    {
                        dsCurefamily.DATA[i].COOP_ID = coop_id;
                        dsCurefamily.DATA[i].EMP_NO = ls_empno.Trim();
                        dsCurefamily.DATA[i].SEQ_NO = i + 1;

                        if (dsCurefamily.RowCount >= 0)
                        {
                            String ls_sql = ("delete hremployeeassist where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno.Trim() + "' and assist_code = '01'");
                            exe.SQL.Add(ls_sql);
                        }


                        string sqlInsertCurefamily = @"insert into hremployeeassist(coop_id,
                            emp_no,   
                            seq_no,   
                            assist_name,
                            assist_state,
                            assist_hosname,
                            assist_posit,
                            assist_amp,
                            assist_sdate,
                            assist_amt,
                            assist_minamt,
                            assist_code
                             )values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'01')";

                        object[] argslistInsert = new object[] { coop_id,
                        dsCurefamily.DATA[i].EMP_NO,
                        dsCurefamily.DATA[i].SEQ_NO,
                        dsCurefamily.DATA[i].ASSIST_NAME,
                        dsCurefamily.DATA[i].ASSIST_STATE,
                        dsCurefamily.DATA[i].ASSIST_HOSNAME,
                        dsCurefamily.DATA[i].ASSIST_POSIT,
                        dsCurefamily.DATA[i].ASSIST_AMP,
                        dsCurefamily.DATA[i].ASSIST_SDATE,
                        dsCurefamily.DATA[i].ASSIST_AMT,
                        dsCurefamily.DATA[i].ASSIST_MINAMT
                        };
                        sqlInsertCurefamily = WebUtil.SQLFormat(sqlInsertCurefamily, argslistInsert);
                        exe.SQL.Add(sqlInsertCurefamily);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hremployeeassist " + ex); }*/

                try
                {
                    //dsEdu
                    for (int i = 0; i < dsEdu.RowCount; i++)
                    {
                        dsEdu.DATA[i].COOP_ID = coop_id;
                        dsEdu.DATA[i].EMP_NO = ls_empno;
                        dsEdu.DATA[i].SEQ_NO = i + 1;
                    }

                    if (dsEdu.RowCount >= 0)
                    {
                        String ls_sql = ("delete hremployeeedu where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno + "'");
                        exe.SQL.Add(ls_sql);
                    }

                    for (int i = 0; i < dsEdu.RowCount; i++)
                    {
                        string sqlInsertEdu = @"insert into hremployeeedu(coop_id,
                            emp_no,   
                            seq_no,   
                            education_code,   
                            edu_inst,   
                            edu_degree,   
                            edu_major,   
                            edu_gpa,   
                            edu_succyear )values({0},{1},{2},{3},{4},{5},{6},{7},{8})";
                        object[] argslistInsert = new object[] { coop_id,
                        dsEdu.DATA[i].EMP_NO,
                        dsEdu.DATA[i].SEQ_NO,
                        dsEdu.DATA[i].education_code,
                        dsEdu.DATA[i].EDU_INST,
                        dsEdu.DATA[i].EDU_DEGREE,
                        dsEdu.DATA[i].EDU_MAJOR,
                        dsEdu.DATA[i].EDU_GPA,
                        dsEdu.DATA[i].EDU_SUCCYEAR};
                        sqlInsertEdu = WebUtil.SQLFormat(sqlInsertEdu, argslistInsert);
                        exe.SQL.Add(sqlInsertEdu);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hremployeeedu " + ex); }
                try
                {
                    //dsTraining                    
                    for (int i = 0; i < dsTraining.RowCount; i++)
                    {
                        dsTraining.DATA[i].COOP_ID = coop_id;
                        dsTraining.DATA[i].EMP_NO = ls_empno;
                        dsTraining.DATA[i].SEQ_NO = i + 1;
                    }

                    if (dsTraining.RowCount >= 0)
                    {
                        String ls_sql = ("delete hremployeetraining where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno + "'");
                        exe.SQL.Add(ls_sql);
                    }

                    for (int i = 0; i < dsTraining.RowCount; i++)
                    {
                        string sqlInsertTraining = @"insert into hremployeetraining(coop_id,
                            emp_no,   
                            seq_no,   
                            tr_date,   
                            tr_code,   
                            tr_subject,   
                            tr_cause,   
                            tr_location,   
                            tr_institution,   
                            tr_fromdate,   
                            tr_todate,   
                            tr_day,   
                            tr_expamt,   
                            tr_certdesc,   
                            remark )values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14})";
                        object[] argslistInsert = new object[] { coop_id,
                        dsTraining.DATA[i].EMP_NO,
                        dsTraining.DATA[i].SEQ_NO,
                        dsTraining.DATA[i].TR_DATE,
                        dsTraining.DATA[i].TR_CODE,
                        dsTraining.DATA[i].TR_SUBJECT,
                        dsTraining.DATA[i].TR_CAUSE,
                        dsTraining.DATA[i].TR_LOCATION,
                        dsTraining.DATA[i].TR_INSTITUTION,
                        dsTraining.DATA[i].TR_FROMDATE,
                        dsTraining.DATA[i].TR_TODATE,
                        dsTraining.DATA[i].TR_DAY,
                        dsTraining.DATA[i].TR_EXPAMT,
                        dsTraining.DATA[i].TR_CERTDESC,
                        dsTraining.DATA[i].REMARK };
                        sqlInsertTraining = WebUtil.SQLFormat(sqlInsertTraining, argslistInsert);
                        exe.SQL.Add(sqlInsertTraining);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hremployeetraining " + ex); }
                try
                {
                    //dsAssist
                    //ls_sql = 
                    //Sta tadelassist = new Sta(state.SsConnectionString);
                    //tadelassist.Exe(ls_sql);

                    for (int i = 0; i < dsAssist.RowCount; i++)
                    {
                        dsAssist.DATA[i].COOP_ID = coop_id;
                        dsAssist.DATA[i].EMP_NO = ls_empno;
                        dsAssist.DATA[i].SEQ_NO = i + 1;
                    }

                    if (dsAssist.RowCount >= 0)
                    {
                        String ls_sql = ("delete hremployeeassist where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno + "'");
                        exe.SQL.Add(ls_sql);
                    }

                    for (int i = 0; i < dsAssist.RowCount; i++)
                    {
                        string sqlInsertAssist = @"insert into hremployeeassist(coop_id,
                            emp_no,   
                            seq_no,   
                            assist_code,   
                            assist_date,   
                            assist_detail,   
                            assist_amt,   
                            assist_remark,
                            assist_learn,
                            assist_use,
                            assist_draw,
                            assist_posit )values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                        object[] argslistInsert = new object[] { coop_id,
                        dsAssist.DATA[i].EMP_NO,
                        dsAssist.DATA[i].SEQ_NO,
                        dsAssist.DATA[i].ASSIST_CODE,
                        dsAssist.DATA[i].ASSIST_DATE,
                        dsAssist.DATA[i].ASSIST_DETAIL,
                        dsAssist.DATA[i].ASSIST_AMT,
                        dsAssist.DATA[i].ASSIST_REMARK,
                        dsAssist.DATA[i].ASSIST_LEARN,
                        dsAssist.DATA[i].ASSIST_USE,
                        dsAssist.DATA[i].ASSIST_DRAW,
                        dsAssist.DATA[i].ASSIST_POSIT};
                        sqlInsertAssist = WebUtil.SQLFormat(sqlInsertAssist, argslistInsert);
                        exe.SQL.Add(sqlInsertAssist);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hremployeeassist " + ex); }
                try
                {
                    //dsGuarantee
                    for (int i = 0; i < dsGuarantee.RowCount; i++)
                    {
                        dsGuarantee.DATA[i].COOP_ID = coop_id;
                        dsGuarantee.DATA[i].EMP_NO = ls_empno;
                        dsGuarantee.DATA[i].SEQ_NO = i + 1;
                    }

                    if (dsGuarantee.RowCount >= 0)
                    {
                        String ls_sql = ("delete hrgarantee where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno + "'");
                        exe.SQL.Add(ls_sql);
                    }

                    for (int i = 0; i < dsGuarantee.RowCount; i++)
                    {
                        string sqlInsertGuarantee = @"insert into hrgarantee(coop_id,
                            emp_no,   
                            seq_no,   
                            GARAN_NAME,   
                            GARAN_BIRTH,   
                            GARAN_WORK,   
                            GARAN_TEL,   
                            GARAN_AGE )values({0},{1},{2},{3},{4},{5},{6},{7})";
                        object[] argslistInsert = new object[] { coop_id,
                        dsGuarantee.DATA[i].EMP_NO,
                        dsGuarantee.DATA[i].SEQ_NO,
                        dsGuarantee.DATA[i].GARAN_NAME,
                        dsGuarantee.DATA[i].GARAN_BIRTH,
                        dsGuarantee.DATA[i].GARAN_WORK,
                        dsGuarantee.DATA[i].GARAN_TEL,
                        dsGuarantee.DATA[i].GARAN_AGE };
                        sqlInsertGuarantee = WebUtil.SQLFormat(sqlInsertGuarantee, argslistInsert);
                        exe.SQL.Add(sqlInsertGuarantee);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hrgarantee " + ex); }
                try
                {
                    //dsExperience
                    for (int i = 0; i < dsExperience.RowCount; i++)
                    {
                        dsExperience.DATA[i].COOP_ID = coop_id;
                        dsExperience.DATA[i].EMP_NO = ls_empno;
                        dsExperience.DATA[i].SEQ_NO = i + 1;
                    }

                    if (dsExperience.RowCount >= 0)
                    {
                        String ls_sql = ("delete hremployeeexperience where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno + "'");
                        exe.SQL.Add(ls_sql);
                    }

                    for (int i = 0; i < dsExperience.RowCount; i++)
                    {
                        string sqlInsertExperience = @"insert into hremployeeexperience(coop_id,
                            emp_no,   
                            seq_no,   
                            corp_name,   
                            pos_name,   
                            yearstart,   
                            yearend,   
                            last_salary,   
                            job_type,   
                            job_desc,   
                            awaycause,   
                            remark )values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})";
                        object[] argslistInsert = new object[] { coop_id,
                        dsExperience.DATA[i].EMP_NO,
                        dsExperience.DATA[i].SEQ_NO,
                        dsExperience.DATA[i].CORP_NAME,
                        dsExperience.DATA[i].POS_NAME,
                        dsExperience.DATA[i].YEARSTART,
                        dsExperience.DATA[i].YEAREND,
                        dsExperience.DATA[i].LAST_SALARY,
                        dsExperience.DATA[i].JOB_TYPE,
                        dsExperience.DATA[i].JOB_DESC,
                        dsExperience.DATA[i].AWAYCAUSE,
                        dsExperience.DATA[i].REMARK };
                        sqlInsertExperience = WebUtil.SQLFormat(sqlInsertExperience, argslistInsert);
                        exe.SQL.Add(sqlInsertExperience);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hremployeeexperience " + ex); }
                try
                {
                    //dsDisoffense
                    for (int i = 0; i < dsDisoffense.RowCount; i++)
                    {
                        dsDisoffense.DATA[i].COOP_ID = coop_id;
                        dsDisoffense.DATA[i].EMP_NO = ls_empno;
                        dsDisoffense.DATA[i].SEQ_NO = i + 1;
                    }

                    if (dsDisoffense.RowCount >= 0)
                    {
                        String ls_sql = ("delete hremployeedisof where coop_id ='" + coop_id + "' and emp_no = '" + ls_empno + "'");
                        exe.SQL.Add(ls_sql);
                    }

                    for (int i = 0; i < dsDisoffense.RowCount; i++)
                    {
                        string sqlInsertDisoffense = @"insert into hremployeedisof(coop_id,
                            emp_no,   
                            seq_no,
                            disof_docno,
                            disof_date,
                            disof_inflic,
                            disof_remark,
                            disoffense_code)values({0},{1},{2},{3},{4},{5},{6},{7})";
                        object[] argslistInsert = new object[] { coop_id,
                        dsDisoffense.DATA[i].EMP_NO,
                        dsDisoffense.DATA[i].SEQ_NO,
                        dsDisoffense.DATA[i].DISOF_DOCNO,
                        dsDisoffense.DATA[i].DISOF_DATE,
                        dsDisoffense.DATA[i].DISOF_INFLIC,
                        dsDisoffense.DATA[i].DISOF_REMARK,
                        dsDisoffense.DATA[i].DISOFFENSE_CODE};
                        sqlInsertDisoffense = WebUtil.SQLFormat(sqlInsertDisoffense, argslistInsert);
                        exe.SQL.Add(sqlInsertDisoffense);
                    }
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage("hremployeedisof " + ex); }
                try
                {
                    exe.Execute();

                    // เพิ่ม process เวลาปรับจากลูกจ้างเป็นพนักงานเลข gen ใหม่ให้ตาม ข้อกำหนดของเลขพนักงาน

                   
                    string sex_02 = dsMain.DATA[0].SEX;
                    string last_documentno_empnew = "";
                    string ans = ls_empno.Substring(2, 1);
                    decimal last_documentno_new = 0;

                    String sql_emptype = @"select last_documentno from cmdocumentcontrol where document_code = 'HREMPLFILEMAS'";

                sql_emptype = WebUtil.SQLFormat(sql_emptype);
                Sdt dt_emptype = WebUtil.QuerySdt(sql_emptype);
                if (dt_emptype.Next())

                {
                    last_documentno_new = dt_emptype.GetDecimal("last_documentno");
                    last_documentno_new = last_documentno_new + 1;
                }


                    if (dsMain.DATA[0].EMPTYPE_CODE == "01" && ans == "9")
                        {

                            if (sex_02 == "M")
                            {
                                sex_02 = "1";
                            }
                            else
                            {
                                sex_02 = "2";
                            }

                            Sta ta = new Sta(state.SsConnectionString);
                            //string postNumber = "";
                            try
                            {
                                //ta.AddInParameter("AVC_COOPID", state.SsCoopId, System.Data.OracleClient.OracleType.VarChar);
                                //ta.AddInParameter("AVC_DOCCODE", "HREMPLFILEMAS", System.Data.OracleClient.OracleType.VarChar);
                                //// ta.AddInParameter("AVC_DOCCODE", sex, System.Data.OracleClient.OracleType.VarChar);
                                //ta.AddReturnParameter("return_value", System.Data.OracleClient.OracleType.VarChar);
                                //ta.ExePlSql("N_PK_DOCCONTROL.OF_GETNEWDOCNO");
                                //last_documentno_empnew = ta.OutParameter("return_value").ToString();
                                last_documentno_empnew = wcf.NCommon.of_getnewdocno(state.SsWsPass, state.SsCoopControl, "HREMPLFILEMAS");
                                last_documentno_empnew = last_documentno_empnew + "00" + last_documentno_new.ToString() + sex_02;
                                ta.Close();
                            }
                            catch
                            {
                                ta.Close();
                            }

                            String sql_empnew = @"UPDATE HREMPLOYEE SET EMP_NO = {0} , EMP_NO_OLD = {1}  WHERE EMP_NO = {2}";
                            sql_empnew = WebUtil.SQLFormat(sql_empnew, last_documentno_empnew, ls_empno, ls_empno);
                            Sdt dt_empnew = WebUtil.QuerySdt(sql_empnew);

                            String sql_empnew1 = @"UPDATE HREMPLOYEEASSIST SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew1 = WebUtil.SQLFormat(sql_empnew1, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew1 = WebUtil.QuerySdt(sql_empnew1);

                            String sql_empnew2 = @"UPDATE HREMPLOYEEFAMILY SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew2 = WebUtil.SQLFormat(sql_empnew2, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew2 = WebUtil.QuerySdt(sql_empnew2);

                            String sql_empnew3 = @"UPDATE HREMPLOYEEEXPERIENCE SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew3 = WebUtil.SQLFormat(sql_empnew3, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew3 = WebUtil.QuerySdt(sql_empnew3);

                            String sql_empnew4 = @"UPDATE HREMPLOYEETRAINING SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew4 = WebUtil.SQLFormat(sql_empnew4, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew4 = WebUtil.QuerySdt(sql_empnew4);

                            String sql_empnew5 = @"UPDATE HRLOGLEAVE SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew5 = WebUtil.SQLFormat(sql_empnew5, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew5 = WebUtil.QuerySdt(sql_empnew5);

                            String sql_empnew6 = @"UPDATE HRLOGCHANGEWORK SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew6 = WebUtil.SQLFormat(sql_empnew6, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew6 = WebUtil.QuerySdt(sql_empnew6);

                            String sql_empnew7 = @"UPDATE HRGARANTEE SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew7 = WebUtil.SQLFormat(sql_empnew7, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew7 = WebUtil.QuerySdt(sql_empnew7);

                            String sql_empnew8 = @"UPDATE HREMPLOYEEDISOF SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew8 = WebUtil.SQLFormat(sql_empnew8, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew8 = WebUtil.QuerySdt(sql_empnew8);

                            String sql_empnew9 = @"UPDATE HRLOGLATE SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew9 = WebUtil.SQLFormat(sql_empnew9, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew9 = WebUtil.QuerySdt(sql_empnew9);

                            String sql_empnew10 = @"UPDATE HRLOGWORKTIME SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew10 = WebUtil.SQLFormat(sql_empnew10, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew10 = WebUtil.QuerySdt(sql_empnew10);

                            String sql_empnew11 = @"UPDATE HRBASEPAYROLLFIXED SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew11 = WebUtil.SQLFormat(sql_empnew11, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew11 = WebUtil.QuerySdt(sql_empnew11);

                            String sql_empnew12 = @"UPDATE HRBASEPAYROLLOTHER SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew12 = WebUtil.SQLFormat(sql_empnew12, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew12 = WebUtil.QuerySdt(sql_empnew12);

                            String sql_empnew13 = @"UPDATE HRPAYROLL SET EMP_NO = {0} WHERE EMP_NO = {1}";
                            sql_empnew13 = WebUtil.SQLFormat(sql_empnew13, last_documentno_empnew, ls_empno);
                            Sdt dt_empnew13 = WebUtil.QuerySdt(sql_empnew13);

                            

                        }
                        else
                        {


                        }

                    

                    ////////////////////////////////////////////////////////////////////////////////////////////////

                    LtServerMessage.Text = WebUtil.CompleteMessage("แก้ไขข้อมูลเรียบร้อย" + " " + ls_empno);
                }
                catch (Exception ex) { LtServerMessage.Text = WebUtil.ErrorMessage(ex); }

                NewClear();
            }
        }

        public void WebSheetLoadEnd()
        {

        }
    }
}