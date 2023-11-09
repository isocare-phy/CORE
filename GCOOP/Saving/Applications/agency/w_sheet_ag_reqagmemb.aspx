<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_reqagmemb.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_reqagmemb" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-family: Tahoma;
            font-size: small;
            font-weight: bold;
        }
        .style2
        {
            font-family: Tahoma;
            font-size: small;
            font-weight: bold;
            width: 548px;
        }
        .style3
        {
            font-family: Tahoma;
            font-size: small;
            font-weight: bold;
            width: 548px;
            height: 220px;
            text-align: center;
        }
    </style>
    <%=initJavaScript%>
    <%=postChgReqAgMemb%> 
    <%=postNewClear%>
    <%=postInitReqAgMemb%>
    <%=postChgReqAgMemb%>
    <%=postSearchChgReqAgMemb%>
    <%=postResign%>
    <%=postReturn %>
    <%=postSetAddress %>


    <script type="text/javascript">
    function SetAddress(memb_addr, mooban, soi, road, tambol, district_code, province_code, postcode)
    {
        objDw_main.SetItem(1, "memb_addr", memb_addr);
        objDw_main.SetItem(1, "mooban", mooban);
        objDw_main.SetItem(1, "soi", soi);
        objDw_main.SetItem(1, "road", road);
        objDw_main.SetItem(1, "tambol", tambol);
        objDw_main.SetItem(1, "district_code", district_code);
        objDw_main.SetItem(1, "province_code", province_code);
        objDw_main.SetItem(1, "postcode", postcode);
        objDw_main.AcceptText();
        postSetAddress();
    }
    function OnDwMainButtonClick(s, r, c) 
    {
        if (c == "b_search") {
            Gcoop.OpenDlg('590', '590', 'w_dlg_member_search.aspx', '');
        }
        else if (c == "b_resign") {
            var member_no = objDw_main.GetItem(1, "member_no");
            var isConfirm = confirm("ยืนยันการลาออกจากผู้ทำการแทนของ เลขประจำตัว : " + member_no + "ใช่หรือไม่ ?");
            if (isConfirm) {
                postResign();
            }
        }
        else if (c == "b_ccresign") {
            var member_no = objDw_main.GetItem(1, "member_no");
            var isConfirm = confirm("ยืนยันการยกเลิกลาออกจากผู้ทำการแทนของเลขประจำตัว : " + member_no + "ใช่หรือไม่ ?");
            if (isConfirm) {
                postReturn();
            }
        }
        else if (c == "b_address") {
            var member_no = null;
            var agentrequest_no = null;
            member_no = objDw_main.GetItem(1, "member_no");
            if (member_no != null) {
                var memb_addr = objDw_main.GetItem(1, "memb_addr");
                var mooban = objDw_main.GetItem(1, "mooban");
                var soi = objDw_main.GetItem(1, "soi");
                var road = objDw_main.GetItem(1, "road");
                var tambol = objDw_main.GetItem(1, "tambol");
                var district_code = objDw_main.GetItem(1, "district_code");
                var province_code = objDw_main.GetItem(1, "province_code");
                var postcode = objDw_main.GetItem(1, "postcode");
                Gcoop.OpenIFrame('350', '300', 'w_dlg_ag_address.aspx', "?memb_addr=" + memb_addr + "&mooban=" + mooban + "&soi=" + soi + "&road=" + road + "&tambol=" + tambol + "&district_code=" + district_code + "&province_code=" + province_code + "&postcode=" + postcode);
            }
        }
        return 0;
    }
    
    function OnDwmainItemChange(s, r, c, v) {
        if (c == "member_no") {
            objDw_main.SetItem(1, "member_no", Gcoop.StringFormat(v, "00000000"));
            objDw_main.AcceptText();
            postChgReqAgMemb();
        }
        return 0;
    }
    function GetMemberFromAgent(agentrequest_no) {
        objDw_main.SetItem(1, "agentrequest_no", agentrequest_no);
        objDw_main.AcceptText();
        postInitReqAgMemb();
    }

    function GetMemDetFromDlg(memberno) {
        objDw_main.SetItem(1, "member_no", memberno);
        objDw_main.AcceptText();
        postSearchChgReqAgMemb();
    }
    
    function MenubarNew() {
        if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
            postNewClear();
        }
    }

    function MenubarOpen() {
        Gcoop.OpenDlg('590', '590', 'w_dlg_ag_searchagmemb.aspx', '');
    }

    function Validate() {
        objDw_main.AcceptText();
        return confirm("ยืนยันการบันทึกข้อมูล");
    }
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td class="style1" colspan="2">
                รายละเอียดตัวแทน
            </td>
        </tr>
        <tr>
            <td class="style2" valign="top" rowspan="2">
                <asp:Panel ID="Panel1" runat="server" Width="450px">
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" 
    AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" 
    ClientScriptable="True"
                    DataWindowObject="d_agentsrv_reqmemb" LibraryList="~/DataWindow/agency/egat_ag_regagmemb.pbl"
                    ClientEventButtonClicked="OnDwMainButtonClick" 
    ClientEventItemChanged="OnDwmainItemChange" Width="450px" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
            <td class="style3" valign="top">
                <asp:Image ID="Img_picture" runat="server" Height="100px" Width="100px" 
                    ImageUrl="~/Applications/agency/image/picture/icon_guest.jpg" />
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <br />
                รูปภาพ<br />
                <br />
                <br />
                <br />
                <asp:Image ID="Img_signature" runat="server" Height="100px" Width="100px" 
                    ImageUrl="~/Applications/agency/image/signature/icon_guest.jpg" />
                <br />
                <asp:FileUpload ID="FileUpload2" runat="server" />
                <br />
                รูปลายเซ็นต์</td>
        </tr>
        <tr>
            <td class="style2" valign="top">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="Hd_resign" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
