<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_sl_contract_adjust.aspx.cs"
    Inherits="Saving.Applications.shrlon.w_sheet_sl_contract_adjust" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=callContractAdjust%>
    <%=getMemberNo%>
    <%=itemChangedReload %>
    <%=postNew%>
    <%=checkRightColl %>
    <%=postMemberFromDlg%>
    <%=getBank%>
    <%=changeColl%>
    <%=jsGetMemberCollno%>
    <%=jsCollCondition%>
    <%=jsCheckCollmastrightBalance%>
    <%=jsCollInitP%>
    <script type="text/javascript">
        function Validate() {
            objdw_main.AcceptText();
            objdw_coll.AcceptText();
            objdw_detpay.AcceptText();
            objdw_detcont.AcceptText();
            objdw_detcontspc.AcceptText();
            var chk = "";
            var rows = objdw_coll.RowCount();
            var amt = Gcoop.ParseInt(objdw_main.GetItem(1, "principal_balance"));
            var amtsum = 0;
            var persum = 0;
            for (var j = 1; j <= rows; j++) {
                var amt_r = 0;
                var per = 0;
                try {
                    amt_r = Gcoop.ParseFloat(objdw_coll.GetItem(j, "use_amt"));
                    per = Gcoop.ParseFloat(objdw_coll.GetItem(j, "coll_percent"));
                } catch (ex) {
                    amt_r = "";
                    per = "";
                }
                amtsum = amtsum + amt_r;
                persum = persum + per;
            }
            if (persum > 100 || amtsum > amt) {
                chk = " >> ตรวจสอบ ยอดค้ำประกันเกินวงเงินค้ำคงเหลือ  เปอร์เซนต์รวม " + persum * 100 + '% , ยอดค้ำรวม ' + amtsum + ', ยอดค้ำคงเหลือ ' + amt;
            }
            return confirm("ยืนยันการบันทึกข้อมูล " + chk);
        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNew();
            }
        }

        function OnButtonClick(s, r, c) {
            var memno = objdw_main.GetItem(1, "member_no");
            if (c == "b_member") {
                Gcoop.GetEl("HfMembDetFlag").value = "0";
                Gcoop.GetEl("Hdbutton").value = "b_member";
                Gcoop.OpenIFrame('600', '600', 'w_dlg_sl_member_search.aspx', '')
            } else if (c == "b_contract") {
                Gcoop.OpenIFrame('630', '600', 'w_dlg_sl_loancontract_search_memno.aspx', "?memno=" + memno);

            } else if (c == "b_recoll") {
                jsCollInitP();
            } else if (c == "b_search") {

                var loancolltype_code = "";
                try {
                    loancolltype_code = objdw_coll.GetItem(r, "loancolltype_code");
                } catch (ex) {
                    loancolltype_code = "";
                }
                //membo ในรายละเอียดค้ำประกัน ตรวจค่า ก่อน  loancolltype_code
                Gcoop.GetEl("HfMembDetFlag").value = "1";
                Gcoop.GetEl("HfMembDetRow").value = r;


                if (loancolltype_code == "01") {
                    //01คนค้ำ ค้นหาทะเบียนหลักทรัพย์
                    var coop_id = Gcoop.GetEl("HdMemcoopId").value;
                    Gcoop.GetEl("HdRowNumber").value = r;
                    Gcoop.OpenDlg('600', '600', 'w_dlg_sl_loanmember_search.aspx', "?coopId=" + coop_id);
                    //Gcoop.OpenIFrame('590', '600', 'w_dlg_sl_member_search.aspx', '');
                } else if (loancolltype_code == "03") {
                    //03 เงินฝากหลักประกัน
                    Gcoop.GetEl("HdRowNumber").value = r;
                    Gcoop.OpenDlg(860, 250, "w_dlg_dp_account_search.aspx", "?member=" + memno);
                    //Gcoop.OpenIFrame('590', '600', 'w_dlg_sl_depmaster_search_coll.aspx', '?memno=' + memno);
                } else if (loancolltype_code == "04") {
                    //04 หลักทรัพย์ค้ำประกัน
                    Gcoop.GetEl("HdRowNumber").value = r;
                    var refCollNo2 = Gcoop.GetEl("HdMemberNo").value;
                    var loantype_code = objdw_main.GetItem(1, "loantype_code");
                    Gcoop.OpenDlg('700', '300', 'w_dlg_sl_collmaster_search_req.aspx', "?member=" + memno + "&loantype_code=" + loantype_code);
                    //Gcoop.OpenIFrame('800', '600', 'w_dlg_sl_collmaster_search.aspx', '?memno=' + memno);
                } else {
                    alert("กรุณาป้อนประเภทรายละเอียดการค้ำประกัน");
                }

            } else if (c == "b_detail") {
                Gcoop.OpenIFrame('590', '600', 'w_dlg_sl_contadjust_coll.aspx', '');
            } else if (c == "b_delete_collateral") {
                if (confirm("คุณต้องการลบรายการ " + r + " ใช่หรือไม่?")) {
                    objdw_coll.DeleteRow(r);
                }
            } else if (c == "b_delete_contintspc") {
                if (confirm("คุณต้องการลบรายการ " + r + " ใช่หรือไม่?")) {
                    objdw_detcontspc.DeleteRow(r);
                }
            }
            else if (c == "b_contadj") {

                var collNo = objdw_coll.GetItem(r, "ref_collno");
                if ((collNo != "") && (collNo != null)) {

                    var refCollNo = objdw_coll.GetItem(r, "ref_collno");
                    var coop_id = objdw_coll.GetItem(r, "coop_id");
                    var collType = objdw_coll.GetItem(r, "loancolltype_code");
                    var coll_amt = objdw_coll.GetItem(r, "coll_amt");
                    var coll_use = objdw_coll.GetItem(r, "coll_useamt");
                    var coll_blance = objdw_coll.GetItem(r, "coll_balance");
                    var base_percent = objdw_coll.GetItem(r, "base_percent");
                    var description = objdw_coll.GetItem(r, "description");
                    var loancolltype_code = objdw_coll.GetItem(r, "loancolltype_code");
                    Gcoop.OpenIFrame('700', '450', 'w_dlg_sl_loanrequest_coll.aspx', "?refCollNo=" + refCollNo + "&coop_id=" + coop_id + "&coll_amt=" + coll_amt + "&coll_use=" + coll_use + "&coll_blance=" + coll_blance + "&collType=" + collType + "&description=" + description + "&loancolltype_code=" + loancolltype_code + "&base_percent= " + base_percent + "&row=" + r + "&loanmemno=" + memno);
                }
                else {
                    alert("ไม่สามารถแสดงรายการได้ กรุณากรอกเลขที่อ้างอิงก่อน");
                }
            }
            return 0;
        }


        //w_dlg_sl_loanmember_search
        function GetValueFromDlgloanMemberSearch(memberno) {
            //ส่งกลับจาก หน้าค้นหาสมาชิก
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;

            objdw_coll.SetItem(rowNumber, "ref_collno", memberno);
            objdw_coll.AcceptText();
            Gcoop.GetEl("HdRefcoll").value = memberno;
            Gcoop.GetEl("HdRefcollrow").value = rowNumber + "";
            jsGetMemberCollno();
        }

        //w_dlg_sl_collmaster_search_req
        function GetValueFromDlgCollmast(collRefNo, collmast_desc, mortgage_price, base_percent) {
            if (collmast_desc == null || collmast_desc == "") {
                collmast_desc = "";
            }
            var desc = collRefNo + ":" + collmast_desc;
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", collRefNo);
            objdw_coll.SetItem(rowNumber, "description", collmast_desc);
            objdw_coll.SetItem(rowNumber, "coll_amt", mortgage_price);
            objdw_coll.SetItem(rowNumber, "coll_balance", mortgage_price * base_percent);
            objdw_coll.SetItem(rowNumber, "use_amt", mortgage_price * base_percent);
            objdw_coll.SetItem(rowNumber, "base_percent", base_percent);
            Gcoop.GetEl("HUseamt").value = mortgage_price;
            Gcoop.GetEl("HdRefcollNO").value = collRefNo;
            objdw_coll.AcceptText();
            jsCheckCollmastrightBalance();

        }

        //w_dlg_dp_account_search
        function NewAccountNo(dept_no, deptaccount_name, prncbal) {
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", dept_no);
            objdw_coll.SetItem(rowNumber, "description", deptaccount_name);
            objdw_coll.SetItem(rowNumber, "coll_amt", prncbal);
            objdw_coll.SetItem(rowNumber, "coll_balance", prncbal);
            objdw_coll.SetItem(rowNumber, "use_amt", prncbal);
            Gcoop.GetEl("HUseamt").value = prncbal;
            objdw_coll.AcceptText();
            jsCheckCollmastrightBalance();
        }

        //รับค่ามาจาก dlg_sl_member_search เลือกใสคนค้ำ
        function GetValueFromDlg(memberno) {
            Gcoop.GetEl("HdRefcoll").value = membernoa;
            var button_name = Gcoop.GetEl("Hdbutton").value;
            if (button_name == "b_member") {
                Gcoop.GetEl("hdmember_no").value = memberno;
                objdw_main.SetItem(1, "member_no", memberno);
                objdw_main.AcceptText();
                postMemberFromDlg();
            }
            else {
                var r = Gcoop.GetEl("HfMembDetRow").value;
                objdw_coll.SetItem(r, "ref_collno", memberno);
                //ตรวจสอบว่าคนค้ำเป็นตัวเองหรือ คนค้ำซ้ำ
                var rows = objdw_coll.RowCount();
                for (var j = 1; j <= rows; j++) {
                    var typecoll = "";
                    var refno = "";
                    try {
                        typecoll = objdw_coll.GetItem(j, "loancolltype_code");
                        refno = objdw_coll.GetItem(j, "ref_collno");
                        if ((Gcoop.ParsetInt(typecoll) == 1) && (memberno == refno)) {
                            alert("มีบัญชีผู้ค้่ำนี้แล้ว");
                        }
                    } catch (ex) {
                        typecoll = "";
                        refno = "";
                    }
                }
                //เรียกเซอรวิสยอดค้ำ
                //checkRightColl();
                jsGetMemberCollno();
            }
            //จำนวนค้ำ + % >> auto
            //1 เลือกสมาชิก ค่า use_amt, coll_percent เซตให้ auto ทุกแถว แชร์เท่าๆกัน
            //            var rows = objdw_coll.RowCount();
            //            var amt_each = amt / rows;
            //            var percent = 100 / rows;
            //            for (var j = 1; j <= rows; j++) {
            //                objdw_coll.SetItem(j, "use_amt", amt_each);
            //                objdw_coll.SetItem(j, "coll_percent", percent);
            //            }

        }

        function GetContFromDlg(contno) {
            objdw_main.SetItem(1, "loancontract_no", contno);
            objdw_coll.AcceptText();
            callContractAdjust();
        }

        //GetValueFromDlgCollmast(collmast_refno) from collmaster_search.aspx [4]
        //        function GetValueFromDlgCollmast(collmast_refno) {
        //            var r = Gcoop.GetEl("HfMembDetRow").value;
        //            Gcoop.GetEl("HdRefcoll").value = collmast_refno;
        //            objdw_coll.SetItem(r, "ref_collno", collmast_refno);
        //            objdw_coll.AcceptText();
        //            // callContractAdjust();
        //            //changeColl();
        //            jsCheckCollmastrightBalance();
        //        }

        //[3] GetContFromDepmasterCollDlg(collmast_refno); from w_dlg_sl_depmaster_search_coll.aspx
        function GetContFromDepmasterCollDlg(collmast_refno, deptaccount_name, prncbal) {
            var r = Gcoop.GetEl("HfMembDetRow").value;
            Gcoop.GetEl("HdRefcoll").value = collmast_refno;
            objdw_coll.SetItem(r, "ref_collno", collmast_refno);
            objdw_coll.AcceptText();
            //callContractAdjust();
            //changeColl();
            jsGetMemberCollno();
        }
        function DwconOnItemChanged(s, r, c, v) {

            if (c == "loanpay_bank" || c == "loanpay_code") {
                objdw_con.SetItem(1, c, v);
                objdw_con.AcceptText();
                getBank();
            }
        }
        function OnItemChanged(s, r, c, v) {

            if (c == "member_no") {
                objdw_main.SetItem(1, c, v);
                objdw_main.AcceptText();
                getMemberNo();
            } else if (c == "loancontract_no") {
                objdw_main.SetItem(1, c, v);
                objdw_main.AcceptText();
                callContractAdjust();
            } else if (c == "contadjust_tdate") {
                objdw_main.SetItem(1, "contadjust_tdate", v);
                objdw_main.AcceptText();
                objdw_main.SetItem(1, "contadjust_date", Gcoop.ToEngDate(v));
                objdw_main.AcceptText();
            } else if (c == "int_continttype") {
                objdw_detcont.SetItem(r, c, v);
                objdw_detcont.AcceptText();
                itemChangedReload();
            } else if (c == "int_continttabcode" || c == "int_intsteptype") {
                //contint
                objdw_detcont.SetItem(r, c, v);
                objdw_detcont.AcceptText();
            } else if (c == "loanpayment_type" || c == "payment_status") {
                //dw_detpay
                objdw_detpay.SetItem(r, c, v);
                objdw_detpay.AcceptText();
            }
        }
        function OnInsert(dwname) {
            var dwname = Gcoop.ParseInt(dwname);
            if (dwname == 1) {
                objdw_coll.SetItem(objdw_coll.RowCount() + 1, "loancolltype_code", 1);
                Gcoop.GetEl("HdRefcollrow").value = "" + (objdw_coll.RowCount() + 1);
                objdw_coll.InsertRow(objdw_coll.RowCount() + 1);
            } else if (dwname == 2) {
                objdw_detcontspc.InsertRow(objdw_detcontspc.RowCount() + 1);
            }
        }

        function OnItemChagedDw_Coll(sender, rowNumber, columnName, newValue) {
            Gcoop.GetEl("HdRowNumber").value = rowNumber;

            if (columnName == "ref_collno") {

                objdw_coll.SetItem(rowNumber, columnName, newValue);
                Gcoop.GetEl("HdRefcoll").value = newValue;
                Gcoop.GetEl("HdRefcollrow").value = rowNumber;

                objdw_coll.AcceptText();
                var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                if (Gcoop.GetEl("HdRefcoll").value == Gcoop.GetEl("HdMemberNo").value && loancolltype_code == "01") {
                    //Gcoop.GetEl("HdRefcoll").value = "";
                    alert("เลขทะเบียนผู้กู้และผู้ค้ำประกันเป็นเลขเดียวกัน");
                }
                else {
                    if (loancolltype_code == "01") {

                        jsGetMemberCollno();
                    }
                    else if (loancolltype_code == "03") {
                        alert(newValue);
                        if ((newValue != null) && (newValue != "")) {
                            Gcoop.OpenDlg(620, 250, "w_dlg_show_accid_dept.aspx", "?dept=" + newValue);
                        }
                    }
                }

            } else if (columnName == "loancolltype_code") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                var loancolltype_code = objdw_coll.GetItem(rowNumber, "loancolltype_code");
                Gcoop.GetEl("HdRefcollrow").value = rowNumber + "";
                if (loancolltype_code == "02") {
                    Gcoop.GetEl("HdRefcoll").value = objdw_main.GetItem(1, "member_no");
                    objdw_coll.SetItem(rowNumber, "ref_collno", objdw_main.GetItem(1, "member_no"));
                    jsGetMemberCollno();
                }

            }
            else if (columnName == "use_amt") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                JsCollInitP();
            }
            else if (columnName == "coll_percent") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
                JsCollInitP();
            }
            else if (columnName == "calcollright_amt") {
                objdw_coll.SetItem(rowNumber, columnName, newValue);
                objdw_coll.AcceptText();
            }
        }

        function GetAccIDDept(dept_no, deptaccount_name, prncbal) {
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", dept_no);
            objdw_coll.SetItem(rowNumber, "description", deptaccount_name);
            objdw_coll.SetItem(rowNumber, "coll_amt", prncbal);
            objdw_coll.SetItem(rowNumber, "coll_balance", prncbal);
            objdw_coll.SetItem(rowNumber, "use_amt", prncbal);
            Gcoop.GetEl("HUseamt").value = prncbal;
            objdw_coll.AcceptText();
            //jsPostreturn();
            jsCollCondition();

        }

        function OnItemChangedIntspc(s, r, c, v) {
            if (c == "int_continttype") {
                objdw_detcontspc.SetItem(r, c, v);
                objdw_detcontspc.AcceptText();
                itemChangedReload();
            } else if (c == "int_timetype" || c == "int_continttabcode") {
                //contintspc
                objdw_detcontspc.SetItem(r, c, v);
                objdw_detcontspc.AcceptText();
            } else if (c == "effective_tdate") {
                objdw_detcontspc.SetItem(r, "effective_tdate", v);
                objdw_detcontspc.AcceptText();
                objdw_detcontspc.SetItem(r, "effective_date", Gcoop.ToEngDate(v));
                objdw_detcontspc.AcceptText();
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtServerMessagecoll" runat="server"></asp:Literal>
    <asp:HiddenField ID="HfLncontno" runat="server" />
    <asp:HiddenField ID="Href_collno" runat="server" />
    <asp:HiddenField ID="HfMembDetFlag" runat="server" />
    <asp:HiddenField ID="HfMembDetRow" runat="server" />
    <asp:HiddenField ID="hdmember_no" runat="server" />
    <asp:HiddenField ID="Hdbutton" runat="server" />
    <asp:HiddenField ID="HdRowNumber" runat="server" />
    <asp:HiddenField ID="HUseamt" runat="server" Value="false" />
    <asp:HiddenField ID="HdCollmaxval1" runat="server" />
    <asp:HiddenField ID="HdCollmaxval2" runat="server" />
    <asp:HiddenField ID="Hdmangrtpermgrp_code" runat="server" Value="true" />
    <asp:HiddenField ID="HdRefcollrow" runat="server" />
    <asp:HiddenField ID="HdRefcollNO" runat="server" />
    <asp:HiddenField ID="HdRefcoll" runat="server" />
    <asp:HiddenField ID="HdMemcoopId" runat="server" />
    <asp:HiddenField ID="HdMemberNo" runat="server" />
    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_loansrv_req_contadj"
        LibraryList="~/DataWindow/shrlon/sl_contract_adjust.pbl" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientEventButtonClicked="OnButtonClick" ClientEventItemChanged="OnItemChanged"
        TabIndex="1">
    </dw:WebDataWindowControl>
    <br />
    <table style="width: 500px">
        <tr>
            <td valign="top" width="20%">
                <div>
                    <asp:Label ID="Label1" runat="server" Text="การเรียกเก็บ" Font-Bold="True" Font-Size="10pt"></asp:Label>
                    <dw:WebDataWindowControl ID="dw_detpay" runat="server" DataWindowObject="d_loansrv_req_contadj_payment"
                        LibraryList="~/DataWindow/shrlon/sl_contract_adjust.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="OnItemChanged" TabIndex="150" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
            </td>
            <td valign="top">
                <div>
                    <asp:Label ID="Label2" runat="server" Text="หลักประกันเงินกู้" Font-Bold="True" Font-Size="10pt"></asp:Label>
                    <dw:WebDataWindowControl ID="dw_coll" runat="server" DataWindowObject="d_loansrv_req_contadj_collateral"
                        LibraryList="~/DataWindow/shrlon/sl_contract_adjust.pbl;~/DataWindow/shrlon/cmcomddw.pbl"
                        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                        ClientScriptable="True" ClientEventItemChanged="OnItemChagedDw_Coll" ClientEventButtonClicked="OnButtonClick"
                        TabIndex="200" ClientFormatting="True" Style="top: 0px; left: 0px">
                    </dw:WebDataWindowControl>
                    <span class="linkSpan" onclick="OnInsert(1)" style="font-size: small; color: #808080;
                        float: left">เพิ่มแถว</span>
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <div>
                    <asp:Label ID="Label5" runat="server" Text="บัญชีเรียกเก็บ" Font-Bold="True" Font-Size="10pt"></asp:Label>
                    <dw:WebDataWindowControl ID="dw_con" runat="server" DataWindowObject="d_loansrv_req_contadj_detkeptype_new"
                        LibraryList="~/DataWindow/shrlon/sl_contract_adjust.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="DwconOnItemChanged" TabIndex="150" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td valign="top" width="20%">
                <asp:Label ID="Label3" runat="server" Text="อัตรา ด/บ ของสัญญา" Font-Bold="True"
                    Font-Size="10pt"></asp:Label>
                <dw:WebDataWindowControl ID="dw_detcont" runat="server" DataWindowObject="d_loansrv_req_contadj_contint"
                    LibraryList="~/DataWindow/shrlon/sl_contract_adjust.pbl;~/DataWindow/shrlon/cmcomddw.pbl"
                    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientScriptable="True" ClientEventItemChanged="OnItemChanged" TabIndex="500"
                    ClientFormatting="True" Style="top: 0px; left: 0px">
                </dw:WebDataWindowControl>
            </td>
            <td valign="top">
                <asp:Label ID="Label4" runat="server" Text="อัตราดอกเบี้ยพิเศษเป็นช่วง" Font-Bold="True"
                    Font-Size="10pt"></asp:Label>
                <dw:WebDataWindowControl ID="dw_detcontspc" runat="server" DataWindowObject="d_loansrv_req_contadj_contintspc"
                    LibraryList="~/DataWindow/shrlon/sl_contract_adjust.pbl;~/DataWindow/shrlon/cmcomddw.pbl"
                    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientScriptable="True" ClientEventItemChanged="OnItemChangedIntspc" ClientEventButtonClicked="OnButtonClick"
                    TabIndex="550" ClientFormatting="True" Style="top: 0px; left: 0px">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="OnInsert(2)" style="font-size: small; color: #808080;
                    float: left">เพิ่มแถว</span>
            </td>
        </tr>
    </table>
</asp:Content>
