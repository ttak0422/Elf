const Path = require("path-browserify");
const Webpack = require("webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");

const Config = {
    htmlTemplate: "./index.html",
    fsEntry: "./App.fsproj",
    scssEntry: "./main.scss",
    outputDir: "./dist",
    assetsDir: "./public",
    devServerPort: 8080,
    devServerProxy: {
        "/socket/*": {
            target: "http://localhost:8085",
            ws: true        
        }
    },
    babelOptions: {}
}

const resolve = filePath =>
    Path.isAbsolute(filePath)
        ? filePath
        : Path.resolve(__dirname, filePath);

const isProduction =
    !process.argv.find(v => v.indexOf("serve") !== -1);

console.log(`resolve: ${resolve(".")}\n isProduction: ${isProduction}`);

const commonPlugins = [
    new HtmlWebpackPlugin({
        filename: "index.html",
        template: resolve(Config.htmlTemplate),
    }),
    new Webpack.ProvidePlugin({
        process: 'process/browser',
    }),
];

module.exports = {
    //resolve.
    // resolve: {
        //     fallback: { 
            //         path: Path,
            //     }
            //     //fallback: { "path": false }
            // },
            entry: isProduction
            ? {
                app: [
                    resolve(Config.fsEntry),
                    resolve(Config.scssEntry),
                ],
        }
        : {
            app: [
                resolve(Config.fsEntry)
            ],
            style: [
                resolve(Config.scssEntry),
            ]
        },
        output: {
            path: resolve(Config.outputDir),
            filename: isProduction ? "[name].[contenthash].js" : "[name].js",
        },
        mode: isProduction ? "production" : "development",
        devtool: isProduction ? false : "eval-source-map",
        plugins:
        isProduction
        ? commonPlugins.concat([
            new MiniCssExtractPlugin({
                filename: "style.css",
            }),
            new CopyWebpackPlugin({
                patterns: [{ from: resolve(Config.assetsDir) }],
            }),
        ])
        : commonPlugins.concat([
            new Webpack.HotModuleReplacementPlugin(),
        ]),
        optimization: {
            chunkIds: "named"
        },
        resolve: {
            // symlinks: false,
            fallback: { "path": require.resolve("path-browserify") },
        },
        devServer: {
            publicPath: "/",
            contentBase: resolve(Config.assetsDir),
            port: Config.devServerPort,
            proxy: Config.devServerProxy,
            hot: true,
            inline: true,
            contentBase: Config.outputDir,
            open: true
        },
        module: {
            rules: [
                {
                    test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        babel: Config.babelOptions,
                    }
                }
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: Config.babelOptions,
                },
            },
            {
                test: /\.(sass|scss|css)$/,
                use: [
                    isProduction ? MiniCssExtractPlugin.loader : 'style-loader',
                    'css-loader',
                    'sass-loader',
                ],
            },
        ]
    }
}