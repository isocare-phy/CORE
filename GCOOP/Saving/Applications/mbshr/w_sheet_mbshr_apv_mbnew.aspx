﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_mbshr_apv_mbnew.aspx.cs" Inherits="Saving.Applications.mbshr.w_sheet_mbshr_apv_mbnew" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-family: Tahoma;
        }
    </style>
    <%=initJavaScript %>
    <%=postInit%>
    <%=postSetStatus%>
    <%=postRequestStatus%>
    <%=postSetMemberNo %>
    <script type="text/javascript">
        function Validate() {
            objDw_option.AcceptText();
            objDw_list.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDwOptionButtonClick(s, r, b) {
            if (b == "b_sch") {
                objDw_option.AcceptText();
                postInit();
            }
            else if (b == "b_lastno") {
                Gcoop.OpenIFrame2('420', '180', 'w_dlg_mbshr_getmembno.aspx', '');
            }
        }

        function OnDwListButtonClick(s, r, b) {
            if (objDw_list.RowCount() > 0) {
                if (b == "b_wait" || b == "b_apv" || b == "b_noapv") {
                    Gcoop.GetEl("Hdbutton").value = b;
                    postSetStatus();
                }
                else if (b == "b_memberno") {
                    if (confirm("คุณต้องการสร้างเลขทะเบียนใช่หรือไม่ ?")) {
                        postSetMemberNo();
                    }
                }
            }
        }

        function OnDwListItemChange(s, r, c, v) {
            if (c == "appl_status") {
                objDw_list.SetItem(r, "appl_status", v);
                objDw_list.AcceptText();
                Gcoop.GetEl("HdRow").value = r + "";
                postRequestStatus();
            }
            else if (c == "approve_tdate") {
                objDw_list.SetItem(1, "approve_tdate", v);
                objDw_list.AcceptText();
                objDw_list.SetItem(1, "apv_date", Gcoop.ToEngDate(v));
                objDw_list.AcceptText();
            }
        }

        function OnDwOptionItemChange(s, r, c, v) {
            if (c == "apply_stdate") {
                objDw_list.SetItem(1, "apply_stdate", v);
                objDw_list.AcceptText();
                objDw_list.SetItem(1, "apply_sdate", Gcoop.ToEngDate(v));
                objDw_list.AcceptText();
            }
            else if (c == "apply_etdate") {
                objDw_list.SetItem(1, "apply_etdate", v);
                objDw_list.AcceptText();
                objDw_list.SetItem(1, "apply_edate", Gcoop.ToEngDate(v));
                objDw_list.AcceptText();
            }

            else if (c == "membdatefix_stdate") {
                objDw_list.SetItem(1, "membdatefix_stdate", v);
                objDw_list.AcceptText();
                objDw_list.SetItem(1, "membdatefix_sdate", Gcoop.ToEngDate(v));
                objDw_list.AcceptText();
            }
            else if (c == "membdatefix_etdate") {
                objDw_list.SetItem(1, "membdatefix_etdate", v);
                objDw_list.AcceptText();
                objDw_list.SetItem(1, "membdatefix_edate", Gcoop.ToEngDate(v));
                objDw_list.AcceptText();
            }

        }

        function MenubarOpen() {
            //Gcoop.OpenDlg('580', '590', 'w_dlg_mbshr_getmembno.aspx', '');
            Gcoop.OpenDlg2('400', '160', 'w_dlg_mbshr_getmembno.aspx', '');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <p>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <table style="width: 100%;">
            <tr>
                <td>
                    <dw:WebDataWindowControl ID="Dw_option" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        ClientScriptable="True" DataWindowObject="d_mbsrv_list_apvmbnew_opt" LibraryList="~/DataWindow/mbshr/mb_apvmbnew.pbl"
                        Style="top: 0px; left: 0px" ClientEventButtonClicked="OnDwOptionButtonClick"
                        ClientEventItemChanged="OnDwOptionItemChange">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td class="style1">
                    <strong>รายละเอียดอนุมัติเลขสมาชิก</strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" Height="400px">
                        <dw:WebDataWindowControl ID="Dw_list" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                            AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                            DataWindowObject="d_mbsrv_list_apvmbnew" LibraryList="~/DataWindow/mbshr/mb_apvmbnew.pbl"
                            Style="top: 0px; left: 0px" ClientEventButtonClicked="OnDwListButtonClick" ClientEventItemChanged="OnDwListItemChange">
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </p>
    <asp:HiddenField ID="Hdbutton" runat="server" />
    <asp:HiddenField ID="HdRow" runat="server" />
</asp:Content>
