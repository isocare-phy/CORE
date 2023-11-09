<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.CriteriaIReport.u_cri_coopid_emperiod.DsMain" %>
<link id="css1" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="iReportDataSourceFormView">
            <tr>
                <td>
                    <div>
                        <span>สาขา:</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="coop_id" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ปี:</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="period" runat="server">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="20%">
                    <div>
                        <span>เลขพนักงาน:</span></div>
                </td>
                <td width="60%">
                    <asp:DropDownList ID="semp_name" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ถึงเลขพนักงาน:</span></div>
                </td>
                <td>
                    <asp:DropDownList ID="eemp_name" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
