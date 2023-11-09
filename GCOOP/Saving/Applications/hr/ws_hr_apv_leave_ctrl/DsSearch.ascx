<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsSearch.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_apv_leave_ctrl.DsSearch" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<%--  
<script type="text/javascript">
    $(document).ready(function () {
        $('#SEARCH_DATE').blur(function () {
            var date = $(this).val().trim();
            var formattedDate = date.substring(0, 2) + "/" + date.substring(2, 4) + "/" + date.substring(4, 8);
            $(this).val(formattedDate);
        });
    });
</script>
--%>
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="10%">
                    <div>
                        <span>วันที่ :</span>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:TextBox ID="leave_date" runat="server" placeholder="วัน/เดือน/ปี"></asp:TextBox>
                    </div>
                </td>
                <%-- 
                <td width="5%">
                    <div>
                        <span>ถึง</span>
                    </div>
                </td>
                <td>
                </td>
                <td width="20%">
                    <div>
                        <asp:TextBox ID="leave_edate" runat="server" placeholder="วัน/เดือน/ปี"></asp:TextBox>
                    </div>
                </td>--%>
                
                <td>
                    <div>
                        <asp:Button ID="b_search" runat="server" Text="ค้นหา" Style="background-color: #DDDDDD;">
                        </asp:Button>
                    </div>
                </td>
        </table>
    </EditItemTemplate>
</asp:FormView>
