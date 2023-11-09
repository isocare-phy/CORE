<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_hr_upload_emp_pic.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_upload_emp_pic_ctrl.ws_hr_upload_emp_pic" %>

<%@ Register src="DsMain.ascx" tagname="DsMain" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var dsMain = new DataSourceTool();
    
    function MenubarOpen() {
        Gcoop.OpenIFrame2('685', '460', 'ws_dlg_hr_master_search.aspx', '');
    }
    function GetEmpNoFromDlg(emp_no) {
        dsMain.SetItem(0, "emp_no", emp_no);
        PostEmpNo();
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
 <asp:Literal ID="LtServerMessege" runat="server"></asp:Literal>
    
    <uc1:DsMain ID="dsMain" runat="server" />
    
    <table class="DataSourceFormView">
        <tr>
            <td width="20%">
                <div>
                    <span>รูปเจ้าหน้าที่ :</span>
                </div>
            </td>
            <td>
                <div>
                    <asp:FileUpload ID="UploadProfile" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <div>
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="Upload" style="width:130px"/>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
