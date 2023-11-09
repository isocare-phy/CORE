<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_divavg_methodpayment.aspx.cs"
    Inherits="Saving.Applications.loantracking.w_sheet_divavg_methodpayment" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postInitMember%>
    <%=postInit%>
    <%=postDeleteRow%>
    <%=postAddRow%>
    <%=postSumAll%>
    <%=postBankbranch%>
    <%=postNewClear%>
    <%=postCBT%>
    <%=postTypeLON%>
    <%=postTypeDEP %>
    <%=postClearType%>
    <%=postClearBranch%>
    <%=postCheckBF%>
    <script type="text/javascript">
     function OnDwDetailClick(s,r,c)
    {
        if (c == "defaultpaytype_flag")
        {
            Gcoop.CheckDw(s, r, c, "defaultpaytype_flag", 1, 0);
            postCheckBF();
        }
        return 0
    }
    
    
    function TypeLON(loancontract_no,loantype_desc)
    {
        objDw_detail.AcceptText();
        Gcoop.GetEl("Hd_loancontract_no").value = loancontract_no;
        Gcoop.GetEl("Hd_loantype_desc").value = loantype_desc;
        postTypeLON();   
    }
    
    function TypeDEP(deptaccount_no,depttype_desc)
    {
        objDw_detail.AcceptText();
        Gcoop.GetEl("Hd_deptaccount_no").value = deptaccount_no;
        Gcoop.GetEl("Hd_depttype_desc").value  = depttype_desc;
        postTypeDEP();
    }
    
    function SetBank(bank_code)
    {
        Gcoop.GetEl("HdBankcode").value = bank_code;
        postBankbranch();
    }
    
    function SetBranch(BranchID)
    {
        Gcoop.GetEl("HdBranchId").value = BranchID;
        postBankbranch();
    }
    
    function DwDetailAddRow()
    {
        postAddRow();
    }
    
    function OnDwDetailButtonClick(s,r,c)
    {
        if(c == "b_del")
        {
            Gcoop.GetEl("HdRowCurrent").value = r + "";
            postDeleteRow();
        }
        else if(c=="b_bank")
        {
            Gcoop.OpenDlg(450,500, "w_dlg_divavg_bank.aspx","");
        }
        else if(c == "b_branch")
        {
            objDw_detail.AcceptText();
            var BankCode = objDw_detail.GetItem(r,"bank_code");
            Gcoop.GetEl("HdRowCurrent").value = r + "";
            if(BankCode == null || BankCode == "")
            {
                alert("กรุณาเลือกรหัสธนาคาร");
            }
            else
            {
                 Gcoop.OpenDlg(500,500, "w_dlg_bankbranch.aspx","?BankCode="+ BankCode);
            }
        }
        return 0;
        
    }
    
    function OnDwmainButtonClick(s,r,b)
    {
        if(b == "b_search")
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
    
     function Validate() 
    {
            objDw_main.AcceptText();
            objDw_detail.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
    }
    
   function OnDwMainItemChange(s,r,c,v)
     {
       if(c == "member_no")
        {
            s.SetItem(1, "member_no", Gcoop.StringFormat(v,"000000"));
            s.AcceptText();
            postInit();
        }
        return 0;
    }
    

    
    function OnDwDetailItemChange(s,r,c,v)
    {
        if(c=="bank_code")
        {
            objDw_detail.SetItem(r,"bank_code",v);
            objDw_detail.AcceptText();
            Gcoop.GetEl("HdBankcode").value = v;
            postClearBranch();
        }
        else if(c=="cut_divamt")
        {
            var dividend_balance = objDw_main.GetItem(1,"dividend_balance");
            
            if(v > dividend_balance)
            {
                alert("ยอดเงินหักปันผลมากกว่าเงินคงเหลือปันผล");
                objDw_detail.SetItem(r,"cut_divamt",0);
                objDw_detail.AcceptText();
            }
            else
            {
                objDw_detail.SetItem(r,"cut_divamt",v);
                objDw_detail.AcceptText();
                Gcoop.GetEl("Hdcut_divamt").value = v;
                Gcoop.GetEl("HdRowCurrent").value = r + "";
                postSumAll();
            }
        }
        else if(c=="cut_avgamt")
        {
            var average_balance = objDw_main.GetItem(1,"average_balance");
            if(v > average_balance)
            {
                alert("ยอดเงินหักเฉลี่ยคืนมากกว่าเงินคงเหลือเฉลี่ยคืน");
                objDw_detail.SetItem(r,"cut_avgamt",0);
                objDw_detail.AcceptText();
            }
            else
            {
                objDw_detail.SetItem(r,"cut_avgamt",v);
                objDw_detail.AcceptText();
                Gcoop.GetEl("Hdcut_avgamt").value = v;
                Gcoop.GetEl("HdRowCurrent").value = r + "";
                postSumAll();
            }
            
        }
        else if(c=="cut_giftamt")
        {
            var gift_balance = objDw_main.GetItem(1,"gift_balance");
            if(v > gift_balance)
            {
                alert("ยอดเงินหักเงินรางวัลมากกว่าเงินคงเหลือเงินรางวัล");
                objDw_detail.SetItem(r,"cut_giftamt",0);
                objDw_detail.AcceptText();
            }
            else
            {
                objDw_detail.SetItem(r,"cut_giftamt",v);
                objDw_detail.AcceptText();
                Gcoop.GetEl("Hdcut_giftamt").value = v;
                Gcoop.GetEl("HdRowCurrent").value = r + "";
                postSumAll();
            }
        }
        else if(c == "divpaytype_code")
        {
            objDw_detail.SetItem(r,"divpaytype_code",v);
            objDw_detail.AcceptText();
            Gcoop.GetEl("Hddivpaytype_code").value = v;
            Gcoop.GetEl("HdRowCurrent").value = r + "";
            //postClearType();
                if(v == "CBT")
                {
                    //Gcoop.OpenDlg(500,500, "w_dlg_cbt.aspx","");
                }
                else if(v == "DEP")
                {
                    var member_no1 = objDw_main.GetItem(1,"member_no");
                    Gcoop.OpenDlg(600,500, "w_dlg_dep.aspx","?member_no="+ member_no1); 
                }
                else if(v == "SHR")
                {
    //                 var member_no = objDw_main.GetItem(1,"member_no");
    //                 Gcoop.OpenDlg(600,500, "w_dlg_shr.aspx","?member_no="+ member_no); 
                }
                else if(v == "LON")
                {
                    var member_no = objDw_main.GetItem(1,"member_no");
                    Gcoop.OpenDlg(600,500, "w_dlg_lon.aspx","?member_no="+ member_no); 
                }
            postCBT();
        }
        else if(c == "description")
        {
            objDw_detail.SetItem(r,"description",v);
            objDw_detail.AcceptText();
            Gcoop.GetEl("Hddescription").value = v;
        }
        else if(c == "bank_accid")
        {
            objDw_detail.SetItem(r,"bank_accid",v);
            objDw_detail.AcceptText();
            Gcoop.GetEl("Hdbank_accid").value = v;
            postBankbranch();
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
            <td>
                รายละเอียดสมาชิก
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Height="132px" 
                    Width="750px">
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                        DataWindowObject="d_divavgsrv_main" LibraryList="~/DataWindow/shrlon/div_avg.pbl"
                        ClientEventButtonClicked="OnDwmainButtonClick" ClientEventItemChanged="OnDwMainItemChange">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                รายละเอียดการทำรายการ
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel2" runat="server" Height="340px" ScrollBars="Auto" 
                    Width="750px">
                    <dw:WebDataWindowControl ID="Dw_detail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        ClientScriptable="True" DataWindowObject="d_divavgsrv_tb_method_payment" LibraryList="~/DataWindow/shrlon/div_avg.pbl"
                        ClientEventButtonClicked="OnDwDetailButtonClick" 
                        ClientEventItemChanged="OnDwDetailItemChange" 
                        ClientEventClicked="OnDwDetailClick">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <span class="linkSpan" onclick="DwDetailAddRow()" style="font-family: Tahoma; font-size: small;
                    float: left; color: #0000CC;">เพิ่มแถว </span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="HdRowCurrent" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="Hdmem_no" runat="server" />
                <asp:HiddenField ID="HdRowBankcode" runat="server" />
                <asp:HiddenField ID="HdBranchId" runat="server" />
                <asp:HiddenField ID="HdBankcode" runat="server" />
                <asp:HiddenField ID="Hdcut_avgamt" runat="server" />
                <asp:HiddenField ID="Hdcut_divamt" runat="server" />
                <asp:HiddenField ID="Hdcut_giftamt" runat="server" />
                <asp:HiddenField ID="Hddivpaytype_code" runat="server" />
                <asp:HiddenField ID="Hddescription" runat="server" />
                <asp:HiddenField ID="Hdbank_accid" runat="server" />
                <asp:HiddenField ID="Hd_loancontract_no" runat="server" />
                 <asp:HiddenField ID="Hd_loantype_desc" runat="server" />
                <asp:HiddenField ID="Hd_deptaccount_no" runat="server" />
                <asp:HiddenField ID="Hd_depttype_desc" runat="server" />
                
            </td>
        </tr>
    </table>
</asp:Content>
