<%@ Page Language="C#" AutoEventWireup="true" Async="true" Inherits="Fiction2Fact.Projects.Login"
    CodeBehind="Login.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Fiction2Fact" Namespace="Fiction2Fact.F2FControls" TagPrefix="F2FControls" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="Content-Language" content="en-us" />
    <meta name="GENERATOR" content="Microsoft FrontPage 6.0" />
    <meta name="ProgId" content="FrontPage.Editor.Document" />
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <%--<meta http-equiv="Content-Type" content="application/javascript; charset=utf-8" />--%>
    <title>:: Login ::</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/bootstrap.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/icons.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/assets/css/app.min.css")%>" />

    <script type="text/javascript">
        //<< Added to store session value for Menu body class  
        $(document).ready(function () {
            sessionStorage.setItem('BodyClass', '');
        });
        //>>
    </script>

    <%--<script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/base64.js") %>">
    </script>
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/dwpcneteg.js") %>">
    </script>
    <script type="text/javascript">
        function onbuttonclientclick() {
            var validated = Page_ClientValidate('grpLogin');
            if (validated) {
                Populatedwpcneteg(document.getElementById('<%= txtPassword.ClientID %>'));
            }
            else {
                alert('Please enter your credentials!!!');
            }
        }
    </script>--%>
    <%-- // << code Added by ramesh more on 13-Mar-2024 CR_1991 for VAPT--%>
    <script src="Content/js/aes.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SubmitsEncry() {

            if (Page_ClientValidate('grpLogin')) {

                var txtpassword = document.getElementById("<%=txtPassword.ClientID %>").value.trim();
                var p1 = 0;
                if (txtPassword == "") {
                    alert('Please enter Password');
                    return false;
                }
                else {
                    var key = CryptoJS.enc.Utf8.parse('8080808080808080');
                    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

                    var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtpassword), key,
                        {
                            keySize: 128 / 8,
                            iv: iv,
                            mode: CryptoJS.mode.CBC,
                            padding: CryptoJS.pad.Pkcs7
                        });

                    document.getElementById("<%=txtPassword.ClientID %>").value = encryptedpassword;
                    p1 = 1;
                }

                if (p1 > 0) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                alert('Please enter credintial.');
                return false;
            }
        }
    </script>
    <%--//>>--%>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body class="account-body accountbg d-block">
    <form id="form1" runat="server" defaultfocus="txtUsername" defaultbutton="btnLogin">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:HiddenField ID="hfValue" runat="server" />
        <!-- Log In page -->
        <div class="container">
            <div class="row vh-100 d-flex justify-content-center">
                <div class="col-12 align-self-center">
                    <div class="row">
                        <div class="col-lg-5 mx-auto">
                            <div class="card">
                                <div class="card-body p-0 border-bottom">
                                    <div class="text-center p-3">
                                        <div class="my-3 d-flex align-items-center justify-content-between">
                                            <img src="<%=Fiction2Fact.Global.site_url("Content/images/logos/innov_grc_logo.png")%>" height="58" />
                                            <%--<img src="<%=Fiction2Fact.Global.site_url("Content/images/logos/fiction2fact_logo.png")%>" height="45" />--%>
                                            <img src="<%=Fiction2Fact.Global.site_url("Content/images/logos/e-compliance_logo.png")%>" height="55" />
                                        </div>
                                        <%--<h4 class="mt-3 mb-1 fw-semibold text-white font-18">Let's Get Started Dastone</h4>--%>
                                    </div>
                                    <p class="text-center text-danger">Please connect with your domain User Id and password.</p>
                                </div>
                                <div class="card-body p-0 border-bottom">
                                    <div class="form-horizontal auth-form p-3">
                                        <div class="form-group mb-2">
                                            <label class="form-label" for="username">User Id <span class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <%--<input type="text" id="txtUserID" placeholder="Enter user Id" class="form-control" autocomplete="off" />--%>
                                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter user Id" MaxLength="30"></asp:TextBox>
                                                <F2FControls:F2FFilteredTextbox runat="server" TargetControlID="txtUsername" />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvUsername" ControlToValidate="txtUsername"
                                                    ValidationGroup="grpLogin" ErrorMessage="*" CssClass="text-danger"
                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <!--end form-group-->

                                        <div class="form-group mb-2">
                                            <label class="form-label" for="userpassword">Password <span class="text-danger">*</span></label>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter password"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ControlToValidate="txtPassword"
                                                    ValidationGroup="grpLogin" ErrorMessage="*" CssClass="text-danger"
                                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <!--end form-group-->

                                        <div class="form-group row my-3">
                                            <asp:Label ID="lblMsg" runat="server" ForeColor="red" Style="font-family: Trebuchet MS;"></asp:Label>
                                        </div>

                                        <div class="form-group mb-3 row">
                                            <div class="col-12">
                                                <asp:LinkButton ID="btnLogin" runat="server" CssClass="btn btn-primary w-100 waves-effect waves-light" Text="Login" OnClientClick="return SubmitsEncry();"
                                                    OnClick="btnLogin_Click" ValidationGroup="grpLogin">
                                                            Log In <i class="fas fa-sign-in-alt ms-1"></i>
                                                </asp:LinkButton>
                                            </div>
                                            <!--end col-->
                                        </div>
                                        <div class="form-group mb-0 row">
                                            <div class="col-12">
                                                <asp:LinkButton ID="btnSSOLogin" runat="server" CssClass="btn btn-danger w-100 waves-effect waves-light" Text="SSO Login"
                                                    OnClick="btnSSOLogin_Click">
                                                    SSO Log In <i class="fas fa-sign-in-alt ms-1"></i>
                                                </asp:LinkButton>
                                            </div>
                                            <!--end col-->
                                        </div>
                                        <!--end form-group-->

                                        <div class="mt-3 d-none">
                                            <asp:LinkButton ID="lbViewCircular" runat="server" Style="color: #FF0000;" OnClick="lbViewCircular_Click" Text="Click here" />, to view the circulars.
                                        </div>
                                    </div>

                                    <div class="card-body text-center">
                                        <%--<div class="account-social">
                                            <h6 class="mb-3">Powered by</h6>
                                        </div>
                                        <div class="my-3 d-flex align-items-center justify-content-between">
                                            <img src="<%=Fiction2Fact.Global.site_url("Content/images/logos/innov_grc_logo.png")%>" height="50" />
                                            <img src="<%=Fiction2Fact.Global.site_url("Content/images/logos/logo_sticker.png")%>" height="50" />
                                        </div>--%>
                                        <div class="account-social">
                                            <h6 class="mb-3">Powered by</h6>
                                        </div>
                                        <div class="text-center">
                                            <a href="https://fiction2fact.com/" target="_blank">
                                                <img src="<%=Fiction2Fact.Global.site_url("Content/images/logos/logo_sticker.png")%>" height="35" /></a>
                                        </div>
                                    </div>


                                    <!--end form-->
                                </div>

                            </div>
                            <!--end card-->
                        </div>
                        <!--end col-->
                    </div>
                    <!--end row-->
                </div>
                <!--end col-->
            </div>
            <!--end row-->
        </div>
        <!--end container-->
        <!-- End Log In page -->

    </form>
</body>
</html>
