const gulp = require("gulp");
const sass = require("gulp-sass");
const cssnano = require('gulp-cssnano');
const rename = require("gulp-rename");
const cached = require("gulp-cached");
const watch = require("gulp-watch");

const imageFiles = "./Assets/Images/**/*";
const imageFilesDestination = "./wwwroot/Images";

const pickrFiles = "./node_modules/pickr-widget/dist/*";
const pickrFilesDestination = "./wwwroot/Pickr";

const sassFiles = "./Assets/Styles/**/*.scss";
const cssFiles = "./wwwroot/Styles/**/*.css";
const stylingFilesDestination = "./wwwroot/Styles";

gulp.task("images", function () {
    return gulp
        .src(imageFiles)
        .pipe(cached("images"))
        .pipe(gulp.dest(imageFilesDestination));
});

gulp.task("pickr", function () {
    return gulp
        .src(pickrFiles)
        .pipe(cached("pickr"))
        .pipe(gulp.dest(pickrFilesDestination));
});

gulp.task("sass:compile", function (callback) {
    gulp.src(sassFiles)
        .pipe(cached("scss"))
        .pipe(sass({ linefeed: "crlf" })).on("error", sass.logError)
        .pipe(gulp.dest(stylingFilesDestination))
        .on("end", callback);
});

gulp.task("sass:minify", ["sass:compile"], function () {
    return gulp.src(cssFiles)
        .pipe(cached("css"))
        .pipe(cssnano())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest(stylingFilesDestination));
});

gulp.task("default", ["images", "pickr", "sass:minify"]);

gulp.task("sass:watch", function () {
    watch(sassFiles, function () {
        gulp.start("sass:minify");
    });
});