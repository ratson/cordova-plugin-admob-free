'use strict'

module.exports = {
  presets: [['@babel/preset-env', { targets: { node: 'current' } }]],
  plugins: ['@babel/plugin-proposal-class-properties'],
  env: {
    production: {
      plugins: [],
    },
    test: {
      plugins: [
        [
          'module-resolver',
          {
            alias: {
              admob: './www/admob',
              'cordova/exec': './tests/js/stub/cordova-exec',
            },
          },
        ],
      ],
    },
  },
}
