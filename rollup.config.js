import babel from 'rollup-plugin-babel'
import nodeResolve from 'rollup-plugin-node-resolve'
import { uglify } from 'rollup-plugin-uglify'

import { browserslist } from './package.json'

export default {
  input: 'src/js/admob.js',
  output: {
    format: 'cjs',
    sourcemap: true,
  },
  external: ['cordova/exec'],
  plugins: [
    babel({
      exclude: 'node_modules/**',
      babelrc: false,
      presets: [
        [
          '@babel/preset-env',
          {
            targets: {
              browsers: browserslist,
            },
            modules: false,
          },
        ],
      ],
      plugins: [
        'add-module-exports',
        '@babel/plugin-syntax-class-properties',
        '@babel/plugin-syntax-object-rest-spread',
        '@babel/plugin-transform-member-expression-literals',
        '@babel/plugin-transform-object-assign',
        '@babel/plugin-transform-property-literals',
        '@babel/plugin-transform-spread',
        '@babel/plugin-proposal-class-properties',
        '@babel/plugin-proposal-object-rest-spread',
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
    uglify(),
  ],
}
