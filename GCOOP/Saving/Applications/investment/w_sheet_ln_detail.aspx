<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_detail.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_detail" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostMemberNo%>
    <%=jsPostListClick%>
    <script type="text/javascript">
        function SheetLoadComplete() {
            var tab = Gcoop.ParseInt(Gcoop.GetEl("HiddenFieldTab").value);
            showTabPage2(tab);
        }

        function showTabPage2(tab) {
            var i = 1;
            var tabamount = 3;
            for (i = 1; i <= tabamount; i++) {
                document.getElementById("tab_" + i).style.visibility = "hidden";
                document.getElementById("stab_" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).style.backgroundColor = "rgb(211,213,255)";
                    Gcoop.GetEl("HiddenFieldTab").value = i + "";
                    if (i == 1) {
                        document.getElementById("td").style.height = "400px";
                    }
                    if (i == 2) {
                        document.getElementById("td").style.height = "400px";
                    }
                    if (i == 3) {
                        document.getElementById("td").style.height = "400px";
                    }
                }
            }
        }

        function OnDwMainItemChanged(sender, row, col, Value) {
            if (col == "member_no") {
                sender.SetItem(row, col, Value);
                sender.AcceptText();
                jsPostMemberNo();
            }
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_member_search_new.aspx", "");
        }

        function ReceiveMemberNo(member_no, memb_name) {
            objDwMain.SetItem(1, "member_no", member_no);
            objDwMain.AcceptText();
            jsPostMemberNo();
        }

        function OnDwListClick(sender, row, col) {
            Gcoop.GetEl("Hd_row").value = row;
            jsPostListClick();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_ln_loan_main"
        LibraryList="~/DataWindow/investment/loan_statement.pbl" ClientScriptable="True" ClientEvents="true"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        AutoRestoreContext="False" ClientEventItemChanged="OnDwMainItemChanged" ClientEventButtonClicked="MenubarOpen">
    </dw:WebDataWindowControl>
    <table style="width:100%;">
        <tr>
            <td>
                <dw:WebDataWindowControl ID="Dwlist" runat="server" DataWindowObject="d_ln_loan_contract"
                    LibraryList="~/DataWindow/investment/loan_statement.pbl" Height="100px"
                    ClientScriptable="True" ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" AutoRestoreContext="False" 
                    ClientEventClicked="OnDwListClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <%-----------------------------------------------------------------------------------------------------------%>
    <table style="width: 100%;">
        <tr valign="top">
            <td>
                <%-----------------------------------------------------------------------------------------------------------%>
                <table style="width: 100%; border: solid 1px; margin-top: 1px">
                    <tr align="center" class="dwtab">
                        <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_1" width="30%"
                            onclick="showTabPage2(1);">
                            รายละเอียด
                        </td>
                        <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_2" width="30%"
                            onclick="showTabPage2(2);">
                            รายการเคลื่อนไหว
                        </td>
                        <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_3" width="30%"
                            onclick="showTabPage2(3);">
                            ค้ำประกัน
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; border: solid 1px; margin-top: 2px" class="dwcontent">
                    <tr>
                        <td id="td" style="height: 285px;" valign="top">
                            <div id="tab_1" style="visibility: visible; position: absolute;">
                                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_ln_loan_detail"
                                    LibraryList="~/DataWindow/investment/loan_statement.pbl" Width="765px" Height="400px"
                                    ClientScriptable="True" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                                </dw:WebDataWindowControl>
                            </div>
                            <div id="tab_2" style="visibility: hidden; position: absolute;">
                                <dw:WebDataWindowControl ID="Dwstatement" runat="server" DataWindowObject="d_ln_loan_statement"
                                    LibraryList="~/DataWindow/investment/loan_statement.pbl" Width="765px" Height="400px"
                                    ClientScriptable="True" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
                                </dw:WebDataWindowControl>
                            </div>
                            <div id="tab_3" style="visibility: hidden; position: absolute;">
                                <dw:WebDataWindowControl ID="Dwcoll" runat="server" DataWindowObject="d_ln_loan_coll"
                                    LibraryList="~/DataWindow/investment/loan_statement.pbl" Width="765px" Height="400px"
                                    ClientScriptable="True" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventButtonClicked="DeleteBizYear">
                                </dw:WebDataWindowControl>
                            </div>
                        </td>
                    </tr>
                </table>
                <%-----------------------------------------------------------------------------------------------------------%>
            </td>
        </tr>
    </table>
    <%-----------------------------------------------------------------------------------------------------------%>
    <asp:HiddenField ID="HiddenFieldTab" runat="server" Value="1" />
        <asp:HiddenField ID="Hd_row" runat="server" />
</asp:Content>
