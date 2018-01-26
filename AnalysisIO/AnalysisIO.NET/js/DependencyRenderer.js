var diameter = 960,
        radius = diameter / 2,
        innerRadius = radius - 240;

var cluster = d3.cluster()
        .size([360, innerRadius]);

var line = d3.radialLine()
    .curve(d3.curveBundle.beta(0.85))
    .radius(function (d) { return d.y; })
    .angle(function (d) { return d.x / 180 * Math.PI; });

var link;
var svg;
var node;

function renderDependencyComparison(olderJson, newerJson) {
    prepareDependencyArea();

    render("", JSON.parse(olderJson), JSON.parse(newerJson));
    scrollTo("#dependencyArea");
}

function renderDependencies(treeJson) {
    prepareDependencyArea();

    renderSingleTree(JSON.parse(treeJson));
    scrollTo("#dependencyArea");
}

function prepareDependencyArea() {
    $("#dependencyArea").html("");
    svg = d3.select("#dependencyArea").append("svg")
        .attr("width", diameter)
        .attr("height", diameter)
        .append("g")
        .attr("transform", "translate(" + radius + "," + radius + ")");

    link = svg.append("g").selectAll(".link");
    node = svg.append("g").selectAll(".node");
}

function renderSingleTree(tree) {

    var classes = flatten(tree).filter(node => node.Dependencies !== undefined);

    var root = packageHierarchy(classes).sum(function (d) { return d.size; });

    configureTreeByRootNode(root);
}

function render(error, oldTree, newTree) {
    if (error) console.log(error);

    var newClasses = flatten(newTree).filter(node => node.Dependencies !== undefined);
    var oldClasses = flatten(oldTree).filter(node => node.Dependencies !== undefined);
    var classes = defineChanges(oldClasses, newClasses);

    var root = packageHierarchy(classes).sum(function (d) { return d.size; });

    configureTreeByRootNode(root);
}

function configureTreeByRootNode(root) {
    cluster(root);


    link = link
      .data(packageImports(root.leaves()))
      .enter().append("path")
        .each(function (d) { d.source = d[0], d.target = d[d.length - 1]; })
        .attr("class", function (d) { return "link " + d.Changed })
        .attr("d", line);

    node = node
      .data(root.leaves())
      .enter().append("text")
        .attr("class", "node")
        .attr("dy", "0.31em")
        .attr("transform", function (d) { return "rotate(" + (d.x - 90) + ")translate(" + (d.y + 8) + ",0)" + (d.x < 180 ? "" : "rotate(180)"); })
        .attr("text-anchor", function (d) { return d.x < 180 ? "start" : "end"; })
        .text(function (d) { return d.data.key; })
        .classed("newClass", function (d) { return d.data.NewObject })
        .classed("deletedClass", function (d) { return d.data.OldObject })
        .on("mouseover", mouseovered)
        .on("mouseout", mouseouted);
}

function flatten(response) {
    return response.Children.reduce(function (flat, toFlatten) {
        return flat.concat(toFlatten.Children.length === 0 ? toFlatten : flatten(toFlatten));
    }, [])
        .sort(function (a, b) { return d3.ascending(a.Identifier, b.Identifier); });
}

function defineChanges(oldClasses, newClasses) {
    //Ensure both lists contain same classes
    oldClasses = oldClasses.concat(newClasses.filter(elem => oldClasses.findIndex(item => item.Identifier === elem.Identifier) === -1)
        .map(elem => { return { Identifier: elem.Identifier, Dependencies: [], NewObject: true, Children: [] } }));
    newClasses = newClasses.concat(oldClasses.filter(elem => newClasses.findIndex(item => item.Identifier === elem.Identifier) === -1)
        .map(elem => { return { Identifier: elem.Identifier, Dependencies: [], OldObject: true, Children: [] } }));

    oldClasses = oldClasses.sort(function (a, b) { return d3.ascending(a.Identifier, b.Identifier); });
    newClasses = newClasses.sort(function (a, b) { return d3.ascending(a.Identifier, b.Identifier); });

    var classes = [];
    oldClasses.forEach((elem, index) => {
        var newElem = newClasses[index];
        if (!elem.Dependencies) {
            classes.push(newElem);
            return;
        };
        var elemsToAdd = [];
        elem.Dependencies.forEach(dep => { //look for deleted dependencies
            if (newElem.Dependencies.indexOf(dep) < 0) {
                elemsToAdd.push({ Name: dep, Change: "linkDeleted" }); //deleted
            }
        });
        var elemstoupdate = [];
        newElem.Dependencies.forEach((dep, index) => { //look for added dependencies
            if (elem.Dependencies.indexOf(dep) < 0) {
                elemstoupdate.push({ Index: index, obj: { Name: dep, Change: "linkAdded" } });
            }
        });
        newElem.NewObject = elem.NewObject; //Mark object as newly added
        elemstoupdate.forEach(elem => {
            newElem.Dependencies[elem.Index] = elem.obj;
        });
        newElem.Dependencies = newElem.Dependencies.concat(elemsToAdd);
        classes.push(newElem);
    });
    return classes;
}

function mouseovered(d) {
    node
        .each(function (n) { n.target = n.source = false; });

    link
        .classed("link--target", function (l) { if (l.target === d) return l.source.source = true; })
        .classed("link--source", function (l) { if (l.source === d) return l.target.target = true; })
      .filter(function (l) { return l.target === d || l.source === d; })
        .raise();

    node
        .classed("node--target", function (n) { return n.target; })
        .classed("node--source", function (n) { return n.source; });
}

function mouseouted() {
    link
        .classed("link--target", false)
        .classed("link--source", false);

    node
        .classed("node--target", false)
        .classed("node--source", false);
}

// Lazily construct the package hierarchy from class names.
function packageHierarchy(classes) {
    var map = {};

    function find(identifier, data) {
        var node = map[identifier], i;
        if (!node) {
            node = map[identifier] = data || { Identifier: identifier, children: [] };
            if (identifier.length) {
                node.parent = find(identifier.substring(0, i = identifier.lastIndexOf(".")));
                node.parent.children.push(node);
                node.key = identifier.substring(i + 1);
            }
        }
        return node;
    }

    classes.forEach(function (d) {
        find(d.Identifier, d);
    });

    return d3.hierarchy(map[""]);
}

// Return a list of imports for the given array of nodes.
function packageImports(nodes) {
    var map = {},
        imports = [];

    // Compute a map from name to node.
    nodes.forEach(function (d) {
        map[d.data.Identifier] = d;
    });

    // For each import, construct a link from the source to target node.
    nodes.forEach(function (d) {
        if (d.data.Dependencies) d.data.Dependencies.forEach(function (i) {
            if (i.Name) {
                var x = map[d.data.Identifier].path(map[i.Name]);
                x.Changed = i.Change;
                imports.push(x);
            } else {
                imports.push(map[d.data.Identifier].path(map[i]));
            }
        });
    });

    return imports;
}