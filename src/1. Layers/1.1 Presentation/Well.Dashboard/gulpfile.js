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
    src: ['content/*.less', '!content/toastr.less', '!content/library.less']
}

gulp.task('less',
    function() {
        return gulp.src(config.src)
            .pipe(less({ compress: true }).on('error', gutil.log))
            .pipe(gulp.dest('content/css'));
    });

gulp.task('watch',
    function() {
        gulp.watch(config.src, ['less']); // Watch all the .less files, then run the less task
    });

gulp.task('default', ['less'], function () { });