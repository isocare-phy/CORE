<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsCurefamily.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsCurefamily" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:Panel ID="Panel2" runat="server">
    <div align="right">
        <span id="Span4" class="NewRowLink" onclick="PostInsertRowCureFamily()" runat="server">เพิ่มแถว</span>
    </div>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <table class="DataSourceRepeater">
                <tr>
                   <th width="5%">
                        ลำดับที่
                    </th>
                    <th width="15%">
                        ชื่อผู้ป่วย
                    </th>
                    <th width="8%">
                        เกี่ยวข้องเป็น
                    </th>
                    <th width="15%">
                        โรงพยาบาล
                    </th>
                    <th width="10%">
                        ประเภทผู้ป่วย
                    </th>
                    <th width="10%">
                        ใบเสร็จรับเงินเลขที่
                    </th>
                    <th width="15%">
                        วันที่ตรวจรักษา
                    </th>
                    <th width="8%">
                        จำนวนเงินที่จ่ายจริง
                    </th>
                    <th width="8%">
                        จำนวนเงินที่ขอเบิก
                    </th>
                    <!--<th width="5%">
                        แนบหลักฐาน(ฉบับ)
                    </th>-->
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
                        <asp:TextBox ID="running_number" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="assist_name" runat="server"></asp:TextBox>
                    </td>
                    <td width="8%">
                        <asp:DropDownList ID="assist_state" runat="server">
                            <asp:ListItem Value = " "> </asp:ListItem>
                            <asp:ListItem Value = "01">ตนเอง </asp:ListItem>
                            <asp:ListItem Value = "02">บิดา </asp:ListItem>
                            <asp:ListItem Value = "03">มารดา </asp:ListItem>
                            <asp:ListItem Value = "04">คู่สมรส </asp:ListItem>
                            <asp:ListItem Value = "05">บุตร</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="assist_hosname" runat="server"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:DropDownList ID="assist_posit" runat="server">
                            <asp:ListItem Value = " "> </asp:ListItem>
                            <asp:ListItem Value = "ผู้ป่วยนอก">ผู้ป่วยนอก </asp:ListItem>
                            <asp:ListItem Value = "ผู้ป่วยใน">ผู้ป่วยใน </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="assist_amp" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="assist_sdate" runat="server" Style="text-align: center;"></asp:TextBox>
                    </td>
                    <td width="8%">
                        <asp:TextBox ID="assist_amt" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <td width="8%">
                        <asp:TextBox ID="assist_minamt" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    </td>
                    <!-- <td width="5%">
                        <asp:TextBox ID="assist_paper" runat="server"></asp:TextBox>
                    </td> -->
                    <td width="5%">
                        <asp:Button ID="b_del" runat="server" Text="ลบ" Style="background-color: #DDDDDD;" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>