<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_entry_stopleavelate.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_entry_stopleavelate" Title="การลางาน"
    ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postShowDetail %>
    <%=postSearchGetMember%>
    <%=postDifferDate%>
    <%=postDifferTime%>
    <%=postDeleteRow%>
    <%=postGetMember %>
    <%=postNewClear%>
    <%=postAddRow%>
    
    <style type="text/css">
        .style5
        {
            font-size: small;
            font-weight: bold;
        }
        .style6
        {
            font-size: small;
        }
    </style>

    <script type="text/javascript">
        function Validate()
        {
            objDwDetail.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg('590', '600', 'w_dlg_hr_master_search.aspx', '');
        }
        
        function GetValueEmplid(emplid)
        {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            objDwMain.SetItem(1, "emplid", Gcoop.Trim(emplid));
            objDwMain.AcceptText();
            postGetMember();
        }
        
        function OnDwDetailButtonClick(s,r,c)
        {
            if(c=="b_del")
            {
                var seq_no = "";
                seq_no = objDwDetail.GetItem(1,"seq_no");
                if(seq_no == "" || seq_no == null || seq_no == "Auto")
                {
                    alert("ยังไม่มีข้อมูลไม่สามารถลบรายการได้");
                }
                else
                {
                    if(confirm("คุณต้องการลบรายการ "+ seq_no +" ใช่หรือไม่?"))
                    {
                        postDeleteRow();
                    }
                }
            }
            return 0;
        }
        
        function OnButtonClicked(s, row, oName)
        {
            if(oName == "b_search")
            {
                MenubarOpen();
            }
            return 0;
        }
        
         function MenubarNew()
         {
            if(confirm("ยืนยันการล้างข้อมูลบนหน้าจอ"))
            {
                postNewClear();
            }
        }
        
        function DwDetailAddRow()
        {
            var emplid = objDwMain.GetItem(1,"emplid");
            if (emplid == "Auto" || emplid == null)
            {
                alert("ไม่พบรหัสมาชิก กรุณากรอกรหัสสมาชิกก่อน");
            }
            else
            {
                postAddRow(); 
            }
        }
        
        function OnListClick(sender, rowNumber, objectName) 
        {
            var seq_no = objDwList.GetItem(rowNumber, "seq_no");
            if(seq_no != "Auto")
            {
                objDwDetail.SetItem(1, "seq_no", Gcoop.Trim(seq_no));
                objDwDetail.AcceptText();
                postShowDetail();
            }
            return 0;
        }

        
        function OnItemChangeMain(s, r, c, v)
        {
           if(c== "emplcode")
           {
               objDwMain.SetItem(r,c,Gcoop.StringFormat(v,"00000"));
               objDwMain.AcceptText();
               postSearchGetMember();
           }
           
           else if(c == "entry_tdate")
           {
                objDwMain.SetItem(1, "entry_tdate", v );
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "entry_date", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
           }
           return 0;
        }
        
        function OnDwDetailItemChange(s,r,c,v)
        {
            if(c == "leavfrom_tdate")
            {
                objDwDetail.SetItem(1,"leavfrom_tdate",v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(1,"leavfromdate", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText();
                
                var leavto_tdate = null;
                leavto_tdate = objDwDetail.GetItem(r,"leavto_tdate");
                if(leavto_tdate == null || leavto_tdate == "")
                {
                    alert("กรุณากรอกข้อมูลวันที่สิ้นสุดด้วย");
                }
                else
                {
                  postDifferDate();
                }
            }
            else if(c == "leavto_tdate")
            {
                objDwDetail.SetItem(r,"leavto_tdate",v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(r,"leavtodate", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText(); 
                
                var leavfrom_tdate = null;
                leavfrom_tdate = objDwDetail.GetItem(r,"leavfrom_tdate");
                if(leavfrom_tdate == null || leavfrom_tdate == "")                
                {
                    alert("กรุณากรอกข้อมูลวันที่เริ่มต้นด้วย");
                }
                else 
                {
                    postDifferDate();
                }
            }
            if(c == "timestart")
            {
                objDwDetail.SetItem(r,"timestart",v);
                objDwDetail.AcceptText();
//                var timestart1 = null;
//                var timeend1 = null;
//                timestart1 = objDwDetail.GetItem(r,"timestart");
//                timeend1 = objDwDetail.GetItem(r,"timeend");
//                if(timestart1.length != 6)
//                {
//                    alert("กรุณากรอกข้อมูลเวลาเริ่มต้นในรูปแบบ HH:MM:SS");
//                }
//                else
//                {
//                    if(timeend1 != null)
//                    {
//                        if(timeend1.length != 6)
//                        {
//                            alert("กรุณากรอกข้อมูลเวลาสิ้นสุดในรูปแบบ HH:MM:SS");
//                        }
//                        else
//                        {
                            postDifferTime();
//                        }
//                    }
//                }
            }
            else if(c == "timeend")
            {
                objDwDetail.SetItem(r,"timeend",v);
                objDwDetail.AcceptText();
                
//                var timestart2 = null;
//                var timeend2 = null;
//                timestart2 = objDwDetail.GetItem(r,"timestart");
//                timeend2 = objDwDetail.GetItem(r,"timeend");
//                if(timeend2.length != 6)
//                {
//                    alert("กรุณากรอกข้อมูลเวลาสิ้นสุดในรูปแบบ HH:MM:SS");
//                }
//                else
//                {
//                    if(timestart2 != null)
//                    {
//                        if(timestart2.length != 6)
//                        {
//                            alert("กรุณากรอกข้อมูลเวลาเริ่มต้นในรูปแบบ HH:MM:SS");
//                        }
//                        else
//                        {
                            postDifferTime();
//                        }
//                    }
//                }
            }
            else if(c == "commit_tdate")
            {
                objDwDetail.SetItem(r,"commit_tdate",v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(r,"commit_date", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText();    
            }
            else if(c== "leavtele")
            {
                objDwDetail.SetItem(r,"leavtele",v);
                objDwDetail.AcceptText();
                var leavtele = objDwDetail.GetItem(r,"leavtele");
                if(leavtele.length != 10)
                {
                    alert("กรุณากรอกข้อมูลเบอร์โทรศัพท์ 10หลัก รูปแบ 089#######");
                }
            }
            return 0;
       }
       
        function OnDwDetailClick(s,r,c)
        {
            if(c=="timestatus")
            {
                
            }
            return 0;
        }
            
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width: 100%;">
        <tr>
            <td class="style5" colspan="2">
                ข้อมูลการทำรายการ
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_employee_head_leave"
                    LibraryList="~/DataWindow/hr/hr_entry_stopleavelate.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" ClientEventItemChanged="OnItemChangeMain" AutoRestoreContext="False"
                    ClientEventButtonClicked="OnButtonClicked" ClientScriptable="True" 
                    ClientFormatting="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style5">
                ข้อมูลการลางานเจ้าหน้าที่
            </td>
            <td class="style5">
                รายละเอียดข้อมูลการลางานเจ้าหน้าที่
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="dw_empleave_list"
                    LibraryList="~/DataWindow/hr/hr_entry_stopleavelate.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientEventClicked="OnListClick"
                    ClientScriptable="True" ClientFormatting="True">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="DwDetailAddRow()" style="font-family: Tahoma; font-size: small;
                    float: left; font-weight: 700; color: #003399;">เพิ่มแถว</span>
                    </br>
                <asp:HiddenField ID="Hd_totaldate" runat="server" />
                <asp:HiddenField ID="Hd_leavtotalhour" runat="server" />
                    </td>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dw_empleave_detail"
                    LibraryList="~/DataWindow/hr/hr_entry_stopleavelate.pbl" Width="500px" Height="100%"
                    AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
                    ClientScriptable="true" ClientEventButtonClicked="OnDwDetailButtonClick" 
                    ClientFormatting="True" ClientEventItemChanged="OnDwDetailItemChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="style6">
                หมายเหตุ : กรุณากรอกข้อมูลเวลาเริ่มต้น และเวลาสิ้นสุดในรูปแบบ HH:MM:SS</td>
        </tr>
    </table>
    
</asp:Content>
