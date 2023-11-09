<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_dp_edit_dept_prncfixed_masdue.aspx.cs" Inherits="Saving.Applications.ap_deposit.w_sheet_dp_edit_dept_prncfixed_masdue" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postAccountNo%>
    <%=postRefresh%>


     <script type="text/javascript">

        var openDlgBy = "";

        function MenubarNew() {
            window.location = Gcoop.GetUrl() + "Applications/ap_deposit/w_sheet_dp_edit_dept.aspx";
        }

        function MenubarOpen() {
            openDlgBy = "Menubar";
            Gcoop.OpenIFrame(900, 600, "w_dlg_dp_account_search.aspx", "");
        }

        function NewAccountNo(memcoopid, accNo) {
            if (openDlgBy == "Menubar") {
                objDwMain.SetItem(1, "deptaccount_no", Gcoop.Trim(accNo));
                Gcoop.GetEl("HfCoopid").value = memcoopid + "";
                Gcoop.GetEl("HfDlg").value = "AccountDlg";
                objDwMain.AcceptText();
                postAccountNo();
            }
           
        }

        function OnDwMainItemChanged(s, r, c, v) {
            if (c == "deptaccount_no") {
                objDwMain.SetItem(r, "deptaccount_no", v);
                objDwMain.AcceptText();
                postAccountNo();
            }
        }

        function OnDwTabMasdueItemChanged(s, r, c, v) {
            if (c == "seq_no") {
                s.SetItem(r, c, v);
                s.AcceptText();
            }
            else if (c == "start_tdate") {
                s.SetItem(r, c, v);
                s.AcceptText();
                s.SetItem(r, "start_date", Gcoop.ToEngDate(v));
                s.AcceptText();
            }
            else if (c == "end_tdate") {
                s.SetItem(r, c, v);
                s.AcceptText();
                s.SetItem(r, "end_date", Gcoop.ToEngDate(v));
                s.AcceptText();
            }
            else if (c == "peroid_count") {
                s.SetItem(r, c, v);
                s.AcceptText();
            }
            else if (c == "peroid_last") {
                s.SetItem(r, c, v);
                s.AcceptText();
            }
        }


  
        function OnDwTabPrncfixedItemChanged(s, r, c, v) {
            if (c == "prnc_no") {
                s.SetItem(r, "prnc_no", v);
                s.AcceptText();
            }
            else if (c == "prnc_amt") {
                s.SetItem(r, "prnc_amt", v);
                s.AcceptText();
            }
            else if (c == "prnc_tdate") {
                s.SetItem(r, "prnc_tdate", v);
                s.AcceptText();
                s.SetItem(r, "prnc_date", Gcoop.ToEngDate(v));
                s.AcceptText();
            //    Gcoop.GetEl("HdRowPrncfix").value = r + "";
            //    postRefresh();
            }
            else if (c == "prncdue_tdate") {
                s.SetItem(r, c, v);
                s.AcceptText();
                s.SetItem(r, "prncdue_date", Gcoop.ToEngDate(v));
                s.AcceptText();
            }
            else if (c == "interest_rate") {
                s.SetItem(r, c, v);
                s.AcceptText();
            }
            else if (c == "lastcalint_tdate") {
                s.SetItem(r, c, v);
                s.AcceptText();
                s.SetItem(r, "lastcalint_date", Gcoop.ToEngDate(v));
                s.AcceptText();
            }
            else if (c == "prnc_bal") {
                s.SetItem(r, c, v);
                s.AcceptText();
            }
            return 0;
        }

 
        function SheetLoadComplete() {
            var tab = Gcoop.ParseInt(Gcoop.GetEl("HdCurrentTab").value);
            ShowTabPage2(tab);
        }

        function ShowTabPage2(tab) {
            var tabamount = 5;
            for (i = 1; i <= tabamount; i++) {
                document.getElementById("tab_" + i).style.visibility = "hidden";
                document.getElementById("stab_" + i).className = "tabTypeTdDefault";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).className = "tabTypeTdSelected";
                    Gcoop.GetEl("HdCurrentTab").value = i + "";
                }
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
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
    <asp:Literal ID="LtCurrentTab" runat="server"></asp:Literal>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_dept_edit_deptmaster"
        LibraryList="~/DataWindow/ap_deposit/dp_edit_dept.pbl" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    <table class="tabTypeDefault">
        <tr>
            <td class="tabTypeTdSelected" id="stab_1" onclick="ShowTabPage2(1);">
                ต้นเงินฝาก
            </td>
            <td class="tabTypeTdDefault" id="stab_2" onclick="ShowTabPage2(2);">
                วันครบกำหนด
            </td>          
        </tr>
    </table>
    <table class="tabTableDetail">
        <tr>
            <td style="height: 200px;" valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">
                <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Vertical">
                    <dw:WebDataWindowControl ID="DwTabPrncfixed" runat="server" DataWindowObject="d_dept_edit_deptprncfixed"
                        LibraryList="~/DataWindow/ap_deposit/dp_edit_dept.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventButtonClicked="OnDwTabPrncfixedButtonClick" ClientEventItemChanged="OnDwTabPrncfixedItemChanged" >
                    </dw:WebDataWindowControl>
                    </asp:Panel>
                </div>
                <div id="tab_2" style="visibility: hidden; position: absolute;">
                    
                    <asp:Panel ID="Panel2" runat="server">
                        <dw:WebDataWindowControl ID="DwTabMasdue" runat="server" LibraryList="~/DataWindow/ap_deposit/dp_edit_dept.pbl"
                            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                            ClientScriptable="True" HorizontalScrollBar="None" Visible="True" DataWindowObject="d_dept_edit_deptmasdue"
                            RowsPerPage="1" ClientEventItemChanged="OnDwTabMasdueItemChanged" ClientEventButtonClicked="OnDwTabMasdueButtonClicked">
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </div>

            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HSelect" runat="server" Value="01" />
    <asp:HiddenField ID="HdCurrentTab" runat="server" Value="1"/>
    <asp:HiddenField ID="HdDeptAccNo" runat="server" />
    <asp:HiddenField ID="HdMemcoopDlg" runat="server" />
    <asp:HiddenField ID="HdCoopidDlg" runat="server" />
    <asp:HiddenField ID="HdRowPrncfix" runat="server" />
    <asp:HiddenField ID="HdRowMemCo" runat="server" />
    <asp:HiddenField ID="HfCoopid" runat="server" />
    <asp:HiddenField ID="HfDlg" runat="server" />
</asp:Content>
