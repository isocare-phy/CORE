<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.assist.ws_as_assistbegin_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />


<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" 
    style="margin-right: 10px" Width="16px" >
    <EditItemTemplate>
         <table class="DataSourceFormView"  style="width:300px;">
          <td width="10%">
                    <div>
                        <span>ปีสวัสดิการ :</span>
                    </div>
                </td>
              <td width="20%">
                    <div>
                        <asp:TextBox ID="assist_year" runat="server" Style="text-align: center" ReadOnly="False"></asp:TextBox>
                    </div>
                </td>        
                <td width="10%">
                    <asp:Button ID="b_select" runat="server" fofont-size="12px" width="100%" Height="100%" Text="ดึงข้อมูล" />
                </td>     
               
        </table>

        

    </EditItemTemplate>


</asp:FormView>
