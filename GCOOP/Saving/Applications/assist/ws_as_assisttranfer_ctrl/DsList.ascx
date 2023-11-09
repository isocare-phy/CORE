<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.assist.ws_as_assisttranfer_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" 
    style="margin-right: 10px" Width="100%" >
    <EditItemTemplate>
         <table class="DataSourceFormView"  style="width:100%;">
                   <tr>
          <td width="18%">
                    <div>
                        <span>ปีสวัสดิการ :</span>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <asp:TextBox ID="ASSIST_YEAR" runat="server" Style="text-align: center" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                    </div>
                </td>         
          </tr>
         <tr>
          <td width="16%">
                    <div>
                        <span>สวัสดิการที่รับโอน :</span>
                    </div>
                </td>
              <td width="25%">
                    <div>
                        <asp:DropDownList ID="ASSISTTYPE_TO" runat="server" Style="text-align: left"></asp:DropDownList>
                    </div>
                </td>
                <td width="16%">
                    <div>
                        <span>สวัสดิการที่โอนย้าย :</span>
                    </div>
                </td>
              <td width="25%">
                    <div>
                        <asp:DropDownList ID="ASSISTTYPE_FROM" runat="server" Style="text-align: left"></asp:DropDownList>
                    </div>
                </td>       
          
          </tr>     
          <tr>
          <td width="16%">
                    <div>
                        <span>ยอดที่ต้องการย้าย :</span>
                    </div>
                </td>
              <td width="25%">
                    <div>
                        <asp:TextBox ID="TRANSFER_AMT" runat="server" Style="text-align: center" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
          </tr>  
          <tr>
                 <td width="16%">
                    <div>
                        <span>วันที่อนุมัติ :</span>
                    </div>
                </td>
              <td width="20%">
                    <div>
                        <asp:TextBox ID="ENTRY_DATE" runat="server" Style="text-align: center"></asp:TextBox>
                    </div>
                </td>              
          </tr>  
          <tr>
                 <td width="16%">
                    <div>
                        <span>หมายเหตุ :</span>
                    </div>
                </td>
              <td width="70%" colspan="5">
                    <div>
                        <asp:TextBox ID="REMARK" runat="server" Style="text-align: left"></asp:TextBox>
                    </div>
                </td>              
          </tr>  
        </table>

        

    </EditItemTemplate>


</asp:FormView>

