<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_finance_ratio.aspx.cs"
    Inherits="Saving.Applications.mis.dlg.w_dlg_finance_ratio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script type="text/javascript">
        function showText() {
            var setIndex = document.getElementById("HiddenField1").value;
            var str = opener.document.getElementById("ctl00_ContentPlace_HiddenString").value;
            var dataset = str.split("|");
            var data = new Array();
            var a = 0;
            var arrDataset;
            for (a; a < dataset.length - 1; a++) {

                arrDataset = dataset[a].split(",");
                if (arrDataset[0] == setIndex) {
                    data[0] = arrDataset[1];
                    data[1] = arrDataset[2];
                    data[2] = arrDataset[3];
                    data[3] = arrDataset[4];
                    data[4] = arrDataset[5];
                    data[5] = arrDataset[6];
                    data[6] = arrDataset[7];
                }
            }
            document.getElementById("Label1").innerHTML = data[0];
            document.getElementById("Label3").innerHTML = data[2];
            document.getElementById("Label4").innerHTML = data[4];
            document.getElementById("Label5").innerHTML = data[3];
            document.getElementById("Label7").innerHTML = data[5];
            document.getElementById("Label8").innerHTML = data[1];
            document.getElementById("Label10").innerHTML = data[6];

        }
        
    </script>

    <title id="pagetitle"></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="True" 
            Font-Size="X-Large" Font-Underline="True"></asp:Label>
        <br />
        <table class="style1">
            <tr>
                <td align="center" width="60%">
                    <asp:Label ID="Label3" runat="server" Text="Label" Font-Size="Large"></asp:Label>
                </td>
                <td width="10%" align="center">
                    <asp:Label ID="Label2" runat="server" Text="="></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Label" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="Label5" runat="server" Text="Label" Font-Size="Large"></asp:Label>
                </td>
                <td align="center">
                    <asp:Label ID="Label6" runat="server" Text="="></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Label" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="Label8" runat="server" Text="Label" Font-Size="Large"></asp:Label>
                </td>
                <td align="center">
                    <asp:Label ID="Label9" runat="server" Text="="></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label10" runat="server" Text="Label" Font-Size="Large"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />

    <script type="text/javascript">        showText();</script>

    </form>
</body>
</html>
