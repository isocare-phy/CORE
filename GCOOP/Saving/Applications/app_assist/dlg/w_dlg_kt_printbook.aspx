<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_kt_printbook.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_kt_printbook" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Print Book</title>
    <%=postPrintBook%>

    <script type="text/javascript">
        function DialogLoadComplete() {
            try {
                var isPostBack = Gcoop.GetEl("HdIsPostBack").value == "true";
                if (isPostBack) {
                    //parent.PrintBookReload();
                }
            } catch (err) { }
            try {
                var isZero = Gcoop.GetEl("HdIsZeroPage").value;
                if (isZero == "true") {
                    ExitIFrame();
                }
            } catch (err) { }
            document.addEventListener("keyup", HandleOnKeyUp, true);
            Gcoop.GetEl("Button1").focus();
        }

        function OnKeyUpEnd(e) {
            if (e.keyCode == "27" || e.keyCode == "109" || e.keyCode == "189") {
                ExitIFrame();
            }
        }

        function ExitIFrame() {
            try {
                parent.PrintBookReload();
            } catch (err) { }
            try {
                parent.RemoveIFrame();
            } catch (err) { }
        }

    </script>

    <style type="text/css">
        #Button1
        {
            width: 243px;
        }
        #Button2
        {
            width: 68px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <dw:WebDataWindowControl ID="DwNewBook" runat="server" DataWindowObject="d_dp_booknew"
            LibraryList="~/DataWindow/ap_deposit/dp_slip.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientValidation="False">
        </dw:WebDataWindowControl>
        <br />
        <dw:WebDataWindowControl ID="DwPrintPrompt" runat="server" DataWindowObject="d_dppbk_prompt_print"
            LibraryList="~/DataWindow/ap_deposit/dp_slip.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True">
        </dw:WebDataWindowControl>
        &nbsp;
        <div align="center">
            <input id="Button1" style="width:65px;" type="button" value="พิมพ์" onclick="postPrintBook()" />
            &nbsp; &nbsp;
            <input id="Button2" style="width:65px;" type="button" value="ปิด" onclick="ExitIFrame()" />
        </div>
        <asp:HiddenField ID="HdAccountNo" runat="server" />
        <asp:HiddenField ID="HdIsZeroPage" runat="server" />
        <asp:HiddenField ID="HdIsPostBack" runat="server" />
    </div>
    </form>
</body>
</html>

