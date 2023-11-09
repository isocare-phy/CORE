<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_appinsnew.aspx.cs"
    Inherits="Saving.Applications.app_assist.w_sheet_as_appinsnew" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=IncDetail%>
    <%=InstypeCode%>
    <%=GetStatus%>
    <%=newClear %>
    <script type="text/javascript">

        function OnDwListClicked(sender, row, column) {
            if (column == "insreqdoc_no") {
                //                alert(objdw_main.GetItem(row, "insreqdoc_no"));
                Gcoop.GetEl("HDinstype_code").value = objdw_main.GetItem(row, "insreqdoc_no");
                InstypeCode();
            }
        }

        function statuschange(s, r, c, v) {
            if (c == "insreq_status") {

                objdw_main.SetItem(r, c, v);
                objdw_main.AcceptText();
                Gcoop.GetEl("Hstatus").value = v;
                Gcoop.GetEl("hrow").value = r + "";
                // alert(v);
                GetStatus();
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function MenubarNew() {

            newClear();
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="HDinstype_code" runat="server" />
    <asp:HiddenField ID="hrow" runat="server" />
    <asp:HiddenField ID="HiddenFieldCode" runat="server" />
    <asp:HiddenField ID="Hstatus" runat="server" />
    <br />
    <table width="100%" border="0">
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="dw_main" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientEventClicked="OnDwListClicked" ClientEventItemChanged="statuschange"
                    ClientFormatting="True" ClientScriptable="True" DataWindowObject="d_sk_renewinsmain"
                    LibraryList="~/DataWindow/app_assist/as_appinsnew.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <br />
                <span style="font: tahoma; font-size: 12px; font-weight: bold;">รายละเอียด</span>
                <br />
                <dw:WebDataWindowControl ID="dw_detail" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" DataWindowObject="d_sk_apreqinsdetail" LibraryList="~/DataWindow/app_assist/as_appinsnew.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
</asp:Content>
