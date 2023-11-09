<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.assist.dlg.ws_dlg_sl_addassisttype_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 500px">
            <tr>
                <td colspan="4">
                    <strong><u>โครงการสวัสดิการประเภทใหม่</u></strong>
                </td>
            </tr>
            <tr>
                <td width="20%">
                    <div>
                        <span>รหัสสวัสดิการ:</span></div>
                </td>
                <td width="15%">
                    <asp:TextBox ID="assisttype_code" runat="server"></asp:TextBox>
                </td>
                <td width="20%">
                    <div>
                        <span>ชื่อสวัสดิการ:</span></div>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="assisttype_desc" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div><span>กลุ่มสวัสดิการ:</span></div>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="assisttype_group" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div><span>คำนวณในรอบ:</span></div>
                </td>
                <td>
                    <asp:DropDownList ID="process_flag" runat="server">
                        <asp:ListItem Value="0">ไม่ระบุ</asp:ListItem>
                        <asp:ListItem Value="1">เดือน</asp:ListItem>
                        <asp:ListItem Value="2">ปี</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <div><span>ตัวคำนวณ:</span></div>
                </td>
                <td>
                    <asp:DropDownList ID="calculate_flag" runat="server">
                        <asp:ListItem Value="1">เกรดเฉลี่ย</asp:ListItem>
                        <asp:ListItem Value="2">อายุ</asp:ListItem>
                        <asp:ListItem Value="3">อายุการเป็นสมาชิก</asp:ListItem>
                        <asp:ListItem Value="4">เงินเดือน</asp:ListItem>
                        <asp:ListItem Value="5">ค่าเสียหาย</asp:ListItem>
                        <asp:ListItem Value="6">ตามประเภทการจ่าย</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div><span>เช็คครอบครัว:</span></div>
                </td>
                <td>
                    <asp:DropDownList ID="family_flag" runat="server">
                        <asp:ListItem Value="0">ไม่เช็ค</asp:ListItem>
                        <asp:ListItem Value="1">เช็ค</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div><span>แยกประเภทสมาชิก:</span></div>
                </td>
                <td>
                    <asp:CheckBox ID="membtype_flag" runat="server" ></asp:CheckBox>
                </td> 
            </tr>
            <tr>
                <td>
                    <div><span>ทุนต่อเนื่อง:</span></div>
                </td>
                <td>
                    <asp:CheckBox ID="stm_flag" runat="server" ></asp:CheckBox>
                </td> 
            </tr>
            <tr>
                <td align="right" colspan="4">
                    <br />
                    &nbsp;
                    <asp:Button ID="b_add" runat="server" Text="ตกลง" Width="70px" />&nbsp;
                    <asp:Button ID="b_cancel" runat="server" Text="ยกเลิก" Width="70px" />
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
