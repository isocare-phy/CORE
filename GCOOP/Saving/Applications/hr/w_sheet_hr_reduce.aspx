<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_reduce.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_reduce" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%=initJavaScript %>
 <%=getEmplid%>
 <%=newRecord %>
 <%=getDetail%>
 <%=deleteRecord%>
 <style type="text/css">
        .style3
        {
            width: 198px;
        }
        .style4
        {
            width: 47px;
        }
    </style>
    <script type="text/javascript">
        function Validate(){
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        function MenubarOpen() {
            Gcoop.OpenDlg('590','600','w_dlg_hr_master_search.aspx','');
        }
        function GetValueEmplid(emplid){
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            objDwMain.SetItem(1, "emplid", Trim(emplid));
            objDwMain.AcceptText();
            getEmplid();
            //document.getElementById("ctl100_ContentPlace_HiddenField1").value = emplid ;
        }
        function OnButtonClicked(s, row, oName){
            if(oName == "b_search"){
                MenubarOpen();
            }else if(oName == "b_delete" ){
                var seq_no = objDwList.GetItem(row, "seq_no") ;
                var detail = "รหัส " + objDwMain.GetItem(1, "emplid");
                detail += " : " + seq_no + " " + objDwList.GetItem(row, "educinst");
                if(confirm("คุณต้องการลบรายการ "+ detail +" ใช่หรือไม่?")){
                    objDwDetail.SetItem(1, "seq_no", Trim(seq_no));
                    objDwDetail.AcceptText();
                    deleteRecord();
                }
            }
            return 0;
        }
         function MenubarNew(){
            if(confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")){
                newRecord();
            }
        }
        function OnListClick(sender, rowNumber, objectName) {
            var seq_no = objDwList.GetItem(rowNumber, "seq_no");
            alert("เลือกรายการ " + rowNumber + " : " + seq_no);
            objDwDetail.SetItem(1, "seq_no", Trim(seq_no));
            objDwDetail.AcceptText();
            getDetail();
            return 0;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td colspan="3">
                &nbsp;
                &nbsp;
                &nbsp;
                <asp:Label ID="Label1" runat="server" Text="ข้อมูลการทำรายการ" Font-Bold="True" 
                    Font-Names="tahoma" Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwMain" runat="server" 
                    DataWindowObject="dw_hr_reducedea_head" 
                    LibraryList="~/DataWindow/hr/hr_reduce.pbl"
                    AutoSaveDataCacheAfterRetrieve="True"
            AutoRestoreDataCache="True"  
            AutoRestoreContext="False" 
                    ClientScriptable="True" ClientEventButtonClicked="OnButtonClicked">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        </table>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <table style="width: 100%;">
        <tr>
            <td class="style3" valign=top>
                &nbsp;
                <asp:Label ID="Label2" runat="server" Text="ข้อมูลลดหย่อนภาษี" 
                    Font-Bold="True" Font-Names="tahoma" Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwList" runat="server" 
                    DataWindowObject="dw_hr_reducetab1" 
                    LibraryList="~/DataWindow/hr/hr_reduce.pbl"
                    AutoSaveDataCacheAfterRetrieve="True"
            AutoRestoreDataCache="True"  
            AutoRestoreContext="False" 
                    ClientScriptable="True"
                    ClientEventClicked="OnListClick">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="MenubarNew()" style="font-family: Tahoma; font-size: 11px; float: left;">เพิ่มแถว</span>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </td>
            <td valign=top>
                &nbsp;
                <asp:Label ID="Label3" runat="server" Text="รายละเอียดข้อมูลการลดหย่อนภาษี" 
                    Font-Bold="True" Font-Names="tahoma" Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" 
                    DataWindowObject="dw_hr_employee_edu_detail" 
                    LibraryList="~/DataWindow/hr/hr_reduce.pbl"
                    Width="480px" Height="400px" HorizontalScrollBar=Auto
                    AutoSaveDataCacheAfterRetrieve="True" 
            AutoRestoreDataCache="True"  
            AutoRestoreContext="False" 
            ClientScriptable="true" ClientEventClicked="OnButtonClicked">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;
            </td>
            <td class="style4">
                &nbsp;</td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HiddenField1" runat="server" />
</asp:Content>
