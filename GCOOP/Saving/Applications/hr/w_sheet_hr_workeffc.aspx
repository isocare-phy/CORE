<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_workeffc.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_workeffc" Title="Untitled Page" ValidateRequest="false"%>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
        }
        .style2
        {
            font-size: small;
            font-weight: 700;
        }
    </style>
   <%=initJavaScript%>
   <%=postGetMember%>
   <%=postAddRow %>
   <%=postDeleteRow %>
   <%=postNewClear%>
   <%=postShowDetail %>
   <%=postSearchGetMember%>
    <script type ="text/javascript" >
     function Validate()
        {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        
        function MenubarOpen() 
        {
            Gcoop.OpenDlg('590','600','w_dlg_hr_master_search.aspx','');
        }
        
        function GetValueEmplid(emplid)
        {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            objDwMain.SetItem(1, "emplid", Gcoop.Trim(emplid));
            objDwMain.AcceptText();
            postGetMember();
        }
        
        function OnDwDetailButtonClick(s,r,c)
        {
            if(c=="b_del")
            {
                var seq_no = "";
                seq_no = objDwDetail.GetItem(1,"seq_no");
                if(seq_no == "" || seq_no == null || seq_no == "Auto")
                {
                    alert("ยังไม่มีข้อมูลไม่สามารถลบรายการได้");
                }
                else
                {
                    if(confirm("คุณต้องการลบรายการ รหัส : "+ seq_no +" ใช่หรือไม่?"))
                    {
                        postDeleteRow();
                    }
                }
            }
            return 0;
        }
        
        function OnButtonClicked(s, row, oName)
        {
            if(oName == "b_search")
            {
                MenubarOpen();
            }
            return 0;
        }
        
         function MenubarNew()
         {
            if(confirm("ยืนยันการล้างข้อมูลบนหน้าจอ"))
            {
                postNewClear();
            }
        }
        
        function DwDetailAddRow()
        {
            var emplid = objDwMain.GetItem(1,"emplid");
            if (emplid == "Auto" || emplid == null)
            {
                alert("ไม่พบรหัสมาชิก กรุณากรอกรหัสสมาชิกก่อน");
            }
            else
            {
                postAddRow(); 
            }
        }
        
        function OnListClick(sender, rowNumber, objectName) 
        {
            var seq_no = objDwList.GetItem(rowNumber, "seq_no");
            if(seq_no != "Auto")
            {
                objDwDetail.SetItem(1, "seq_no", Gcoop.Trim(seq_no));
                objDwDetail.AcceptText();
                postShowDetail();
            }
            return 0;
        }

        
        function OnItemChangeMain(s, r, c, v)
        {
           if(c== "emplcode")
           {
               objDwMain.SetItem(1,"emplcode", Gcoop.StringFormat(v,"00000"));
               objDwMain.AcceptText();
               postSearchGetMember();
           }
           else if(c == "entry_tdate")
           {
                objDwMain.SetItem(1, "entry_tdate", v );
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "entry_date", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
           }
           return 0;
        }
        
        function OnDwDetailItemChange(s,r,c,v) {
            objDwDetail.SetItem(r, c, v);
            objDwDetail.AcceptText();
            if(c == "docudate_tdate")
            {
                objDwDetail.SetItem(1,"docudate_tdate",v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(1,"docudate", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText();
            }
            else if(c == "weappvdate_tdate")
            {
                objDwDetail.SetItem(1,"weappvdate_tdate",v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(1,"weappvdate", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText();    
            }
            return 0;
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width: 100%;">
        <tr>
            <td colspan="2" class="style2">
                ข้อมูลการทำรายการ
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                    DataWindowObject="dw_hr_workeffcdea_head" 
                    LibraryList="~/DataWindow/hr/hr_workeffcdea.pbl" 
                    ClientEventItemChanged="OnItemChangeMain" 
                    ClientEventButtonClicked="OnButtonClicked">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1">
                ข้อมูลความดี / ความผิด
            </td>
            <td class="style1">
                รายละเอียดข้อมูลความดี / ความผิด
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" 
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientFormatting="True" ClientScriptable="True" 
                    DataWindowObject="dw_hr_workeffcdea_list" 
                    LibraryList="~/DataWindow/hr/hr_workeffcdea.pbl" 
                    ClientEventClicked="OnListClick">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="DwDetailAddRow()" style="font-family: Tahoma; font-size: small;
                    float: left; font-weight: 700; color: #003399;">เพิ่มแถว</span></td>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" DataWindowObject="dw_hr_workeffcdea_detail" 
                    LibraryList="~/DataWindow/hr/hr_workeffcdea.pbl" 
                    ClientEventButtonClicked="OnDwDetailButtonClick" 
                    ClientEventItemChanged="OnDwDetailItemChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
