<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_kt_membinfokt.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_membinfokt" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postAccountNo%>
    <%=postAssistType %>

    <script type="text/javascript">
        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_dp_account_search.aspx", "");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            if (c == "member_no") {
                v = Gcoop.StringFormat(v, "00000000");
                NewAccountNo(v);
                return 0;
            }
            if (c == "assisttype_desc") {
                NewAssistType(v);
                return 0;
            }
        }

        function MenubarNew() {
            window.location = Gcoop.GetUrl() + "Applications/ap_deposit/w_sheet_dp_deptedit.aspx";
        }

        function NewAccountNo(accNo) {
            objDwMain.SetItem(1, "member_no", Gcoop.Trim(accNo));
            objDwMain.AcceptText();
            postAccountNo();
        }
        function NewAssistType(assType) {
            objDwMain.SetItem(1, "assisttype_desc", Gcoop.Trim(assType));
            objDwMain.AcceptText();
            postAssistType();
        }

        function OnDwTab1Click(s, r, c) {
            if (c == "datawindow")
                return 0;
            //alert(c != "datawindow");
            if (c == "entry_tdate") {
                alert("C  = " + objDwTab1.GetItem(r, c));
            }
        }

        function ShowTabPage2(tab) {
            var i = 1;
            var tabamount = 5;
            for (i = 1; i <= tabamount; i++) {
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).className = "tabTypeTdSelected";
                } else {
                    document.getElementById("tab_" + i).style.visibility = "hidden";
                    document.getElementById("stab_" + i).className = "tabTypeTdDefault";
                }
            }
        }

        function Validate() {
            return confirm("ต้องเปลี่ยนแปลงสถานะการรอบัญชีใช่หรือไม่");
        }
    </script>

    <style type="text/css">
        .tabTypeDefault
        {
            width: 100%;
            border-spacing: 2px;
        }
        .tabTypeTdDefault
        {
            width: 20%;
            height: 45px;
            font-family: Tahoma, Sans-Serif, Times;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            color: #777777;
            border: solid 1px #55A9CD;
            background-color: rgb(200,235,255);
            cursor: pointer;
        }
        .tabTypeTdSelected
        {
            width: 20%;
            height: 45px;
            font-family: Tahoma, Sans-Serif, Times;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            color: #660066;
            border: solid 1px #77CBEF;
            background-color: #76EFFF;
            cursor: pointer;
            text-decoration: underline;
        }
        .tabTypeTdDefault:hover
        {
            color: #882288;
            border: solid 1px #77CBEF;
            background-color: #98FFFF;
        }
        .tabTypeTdSelected:hover
        {
            color: #882288;
            border: solid 1px #77CBEF;
            background-color: #98FFFF;
        }
        .tabTableDetail
        { 
            width: 99%;
        }
        .tabTableDetail td
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server" Text=""></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_dp_kt_dept_edit_master_kt"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" ClientEventItemChanged="OnDwMainItemChanged"
        ClientFormatting="True">
    </dw:WebDataWindowControl>

    <asp:HiddenField ID="HSelect" runat="server" Value="01" />
</asp:Content>

