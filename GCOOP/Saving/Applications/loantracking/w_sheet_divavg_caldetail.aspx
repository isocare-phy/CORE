<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_divavg_caldetail.aspx.cs" Inherits="Saving.Applications.loantracking.w_sheet_divavg_caldetail" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
        }
    </style>
    <%=initJavaScript%>
    <%=postInit%>
    <%=postNewClear %>
    <%=postCalDivDetail%>
    <%=postInitMember%>
    <script type ="text/javascript">
    function OnDwmainButtonClick(s,r,b)
    {
        if(b == "b_procdivavg")
        {
            var member_no = objDw_main.GetItem(1,"member_no");
            if(member_no == "" || member_no == null)
            {
                alert("กรุณากรอกรหัสสมาชิก");
            }
            else
            {
                postCalDivDetail();                
            }
        }
        else if(b == "b_search")
        {
             Gcoop.OpenDlg(670,600, "w_dlg_divavg_memb.aspx","");
        }
        return 0;
    }
    
    function SearchMember(member_no)
   {
        Gcoop.GetEl("Hdmem_no").value = member_no; 
        postInitMember();
   }
   
    function MenubarNew()
    {
       if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) 
       {
           postNewClear();
       }
    }
    
    function OnDwMainItemChange(s,r,c,v)
     {
        if(c == "member_no")
        {
            s.SetItem(1, "member_no", Gcoop.StringFormat(v,"00000000"));
            s.AcceptText();
            postInit();
        }
        else if(c == "startacc_tdate")
        {
            s.SetItem(1, "startacc_tdate", v );
            s.AcceptText();
            s.SetItem(1, "startacc_date", Gcoop.ToEngDate(v));
            s.AcceptText();
        }
        else if(c== "endacc_tdate")
        {
            s.SetItem(1, "endacc_tdate", v );
            s.AcceptText();
            s.SetItem(1, "endacc_date", Gcoop.ToEngDate(v));
            s.AcceptText();  
        }
        return 0;
    }
    
    function Validate() 
    {
            objDw_main.AcceptText();
            objDw_detail.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
    }
    </script> 
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width:100%;">
        <tr>
            <td class="style1">
                เงื่อนไขการคำนวณเงินปันผลเฉลี่ยคืน</td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge">
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" 
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                        ClientFormatting="True" ClientScriptable="True" 
                        DataWindowObject="d_divavgsrv_detaildivavg_main" 
                        LibraryList="~/DataWindow/shrlon/div_avg.pbl" 
                        ClientEventButtonClicked="OnDwmainButtonClick" 
                        ClientEventItemChanged="OnDwMainItemChange">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="style1">
                รายละเอียดการคำนวณปันผล</td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Height="300px" 
                    ScrollBars="Vertical">
                    <asp:Panel ID="Panel3" runat="server">
                        <dw:WebDataWindowControl ID="Dw_detail" runat="server" 
                            DataWindowObject="d_sl_10_caldiv_info_shrstm_day" 
                            LibraryList="~/DataWindow/shrlon/div_avg.pbl" AutoRestoreContext="False" 
                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                            ClientFormatting="True" ClientScriptable="True" ClientValidation="False">
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="Hdmem_no" runat="server" />
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
