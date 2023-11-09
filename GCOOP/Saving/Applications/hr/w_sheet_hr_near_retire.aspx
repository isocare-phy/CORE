<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_near_retire.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_near_retire" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
            font-family: Tahoma;
        }
        .style2
        {
            font-size: small;
            font-family: Tahoma;
            color: #FF3300;
        }
        </style>
    <%=postSearch%>    <%=postNewClear %>
    <%=postNewClear %>
    <script type ="text/javascript">
        function Validate() 
        {
            //objDwMain.AcceptText();
           // return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNewClear();
            }
        }
        
        function OnDwMainButtonClick(s, r, c) 
        {
            if (c == "b_search") 
            {
                var near_year = null;
                near_year = objDw_main.GetItem(1, "ai_year");
                if (near_year == null) {
                    alert("กรุณาเลือกจำนวนปีที่ใกล้เกษียณอายุ");
                }
                else {
                    postSearch();
                }
            }
            return 0;
        }

        function OnDwMainItemChange(s, r, c, v) 
        {
            if (c == "ai_year") 
            {
                objDw_main.SetItem(1, "ai_year",v);
                objDw_main.AcceptText();
                Gcoop.GetEl("Hd_year").value = v;
            }
            return 0;
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    
    <table style="width:100%;">
        <tr>
            <td class="style1" colspan="3">
                เลือกปี</td>
        </tr>
        <tr>
            <td colspan="3">
                <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" 
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientFormatting="True" ClientScriptable="True" 
                    DataWindowObject="dw_hr_bkempoy_year" 
                    LibraryList="~/DataWindow/hr/hr_bkemploy.pbl" 
                    ClientEventButtonClicked="OnDwMainButtonClick" 
                    ClientEventItemChanged="OnDwMainItemChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <span class="style1">รายชื่อเจ้าหน้าที่ใกล้เกษียณอายุ&nbsp; </span>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <dw:WebDataWindowControl ID="Dw_detail" runat="server" 
                    AutoRestoreContext="False" AutoRestoreDataCache="True" 
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" 
                    ClientScriptable="True" DataWindowObject="dw_hr_bkemploy_near" 
                    LibraryList="~/DataWindow/hr/hr_bkemploy.pbl" RowsPerPage="15">
                    <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                    </PageNavigationBarSettings>
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                <span class="style1">จำนวน </span><b>&nbsp;
                <asp:Label ID="lbl_rowcount" runat="server" CssClass="style2" Text="0"></asp:Label>
&nbsp; </b><span class="style1">&nbsp;ราย</span></td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hidden_search" runat="server" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:HiddenField ID="Hd_year" runat="server" />
                </td>
        </tr>
    </table>
    
</asp:Content>
