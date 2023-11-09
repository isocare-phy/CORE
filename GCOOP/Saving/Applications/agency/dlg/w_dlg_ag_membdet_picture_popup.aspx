<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_ag_membdet_picture_popup.aspx.cs"
    Inherits="Saving.Applications.agency.dlg.w_dlg_ag_membdet_picture_popup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            text-align: center;
            font-size: small;
            font-weight: bold;
            height: 28px;
        }
        .style2
        {
            text-align: left;
            font-size: small;
            width: 303px;
        }
        .style3
        {
            color: #CC0000;
            font-weight: bold;
            font-size: small;
            font-family: Tahoma;
        }
        .style4
        {
            color: #333399;
            font-weight: bold;
            font-family: Tahoma;
            font-size: small;
        }
        .style5
        {
            text-align: left;
            font-size: small;
            font-weight: bold;
        }
        #B_close
        {
            width: 57px;
            height: 21px;
        }
        .style6
        {
            height: 280px;
        }
        .style7
        {
            text-align: center;
            font-size: small;
            font-weight: bold;
            height: 28px;
            width: 303px;
        }
        .style8
        {
            height: 280px;
            width: 303px;
        }
    </style>
</head>

<script type="text/javascript">
    function OnCloseDialog() {
        parent.RemoveIFrame();
    }
</script>

<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <table style="width: 80%; height: 486px;">
            <tr>
                <td class="style7">
                    รูปภาพ
                </td>
                <td class="style1">
                    รูปลายเซ็นต์
                </td>
            </tr>
            <tr>
                <td style="border-style: ridge; border-color: #008000; text-align: center" 
                    valign="top" class="style8">
                    <asp:Image ID="Img_picture" runat="server" Height="300px" Width="300px" 
                        ImageUrl="~/Applications/agency/image/picture/icon_guest.jpg" />
                </td>
                <td style="border-style: ridge; border-color: #008000; text-align: center"  
                    valign="top" class="style6">
                    <asp:Image ID="Img_signature" runat="server" Height="300px" Width="300px" 
                        ImageUrl="~/Applications/agency/image/signature/icon_guest.jpg" />
                </td>
            </tr>
            <tr>
                <td class="style5" colspan="2">
                    ลำดับการคีย์ :
                    &nbsp;
                    <asp:Label ID="lbl_agentrequestno" runat="server" CssClass="style3"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style5" colspan="2">
                    &nbsp;เลขประจำตัว :
                    &nbsp;
                    <asp:Label ID="lbl_memberno" runat="server" CssClass="style4" 
                        ForeColor="#CC00CC"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style5" colspan="2">
                    ชื่อสกุล :
                    &nbsp;
                    <asp:Label ID="lbl_name" runat="server" CssClass="style4"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <input id="B_close" type="button" value="ปิด" onclick="OnCloseDialog()" /></td>
                <td valign="top">
                    &nbsp;<asp:HiddenField ID="Hd_seqno" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
