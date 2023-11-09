<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_dlg_sl_shrgift_wizard.aspx.cs" Inherits="Saving.Applications.shrlon.w_dlg_sl_shrgift_wizard" Title="Untitled Page" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsProcess %>
    <%=typeProcStatus%>
    <style type="text/css">
        .style3
        {
            width: 198px;
        }
        .style4
        {
            width: 47px;
        }
    </style>

    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function Click_process(s,r,c){
            if(c=="b_process"){      
             Gcoop.OpenDlg("250","180", "w_dlg_RunShgiftProcess_progressbar.aspx","");
             objdw_criteria.AcceptText();
             jsProcess();
             
            }
        
        }
        function ItemChanged(s, r, c, v){
            if(c == "rangedata_type"){
                objdw_criteria.SetItem(r,c,v);
                objdw_criteria.AcceptText();
                typeProcStatus(); 
            }    
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width:100%;">

        <tr>
            <td colspan="3">
                <dw:WebDataWindowControl ID="dw_criteria" runat="server" DataWindowObject="d_slproc_shrgift_criteria"
                    LibraryList="~/DataWindow/shrlon/sl_shlnproc.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientFormatting="True" ClientEventButtonClicked="Click_process" ClientEventItemChanged="ItemChanged">
                </dw:WebDataWindowControl>
            </td>
        </tr>

    </table>
</asp:Content>
