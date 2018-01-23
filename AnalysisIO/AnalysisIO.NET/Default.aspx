<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AnalysisIO.NET.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AnalysisIO</title>
    <link rel="stylesheet" href="css/bootstrap.min.css"/>
    <link rel="stylesheet" href="css/jquery.fancybox.css"/>
    <link rel="stylesheet" href="css/main.css"/>
    <link rel="stylesheet" href="css/responsive.css"/>
    <link rel="stylesheet" href="css/animate.min.css"/>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css"/>
    <script type="text/javascript" src="js/D3.min.js"></script>
    
    <!-- JS FILES -->
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/jquery.fancybox.pack.js"></script>
        <script src="js/jquery.waypoints.min.js"></script>
        <script src="js/retina.min.js"></script>
        <script src="js/modernizr.js"></script>
        <script src="js/main.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- header section -->
        <section class="banner" role="banner">
            <header id="header">
                <!-- navigation section  -->
                <div class="header-content clearfix">
                    <nav class="navigation" role="navigation">
                        <ul class="primary-nav">
                            <li><a href="#">>Dead Link</a></li>
                            <li><a href="#">Dead Link</a></li>
                            <li><a href="#">Dead Link</a></li>
                        </ul>
                    </nav>
                    <a href="#" class="nav-toggle">Menu<span></span></a>
                </div>
                <!-- navigation section  -->
            </header>

            <!-- banner text -->
            <div class="container">
                <div class="col-md-10 col-md-offset-1">
                    <div class="banner-text text-center">
                        <h1>Analysis.IO</h1>
                        <p>
                            Analyze the dependencies of open source git C# projects.<br/>
                            Scroll down to try it out.
                        </p>
                        <!-- banner text -->
                    </div>
                </div>
            </div>
        </section>
        <!-- header section -->
        <!-- description text section -->
        <section id="descripton" class="section descripton">
            <div class="container">
                <div class="col-md-10 col-md-offset-1 text-center">
                    <div id="uxJson" runat="server"></div>     
                </div>
            </div>
        </section>
        <!-- description text section -->
        <!-- portfolio section -->
        <section id="works" class="works section no-padding">
            <div class="container-fluid">
                <div class="row no-gutter">
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/work-1.jpg" class="work-box">
                            <img src="images/work-1.jpg" alt=""/>
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Project Name</h5>
                                    <p>Logo Design</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/work-2.jpg" class="work-box">
                            <img src="images/work-2.jpg" alt=""/>
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Project Name</h5>
                                    <p>Website Design</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/work-3.jpg" class="work-box">
                            <img src="images/work-3.jpg" alt=""/>
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Project Name</h5>
                                    <p>Branding</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/work-4.jpg" class="work-box">
                            <img src="images/work-4.jpg" alt=""/>
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Project Name</h5>
                                    <p>Graphic Design</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/work-5.jpg" class="work-box">
                            <img src="images/work-5.jpg" alt=""/>
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Project Name</h5>
                                    <p>Website Design</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/work-6.jpg" class="work-box">
                            <img src="images/work-6.jpg" alt=""/>
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Project Name</h5>
                                    <p>Logo Design</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/work-7.jpg" class="work-box">
                            <img src="images/work-7.jpg" alt=""/>
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Project Name</h5>
                                    <p>Branding</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/work-8.jpg" class="work-box">
                            <img src="images/work-8.jpg" alt=""/>
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Project Name</h5>
                                    <p>Website Design</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                </div>
            </div>
        </section>
        <!-- portfolio section -->
        <!-- hire me section -->
        <section id="hireme" class="section hireme">
            <div class="container">
                <div class="col-md-8 col-md-offset-2 text-center">
                    <h3>Need something specific?</h3>
                    <p>We are currently crafting new products but would love to hear from you.</p>
                    <a href="contact.html" class="btn btn-large">Hire me</a>
                </div>
            </div>
        </section>
        <!-- hire me section -->
        <!-- footer -->
        <footer class="section footer">
            <div class="footer-bottom">
                <div class="container">
                    <div class="col-md-12">
                        <p>
                            <ul class="footer-share">
                                <li><a href="#"><i class="fa fa-facebook"></i></a></li>
                                <li><a href="#"><i class="fa fa-twitter"></i></a></li>
                                <li><a href="#"><i class="fa fa-linkedin"></i></a></li>
                                <li><a href="#"><i class="fa fa-google-plus"></i></a></li>
                                <li><a href="#"><i class="fa fa-pinterest-p"></i></a></li>
                                <li><a href="#"><i class="fa fa-vimeo"></i></a></li>
                            </ul>
                        </p>
                        <p>
                            © 2015 All rights reserved. All Rights Reserved<br>
                            Made with <i class="fa fa-heart pulse"></i>by <a href="http://www.designstub.com/">Designstub</a>
                        </p>
                    </div>
                </div>
            </div>
        </footer>
        <!-- footer -->

        
    </form>
</body>
</html>
