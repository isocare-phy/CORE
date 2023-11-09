<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.assist.ws_sheet_as_genrequest_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="15%">
                    <div>
                        <span>วันที่ทำรายการ:</span>
                    </div>
                </td>
                <td width="15%">
                    <asp:TextBox ID="work_date" runat="server" ReadOnly="true" Style="text-align:center" BackColor="#DDDDDD" ></asp:TextBox>
                </td>
                <td width="10%">
                    <span>ปีประมวล:</span>
                </td>
                <td width="15%">
                    <asp:DropDownList ID="process_year" runat="server" Style="text-align:center">
                    </asp:DropDownList>
                </td>
                <td width="15%">
                    <span>เดือนประมวล:</span>
                </td>
                <td width="15%">
                    <asp:DropDownList ID="process_month" runat="server" >
                        <asp:ListItem Value="01" >มกราคม</asp:ListItem>
                        <asp:ListItem Value="02" >กุมภาพันธ์</asp:ListItem>
                        <asp:ListItem Value="03" >มีนาคม</asp:ListItem>
                        <asp:ListItem Value="04" >เมษายน</asp:ListItem>
                        <asp:ListItem Value="05" >พฤษภาคม</asp:ListItem>
                        <asp:ListItem Value="06" >มิถุนายน</asp:ListItem>
                        <asp:ListItem Value="07" >กรกฎาคม</asp:ListItem>
                        <asp:ListItem Value="08" >สิงหาคม</asp:ListItem>
                        <asp:ListItem Value="09" >กันยายน</asp:ListItem>
                        <asp:ListItem Value="10" >ตุลาคม</asp:ListItem>
                        <asp:ListItem Value="11" >พฤศจิกายน</asp:ListItem>
                        <asp:ListItem Value="12" >ธันวาคม</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span>ประเภทเงิน:</span>
                </td>
                <td>
                    <asp:DropDownList ID="moneytype_code" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <span>ธนาคาร:</span>
                </td>
                <td>
                    <asp:DropDownList ID="expense_bank" runat="server" >
                    </asp:DropDownList>
                </td>
                <td>
                    <span>สาขา:</span>
                </td>
                <td>
                    <asp:DropDownList ID="expense_branch" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span>รหัสบัญชี:</span>
                </td>
                <td>
                    <asp:DropDownList ID="tofrom_accid" runat="server" BackColor="#FFFF99" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <span>ประเภทสวัสดิการ:</span>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="assisttype_code" runat="server" ></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align = "center" colspan = "6">
                    <br />
                    <asp:Button ID="b_process" runat="server" Text="ประมวลใบคำขอ" />  
                    <asp:Button ID="b_save" runat="server" Text="บันทึก" />  
                </td>
            </tr>
        </table>
        <br /> 
        <div><u>รายการใบคำขอ</u></div>
        <table class="DataSourceFormView">
            <tr>
                <td width="15%" colspan="2">
                    <asp:CheckBox ID="all_check" runat="server" Text=" เลือกทั้งหมด" />
                </td>
                <td width="7%">
                </td>
                <td width="33%">
                </td>
                <td width="15%">
                </td>
                <td width="13%">
                </td>
                <td width="17%">
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
