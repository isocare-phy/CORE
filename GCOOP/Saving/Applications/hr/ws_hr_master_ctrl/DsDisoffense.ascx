<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDisoffense.ascx.cs"
    Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsDisoffense" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel3" runat="server">
    <div align="right">
        <span id="Span4" class="NewRowLink" onclick="PostInsertRowDisoffense()" runat="server">เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel4" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
        <tr>
            <th width="5%">
                ครั้งที่
            </th>
            <th width="20%">
                ประเภทกาทำผิด
            </th>
            <th width="15%">
                เลขที่หนังสือ
            </th>
            <th width="20%">
                ลงวันที่
            </th>
            <th width="20%">
                การลงโทษ
            </th>
            <th width="15%">
                หมายเหตุ
            </th>
            <th width="5%">
            </th>
        </tr>
    </table>
</asp:Panel>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <table class="DataSourceRepeater">
                 <tr>
                    <td width="5%">
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center; "
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:DropDownList ID="disoffense_code" runat="server"> 
                        </asp:DropDownList>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="disof_docno" runat="server" ></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="disof_date" runat="server" ></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="disof_inflic" runat="server" ></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="disof_remark" runat="server" ></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" Style="background-color: #DDDDDD;" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>