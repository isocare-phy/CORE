﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.CriteriaIReport.u_cri_nowdate_bycoopid.DsMain" %>
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
                <td colspan="2">
                    <asp:DropDownList ID="coop_id" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
              <tr>
                <td>
                    <div>
                        <span>สาขา:</span>
                    </div>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="coop_id2" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>วันที่:</span></div>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="operate_date" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
