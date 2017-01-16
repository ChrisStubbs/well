﻿var path = require('path')
var webpack = require('webpack')
//var HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
    entry: ["./app/main.ts"],
    devtool: "source-map",
    output: {
        path: path.join(__dirname, 'Scripts/angular2/'),
        filename: "app.js",
        sourceMapFilename: 'app.js.map'
    }, 
    debug: true,
    plugins: [
        new webpack.optimize.OccurrenceOrderPlugin()//,
        //new HtmlWebpackPlugin({
        //    template: './src/index.html'
        //})
    ],
    module: {
        preLoaders: [
            { test: /\.ts$/, exclude: path.resolve(__dirname, 'app/shared/primeng'), loader: 'tslint-loader' }
        ],
        loaders: [
            { test: /\.ts$/, exclude: /node_modules/, loader: "awesome-typescript-loader" }
        ]
    },
    resolve: {
            extensions: ['', '.ts', '.js']
        }
}