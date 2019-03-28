// There are multiple ways of managing your resources (including scripts, stylesheets, images etc.). Orchard Core
// itself provides its own pipeline for it using an Assets.json file. We don't use it this time but check the
// http://docs.orchardproject.net/en/latest/Documentation/Processing-client-side-assets/ documentation for more
// information.

// Here you will see a standalone Gulpfile for copying third-party resources from the node_modules folder to wwwroot
// folder and also compiling our own resources (styles and scripts) and moving the results to the wwwroot folder as
// well.

const gulp = require('gulp');
// Gulp plugin used for compiling sass files. The sass compiler needs to be set explicitely.
const sass = require('gulp-sass');
sass.compiler = require('node-sass');
// Minifies css files.
const cleanCss = require('gulp-clean-css');
// Renames the file so the result will have a different name (i.e. .min.css or .min.js).
const rename = require('gulp-rename');
// Cache the result so the task won't be fully executed if it is not necessary.
const cached = require('gulp-cached');
// Gulp watcher if needed when we are actively developing a resource.
const watch = require('gulp-watch');

const imageFiles = './Assets/Images/**/*';
const imageFilesDestination = './wwwroot/Images';

const pickrFiles = './node_modules/pickr-widget/dist/*';
const pickrFilesDestination = './wwwroot/Pickr';

const sassFiles = './Assets/Styles/**/*.scss';
const cssFiles = './wwwroot/Styles/**/*.css';
const stylingFilesDestination = './wwwroot/Styles';

// This task will collect all the images and move it to the wwwroot folder.
gulp.task('images', () =>
    gulp
        .src(imageFiles)
        .pipe(cached('images'))
        .pipe(gulp.dest(imageFilesDestination)));

// Task specifically created for our third-party plugin, pickr. It will just copy the files to the wwwroot folder.
gulp.task('pickr', () => 
    gulp
        .src(pickrFiles)
        .pipe(cached('pickr'))
        .pipe(gulp.dest(pickrFilesDestination)));

// It will compile our sass files to css.
gulp.task('sass:compile', () => sassCompilerPipelineFactory());

// Default task that executes all the required tasks to initialize the module assets.
gulp.task('default', gulp.parallel('images', 'pickr', 'sass:compile'));

// This task won't be executed automatically, if you want to test this, you need to execute it in the Task Runner
// Explorer. With this you'll be able to automatically compile and minify the sass files right after when you save them.
gulp.task('sass:watch', () =>
    watch(
        sassFiles,
        {
            verbose: true
        },
        () => sassCompilerPipelineFactory()));

// The actual pipeline is in a separate function so it can be used in the watch task as well.        
const sassCompilerPipelineFactory = () =>
    gulp.src(sassFiles)
        .pipe(cached('scss'))
        .pipe(sass({ linefeed: 'crlf' })).on('error', sass.logError)
        .pipe(gulp.dest(stylingFilesDestination))
        .pipe(cleanCss({ compatibility: 'ie8' }))
        .pipe(rename({ extname: '.min.css' }))
        .pipe(gulp.dest(stylingFilesDestination));

// NEXT STATION: Lombiq.TrainingDemo.csproj and find the target with the 'NpmInstall' name.