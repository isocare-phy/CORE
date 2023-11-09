<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsLate.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_late_ctrl.DsLate" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
    <table class ="DataSourceRepeater" align="center" style="width: 700px;">
        <tr>
            <th width="8%">
                ลำดับที่
            </th>
            <th width="20%">
                วันที่
            </th>
            <th width="20%">
                จำนวนชั่วโมงที่สาย
            </th>
          <!--  <th>
                สาเหตุ
            </th> -->
        </tr>
    </table>

<asp:Repeater ID="Repeater1" runat="server">
    <ItemTemplate>
    <table class="DataSourceRepeater" align="center" style="width: 700px;">
            <tr>
                <td width="8%">
                    <div>
                        <asp:TextBox ID="seq_no" runat="server" style="text-align:center;"></asp:TextBox>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:TextBox ID="late_date" runat="server" style="text-align:center;"></asp:TextBox>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:TextBox ID="late_ttime" runat="server"  style="text-align:center;"></asp:TextBox>
                    </div>
                </td>
                <!--<td>
                    <div>
                        <asp:TextBox ID="late_cause" runat="server"></asp:TextBox>
                    </div>
                </td> -->
            </tr>
        </table>
    </ItemTemplate>
</asp:Repeater>