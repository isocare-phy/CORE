<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_acc_vc_report_journal.aspx.cs" Inherits="Saving.Applications.account.w_acc_vc_report_journal" Title="Untitled Page" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=changeValue%>
    <%=postOpenReport%>
    <%=postOpenPDF%>
    <%=onSetDate%>
    <%=initJavaScript%>
    
    <script type="text/javascript">
    
        function OpenProcess(s,r,c){
            var isconfirm = confirm("ยืนยันการจัดทำงบทดลอง ?");
            if (!isconfirm){
            return false;        
            }
            postOpenReport();
        }
        function OpenPDF(){
            var isconfirm = confirm("ยืนยัน PDF ?");
            if (!isconfirm){
            return false;        
            }
            postOpenPDF();
        }
        
        function OnFDate(s,r,c,v){
            var d,m,y,sedate
            sedate = new Array();
            sedate = v.split('/');
            v = sedate[0]+sedate[1]+sedate[2];
                d = v.substring(0,2);
                m = v.substring(2,4);
                y = v.substring(4,8);
                if(c == "start_tdate"){
                    Gcoop.GetEl("HdSDateTH").value = d+"/"+m+"/"+y;
                    y = y - 543;
                    Gcoop.GetEl("HdSDateUS").value = d+"/"+m+"/"+y;
                }
                if(c == "end_tdate"){
                    Gcoop.GetEl("HdEDateTH").value = d+"/"+m+"/"+y;
                    y = y - 543;
                    Gcoop.GetEl("HdEDateUS").value = d+"/"+m+"/"+y;
                }
        }
        function OnDWDateItemChange(s,r,c,v)//เปลี่ยนข้อมูล
        {
            if(c=="start_tdate")
            {
               s.SetItem(1, "start_tdate", v );
               s.AcceptText();
               s.SetItem(1, "start_date", Gcoop.ToEngDate(v));
               s.AcceptText();
            }
            else if(c=="end_tdate")
            {
               s.SetItem(1, "end_tdate", v );
               s.AcceptText();
               s.SetItem(1, "end_date", Gcoop.ToEngDate(v));
               s.AcceptText();
            }
            OnFDate(s,r,c,v);
        }
    </script>
    
    <style type="text/css">
        .style2
        {
            float: right;
            cursor: pointer;
            font-weight: bold;
            color: #000099;
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td valign="top" align="center">
                <dw:WebDataWindowControl ID="dw_main" runat="server" AutoRestoreContext="False" 
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientScriptable="True" DataWindowObject="d_ac_cri_date10_rang" 
                    LibraryList="~/DataWindow/account/acc_report_design.pbl"
                    ClientEventItemChanged="OnDWDateItemChange" ClientFormatting="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td valign="top" align="right">
                <span id="OpenProcess" class="style2" onclick="OpenProcess()">จัดทำใบสรุป Voucher</span> 
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td valign="top" align="right">
                <span id="Span1" class="style2" onclick="OpenPDF()">PDF</span> 
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td valign="top">
                    <dw:WebDataWindowControl ID="dw_rpt" runat="server" Width="740" Height="600" HorizontalScrollBar="Fixed"
                        VerticalScrollBar="Fixed" AutoRestoreContext="False" 
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                        ClientScriptable="True" DataWindowObject="r_day10_journal" 
                        LibraryList="~/DataWindow/account/acc_report_design.pbl" 
                        UseCurrentCulture="True" BorderStyle="Double" BorderColor="#99CCFF">
                    </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdSDateTH" runat="server" />
    <asp:HiddenField ID="HdEDateTH" runat="server" />
    <asp:HiddenField ID="HdSDateUS" runat="server" />
    <asp:HiddenField ID="HdEDateUS" runat="server" />
</asp:Content>
