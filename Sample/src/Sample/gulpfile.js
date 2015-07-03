/// <binding BeforeBuild='typescript' Clean='clean' />

var gulp = require("gulp");
var rimraf = require("rimraf");
var fs = require("fs");
var ts = require("gulp-typescript");

eval("var project = " + fs.readFileSync("./project.json"));

var paths = {
  lib: "./" + project.webroot + "/lib/"
};

gulp.task("clean", function (cb) {
    rimraf("./" + project.webroot + "/scripts/", cb);
});

gulp.task("typescript", function (cb) {
    var result = gulp.src('Scripts/**/*.ts')
        .pipe(ts({
            module: "amd"
        }));

    return result.js.pipe(gulp.dest('wwwroot/scripts'));
});