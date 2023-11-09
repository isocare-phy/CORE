<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.hr.w_hr_apv_worktime_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            
            <th width="23%">
                ชื่อ-นามสกุล
            </th>
            <th width="10%">
                เข้างานประจำวันที่
            </th>
            <th width="6%">
                เข้างานเวลา
            </th>
            <th width="6%">
                ออกงานเวลา
            </th>
            <th width="10%">
                สาย/ไม่สาย
            </th>
            <th width="10%">
               สถานะลงเวลาเข้างาน
            </th>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Height="500px" ScrollBars="Auto" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    
                    <td width="23%">
                        <asp:TextBox ID="full_name" runat="server" Style="text-align: left; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="work_date" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="6%">
                        <asp:TextBox ID="start_time" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="6%">
                        <asp:TextBox ID="end_time" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="late_time" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="apv_worktime" runat="server" Style="text-align: center; background-color: #DDDDDD;"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                   
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
