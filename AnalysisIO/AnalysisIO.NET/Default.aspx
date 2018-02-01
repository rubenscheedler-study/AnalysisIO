<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AnalysisIO.NET.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AnalysisIO</title>
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/jquery.fancybox.css" />
    <link rel="stylesheet" href="css/main.css" />

    <link rel="stylesheet" href="css/responsive.css" />
    <link rel="stylesheet" href="css/animate.min.css" />
    <link rel="stylesheet" href="css/style.css" />
    <!--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css" />-->
    <script type="text/javascript" src="js/D3.min.js"></script>

    <!-- JS FILES -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/jquery.fancybox.pack.js"></script>
    <script src="js/jquery.waypoints.min.js"></script>
    <script src="js/retina.min.js"></script>
    <script src="js/modernizr.js"></script>
    <script src="js/main.js"></script>
    <script src="https://d3js.org/d3.v4.min.js"></script>
    <script src="js/Default.js"></script>
    <script src="js/DependencyRenderer.js"></script>
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
                            <li><a href="#projectPicker">Start analyzing</a></li>
                            <li><a href="#works">Examples</a></li>
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
                            Analyze the dependencies of open source git C# projects.<br />
                            Scroll down to try it out.
                        </p>
                        <!-- banner text -->
                    </div>
                </div>
            </div>
        </section>

        <!-- REPO + PROJECT -->
        <section id="projectPicker" class="section">
            <div class="container">
                <div class="row">
                    <div class="form-group col-sm-4">
                        <label for="repoInput">Owner</label>
                        <input type="text" class="form-control" id="repoInput" aria-describedby="repoHelp" placeholder="Rationally" />
                        <small id="repoHelp" class="form-text text-muted">Enter the name of a repository's owner (leave empty for an example).</small>
                    </div>
                    <div class="form-group col-sm-4">
                        <label for="projectInput">Repository</label>
                        <input type="text" class="form-control" id="projectInput" aria-describedby="projectHelp" placeholder="Rationally_visio" />
                        <small id="projectHelp" class="form-text text-muted">Enter one of the owner's C# repositories (leave empty for an example)</small>
                    </div>
                    <div class="col-sm-4">
                        <button class="btn btn-large with-top-margin" id="submitProject" title="The next step will be to select releases to compare.">Find Releases</button>
                    </div>
                </div>

            </div>
        </section>

        <div id="releasePickBarrier"></div>

        <!-- RELEASE -->
        <section id="releasePicker" class="section">
            <div class="container">
                <div class="row">
                    <div class="form-group col-sm-4">
                        <label for="releaseDropdown1">Release #1 (older)</label>
                        <select class="form-control" id="releaseDropdown1" aria-describedby="releaseHelp1">
                            <option value="default">Pick a release...</option>
                        </select>
                        <small id="releaseHelp1" class="form-text text-muted">Pick one of the releases.</small>
                    </div>
                    <div class="form-group col-sm-4">
                        <label for="releaseDropdown2">Release #2 (newer)</label>
                        <select class="form-control" id="releaseDropdown2" aria-describedby="releaseHelp2">
                            <option value="default">Pick a release...</option>
                        </select>
                        <small id="releaseHelp2" class="form-text text-muted">Pick one of the releases.</small>
                    </div>
                    <div class="col-sm-4">
                        <div class="row">
                            <div class="col">
                                <button class="btn btn-large with-top-margin" id="submitReleases">Visualize Dependencies</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <div id="dependencyBarrier"></div>

        <!-- DEPENDENCIES -->
        <section id="dependencySection" class="section">
            <div id="preloader_container" class="container">
                <img src="images/preloader.gif"/>
            </div>
            <div class="container">
                <div id="dependencyArea">
                </div>
            </div>
            <div class="container">
                <div id="legend-for-double-tree">
                    <div class="row">
                        <table class=" legend-table table col-sm-4">
                            <tr>
                                <td class="legend-orange">Orange</td>
                                <td>Ingoing dependency.</td>
                            </tr>
                            <tr>
                                <td class="legend-purple">Purple</td>
                                <td>Outgoing dependency.</td>
                            </tr>
                            <tr>
                                <td class="legend-green">Green</td>
                                <td>Dependency introduced in the newer release.</td>
                            </tr>
                            <tr>
                                <td class="legend-red">Red</td>
                                <td>Dependency removed since the newer release.</td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </section>

        <div class="col-md-10 col-md-offset-1 text-center">
            <div id="uxJson" runat="server"></div>
        </div>
        <!-- description text section -->
        <!-- portfolio section -->
        <section id="works" class="works section no-padding">
            <div class="container-fluid">
                <div class="row no-gutter">
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/project_icon.png" class="work-box" data-repo="Rationally" data-project="Rationally_Visio">
                            <img src="images/project_icon_rationally.png" alt="" />
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Rationally</h5>
                                    <p>Design Documentation Application</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/project_icon.png" class="work-box" data-repo="OpenRA" data-project="OpenRA">
                            <img src="images/project_icon_openra.png" alt="" />
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>OpenRA</h5>
                                    <p>Game Engine</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/project_icon.png" class="work-box" data-repo="hypzeh" data-project="Smallify">
                            <img src="images/project_icon_spotify.png" alt="" />
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Smallify</h5>
                                    <p>Compact spotify plugin</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>
                    <div class="col-lg-3 col-md-6 col-sm-6 work">
                        <a href="images/project_icon.png" class="work-box" data-repo="rubenscheedler" data-project="AnalysisIO">
                            <img src="images/project_icon_analysisio.png" alt="" />
                            <div class="overlay">
                                <div class="overlay-caption">
                                    <h5>Analysis.IO</h5>
                                    <p>This application!</p>
                                </div>
                            </div>
                            <!-- overlay -->
                        </a>
                    </div>

                </div>
            </div>
        </section>
        <!-- portfolio section -->

        <!-- footer -->
        <footer class="section footer">
            <div class="footer-bottom">
                <div class="container">
                    <div class="col-md-12">
                        <p>
                            © 2017 Ronald Kruizinga and Ruben Scheedler
                        </p>
                    </div>
                </div>
            </div>
        </footer>
        <!-- footer -->


    </form>
</body>
</html>
