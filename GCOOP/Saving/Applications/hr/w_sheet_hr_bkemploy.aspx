<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_bkemploy.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_bkemploy" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-family: Tahoma;
            font-weight: bold;
            font-size: small;
        }
        .style2
        {
            font-size: small;
        }
        .style3
        {
            font-size: small;
            font-weight: bold;
        }
    </style>
    <%=initJavaScript %>
    <%=postCalculate%>
    <%=postNewClear%>
    <script type ="text/javascript">
        function OnDwMainButtonClick(s,r,c)
        {
            if(c=="b_cal")
            {
                objDw_main.AcceptText();
                postCalculate();
            }
            return 0;
        }
        
        function Validate()
        {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        
         function MenubarNew()
         {
            if(confirm("ยืนยันการล้างข้อมูลบนหน้าจอ"))
            {
                postNewClear();
            }
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width:100%;">
        <tr>
            <td class="style1">
                รายละเอียดการดึงข้อมูล</td>
        </tr>
        <tr>
            <td>
                <span class="style2">ปีที่เกษียณ </span>&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txt_year" runat="server" Width="69px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
                <asp:Button ID="B_ok" runat="server" Text="ดึงข้อมูล" 
                    UseSubmitBehavior="False" onclick="B_ok_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                รายละเอียดเจ้าหน้าที่ใกล้เกษียณอายุ</td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" 
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientFormatting="True" ClientScriptable="True" 
                    DataWindowObject="dw_hr_bkemploy_detail" 
                    LibraryList="~/DataWindow/hr/hr_bkemploy.pbl" 
                    ClientEventButtonClicked="OnDwMainButtonClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
 
</asp:Content>
