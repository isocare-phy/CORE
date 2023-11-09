<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.ap_deposit.w_sheet_dp_approve_chgdept_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="20%">
                    <div>
                        <span>วันที่ร้องขอ:</span>
                    </div>
                </td>
                <td width="20%">
                     <asp:TextBox ID="select_date" runat="server" style="text-align:center"></asp:TextBox>
                </td>
                <td width="5%">
                    <div>
                        <span>-</span>
                    </div>
                </td>
                  <td width="20%">
                     <asp:TextBox ID="end_date" runat="server" style="text-align:center"></asp:TextBox>
                </td>
                <td >
                    <asp:Button ID="b_search" runat="server" Text="ค้นหา" Width="55px" />
                </td>
            </tr>
        </table>
         <br />
        <table class="DataSourceFormView">
            <tr>
                <td>
                    <div><u>รายการใบคำขอ</u></div>
                </td>
            </tr>
            <tr>
                <td width="55%" colspan="2">
                    <asp:CheckBox ID="all_check" runat="server" Text=" เลือกทั้งหมด" />
                </td>             
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
