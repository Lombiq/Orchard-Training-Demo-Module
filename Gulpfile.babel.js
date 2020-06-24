'use strict';

// There are multiple ways of managing your resources (including scripts, stylesheets, images etc.). Orchard Core
// itself provides its own pipeline for it using an Assets.json file. We don't use it this time but check the
// http://docs.orchardproject.net/en/latest/Documentation/Processing-client-side-assets/ documentation for more
// information.

// Here you will see a standalone Gulpfile for copying third-party resources from the node_modules folder to wwwroot
// folder and also compiling our own resources (styles and scripts) and moving the results to the wwwroot folder as
// well. Note the .placeholder file in the wwwroot folder: that's a workaround that makes it possible to serve newly
// compiled static files during the first app start too (the issue is elaborated a bit in the text in the file itself).

// You've might noticed that the Gulpfile filename has a babel suffix which means that you can use code syntax from ES6
// and above with the help of Babel. To configure this you need to install @babel/core, @babel/preset-env and
// @babel/register Node packages and add a Babel config file to the root, see: .babelrc file.

import gulp from 'gulp';
// Gulp plugin used for compiling sass files. The sass compiler needs to be set explicitly.
import sass from 'gulp-sass';
import nodeSass from 'node-sass';
sass.compiler = nodeSass;
// Minifies css files.
import cleanCss from 'gulp-clean-css';
// Renames the file so the result will have a different name (i.e. .min.css or .min.js).
import rename from 'gulp-rename';
// Cache the result so the task won't be fully executed if it is not necessary.
import cached from 'gulp-cached';
// Gulp watcher if needed when we are actively developing a resource.
import watch from 'gulp-watch';
// This is a helper for generating a gulp pipeline for harvesting Vue applications from the current
// project's Assets folder and compiling them to wwwroot.
import getVueAppCompilerPipeline from '../Lombiq.VueJs/Assets/Scripts/helpers/get-vue-app-compiler-pipeline';

const paths = {
    imageFiles: './Assets/Images/**/*',
    imageFilesDestination: './wwwroot/Images',

    pickrFiles: './node_modules/pickr-widget/dist/*',
    pickrFilesDestination: './wwwroot/Pickr',

    sassFiles: './Assets/Styles/**/*.scss',
    stylingFilesDestination: './wwwroot/Styles'
};

// This task will collect all the images and move it to the wwwroot folder.
gulp.task('images', () =>
    gulp
        .src(paths.imageFiles)
        .pipe(cached('images'))
        .pipe(gulp.dest(paths.imageFilesDestination)));

// Task specifically created for our third-party plugin, pickr. It will just copy the files to the wwwroot folder.
gulp.task('pickr', () => 
    gulp
        .src(paths.pickrFiles)
        .pipe(cached('pickr'))
        .pipe(gulp.dest(paths.pickrFilesDestination)));

// It will compile our sass files to css.
gulp.task('sass:compile', () => getSassCompilerPipeline());

// This gulp task is for harvesting and compiling Vue applications in the current project.
gulp.task('vue:compile', () => getVueAppCompilerPipeline());

// Default task that executes all the required tasks to initialize the module assets.
gulp.task('default', gulp.parallel('images', 'pickr', 'sass:compile', 'vue:compile'));

// This task won't be executed automatically, if you want to test this, you need to execute it in the Task Runner
// Explorer. With this you'll be able to automatically compile and minify the sass files right after when you save them.
gulp.task('sass:watch', () =>
    watch(
        paths.sassFiles,
        {
            verbose: true
        },
        () => getSassCompilerPipeline()));
 
// The actual pipeline is in a separate function so it can be used in the watch task as well.        
const getSassCompilerPipeline = () =>
    gulp.src(paths.sassFiles)
        .pipe(cached('scss'))
        .pipe(sass({ linefeed: 'crlf' })).on('error', sass.logError)
        .pipe(gulp.dest(paths.stylingFilesDestination))
        .pipe(cleanCss({ compatibility: 'ie8' }))
        .pipe(rename({ extname: '.min.css' }))
        .pipe(gulp.dest(paths.stylingFilesDestination));

// NEXT STATION: Lombiq.TrainingDemo.csproj and find the target with the 'NpmInstall' name and then come back.

// END OF TRAINING SECTION: Compiling resources using Gulp

// This is the end of the training. It is always hard to say goodbye so... don't do it. Let us know your thoughts about
// this module or Orchard Core itself on GitHub or send us an email at crew@lombiq.com instead. If you feel like you
// need some more training in developing Orchard Core web applications, don't hesitate to contact us!