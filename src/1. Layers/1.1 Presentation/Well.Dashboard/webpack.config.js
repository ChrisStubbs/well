﻿﻿var path = require('path');
var webpack = require('webpack');
var BitBarWebpackProgressPlugin = require("bitbar-webpack-progress-plugin");
//const ExtractTextPlugin = require('extract-text-webpack-plugin');

//http://engineering.invisionapp.com/post/optimizing-webpack/

module.exports = {
    entry: ["./app/main.ts"],
    devtool: "cheap-module-eval-source-map",
    output: {
        path: path.join(__dirname, 'Scripts/angular2/'),
        filename: "app.js",
        sourceMapFilename: 'app.js.map'
    },
    debug: true,
    plugins: [
        new webpack.optimize.OccurrenceOrderPlugin(),
        new BitBarWebpackProgressPlugin(), 
        //new ExtractTextPlugin('site.css'),
    ],
    module: {
        preLoaders: [
            { test: /\.ts$/, exclude: path.resolve(__dirname, 'app/shared/primeng'), loader: 'tslint-loader' }
        ],
        loaders: [
            { test: /\.ts$/, exclude: /node_modules/, loader: "awesome-typescript-loader" },
            // { test: /\.(jpe?g|png|gif|svg)$/i, loaders: ['file?hash=sha512&digest=hex&name=[hash].[ext]', 'image-webpack?bypassOnDebug&optimizationLevel=7&interlaced=false']},
            // { test: /\.(css|less)$/, exclude: /node_modules/, loader:  ExtractTextPlugin.extract('css?sourceMap!less?sourceMap') },          
        ],
        // rules: [{
        //         test: /\.less$/,
        //         use: ExtractTextPlugin.extract( {
        //             fallbackLoader: 'style-loader',
        //             loaders: [
        //                 { loader: 'css-loader', options: { sourceMap: true, importLoaders: 1 }},
        //                 { loader: 'less-loader', options: { sourceMap: true } }
        //             ]
        //         })
        //     }
        // ]
    },
    tslint: {
        failOnHint: true,
        configuration: require('./tslint.json')
    },
    resolve: {
        extensions: ['', '.ts', '.js']
    }
};