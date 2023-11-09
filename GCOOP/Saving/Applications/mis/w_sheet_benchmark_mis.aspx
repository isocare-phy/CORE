<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_benchmark_mis.aspx.cs" Inherits="Saving.Applications.mis.w_sheet_benchmark_mis" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsClickSubmit %>
    <%=popupReport%>
    <%=runProcess%>
    <%=downloadPDF%>
    <script type="text/javascript">
        function Validate() {
        }
        function ClickSubmit() {
            jsClickSubmit();
        }
        function B_retrieve_onclick() {

        }

        function pdfbutton_onclick() {

        }

        //ฟังก์ชั่นรายงาน**********************************************************************
        function OnClickLinkNext() {

            objdw_cri.AcceptText();
            //alert("aaaa");  //DEBUG
            popupReport();
        }

    </script>
    <style type="text/css">
        .style1
        {
            width: 76%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <span style="cursor: pointer" onclick="OnClickLinkNext();">-พิมพ์รายงานวัดประสิทธิภาพ</span>
    <br />
    <table style="width: 100%; border: solid 1px; margin-top: 5px">
        <tr>
            <td class="style1">
                <dw:WebDataWindowControl ID="dw_cri" runat="server" DataWindowObject="d_mis_cri_monthyear_nobutton"
                    LibraryList="~/DataWindow/mis.pbl;~/DataWindow/mis/criteria.pbl" ClientEventItemChanged="ItemChangeDwCri"
                    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" ClientScriptable="True" Width="600">
                </dw:WebDataWindowControl>
            </td>
            <td width="20%">
                <input type="button" id="B_retrieve" onclick="ClickSubmit();" value="แสดงข้อมูล"
                    style="margin-left: 65px;" onclick="return B_retrieve_onclick()" />
                <%--<asp:Button ID="B_retrieve" runat="server" onclick="B_click" Text="แสดงข้อมูล" />--%>
            </td>
            <%--<td>
                <input id="pdfbutton" onclick="return pdfbutton_onclick()" onclick="downloadPDF()"
                    type="button" value="ดาวน์โหลดเป็น PDF" />
            </td> --%>
        </tr>
        <tr>
            <td colspan="2">
                <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_mis_ratio_values"
                    LibraryList="~/DataWindow/mis/camels.pbl" ClientScriptable="True" ClientEventButtonClicked="OnOpenDlgVar"
                    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" Width="740" VerticalScrollBar="Auto" BorderWidth="1"
                    Height="500">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HdcheckPdf" runat="server" Value="False" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
    <asp:HiddenField ID="Hdcommitreport" runat="server" Value="false" />
</asp:Content>
