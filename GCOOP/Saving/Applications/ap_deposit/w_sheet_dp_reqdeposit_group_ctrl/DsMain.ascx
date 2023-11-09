<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.ap_deposit.w_sheet_dp_reqdeposit_group_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="5%">
                    <div>
                        <span>วันที่ทำรายการ:</span>
                    </div>
                </td>
                <td width="10%">
                    <asp:TextBox ID="operate_date" runat="server" Style="text-align:center;" ReadOnly="true"></asp:TextBox>
                </td>                
                <td width="5%">
                    <asp:Button ID="b_save" runat="server" Text="ประมวลเปิดบัญชี" />  
                </td>                                    
            </tr>
        </table>   
         <div><u Style="font-size:14px">รายการเปิดบัญชี</u></div>
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
