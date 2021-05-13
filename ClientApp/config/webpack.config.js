const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const merge = require("webpack-merge");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");

const { environment, projectPath } = require("./utils/constant");
const entry = require("./utils/entry");
const path = require('path');

module.exports = env => {
  env = environment[env] || environment.development;
  const isDevBuild = !(env === environment.production);
  const sharedConfig = () => ({
    mode: env,
    stats: { modules: false },
    resolve: {
      extensions: [".js", ".jsx"]
    },
    output: {
      path: path.resolve(__dirname,'../../Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI/wwwroot'),
      filename: "[name].js",
      chunkFilename: 'js/[name].[contenthash].chunk.js'
    },
    optimization: {
      splitChunks: {
        cacheGroups: {
          vendor: {
            name: 'vendor',
            test: /[\\/]node_modules[\\/]/,
            chunks: 'all'
           }
        }
      }
    },
    module: {
      rules: [{
        test: /\.(js|jsx)$/,
        use: "babel-loader",
        exclude: /(node_modules|bower_components)/
      },
      {
        test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|eot|ttf)$/,
        use: [{
          loader: "url-loader",
          options: {
            limit: 4096,
            name: "[name].[ext]",
            outputPath: "css/fonts/",
            publicPath: "fonts/"
          }
        }]
      }]
    },
    node: {
      fs: 'empty'
    }
  });

  const clientBundleConfig = merge(sharedConfig(), {
    entry: entry,
    devServer: isDevBuild ?  {
      contentBase: projectPath.rootPath + '/build',
      compress: true,
      hot: true,
      port: 8080
    } : undefined,
    module: {
      rules: [{
        test: /\.s?[ac]ss$/,
        use: [{
          loader: 'style-loader',
        },
        {
          loader: MiniCssExtractPlugin.loader,
        },
        {
          loader: 'css-loader',
        }, {
          loader: 'resolve-url-loader',
        }, {
          loader: 'sass-loader',
          options: {
            sourceMap: true,
            sourceMapContents: false
          }
        }]
      }]
    },
    plugins: [
      new webpack.DefinePlugin({
        'process.env': {
          'NODE_ENV': JSON.stringify(env)
      }}),
      new FixStyleOnlyEntriesPlugin(),
      new MiniCssExtractPlugin({
        filename: "[name].css",
        chunkFilename: 'css/[name].css',
      }),
      new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
      new BundleAnalyzerPlugin(),
      new CleanWebpackPlugin({ cleanOnceBeforeBuildPatterns: ["js/**/*.*", "css/**/*.*"] })
    ].concat(isDevBuild ? [] : [new OptimizeCSSAssetsPlugin()]),
    devtool: isDevBuild ? "source-map" : undefined
  });

  return [clientBundleConfig];
};
