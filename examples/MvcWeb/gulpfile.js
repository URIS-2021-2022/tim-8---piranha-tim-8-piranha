var gulp = require('gulp'),
    sass = require('gulp-sass')
var cssmin = require("gulp-cssmin")
var rename = require("gulp-rename");

gulp.task('min:css', function () {
    return gulp.src('assets/scss/style.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(cssmin())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest('wwwroot/assets/css'));
    });
