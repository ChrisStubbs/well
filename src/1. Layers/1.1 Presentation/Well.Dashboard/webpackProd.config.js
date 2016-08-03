﻿require('ts-loader');
var webpack = require("webpack");

module.exports = {
    entry: {
        polyfills: "./app/shared/polyfills.ts",
        vendor: "./app/shared/vendor.ts",
        app: ["./app/main.ts"]
    },
    output: {
        path: "./Scripts/angular2/"
        ,filename: "[name]Bundle.js"
    },
    plugins: [
        new webpack.optimize.UglifyJsPlugin({minimize: true, comments: false}),
        new webpack.optimize.DedupePlugin(),
        new webpack.optimize.CommonsChunkPlugin({name: ['app', 'vendor', 'polyfills']})
    ],
    module: {
    loaders: [
    {
        test: /\.ts$/,
        exclude: /node_modules/,
        loader: "ts-loader"
    }]
},
	
resolve: {
        extensions: ['', '.js', '.ts']
        }
}