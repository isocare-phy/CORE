<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_hr_master_search.aspx.cs" Inherits="Saving.Applications.hr.w_dlg_hr_master_search" ValidateRequest="false" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<% Page_LoadComplete(); %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ค้นหาข้อมูลพนักงาน</title>
    <script type="text/javascript">
        function OnDetailClick(sender, rowNumber, objectName) {
        var emplid = objdw_detail.GetItem(rowNumber, "emplid");
        var emplfirsname = objdw_detail.GetItem(rowNumber, "emplfirsname");
        var isConfirm = confirm("ต้องการเลือก : " + emplid + " " + emplfirsname + "  ใช่หรือไม่");
            if(isConfirm){
                window.opener.GetValueEmplid(emplid);  
                window.close();
            }
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
                <td valign="top" width="600" class="style1">
                    <dw:WebDataWindowControl ID="dw_data" runat="server" ClientScriptable="True" 
                        DataWindowObject="d_hr_employee_search" HorizontalScrollBar="NoneAndClip" 
                        LibraryList="~/DataWindow/hr/hr_search.pbl" UseCurrentCulture="True" 
                        VerticalScrollBar="NoneAndClip" Width="600px" Height="90px">
                    </dw:WebDataWindowControl>
                    <br />
                </td>
                <td valign="top" class="style1">
                    <asp:Button ID="BSearch" runat="server" Text="ค้น..." OnClick="BSearch_Click" Height="75px" />
                </td>
            </tr>
        </table>
        <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_hr_employee_search_detail"
            LibraryList="~/DataWindow/hr/hr_search.pbl" 
            Width="820px" Style="top: 0px; left: 0px"
            RowsPerPage="10" HorizontalScrollBar="NoneAndClip" VerticalScrollBar="NoneAndClip"
            ClientEventClicked="OnDetailClick" ClientScriptable="True" UseCurrentCulture="True" >
            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
        <br />
    </div>
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" Visible="False" />
    </form>
</body>
</html>