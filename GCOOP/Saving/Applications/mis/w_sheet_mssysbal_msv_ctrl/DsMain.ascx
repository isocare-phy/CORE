<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.mis.w_sheet_mssysbal_msv_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<%--<center>
    <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="Medium">รายงานกำหนดสิทธ์การเข้าใช้งานหน้าจอของระบบต่างๆ </asp:Label>
</center>--%>
<br />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="iReportDataSourceFormView">
            <tr>
                <td>
                    <div>
                        <span>วันที่ : </span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="work_date" runat="server" Style="width: 150px;"></asp:TextBox>
                        <asp:Button ID="btn_search" runat="server" Text="ค้นหา" Height="25" />
                        <asp:Button ID="btn_process" runat="server" Text="ประมวลผล" Height="25" />
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
