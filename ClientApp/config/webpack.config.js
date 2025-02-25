const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const { merge } = require("webpack-merge");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

const { environment } = require("./utils/constant");
const entry = require("./utils/entry");
const path = require("path");

module.exports = (env = {}) => {
  const mode = env.production ? environment.production : environment.development;
  const isDevBuild = !(mode === environment.production);
  const outputPath = env.output || "../../Fanex.Athena.AnalyticsAPI/Fanex.Athena.AnalyticsAPI/wwwroot";

  const sharedConfig = {
    mode,
    stats: { modules: false },
    resolve: {
      extensions: [".js", ".jsx", ".scss", ".sass"],
    },
    output: {
      path: path.resolve(__dirname, outputPath),
      filename: "js/[name].js",
      chunkFilename: "js/[name].[contenthash].chunk.js",
      assetModuleFilename: "assets/[name][ext]",
      publicPath: "/",
    },
    optimization: {
      splitChunks: {
        chunks: 'all',
        cacheGroups: {
          vendor: {
            name: "vendor",
            test: /[\\/]node_modules[\\/](?!.*\.css$)/,
            chunks: "all",
            priority: 1,
            reuseExistingChunk: true,
            enforce: true
          },
          vendorCss: {
            name: "vendor",
            test: /[\\/]node_modules[\\/].*\.css$/,
            chunks: "all",
            enforce: true,
            priority: 10
          },
        },
      },
      minimizer: isDevBuild ? [] : [new CssMinimizerPlugin()],
    },
    module: {
      rules: [
        {
          test: /\.(js|jsx)$/,
          use: "babel-loader",
          exclude: /node_modules/,
        },
        {
          test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|eot|ttf)$/,
          type: "asset/resource",
          generator: {
            filename: 'fonts/[name][ext]',
            publicPath: '/'
          }
        },
      ],
    },
  };

  const clientBundleConfig = merge(sharedConfig, {
    entry: entry,
    module: {
      rules: [
        {
          test: /\.s?[ac]ss$/,
          use: [
            {
              loader: MiniCssExtractPlugin.loader,
              options: {
                esModule: false
              }
            },
            {
              loader: "css-loader",
              options: {
                url: true,
                import: true,
                importLoaders: 2
              },
            },
            {
              loader: "resolve-url-loader",
              options: { sourceMap: true },
            },
            {
              loader: "sass-loader",
              options: {
                sourceMap: true,
                implementation: require("node-sass"),
                sassOptions: {
                  includePaths: [path.resolve(__dirname, "../src/styles")],
                },
              },
            },
          ],
        },
      ],
    },
    plugins: [
      new webpack.DefinePlugin({
        "process.env.NODE_ENV": JSON.stringify(mode),
      }),
      new MiniCssExtractPlugin({
        filename: "css/[name].css",
        chunkFilename: "css/[name].css",
        ignoreOrder: false
      }),
      new webpack.IgnorePlugin({ resourceRegExp: /^\.\/locale$/, contextRegExp: /moment$/ }),
      new CleanWebpackPlugin({
        cleanOnceBeforeBuildPatterns: ["js/**/*.*", "css/**/*.*", "fonts/**/*.*"],
      }),
    ],
    devtool: isDevBuild ? "source-map" : false,
    watch: isDevBuild,
  });

  return [clientBundleConfig];
};