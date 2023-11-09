<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsLeaveout.ascx.cs" 
Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsLeaveout" %>

<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            <th width="5%">
                ลำดับ
            </th>
            <th width="20%">
                ประเภทการลา
            </th>
            <th width="30%">
                ลาตั้งแต่เวลา-เวลา
            </th>
            <th width="10%">
                จำนวนชั่วโมง
            </th>
            <th width="35%">
                เหตุผลการลา
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="300px" HorizontalAlign="Left" ScrollBars="Auto" >
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="leave_desc" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="start_time" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="last_time" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True" ></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="totaltime" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="35%">
                        <asp:TextBox ID="leave_cause" runat="server" Style="text-align: left; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
