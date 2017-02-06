﻿var path = require('path');
var webpack = require('webpack');
var BitBarWebpackProgressPlugin = require("bitbar-webpack-progress-plugin");
const ExtractTextPlugin = require('extract-text-webpack-plugin');

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
        new webpack.optimize.OccurrenceOrderPlugin(),
        new BitBarWebpackProgressPlugin(), 
        new ExtractTextPlugin('site.css'),
    ],
    module: {
        preLoaders: [
            { test: /\.ts$/, exclude: path.resolve(__dirname, 'app/shared/primeng'), loader: 'tslint-loader' }
        ],
        loaders: [
            { test: /\.ts$/, exclude: /node_modules/, loader: "awesome-typescript-loader" }, 
            { test: /\.(jpe?g|png|gif|svg)$/i, loaders: ['file?hash=sha512&digest=hex&name=[hash].[ext]', 'image-webpack?bypassOnDebug&optimizationLevel=7&interlaced=false']},
            //{ test: /\.(png|gif|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' },
            { test: /\.(css|less)$/, exclude: /node_modules/, loader:  ExtractTextPlugin.extract('css?sourceMap!less?sourceMap') },          
        ],
        rules: [{
                test: /\.less$/,
                use: ExtractTextPlugin.extract( {
                    fallbackLoader: 'style-loader',
                    loaders: [
                        { loader: 'css-loader', options: { sourceMap: true, importLoaders: 1 }},
                        { loader: 'less-loader', options: { sourceMap: true } }
                    ]
                })
            }
        ]
    },
    resolve: {
        extensions: ['', '.ts', '.js']
    }
};