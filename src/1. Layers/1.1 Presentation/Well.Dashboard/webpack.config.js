/// <binding AfterBuild='Run - Development' />
require('ts-loader');
var webpack = require("webpack");

module.exports = {
    entry: {
        vendor: "./app/shared/vendor.ts",
        app: ["./app/main.ts"]
    },
    devtool: "source-map",
    output: {
        path: "./Scripts/angular2/"
        ,filename: "[name]Bundle.js"
    },
    plugins: [
        //new webpack.optimize.UglifyJsPlugin({minimize: true})
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