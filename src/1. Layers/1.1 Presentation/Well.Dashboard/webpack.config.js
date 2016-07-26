require('ts-loader');
var webpack = require("webpack");

module.exports = {
    entry: ["./app/branch/main.ts", "./app/clean/main.ts"],
    devtool: "source-map",
    output: {
        path: "./Scripts/angular2",
        filename: "bundle.js"
    },
    plugins: [
        new webpack.optimize.UglifyJsPlugin({minimize: true})
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