import babel from 'rollup-plugin-babel'
import nodeResolve from 'rollup-plugin-node-resolve'

export default {
  entry: 'src/js/admob.js',
  external: [
    'cordova/exec',
  ],
  plugins: [
    babel({
      exclude: 'node_modules/**',
      externalHelpers: false,
      externalHelpersWhitelist: [
        'classCallCheck',
        'createClass',
        'extends',
        'instanceof',
        'typeof',
      ],
      babelrc: false,
      presets: [
        ['env', {
          targets: {
            browsers: [
              '> 1%',
              'last 4 versions',
              'Android > 2',
              'last 2 ChromeAndroid versions',
            ],
          },
          modules: false,
        }],
        'stage-2',
      ],
      plugins: [
        'add-module-exports',
        'external-helpers',
        'transform-es3-member-expression-literals',
        'transform-es3-property-literals',
        'transform-object-assign',
        ['module-resolver', {
          alias: {
            lodash: 'lodash-es',
          },
        }],
      ],
    }),
    nodeResolve({
      jsnext: true,
      browser: true,
    }),
  ],
  format: 'cjs',
  sourceMap: true,
}
