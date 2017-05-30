/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    fs = require("fs"),
    less = require("gulp-less"),
    sass = require("gulp-sass"),
    minify = require('gulp-minify'),
    cssmin = require('gulp-cssmin'),
    rename = require('gulp-rename'),
    gutil = require('gulp-util');

gulp.task("less", function () {
    return gulp.src(["!wwwroot/bower-libs/**", "wwwroot/**/*.less"], { base: "./" })
        .pipe(less())
        .pipe(gulp.dest("."));
});

gulp.task("sass", function () {
    return gulp.src(["!wwwroot/bower-libs/**", "wwwroot/**/*.scss"], { base: "./" })
        .pipe(sass())
        .pipe(gulp.dest("."));
});

gulp.task("minify-js", function () {
    return gulp.src(["!wwwroot/bower-libs/**", "wwwroot/**/*.js", "!wwwroot/**/*.min.js"], { base: "./" })
        .pipe(minify({
                ext: {
                    min: '.min.js'
                }
            })
            .on('error', gutil.log))
        .pipe(gulp.dest("."));
});

gulp.task("minify-css", function () {
    return gulp.src(['!wwwroot/bower-libs/**', 'wwwroot/**/*.css', '!wwwroot/**/*.min.css'], { base: "./" })
        .pipe(cssmin())
        .pipe(rename({ suffix: ".min" }))
        .pipe(gulp.dest("."));
});