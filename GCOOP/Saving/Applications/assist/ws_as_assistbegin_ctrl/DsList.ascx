<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.assist.ws_as_assistbegin_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
    <script language="JavaScript">
        function chkNumber(ele) {
            var vchar = String.fromCharCode(event.keyCode);
            if ((vchar < '0' || vchar > '9') && (vchar != '.')) return false;
            ele.onKeyPress = vchar;
        }
</script>
<asp:Panel ID="Panel"   runat="server" >
   
    <table class="DataSourceRepeater" width="100%">
        <tr>
            <th width="5%">
                รหัส
            </th>
            <th width="30%">
                ประเภทสวัสดิการ
            </th>
             <th width="15%">
                ยอดยกมาต้นปี
            </th>

        </tr>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <asp:TextBox ID="ASSISTTYPE_CODE" runat="server" Style="text-align: center; "  ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="ASSISTTYPE_DESC" runat="server" Style="text-align: left;" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="BEGIN_AMOUNT" runat="server" Style="text-align: right;"  ToolTip="#,##0.00" onfocus="if(this.value=='0.00')this.value='';"  onblur="if(this.value=='')this.value='0.00'" OnKeyPress="return chkNumber(this)" MaxLength="15"></asp:TextBox>
                    </td>

                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
