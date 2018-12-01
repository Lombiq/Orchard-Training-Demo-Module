const gulp = require("gulp");

const imageFiles = "Assets/Images/**/*";
const imageFilesDestination = "wwwroot/Images";

const pickrFiles = "node_modules/pickr-widget/dist/*";
const pickrFilesDestination = "wwwroot/pickr";

gulp.task("images", function () {
    return gulp
        .src(imageFiles)
        .pipe(gulp.dest(imageFilesDestination));
});

gulp.task("pickr", function () {
    return gulp
        .src(pickrFiles)
        .pipe(gulp.dest(pickrFilesDestination));
});

gulp.task("default", ["images", "pickr"]);