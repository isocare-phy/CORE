<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_dp_printbook_int.aspx.cs"
    Inherits="Saving.Applications.ap_deposit.dlg.w_dlg_dp_printbook_int" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>พิมพ์ใบสำคัญจ่าย</title>
    <%=initJavaScript %>

    <script type="text/javascript">
        function btnCancelClick(){
            window.close();
            return;
        }
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            พิมพ์ใบสำคัญจ่าย</center>
        <br />
        <div align="center">
            <asp:Button ID="btnCommit" runat="server" Text="ตกลง" onclick="btnCommit_Click" />
            &nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" onclientclick="btnCancelClick()" />
        </div>
    </div>
    <asp:Label ID="Label1" runat="server"></asp:Label>
    <asp:HiddenField ID="HdDeptAccountNo" runat="server" />
    <asp:HiddenField ID="HdPassBookNo" runat="server" />
    <asp:HiddenField ID="HdCloseIFrame" runat="server" />
    <asp:HiddenField ID="HdSubmit" runat="server" />
    </form>
</body>
</html>
