<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsLog.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_workout_ctrl.DsLog" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
          <!--  <th width="5%">
                ลำดับ
            </th>-->
            <th width="28%">
                รายละเอียด
            </th>
            <th width="10%">
                วันที่ร้องขอโอที
            </th>
            <th width="10%">
                ตั้งเเต่เวลา
            </th>
           <th width="10%">
                ถึงเวลา
            </th>
            <th width="5%">
                ชม.
            </th>
              <th width="7%">
                สถานะ
            </th>
              <th width="5%">
                
            </th>
            <!--<th width="10%">
               
            </th>-->
            
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="500px" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                        <asp:HiddenField ID="seq_no"></asp:HiddenField>

                    <td width="28%">
                        <asp:TextBox ID="ottype" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="date_work" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="work_in" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="work_out" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:TextBox ID="ot_p" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="7%">
                        <asp:TextBox ID="apv_ot_status" runat="server" Style="text-align: center; background-color: #DDDDDD;" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_detail" runat="server" Text="แก้ไข" Style="background-color: #DDDDDD;"></asp:Button>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>