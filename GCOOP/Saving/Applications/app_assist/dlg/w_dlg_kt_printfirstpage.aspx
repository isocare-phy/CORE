<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_kt_printfirstpage.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_kt_printfirstpage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>พิมพ์ปกสมุด</title>
    <%=printFirstPage%>

    <script type="text/javascript">
        function DialogLoadComplete() {
            try {
                var closeIFrame = Gcoop.GetEl("HdCloseIFrame").value;
                if (closeIFrame == "true") {
                    parent.RemoveIFrame();
                }
            } catch (err) { }
            Gcoop.GetEl("btnCommit").focus();
        }

        function btnCommitClick() {
            printFirstPage();
            parent.CommitPrintFirstPage();
            return;
        }

        function btnCancelClick() {
            parent.RemoveIFrame();
            return;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            พิมพ์ปกสมุด</center>
        <br />
        <div align="center">
            <asp:Button ID="btnCommit" runat="server" Text="ตกลง" onclientclick="btnCommitClick()" />
            &nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" onclientclick="btnCancelClick()" />
        </div>
    </div>
    <asp:Label ID="Label1" runat="server"></asp:Label>
    <asp:HiddenField ID="HdDeptAccountNo" runat="server" />
    <asp:HiddenField ID="HdPassBookNo" runat="server" />
    <asp:HiddenField ID="HdCloseIFrame" runat="server" />
    </form>
</body>
</html>
