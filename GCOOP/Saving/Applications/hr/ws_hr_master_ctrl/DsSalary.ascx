<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsSalary.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsSalary" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="FormStyle">
            <tr>
                <td width="25%">
                    เงินเดือน:
                </td>
                <td width="25%">
                    <asp:TextBox ID="salary_amt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td width="25%">
                    วิธีการจ่ายเงินเดือน:
                </td>
                <td width="25%">
                    <asp:DropDownList ID="salexp_code" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="CSH">เงินสด</asp:ListItem>
                        <asp:ListItem Value="CBT">บัญชีธนาคาร</asp:ListItem>
                        <asp:ListItem Value="TRN">บัญชีสหกรณ์</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    ธนาคาร:
                </td>
                <td>
                    <asp:DropDownList ID="salexp_bank" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    สาขา:
                </td>
                <td>
                    <asp:DropDownList ID="salexp_branch" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    เลขบัญชี:
                </td>
                <td>
                    <asp:TextBox ID="salexp_accid" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <span style="font-size: 12px;"><font color="#cc0000"><u><strong>ภาษี</strong></u></font></span>
                </td>
            </tr>
            <tr>
                <td>
                    วิธีการคำนวนภาษี:
                </td>
                <td>
                    <asp:DropDownList ID="tax_calcode" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="INC">รวมกับเงินเดือน</asp:ListItem>
                        <asp:ListItem Value="EXC">ไม่รวมกับเงินเดือน</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    ภาษีถูกหักยกมา:
                </td>
                <td>
                    <asp:TextBox ID="tax_bfamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <span style="font-size: 12px;"><font color="#cc0000"><u><strong>ประกันสังคม</strong></u></font></span>
                </td>
            </tr>
            <tr>
                <td>
                    เคยมีประกันสังคมหรือไม่:
                </td>
                <td>
                    <asp:DropDownList ID="ss_appfirststs" runat="server">
                        <asp:ListItem Value="0">ไม่เคย</asp:ListItem>
                        <asp:ListItem Value="1">เคยมี</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    หักเงินประกันสังคมหรือไม่:
                </td>
                <td>
                    <asp:DropDownList ID="ss_status" runat="server">
                        <asp:ListItem Value="0">ไม่หัก</asp:ListItem>
                        <asp:ListItem Value="1">หัก</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    วันที่สมัครประกันสังคม:
                </td>
                <td>
                    <asp:TextBox ID="ss_appdate" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    ยอดเงินประกันสังคมยกมา:
                </td>
                <td>
                    <asp:TextBox ID="ss_bfamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    สถานพยาบาลที่เลือก:
                </td>
                <td>
                    <asp:TextBox ID="ss_hospital" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <span style="font-size: 12px;"><font color="#cc0000"><u><strong>กองทุน</strong></u></font></span>
                </td>
            </tr>
            <tr>
                <td>
                    หักเงินสะสมกองทุนสำรองหรือไม่:
                </td>
                <td>
                    <asp:DropDownList ID="provf_status" runat="server">
                        <asp:ListItem Value="0">ไม่หัก</asp:ListItem>
                        <asp:ListItem Value="1">หัก</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    เงินกองทุนยกมา:
                </td>
                <td>
                    <asp:TextBox ID="provf_bfamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    กองทุนสำรองฯ นายจ้างร้อยละ:
                </td>
                <td>
                    <asp:TextBox ID="provf_corprate" runat="server" Style="text-align: center;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td>
                    วันที่สมัครเข้ากองทุนสำรองฯ:
                </td>
                <td>
                    <asp:TextBox ID="provf_appdate" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    กองทุนสำรองฯ จนท.ร้อยละ:
                </td>
                <td>
                    <asp:TextBox ID="provf_emprate" runat="server" Style="text-align: center;" ToolTip="#,##0.00"></asp:TextBox>
                </td>
                <td>
                    วันที่ลาออกจากกองทุนสำรองฯ:
                </td>
                <td>
                    <asp:TextBox ID="provf_resigndate" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
