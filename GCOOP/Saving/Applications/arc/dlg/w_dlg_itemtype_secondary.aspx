<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_itemtype_secondary.aspx.cs" Inherits="Saving.Applications.arc.dlg.w_dlg_itemtype_secondary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<html>
    <head id="Head1" runat="server">
    <%=postSaveData%>
     <%=postAddrow%>
    <title>ข้อมูลหมวดหน่วยงานย่อย</title>
    <script type="text/javascript">
        function OnButtonCloseClick() {
            //alert("Close");
            //postCloseIF();
            parent.RemoveIFrame();
        }

        function OnButtonSaveClick() {
            var isConfirm = confirm("ยืนยันการบันทึกข้อมูล");
            if (isConfirm) {
                postSaveData();
            }
        }

        function OnInsert() 
        {
            postAddrow();
        }
    </script>
   
    </head>
   <body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
     <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Width="465px" Height="540px">
      <div align="center">   
      <div align="center">
           <dw:WebDataWindowControl 
                ID="DwSecond" runat="server" DataWindowObject="dw_hr_itemtype_secondary"
                LibraryList="~/DataWindow/arc/hr_constant.pbl" ClientScriptable="True" RowsPerPage="50"
                AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
                ClientFormatting="True">
            </dw:WebDataWindowControl>
       </div>
            <span  onclick="OnInsert()" style="font-size: small; color: #808080; cursor: pointer; ">
                เพิ่มแถว</span> 
            <input id="buttonSaveData" style="width: 80px;" type="button" value="บันทึก" onclick="OnButtonSaveClick()" />
                &nbsp;
            <input id="buttonCloseIFrame" type="button" value="ปิดหน้าจอ" onclick="OnButtonCloseClick()"
                style="width: 80px;" />
       </div>
       </asp:Panel>
        <asp:HiddenField ID="HdItemtype_id" runat="server" />
    </form>
</body>
</html>