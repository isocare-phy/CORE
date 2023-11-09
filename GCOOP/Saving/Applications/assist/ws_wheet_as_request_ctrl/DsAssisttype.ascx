<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsAssisttype.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_ctrl.DsAssisttype" %>
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
                <td width="15%">
                    <span>ปีสวัสดิการ:</span>
                </td>
                <td width="15%" style="text-align:center">
                    <asp:DropDownList ID="ass_year" runat="server" ></asp:DropDownList>
                </td>                                                
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>