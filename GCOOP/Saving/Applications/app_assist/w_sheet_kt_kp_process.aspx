<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_kt_kp_process.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_kp_process" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsKpPorcess%>
    <%=ClearProcess%>
    <%=jsProc_status %>
    <script type="text/javascript">
        function OnDwMainButtonClicked(s, r, c) 
        {
            if (c == "b_accept") {
                jsKpPorcess();
            }
        }

        function SheetLoadComplete() 
        {
            if (Gcoop.GetEl("HdRunProcess").value == "true") 
            {
                Gcoop.OpenProgressBar("ประมวลผล...", true, true, jsClear);
            }
        }

        function jsClear() {
            ClearProcess();
        }

        function OnDwmainItemChanged(s, r, c, v) {
        
            switch(c){
                case "receive_year":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    break;
                case "receive_month":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    break;
                case "proc_status":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    jsProc_status();
                    break;
                case "emp_type":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    break;
                case "group_text":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    break;
                case "mem_text":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    break;
            }
//            if (c == "receive_year") 
//            {
//                s.SetItem(r, c, v);
//                s.AcceptText();

//            }
//            else if (c == "receive_month") 
//            {
//                s.SetItem(r, c, v);
//                s.AcceptText();
//            }
//            else if (c == "proc_status") 
//            {
//                s.SetItem(r, c, v);
//                s.AcceptText();
//            }
//            else if (c == "emp_type") 
//            {
//                s.SetItem(r, c, v);
//                s.AcceptText();
//            }
        }

        function Validate() {
            alert("ไม่สามารถใช้ปุ่ม Save ได้");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="dw_main" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"  LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" 
        DataWindowObject="d_kp_asn_option"
       ClientFormatting="True"
        ClientEventButtonClicked="OnDwMainButtonClicked" Style="top: 0px; left: 0px" ClientEventItemChanged="OnDwmainItemChanged">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRunProcess" runat="server" />
</asp:Content>
