$(function() {
    $("#submitProject").click(submitProjectClicked);
    $("#submitReleases").click(submitReleasesClicked);
    $(".work-box").click(loadProject);
});

function loadProject(e) {
    e.preventDefault();
    $("#repoInput").val($(this).attr("data-repo"));
    $("#projectInput").val($(this).attr("data-project"));
    $("#submitProject").click();
    return false;
}

function submitReleasesClicked(e) {
    e.preventDefault();

    var tag1 = $("#releaseDropdown1").val();
    var tag2 = $("#releaseDropdown2").val();

    if (tag1 && !tag2) {
        renderDependencies(tag1);
        return;
    }
    if (!tag1 && tag2) {
        renderDependencies(tag2);
        return;
    } 
}

function renderDependencies(tag) {
    GetDependenciesOfOneReleaseRequest(getRepo(), getProject(), tag).done(function(response, status, xhr) {

    });
}

function submitProjectClicked(e) {

    e.preventDefault();//revent form submit

    if (validateSubmittedProject()) {
        var repo = $("#repoInput").val() || $("#repoInput").attr("placeholder");
        var project = $("#projectInput").val() || $("#projectInput").attr("placeholder");
        //store for other components to access the currently picked repo and project
        $("#repoInput").attr("data-selected", repo);
        $("#projectInput").attr("data-selected", project);
        
        GetReleasesRequest(repo, project).done(function(response, status, xhr) {
            fillReleaseDropdowns(response.d);
            scrollTo($("#releasePickBarrier"));
            $("#releasePicker").slideDown(1000);
            //scrollTo($("#releasePicker"));
        });
    }
}

function getRepo() {
    return $("#repoInput").attr("data-selected");
}

function getProject() {
    return $("#projectInput").attr("data-selected");
}

function scrollTo(elem) {
    $('html, body').animate({
        scrollTop: $(elem).offset().top
    }, 1000);
}

function validateSubmittedProject() {
    return true; //empty is allowed, with default values
}

function GetReleasesRequest(repo, project) {
    return $.ajax(
        "default.aspx/Releases",
        {
            data: JSON.stringify({ repo: repo, projectName: project }),
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            error: function(xhr,status,errorThrown) {
                failureWarning("Something went wrong while fetching the releases of the chosen project.");
            }
        });
}

function GetDependenciesOfOneReleaseRequest(repo, project, tagName) {
    return $.ajax(
        "default.aspx/Dependencies",
        {
            data: JSON.stringify({ repo: repo, projectName: project, tagName: tagName }),
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            error: function (xhr, status, errorThrown) {
                failureWarning("Something went wrong while fetching the dependencies of the release with tagname " + tagName);
            }
        });
}

function failureWarning(message) {
    alert(message);
}

function fillReleaseDropdowns(releases) {
    $("#releaseDropdown1,#releaseDropdown2").html("");
    $("#releaseDropdown1,#releaseDropdown2").append("<option value=''>No release selected...</option>");
    releases.forEach(r => $("#releaseDropdown1,#releaseDropdown2").append("<option value='"+r.TagName+"'>"+r.Name+ "(" + r.TagName + ")</option>"));
}