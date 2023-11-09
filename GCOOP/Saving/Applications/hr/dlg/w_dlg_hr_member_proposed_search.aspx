<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_hr_member_proposed_search.aspx.cs" 
Inherits="Saving.Applications.hr.dlg.w_dlg_hr_member_proposed_search"  %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title></title>
 <script type="text/javascript">

     function OnDetailClick(sender, rowNumber, objectName) {
         var seq_empid = objdw_detail.GetItem(rowNumber, "seq_empid");
         var member_mbnumber = objdw_detail.GetItem(rowNumber, "member_mbnumber");
         window.opener.GetValueEmplid(seq_empid); 
         window.close();
     }
             
    </script>
    <style type="text/css">
        .style1
        {
            height: 102px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table>
            <tr>
                <td> 
                    <dw:WebDataWindowControl ID="dw_data" runat="server" ClientScriptable="True" 
                        DataWindowObject="hr_member_proposed_search" HorizontalScrollBar="NoneAndClip" 
                        LibraryList="~/DataWindow/hr/hr.pbl" UseCurrentCulture="True" 
                        VerticalScrollBar="NoneAndClip" width="820px" Height="90px" 
                        ClientEventButtonClicked="DwItemButtonsearchclick"> 
                    </dw:WebDataWindowControl>
                    <br />
                </td>
                <td valign="top" class="style1"> 
         
                </td>
            </tr>
        </table>
        <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="hr_member_proposedregis_view"
            LibraryList="~/DataWindow/hr/hr.pbl" 
            Width="870" Style="top: 0px; left: 0px"
            RowsPerPage="10" HorizontalScrollBar="NoneAndClip" VerticalScrollBar="NoneAndClip"
            ClientEventClicked="OnDetailClick" ClientScriptable="True" UseCurrentCulture="True" >
            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
        <br />
    </div>
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" Visible="False" />
    </form>
<%--         function DwItemButtonsearchclick(sender, rowNumber, buttonName) {
         if (buttonName == "b_search") {
             var mb_membernumber = "";
             mb_membernumber = objdw_data.GetItem(rowNumber, "mb_membernumber");
             if (confirm("ต้องการค้นหาเลขที่ " + mb_membernumber + " ใช่หรือไม่ ")) {
                 postShowItemMain();
             }
             return 0;
         }
         alert(seq_empid);
     }--%>
</body>
</html>