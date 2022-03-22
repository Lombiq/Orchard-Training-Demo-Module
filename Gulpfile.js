// There are multiple ways of managing your resources (including scripts, stylesheets, images etc.). Orchard Core
// itself provides its own pipeline for it using an Assets.json file. We don't use that since this is a standalone
// module only using Orchard from NuGet.

// Here you will see a standalone Gulpfile for copying third-party resources from the node_modules folder to wwwroot
// folder and also compiling our own resources (styles and scripts) and moving the results to the wwwroot folder as
// well. Note the .placeholder file in the wwwroot folder: that's a workaround that makes it possible to serve newly
// compiled static files during the first app start too (the issue is elaborated a bit in the text in the file itself).

// Note that this utilizes Lombiq's NPM MSBuild Targets (https://github.com/Lombiq/NPM-Targets) and Gulp Extensions
// (https://github.com/Lombiq/Gulp-Extensions) projects to hook Gulp build up with the .NET build pipeline, as well as
// to provide SCSS and JS linting. Check out their documentations for more details!

const gulp = require('gulp');
// Gulp watcher if needed when we are actively developing a resource.
const watch = require('gulp-watch');

// Importing an SCSS-compiling Gulp task from the Gulp Extensions project.
const scssTargets = require('../../Utilities/Lombiq.Gulp.Extensions/Tasks/scss-targets');

// It's handy to define all the paths beforehand.
const assetsBasePath = './Assets/';
const distBasePath = './wwwroot/';
const stylesBasePath = assetsBasePath + 'Styles/';
const stylesDistBasePath = distBasePath + 'css/';
const imagesBasePath = assetsBasePath + 'Images/';
const imagesDistBasePath = distBasePath + 'images/';
const pickrBasePath = './node_modules/pickr-widget/dist/';
const pickrDistBasePath = distBasePath + 'pickr/';

gulp.task('build:styles', scssTargets.build(stylesBasePath, stylesDistBasePath));

// This task will collect all the images and move them to the wwwroot folder.
gulp.task('copy:images', () => gulp
    .src(imagesBasePath + '**/*')
    .pipe(gulp.dest(imagesDistBasePath)));

// Task specifically created for our third-party plugin, pickr. It will just copy the files to the wwwroot folder.
gulp.task('pickr', () => gulp
    .src(pickrBasePath + '*')
    .pipe(gulp.dest(pickrDistBasePath)));

// Default task that executes all the required tasks to initialize the module assets.
gulp.task('default', gulp.parallel('build:styles', 'copy:images', 'pickr'));

// This task won't be executed automatically, if you want to test this, you need to execute it in the Task Runner
// Explorer. With this you'll be able to automatically compile and minify the sass files right after when you save them.
gulp.task('watch', () => {
    watch(stylesBasePath + '**/*.scss', { verbose: true }, gulp.series('build:styles'));
});

// Note that while we demonstrated a Gulp build that takes care of a lot of things here, if you just want to build SCSS
// and/or JS, check out the shortcuts documented here:
// https://github.com/Lombiq/Gulp-Extensions#tips-for-using-and-naming-multiple-gulp-tasks.

// END OF TRAINING SECTION: Compiling resources using Gulp

// NEXT STATION: GraphQL/Startup.cs
