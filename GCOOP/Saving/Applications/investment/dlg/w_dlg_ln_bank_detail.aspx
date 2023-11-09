<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_ln_bank_detail.aspx.cs" Inherits="Saving.Applications.investment.dlg.w_dlg_sh_bank_detail" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%=jsBankClick%>
<%=jsBranchClick%>
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        function OnBankClick(s, r, c) {
            Gcoop.GetEl("HdRow").value = r;
            jsBankClick();
        }
        function OnBranchClick(s, r, c) {
            Gcoop.GetEl("HdRow").value = r;
            jsBranchClick();
        }
        function LabelClick() {
            bank_code = Gcoop.GetEl("HdBankCode").value;
            bank_desc = Gcoop.GetEl("HdBankDesc").value;
            bank_branch = Gcoop.GetEl("HdBranchCode").value;
            branch_desc = Gcoop.GetEl("HdBranchDesc").value;
            parent.GetDetail(bank_code, bank_desc, bank_branch, branch_desc);
            parent.RemoveIFrame();
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div align="center" style="background-color: #C0C0C0">
        <asp:Label ID="Label1" runat="server" Text="กรุณาเลือกธนาคาร" 
            onclick="LabelClick();" Font-Underline="True"></asp:Label>
    </div>
    <table style="width: 100%;" border="1px">
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwBank" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientValidation="False" DataWindowObject="d_dlg_select_bank_detail" LibraryList="~/DataWindow/investment/loan.pbl"
                    ClientEventClicked="OnBankClick">
                </dw:WebDataWindowControl>
            </td>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwBranch" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientValidation="False" DataWindowObject="d_dlg_select_branch_detail" LibraryList="~/DataWindow/investment/loan.pbl"
                    ClientEventClicked="OnBranchClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdRow" runat="server" Value="" />
    <asp:HiddenField ID="HdBankCode" runat="server" Value="" />
    <asp:HiddenField ID="HdBankDesc" runat="server" Value="" />
    <asp:HiddenField ID="HdBranchCode" runat="server" Value="" />
    <asp:HiddenField ID="HdBranchDesc" runat="server" Value="" />
    </form>
</body>
</html>
