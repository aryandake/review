<%@ Page Language="C#" MasterPageFile="~/Projects/Temp4.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Default" Title="Home" ValidateRequest="false" CodeBehind="Default.aspx.cs" %>

<%@ Import Namespace="Fiction2Fact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<link type="text/css" href="<%=Fiction2Fact.Global.site_url("Content/images/legacy/HomePageScrollerFiles/skdslider.css")%>" rel="stylesheet">--%>

    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/images/legacy/HomePageScrollerFiles/skdslider.min.js")%>"></script>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            //jQuery('#demo1').skdslider({delay:5000, animationSpeed: 2000,showNextPrev:true,showPlayButton:true,autoSlide:true,animationType:'fading'});
            //jQuery('#demo2').skdslider({delay:5000, animationSpeed: 1000,showNextPrev:true,showPlayButton:false,autoSlide:true,animationType:'sliding'});
            jQuery('#demo3').skdslider({ delay: 5000, animationSpeed: 2000, showNextPrev: true, showPlayButton: true, autoSlide: true, animationType: 'fading' });

            jQuery('#responsive').change(function () {
                $('#responsive_wrapper').width(jQuery(this).val());
                $(window).trigger('resize');
            });
        });
    </script>

    <script type="text/javascript">
        function pausescroller(content, divId, divClass, delay) {
            this.content = content //message array content
            this.tickerid = divId //ID of ticker div to display information
            this.delay = delay //Delay between msg change, in miliseconds.
            this.mouseoverBol = 0 //Boolean to indicate whether mouse is currently over scroller (and pause it if it is)
            this.hiddendivpointer = 1 //index of message array for hidden div
            document.write('<div id="' + divId + '" class="' + divClass + '" style="position: relative; color:#585858; overflow: hidden"><div style="position: absolute; color:#585858; width: 100%" id="' + divId + '1">' + content[0] + '</div><div style="position: absolute; width: 100%; visibility: hidden" id="' + divId + '2">' + content[1] + '</div></div>')
            var scrollerinstance = this
            if (window.addEventListener) //run onload in DOM2 browsers
                window.addEventListener("load", function () { scrollerinstance.initialize() }, false)
            else if (window.attachEvent) //run onload in IE5.5+
                window.attachEvent("onload", function () { scrollerinstance.initialize() })
            else if (document.getElementById) //if legacy DOM browsers, just start scroller after 0.5 sec
                setTimeout(function () { scrollerinstance.initialize() }, 500)
        }

        pausescroller.prototype.initialize = function () {
            this.tickerdiv = document.getElementById(this.tickerid)
            this.visiblediv = document.getElementById(this.tickerid + "1")
            this.hiddendiv = document.getElementById(this.tickerid + "2")
            this.visibledivtop = parseInt(pausescroller.getCSSpadding(this.tickerdiv))
            //set width of inner DIVs to outer DIV's width minus padding (padding assumed to be top padding x 2)
            this.visiblediv.style.width = this.hiddendiv.style.width = this.tickerdiv.offsetWidth - (this.visibledivtop * 2) + "px"
            this.getinline(this.visiblediv, this.hiddendiv)
            this.hiddendiv.style.visibility = "visible"
            var scrollerinstance = this
            document.getElementById(this.tickerid).onmouseover = function () { scrollerinstance.mouseoverBol = 1 }
            document.getElementById(this.tickerid).onmouseout = function () { scrollerinstance.mouseoverBol = 0 }
            if (window.attachEvent) //Clean up loose references in IE
                window.attachEvent("onunload", function () { scrollerinstance.tickerdiv.onmouseover = scrollerinstance.tickerdiv.onmouseout = null })
            setTimeout(function () { scrollerinstance.animateup() }, this.delay)
        }


        // -------------------------------------------------------------------
        // animateup()- Move the two inner divs of the scroller up and in sync
        // -------------------------------------------------------------------

        pausescroller.prototype.animateup = function () {
            var scrollerinstance = this
            if (parseInt(this.hiddendiv.style.top) > (this.visibledivtop + 5)) {
                this.visiblediv.style.top = parseInt(this.visiblediv.style.top) - 5 + "px"
                this.hiddendiv.style.top = parseInt(this.hiddendiv.style.top) - 5 + "px"
                setTimeout(function () { scrollerinstance.animateup() }, 50)
            }
            else {
                this.getinline(this.hiddendiv, this.visiblediv)
                this.swapdivs()
                setTimeout(function () { scrollerinstance.setmessage() }, this.delay)
            }
        }

        // -------------------------------------------------------------------
        // swapdivs()- Swap between which is the visible and which is the hidden div
        // -------------------------------------------------------------------

        pausescroller.prototype.swapdivs = function () {
            var tempcontainer = this.visiblediv
            this.visiblediv = this.hiddendiv
            this.hiddendiv = tempcontainer
        }

        pausescroller.prototype.getinline = function (div1, div2) {
            div1.style.top = this.visibledivtop + "px"
            div2.style.top = Math.max(div1.parentNode.offsetHeight, div1.offsetHeight) + "px"
        }

        pausescroller.prototype.setmessage = function () {
            var scrollerinstance = this
            if (this.mouseoverBol == 1) //if mouse is currently over scoller, do nothing (pause it)
                setTimeout(function () { scrollerinstance.setmessage() }, 100)
            else {
                var i = this.hiddendivpointer
                var ceiling = this.content.length
                this.hiddendivpointer = (i + 1 > ceiling - 1) ? 0 : i + 1
                this.hiddendiv.innerHTML = this.content[this.hiddendivpointer]
                this.animateup()
            }
        }

        pausescroller.getCSSpadding = function (tickerobj) { //get CSS padding value, if any
            if (tickerobj.currentStyle)
                return tickerobj.currentStyle["paddingTop"]
            else if (window.getComputedStyle) //if DOM2
                return window.getComputedStyle(tickerobj, "").getPropertyValue("padding-top")
            else
                return 0
        }
    </script>

    <style>
        .title-text, .card-title {
            text-transform: capitalize;
            letter-spacing: 0.02em;
            font-size: 13px;
            font-weight: 500;
            margin: 0;
            color: #2c3652;
            text-shadow: 0 0 1px rgba(241,245,250,0.1);
            font-family: "Poppins",sans-serif;
        }

        .subHeading {
            font-size: 12px;
            font-weight: 400;
        }
    </style>
    <style>
        .title-text, .card-title-new {
            text-transform: capitalize;
            letter-spacing: 0.02em;
            font-size: 16px;
            font-weight: 600;
            margin: 0;
            color: #2c3652;
            text-shadow: 0 0 1px rgba(241,245,250,0.1);
            font-family: "Poppins",sans-serif;
        }
    </style>

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Home</h4>
                        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="custom-alert-box"></asp:Label>
                    </div>
                    <!--end col-->
                </div>
                <!--end row-->
            </div>
            <!--end page-title-box-->
        </div>
        <!--end col-->
    </div>
    <!--end row-->
    <!-- end page title end breadcrumb -->

    <div class="row">
        <div class="col-12">
            <div class="card" ID="divCardBody" runat="server">
                <div class="card-body">
                    <%--<asp:Panel ID="pnlCircDashboard" runat="server" Visible="false">--%>
                    <asp:Panel ID="pnlCircDashboard" runat="server" Visible="false">
                        <div class="card">
                            <div class="card-header bg-blue">
                                <div class="row align-items-center">
                                    <div class="col">
                                        <%--card-title custom-ch-bg-color--%>
                                        <h4 class="card-title-new text-white">Circular</h4>
                                    </div>
                                    <!--end col-->
                                </div>
                                <!--end row-->
                            </div>
                            <!--end card-header-->
                            <div class="card-body">
                                <div class="row justify-content-center">
                                    <div class="col-md-6 col-lg-3">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Completed within target date</h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblCWTDCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblCWTD" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Completed after target date</h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblCATDCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblCATD" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Not yet due</h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblNYDCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblNYD" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Due but not completed</h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblDBNCCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblDBNC" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                </div>
                            </div>
                            <!--end card-body-->
                        </div>
                        <!--end card-->
                    </asp:Panel>

                    <%--<asp:Panel ID="pnlFilDashboard" runat="server" Visible="false">--%>
                    <asp:Panel ID="pnlFilDashboard" runat="server" Visible="false">
                        <div class="card">
                            <div class="card-header bg-blue">
                                <div class="row align-items-center">
                                    <div class="col">
                                        <h4 class="card-title-new text-white">Filings</h4>
                                    </div>
                                    <!--end col-->
                                </div>
                                <!--end row-->
                            </div>
                            <!--end card-header-->
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-6 col-lg-3" id="divT_TDA" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Tasks Due for Action <span class="subHeading">(for the month)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblT_TDACount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblT_TDA" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divT_TNCNC" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Tasks Not Closed and Non Compliant<span class="subHeading">(up till date)</span></h4>
                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblT_TNCNCCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblT_TNCNC" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divT_TNSNC" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Tasks Not Submitted & Non Compliant<span class="subHeading">(up till date)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblT_TNSNCCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblT_TNSNC" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divT_TOWT" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Tasks Open within timelines<span class="subHeading">(for the month)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblT_TOWTCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblT_TOWT" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divT_CT" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Compliant Tasks<span class="subHeading">(for the month)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblT_CTCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblT_CT" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divT_NCT" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Non Compliant Tasks<span class="subHeading">(for the month)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblT_NCTCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblT_NCT" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>

                                    <div class="col-md-6 col-lg-3" id="divR_TDA" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Tasks Due for Action<span class="subHeading">(for the month)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblR_TDACount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblR_TDA" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divR_TOA" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Tasks Overdue for Action<span class="subHeading">(up till date)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblR_TOACount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblR_TOA" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divR_NCTNC" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Non Compliant Tasks & Not Closed<span class="subHeading">(up till date)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblR_NCTNSCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblR_NCTNS" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divR_CT" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Compliant Tasks<span class="subHeading">(for the month)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblR_CTCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblR_CT" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                    <div class="col-md-6 col-lg-3" id="divR_NCT" runat="server" visible="false">
                                        <div class="card report-card">
                                            <div class="card-body">
                                                <div class="row d-flex justify-content-center">
                                                    <div class="col">
                                                        <h4 class="card-title">Non Compliant Tasks<span class="subHeading">(for the month)</span></h4>

                                                        <div class="col-auto align-self-center">
                                                            <div class="report-main-icon bg-light-alt">
                                                                <h2 class="mb-0">
                                                                    <asp:Label ID="lblR_NCTCount" runat="server"></asp:Label></h2>
                                                            </div>
                                                        </div>

                                                        <asp:Label ID="lblR_NCT" CssClass="btn btn-primary btn-sm" runat="server" Text="Click Here"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end card-body-->
                                        </div>
                                        <!--end card-->
                                    </div>
                                </div>
                                <!--end card-body-->
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <!-- end row -->

</asp:Content>
