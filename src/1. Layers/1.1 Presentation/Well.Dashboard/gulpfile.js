/// <binding BeforeBuild='default' ProjectOpened='watch' />
/// <vs BeforeBuild='default' />
var gulp = require('gulp');
var less = require('gulp-less');
var autoprefixer = require('gulp-autoprefixer');
var minifyCSS = require('gulp-minify-css');
var gutil = require('gulp-util');
var watch = require('gulp-watch');

var config = {
    //Include all js files but exclude any min.js files
    src: ['content/*.less', '!content/toastr.less', '!content/library.less'],
    angularSrc: [
        "node_modules/es6-shim/es6-shim.js",
        "node_modules/systemjs/dist/system-polyfills.js",
        "node_modules/angular2/bundles/angular2-polyfills.js",
        "node_modules/systemjs/dist/system.src.js",
        "node_modules/rxjs/bundles/rx.js",
        "node_modules/angular2/bundles/angular2.dev.js",
        "node_modules/angular2/bundles/router.dev.js",
        "node_modules/angular2/bundles/http.dev.js"
    ]
}

gulp.task('less',
    function() {
        return gulp.src(config.src)
            .pipe(less({ compress: true }).on('error', gutil.log))
            .pipe(gulp.dest('content/css'));
    });

gulp.task('angular2',
    function () {
        return gulp.src(config.angularSrc)
            .pipe(gulp.dest('Scripts/angular2'));
    });

gulp.task('watch',
    function() {
        gulp.watch(config.src, ['less']); // Watch all the .less files, then run the less task
    });

gulp.task('default', ['less', 'angular2'], function () { });