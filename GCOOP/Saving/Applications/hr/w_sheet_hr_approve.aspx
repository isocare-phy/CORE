<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_approve.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_approve" Title="อนุมัติสำรองบำเน็จ" ValidateRequest="false" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%=getEmplid%>
 <%=newRecord %>
 <%=checkValue%>
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
     function MenubarOpen() {
            Gcoop.OpenDlg('590','600','w_dlg_hr_master_search.aspx','');}
        function MenubarSave(){
           if(confirm("ยืนยันการบันทึกข้อมูล")){
                checkValue();
            }
        }
        function GetValueEmplid(emplid){
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            objDwMain.SetItem(1, "emplid", Trim(emplid));
            objDwMain.AcceptText();
            getEmplid();
        }
        function OnButtonClicked(s, row, oName){
            if(oName == "b_search"){
                MenubarOpen();
            }
            return 0;
        }
         function MenubarNew(){
            if(confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")){
                newRecord();
            }
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
                    DataWindowObject="d_acc_bkemp" Width="740px" HorizontalScrollBar=Auto
                    LibraryList="~/DataWindow/hr/hr_approve.pbl"
                    AutoSaveDataCacheAfterRetrieve="True"
            AutoRestoreDataCache="True"  
            AutoRestoreContext="False" 
                    >
                </dw:WebDataWindowControl>
            </td>
        </tr>
        </table>
    &nbsp;&nbsp;&nbsp;&nbsp;
    
        </tr>
      
    </table>
</asp:Content>


