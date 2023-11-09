<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_gph_payout_lon.aspx.cs" Inherits="Saving.Applications.mis.w_sheet_gph_payloan_month" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postMonthRetrieve %>
    <%=postQuarterRetrieve %>
    <%=postHalfRetrieve %>
    <%=postYearRetrieve %>
    <style type="text/css">
        .style1
        {
            width: 100%;
            height: 367px;
        }
        .style2
        {
            height: 38px;
        }
        .style3
        {
            width: 625px;
            height: 67px;
        }
        .style4
        {
            height: 67px;
        }
    </style>

    <script type="text/javascript">

        function showTabPage(tab) {
            var i = 1;
            var tabamount = 4;
            for (i = 1; i <= tabamount; i++) {
                document.getElementById("tab_" + i).style.visibility = "hidden";
                document.getElementById("stab_" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).style.backgroundColor = "rgb(211,213,255)";
                    document.getElementById("ctl00_ContentPlace_HdCurrentTap").value = i + "";
                }
            }
        }

        function RetrieveClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_month") {
                postMonthRetrieve();
            }
            else if (buttonName == "b_quarter") {
                postQuarterRetrieve();
            }
            else if (buttonName == "b_half") {
                postHalfRetrieve();
            }
            else if (buttonName == "b_year") {
                postYearRetrieve();
            }
        }   
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%; border: solid 1px; margin-top: 5px">
        <tr align="center" class="dwtab">
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_1" width="25%"
                onclick="showTabPage(1);" width="20%">
                รายงานเทียบเดือน
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_2" width="25%"
                onclick="showTabPage(2);" width="20%">
                รายงานเทียบไตรมาส
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_3" width="25%"
                onclick="showTabPage(3);" width="20%">
                รายงานเทียบครึ่งปี
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer;" id="stab_4" width="25%"
                onclick="showTabPage(4);" width="20%">
                รายงานเทียบปี
            </td>
        </tr>
    </table>
    <br />
    <table style="width: 100%; height: 800px;" class="dwcontent">
        <tr>
            <td valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">
                    <table width="724">
                        <tr>
                            <td class="style2" colspan="2">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Large" Text="การจ่ายเงินกู้เทียบเดือน"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <dw:WebDataWindowControl ID="dw_cri_month" runat="server" DataWindowObject="d_mis_cri_monthyear"
                                    LibraryList="~/DataWindow/mis/criteria.pbl" ClientEventButtonClicked="RetrieveClick"
                                    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                                    ClientScriptable="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dw:WebDataWindowControl ID="dw_graph_month" runat="server" DataWindowObject="d_mis_gph_payout_lon"
                                    Height="400px" LibraryList="~/DataWindow/mis/payout_lon.pbl" Width="720px">
                                    <GraphConfigurations ImageFormat="Gif" RenderOption="ImageFile" GraphDynamicImageFileUrlPath="~/Applications/mis/graph" />
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align: top">
                                <dw:WebDataWindowControl ID="dw_data_month" runat="server" DataWindowObject="d_mis_gphdata_payout_lon"
                                    LibraryList="~/DataWindow/mis/payout_lon.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                    AutoSaveDataCacheAfterRetrieve="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab_2" style="visibility: hidden; position: absolute;">
                    <table style="height: 600px; width: 724">
                        <tr>
                            <td class="style2" colspan="2">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Large" Text="การส่งหุ้นเทียบไตรมาส"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <dw:WebDataWindowControl ID="dw_cri_quar" runat="server" DataWindowObject="d_mis_cri_quarteryear"
                                    LibraryList="~/DataWindow/mis/criteria.pbl" ClientEventButtonClicked="RetrieveClick"
                                    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                                    ClientScriptable="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dw:WebDataWindowControl ID="dw_graph_quar" runat="server" DataWindowObject="d_mis_gph_payout_lon_quarter"
                                    Height="400px" LibraryList="~/DataWindow/mis/payout_lon.pbl" Width="720px" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True">
                                    <GraphConfigurations ImageFormat="Gif" RenderOption="ImageFile" GraphDynamicImageFileUrlPath="~/Applications/mis/graph" />
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align: top">
                                <dw:WebDataWindowControl ID="dw_data_quar" runat="server" DataWindowObject="d_mis_gphdata_payout_lon_quarter"
                                    LibraryList="~/DataWindow/mis/payout_lon.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                    AutoSaveDataCacheAfterRetrieve="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab_3" style="visibility: hidden; position: absolute;">
                    <table style="height: 600px; width: 724">
                        <tr>
                            <td class="style2" colspan="2">
                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Large" Text="การส่งหุ้นเทียบครึ่งปี"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <dw:WebDataWindowControl ID="dw_cri_half" runat="server" DataWindowObject="d_mis_cri_halfyear"
                                    LibraryList="~/DataWindow/mis/criteria.pbl" ClientEventButtonClicked="RetrieveClick"
                                    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                                    ClientScriptable="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dw:WebDataWindowControl ID="dw_graph_half" runat="server" Height="400px" LibraryList="~/DataWindow/mis/payout_lon.pbl"
                                    DataWindowObject="d_mis_gph_payout_lon_half" Width="720px" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True">
                                    <GraphConfigurations ImageFormat="Gif" RenderOption="ImageFile" GraphDynamicImageFileUrlPath="~/Applications/mis/graph" />
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align: top">
                                <dw:WebDataWindowControl ID="dw_data_half" runat="server" LibraryList="~/DataWindow/mis/payout_lon.pbl"
                                    DataWindowObject="d_mis_gphdata_payout_lon_half" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                    AutoSaveDataCacheAfterRetrieve="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab_4" style="visibility: hidden; position: absolute;">
                    <table style="height: 600px; width: 724">
                        <tr>
                            <td class="style2" colspan="2">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Large" Text="การส่งหุ้นเทียบปี"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <dw:WebDataWindowControl ID="dw_cri_year" runat="server" DataWindowObject="d_mis_cri_year"
                                    LibraryList="~/DataWindow/mis/criteria.pbl" ClientEventButtonClicked="RetrieveClick"
                                    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                                    ClientScriptable="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dw:WebDataWindowControl ID="dw_graph_year" runat="server" Height="400px" LibraryList="~/DataWindow/mis/payout_lon.pbl"
                                    Width="720px" DataWindowObject="d_mis_gph_payout_lon_year" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True">
                                    <GraphConfigurations ImageFormat="Gif" RenderOption="ImageFile" GraphDynamicImageFileUrlPath="~/Applications/mis/graph" />
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="vertical-align: top">
                                <dw:WebDataWindowControl ID="dw_data_year" runat="server" DataWindowObject="d_mis_gphdata_payout_lon_year"
                                    LibraryList="~/DataWindow/mis/payout_lon.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                    AutoSaveDataCacheAfterRetrieve="True">
                                </dw:WebDataWindowControl>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdCurrentTap" runat="server" />
    <asp:Literal ID="LtChangeTap" runat="server"></asp:Literal>
</asp:Content>
