<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_account_detail.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_account_detail" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=GetMainDetail%>
    <%=popupReport%>
    <%=runProcess%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้าจอ")) {
                window.location = "";
            }
        }
        function showTabPage(tab) {
            var i = 1;
            var tabamount = 6;
            for (i = 1; i <= tabamount; i++) {
                document.getElementById("tab_" + i).style.visibility = "hidden";
                document.getElementById("stab_" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).style.backgroundColor = "rgb(211,213,255)";
                    Gcoop.GetEl("HiddenFieldTab").value = i + "";
                }
            }
        }
        function SheetLoadComplete() {
        

            var tab = Gcoop.ParseInt(Gcoop.GetEl("HiddenFieldTab").value);
            showTabPage(tab);
            if (tab == "2") {
                try 
                {
                    Gcoop.SetLastFocus("int_start_tdate_" + (objDwIntRate.RowCount() - 1));
                    Gcoop.Focus();
                } catch (err) { }
            }
            if (Gcoop.GetEl("HdIspostback").value == "false") {
            
                Gcoop.SetLastFocus("account_no_0");
                Gcoop.Focus();
            }
        }
        function OnDwMainItemChanged(s, r, c, v) {
            Gcoop.GetEl("HdcheckPdf").value = "False";
            objDwMain.SetItem(r, c, v);
            objDwMain.AcceptText();
            if (c == "account_no") {
                GetMainDetail();
            }
        }
        function ChooseAcc(account_no) {
            objDwMain.SetItem(1, "account_no", account_no);
            objDwMain.AcceptText();
            GetMainDetail();
            Gcoop.GetEl("HdcheckPdf").value = "False";
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_account_list.aspx", "");
        }
        function OnDwMainButtonClicked(s, r, c, v) {
            if (c == "b_accountno") {
                Gcoop.OpenIFrame("630", "350", "w_dlg_account_list.aspx", "");
            }
        }
        function insert() {
            row = objDwIntRate.InsertRow(objDwIntRate.RowCount() + 1);
        }
        function OnDwIntRateClick(s,r,c,v) {

            if (c = "b_del") {
                objDwIntRate.DeleteRow(r);
            }
        }
        function OnClickLinkNextReport() {
            //objdw_main.AcceptText();
            popupReport();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="HdIspostback" runat="server" />
    <asp:HiddenField ID="HiddenFieldTab" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HdcheckPdf" runat="server" Value="False" />
    <table style="width: 100%;">
        <tr>
            <td align="left">
                <span style="cursor: pointer" onclick="OnClickLinkNextReport();">พิมพ์ตารางการตัดบัญชีส่วนเกิน/ส่วนต่ำเงินลงทุน
               </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_show_account_detail"
        LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="OnDwMainItemChanged"
        ClientFormatting="True" TabIndex="1" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <table style="width: 100%; border: solid 1px; margin-top: 5px">
        <tr align="center" class="dwtab">
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_1" width="16.6%"
                onclick="showTabPage(1);">
                รายการเคลื่อนไหว
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_2" width="16.6%"
                onclick="showTabPage(2);">
                อัตราดอกเบี้ย
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_3" width="16.6%"
                onclick="showTabPage(3);">
                วันรับดอกเบี้ย
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_4" width="16.6%"
                onclick="showTabPage(4);">
                ประวัติการรับดอกเบี้ย
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_5" width="16.6%"
                onclick="showTabPage(5);">
                ประวัติการจำนำ
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_6" width="16.6%"
                onclick="showTabPage(6);">
                รายละเอียด Duration
            </td>
        </tr>
    </table>
    <table style="width: 100%; border: solid 1px; margin-top: 2px" class="dwcontent">
        <tr>
            <td style="height: 400px;" valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <dw:WebDataWindowControl ID="DwMove" runat="server" DataWindowObject="d_show_movement_detail"
                        LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientFormatting="True"
                        Height="399px" Width="745px">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_2" style="visibility: hidden; position: absolute;">
                    <%--<input id="Button1" type="button" value="เพิ่มแถว" onclick="insert();" />--%>
                    <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" onclick="insert();" Font-Size="Medium"
                        Font-Underline="True" ForeColor="#0000CC"></asp:Label>
                    <dw:WebDataWindowControl ID="DwIntRate" runat="server" DataWindowObject="d_show_intrate_detail"
                        LibraryList="~/DataWindow/pm/pm_investment.pbl" Height="399px" Width="745px"
                        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                        ClientScriptable="True" ClientFormatting="True" ClientEventButtonClicked="OnDwIntRateClick"
                        TabIndex="500">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_3" style="visibility: hidden; position: absolute;">
                    <dw:WebDataWindowControl ID="DwDueDate" runat="server" DataWindowObject="d_show_duedate_detail"
                        LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" Height="399px"
                        Width="745px" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_4" style="visibility: hidden; position: absolute;">
                    <dw:WebDataWindowControl ID="DwIntHis" runat="server" DataWindowObject="d_show_inthistory_detail"
                        LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" Height="399px"
                        Width="745px" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_5" style="visibility: hidden; position: absolute;">
                    <dw:WebDataWindowControl ID="DwWarranty" runat="server" DataWindowObject="d_show_warranty_history_detail"
                        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                        ClientScriptable="True" LibraryList="~/DataWindow/pm/pm_investment.pbl" Height="399px"
                        Width="745px" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_6" style="visibility: hidden; position: absolute;">
                    <dw:WebDataWindowControl ID="DwDuration" runat="server" DataWindowObject="d_show_duration_detail"
                        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                        ClientScriptable="True" LibraryList="~/DataWindow/pm/pm_investment.pbl" Height="399px"
                        Width="745px" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
