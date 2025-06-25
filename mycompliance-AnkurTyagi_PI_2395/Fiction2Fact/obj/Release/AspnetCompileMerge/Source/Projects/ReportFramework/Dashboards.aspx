<%@ Page Title="Framework Report" Language="C#" MasterPageFile="~/Projects/Temp3.master" AutoEventWireup="true"
    Inherits="Fiction2Fact.Projects.Reports.Dashboards" CodeBehind="Dashboards.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="<%=Fiction2Fact.Global.site_url("Content/js/legacy/ChartJs/chart.min.js")%>"></script>

    <style>
        .container {
            width: 200px;
            margin: 0 auto;
        }

        .canvas-container {
            /*min-height: 300px;*/
            max-height: 600px;
            position: relative;
        }

        .widget {
            position: relative;
            margin-bottom: 80px;
            /*background: #efefef;*/
            padding: 12px;
            margin-bottom: 30px;
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
        }

            .widget h3 {
                /*margin: -12px 0 12px -12px;*/
                padding: 12px;
                width: 100%;
                text-align: center;
                color: #627b86;
                line-height: 2em;
                background: #d0dde3;
            }

        /*.chart-legend ul {
            list-style: none;
            width: 100%;
            margin: 30px auto 0;
        }*/

        .chart-legend {
            padding-top: 10px;
        }

            .chart-legend li {
                text-indent: 16px;
                line-height: 24px;
                position: relative;
                font-weight: 200;
                display: block;
                /*float: left;
            width: 50%;*/
                font-size: 0.8em;
            }

                .chart-legend li:before {
                    display: block;
                    width: 10px;
                    height: 16px;
                    position: absolute;
                    left: 0;
                    top: 3px;
                    content: "";
                }

        .val4:before {
            /*background-color: rgba(255, 99, 132, 0.2);
            border-color: rgb(255, 99, 132);*/
            background-color: rgb(255, 99, 132);
        }

        .val3:before {
            /*background-color: rgba(255, 159, 64, 0.2);
            border-color: rgb(54, 162, 235);*/
            background-color: rgb(54, 162, 235);
        }

        .val2:before {
            /*background-color: rgba(255, 205, 86, 0.2);
            border-color: rgb(255, 205, 86);*/
            background-color: rgb(255, 205, 86);
        }

        .val1:before {
            /*background-color: rgba(75, 192, 192, 0.2);
            border-color: #009933;*/
            background-color: #009933;
        }

        @media only screen and (min-width:300px) {
            .container {
                width: 300px;
                margin: 0 auto;
            }
        }

        @media only screen and (min-width:600px) {
            .container {
                width: 580px;
                margin: 0 auto;
            }

            .third {
                float: left;
                width: 47.5%;
                margin-left: 5%;
            }

                .third:first-child {
                    margin-left: 0;
                }

                .third:last-child {
                    display: block;
                    width: 100%;
                    margin-left: 0;
                }
        }

        @media only screen and (min-width:960px) {
            .container {
                width: 1300px;
            }

            .third {
                float: left;
                width: 30%;
                margin-left: 2.5%;
                margin-right: 2.5%;
            }

                .third:first-child {
                    margin-left: 0;
                }

                .third:last-child {
                    margin-right: 0;
                    margin-left: 2.5%;
                    width: 30%;
                }
        }

        @media only screen and (min-width:1440px) {
            .container {
                width: 1120px;
            }
        }

        @media only screen and (min-width:1360px) {
            .container {
                width: 1300px;
            }
        }
    </style>

    <!-- Page-Title -->
    <div class="row">
        <div class="col-sm-12">
            <div class="page-title-box">
                <div class="row">
                    <div class="col">
                        <h4 class="page-title">Dashboards</h4>
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
            <div class="card">
			    <div class="card-body">
                    <asp:Literal runat="server" ID="litChart" />
			    </div>
			</div>
        </div> 
    </div>
    <!-- end row -->

    
</asp:Content>
