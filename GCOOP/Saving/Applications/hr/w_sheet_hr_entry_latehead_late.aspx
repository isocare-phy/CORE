<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_entry_latehead_late.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_entry_latehead_late" Title="ประวัติการมาสาย"
    ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%=postSearchGetMember%>
   <%=postShowDetail %>
   <%=postSearchGetMember%>
   <%=postDifferTime%>
   <%=postAddRow%>
   <%=postGetMember%>
   <%=postDeleteRow%>

    <style type="text/css">
        .style5
        {
            font-size: small;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">
        function Validate()
        {
            objDwDetail.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        
        function MenubarOpen() 
        {
            Gcoop.OpenDlg('590','600','w_dlg_hr_master_search.aspx','');
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
                    if(confirm("คุณต้องการลบรายการ รหัส : "+ seq_no +" ใช่หรือไม่?"))
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
               objDwMain.SetItem(1, "emplcode" , Gcoop.StringFormat(v,"00000"));
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
                objDwDetail.SetItem(r,"leavfrom_tdate",v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(r,"leavfromdate", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText();
            }
            
            else if(c == "timeend")
            {
                objDwDetail.SetItem(r , "timeend" , v);
                objDwDetail.AcceptText();
//                if(v.length >6 || v.length <6)
//                {
//                    alert("กรุณากรอกข้อมูลเวลาในรูปแบบ HH:MM:SS")
//                }
//                else 
//                {
                    postDifferTime();
//                }
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
            <td colspan="2">
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_employee_head_edu"
                    LibraryList="~/DataWindow/hr/hr_entry_latehead_late.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False"
                    ClientScriptable="True" ClientEventButtonClicked="OnButtonClicked" 
                    ClientFormatting="True" ClientEventItemChanged="OnItemChangeMain">
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
                ข้อมูลการมาสายของเจ้าหน้าที่
            </td>
            <td class="style5">
                รายละเอียดข้อมูลการมาสายของเจ้าหน้าที่
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="dw_latehead_list"
                    LibraryList="~/DataWindow/hr/hr_entry_latehead_late.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True"
                    ClientEventClicked="OnListClick" ClientFormatting="True">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="DwDetailAddRow()" style="font-family: Tahoma; font-size: small;
                    float: left; font-weight: 700; color: #003399;">เพิ่มแถว</span><br>
                <asp:HiddenField ID="Hd_timerent" runat="server" />
                <asp:HiddenField ID="Hd_totaldate" runat="server" />
            </td>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dw_latehead_detail"
                    LibraryList="~/DataWindow/hr/hr_entry_latehead_late.pbl" Width="480px" Height="200px"
                    HorizontalScrollBar="Auto" AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True"
                    AutoRestoreContext="False" ClientScriptable="true" ClientFormatting="True" 
                    ClientEventButtonClicked="OnDwDetailButtonClick" 
                    ClientEventItemChanged="OnDwDetailItemChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
                </td>
            <td class="style5">
                หมายเหตุ : กรุณากรอกข้อมูลเวลาในรูปแบบ HH:MM:SS</td>
        </tr>
    </table>
</asp:Content>
