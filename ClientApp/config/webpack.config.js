const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const merge = require("webpack-merge");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");

const { environment } = require("./utils/constant");
const entry = require("./utils/entry");
const path = require('path');

module.exports = env => {
  const mode = environment[env.mode] || environment.development;
  const isDevBuild = !(mode === environment.production);
  const outputPath = env.output || '../../Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI/wwwroot';
  const sharedConfig = () => ({
    mode,
    stats: { modules: false },
    resolve: {
      extensions: [".js", ".jsx"]
    },
    output: {
      path: path.resolve(__dirname, outputPath),
      filename: "[name].js",
      chunkFilename: 'js/[name].[contenthash].chunk.js'
    },
    optimization: {
      splitChunks: {
        cacheGroups: {
          default: false,
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
          'NODE_ENV': JSON.stringify(mode)
      }}),
      new FixStyleOnlyEntriesPlugin(),
      new MiniCssExtractPlugin({
        filename: "[name].css",
        chunkFilename: 'css/[name].css',
      }),
      new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
      new CleanWebpackPlugin({ cleanOnceBeforeBuildPatterns: ["js/**/*.*", "css/**/*.*"] })
    ].concat(isDevBuild ? [] : [new OptimizeCSSAssetsPlugin()])
    .concat(env.disableAnalyzer ? [] : [new BundleAnalyzerPlugin()]),
    devtool: isDevBuild ? "source-map" : undefined,
    watch: isDevBuild
  });

  return [clientBundleConfig];
};
