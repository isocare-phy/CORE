<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsAssisttype.ascx.cs" Inherits="Saving.Applications.assist.ws_sheet_as_ucfassist_ctrl.DsAssisttype" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">            
            <tr>               
                 <td width="110px">
                    <span>ประเภทสวัสดิการ:</span>
                </td>
                <td>
                    <asp:DropDownList ID="assisttype_code" runat="server" ></asp:DropDownList>
                </td>                                                
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>