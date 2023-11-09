<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_mem_ucfmembgroup.aspx.cs" Inherits="Saving.Applications.mbshr.ws_mem_ucfmembgroup_ctrl.ws_mem_ucfmembgroup" %>

<%@ Register Src="DsSearch.ascx" TagName="DsSearch" TagPrefix="uc2" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool();
        var dsList = new DataSourceTool();
        var dsSearch = new DataSourceTool();

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDsSearchClicked(s, r, c) {

            if (c == "b_add") {
                var ls_membgroup_code = "";
                Gcoop.OpenIFrame3("630", "450", "wd_mem_adjmembgroup.aspx", "?ls_membgroup_code=" + ls_membgroup_code);
            }
            else if (c == "b_search") {
                GetSearch();
            }
        }
        function RefreshFromDlg() {
            GetSearch();
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table align="center">
        <tr>
            <td>
                <uc2:DsSearch ID="dsSearch" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
               <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="DataSourceRepeater">
                <Columns>
                    <asp:BoundField DataField="membgroup_code" HeaderText="รหัสหน่วย" >
                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="membgroup_desc" HeaderText="ชื่อหน่วย" >
                    <ItemStyle HorizontalAlign="Left" Width="60%"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="membgroup_control" HeaderText="หน่วยคุม" >
                    <ItemStyle HorizontalAlign="Center" Width="10%"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="kpgroup_code" HeaderText="กลุ่มเรียกเก็บ" >
                    <ItemStyle HorizontalAlign="Center" Width="10%"  />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="แก้ไข">
                        <ItemTemplate>
                            <asp:Button
                                CssClass="btn btn-danger"
                                OnClick="btnEditing_Click"
                                Text="Edit"
                                ID="btnEditing"
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="ลบ">
                        <ItemTemplate>
                            <asp:Button
                                CssClass="btn btn-danger"
                                OnClientClick="return confirm('คุณต้องการลบข้อมูลรายการนี้ใช่หรือไม่ ?');"
                                OnClick="btnDelete_Click"
                                Text="Delete"
                                ID="btnDelete"
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdMembgroup" runat="server" />
</asp:Content>