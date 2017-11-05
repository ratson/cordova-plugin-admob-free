import babel from 'rollup-plugin-babel'
import nodeResolve from 'rollup-plugin-node-resolve'

import { browserslist } from './package.json'

export default {
  input: 'src/js/admob.js',
  output: {
    format: 'cjs',
  },
  external: ['cordova/exec'],
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
        [
          'env',
          {
            targets: {
              browsers: browserslist,
            },
            modules: false,
          },
        ],
        'stage-2',
      ],
      plugins: [
        'add-module-exports',
        'external-helpers',
        'transform-es3-member-expression-literals',
        'transform-es3-property-literals',
        'transform-object-assign',
        [
          'module-resolver',
          {
            alias: {
              lodash: 'lodash-es',
            },
          },
        ],
      ],
    }),
    nodeResolve({
      jsnext: true,
      browser: true,
    }),
  ],
  sourcemap: true,
}
