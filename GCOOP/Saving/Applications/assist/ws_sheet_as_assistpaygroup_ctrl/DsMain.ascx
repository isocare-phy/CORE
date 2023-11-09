<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.assist.ws_sheet_as_assistpaygroup_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="7%" valign="top">
                    <div>
                        <span>ทะเบียน:</span>
                    </div>
                </td>
                <td width="10%" valign="top">
                    <asp:TextBox ID="member_no" runat="server" Style="text-align: center; "></asp:TextBox> 
                </td>
                <td width="3%" valign="top">
                    <asp:Button ID="b_searchMem" runat="server" Text="..." Width="20px" />
                </td>
                <td width="10%" valign="top">
                    <div>
                        <span>ประเภทสวัสดิการ:</span>
                    </div>
                </td>
                <td width="5%" valign="top">
                    <asp:TextBox ID="assist_code" runat="server"></asp:TextBox>
                </td>
                <td width="15%" valign="top">
                    <asp:DropDownList ID="assisttype_code" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="10%" valign="top">
                    <div>
                        <span>ประเภทเงิน:</span>
                    </div>
                </td>
                <td  width="10%" valign="top">
                    <asp:DropDownList ID="moneytype_code" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="20%" valign="top">
                    <asp:Button ID="b_search" runat="server" Text="ดึงข้อมูล" Width="60px" />   
                    <asp:Button ID="b_pay" runat="server" Text="จ่ายสวัสดิการ" Width="90" BackColor="Blue" Font-Bold="true" Font-Size="14px" ForeColor="White"/>
                </td>
            </tr>
        </table>
        <table class="DataSourceFormView">
            <tr>                
                <td width="10%">
                    <div>
                        <span style="font-size: 11px;">ธนาคาร :</span>
                    </div>
                </td>
                <td width="15%">
                    <asp:DropDownList ID="expense_bank" runat="server" >
                    </asp:DropDownList>
                </td>  
                <td width="10%">
                    <div>
                        <span style="font-size: 11px;">สาขา :</span>
                    </div>
                </td>
                <td width="15%">
                    <asp:DropDownList ID="expense_branch" runat="server">
                    </asp:DropDownList>
                </td>                
                <td width="10%">
                    <div>
                        <span style="font-size: 11px;">รหัสบัญชี :</span>
                    </div>
                </td>
                <td width="15%">
                    <asp:DropDownList ID="tofrom_accid" runat="server" BackColor="#FFFF99" >
                    </asp:DropDownList>
                </td>
            </tr>          
        </table>
        <br />
        <table class="DataSourceFormView">
            <tr>
                <td width="15%" colspan="2">
                    <asp:CheckBox ID="select_check" runat="server" Text=" เลือกทั้งหมด" />
                </td>                             
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
