<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_emplinfodea.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_emplinfodea" Title="Untitled Page" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%=initJavaScript %>
 <%=getEmplid%>
 <%=newEmpl %>
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
                newEmpl();
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
                    DataWindowObject="dw_hr_master_head" 
                    LibraryList="~/DataWindow/hr/hr_master.pbl"
                    AutoSaveDataCacheAfterRetrieve="True"
            AutoRestoreDataCache="True"  
            AutoRestoreContext="False" Width="740px"
            ClientScriptable="true" ClientEventButtonClicked="OnButtonClicked"        >
                </dw:WebDataWindowControl>
            </td>
        </tr>
        </table>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <table style="width: 100%;">
        <tr>
            <td valign=top style="width: 100%;" >
                &nbsp;
                <asp:Label ID="Label3" runat="server" Text="รายละเอียดข้อมูลเพิ่มเติมเจ้าหน้าที่" 
                    Font-Bold="True" Font-Names="tahoma" Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" 
                    DataWindowObject="dw_hr_emplinfidea" 
                    LibraryList="~/DataWindow/hr/hr_master.pbl"
                    Width="740px" Height="800px" HorizontalScrollBar=Auto
                    AutoSaveDataCacheAfterRetrieve="True"
            AutoRestoreDataCache="True"  
            AutoRestoreContext="False" ClientScriptable="true" >
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
</asp:Content>
