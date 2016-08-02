/// <binding AfterBuild='Run - Development' />
require('ts-loader');
var webpack = require("webpack");

module.exports = {
    entry: {
        polyfills: "./app/shared/polyfills.ts",
        vendor: "./app/shared/vendor.ts",
        app: ["./app/main.ts"]
    },
    //debug: true,
    devtool: "cheap-module-eval-source-map",
    output: {
        path: "./Scripts/angular2/"
        ,filename: "[name]Bundle.js"
    },
    plugins: [
        //new webpack.optimize.UglifyJsPlugin({minimize: true}),
        //new webpack.optimize.CommonsChunkPlugin({name: ['app', 'vendor', 'polyfills']})
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