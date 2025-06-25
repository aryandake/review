<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorNotFound.aspx.cs" Inherits="Fiction2Fact.ErrorNotFound" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>:: Not Found ::</title>
    <link id="Link2" rel="stylesheet" type="text/css" href="css/main.css" runat="server" />
    <link id="Link3" rel="stylesheet" type="text/css" href="css/style.css" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" valign="top">
                        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top"></td>
                                <td align="right" valign="bottom">
                                    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0"
                                        height="50" width="220">
                                        <param name="movie" value="images/logo_animated.swf">
                                        <param name="quality" value="high">
                                        <param name="wmode" value="transparent">
                                        <embed src="images/logo_animated.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer"
                                            type="application/x-shockwave-flash" height="50" width="220"></embed>
                                    </object>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%--<table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="2%" align="left" valign="top" background="images/box_center.png">
                        <img src="images/box_left.png" width="13" height="38" /></td>
                    <td width="96%" align="left" valign="middle" background="images/box_center.png" class="bold">
                        Resource Not Found
                    </td>
                    <td width="2%" align="right" valign="top" background="images/box_center.png">
                        <img src="images/box_right.png" width="13" height="38" /></td>
                </tr>
                <tr>
                    <td align="left" valign="top" bgcolor="#FFFFFF" class="txt_box_left">
                        &nbsp;</td>
                    <td height="40" align="left" valign="top" bgcolor="#FFFFFF">
                        <div style="min-height: 350px;font-family:Trebuchet MS;">
                            <br />
                            <center>
                                <h1>
                                   <span style="color:Red;">Resource Not Found</span> 
                                </h1>
                                <br />
                                <span style="color:#00b8e4;">The resource Not found, Kindly contact to your Administrator.</span> 
                                <br />
                                <br />
                                <asp:Button ID="btnHome" runat="server" Text="Go To Home Page" OnClick="btnHome_Click"
                                    CssClass="html_button" />
                                <br />
                                <br />
                            </center>
                        </div>
                    </td>
                    <td align="right" valign="top" bgcolor="#FFFFFF" class="txt_box_right">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="left" valign="bottom" background="images/box_bottom_line.png" bgcolor="#FFFFFF">
                        <img src="images/box_bottom_left.png" width="15" height="15" /></td>
                    <td background="images/box_bottom_line.png" bgcolor="#FFFFFF">
                        <img src="images/spacer.gif" width="1" height="10" /></td>
                    <td align="right" valign="bottom" background="images/box_bottom_line.png" bgcolor="#FFFFFF">
                        <img src="images/box_bottom_right.png" width="15" height="15" /></td>
                </tr>
            </table>--%>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="background-color: #CCCCCC;">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" valign="middle">
                        <img src="Content/images/legacy/404_1.png" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <center>
                <br />
                <br />
                <asp:Button ID="btnHome" runat="server" Text="Go To Home Page" OnClick="btnHome_Click"
                    CssClass="html_button" />
                <br />
                <br />
            </center>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="98%" style="font-size: 8pt; font-family: Trebuchet MS; color: #FFFFFF"
                        colspan="2" align="center">
                        <table width="98%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td background="images/box_bottom_line.png">&nbsp;</td>
                            </tr>
                            <tr>
                                <td height="25" align="right" valign="bottom" class="copyright">2023-24 DSP Asset Managers Private Limited | Powered
                                    by
                                    <a href="http://www.fiction2fact.com" target="_new">
                                        <img src="images/logo_sticker.png" alt="Fiction2Fact Solutions Private Limited" height="20px" width="100px"
                                            style="vertical-align: bottom; border: none;" />
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
