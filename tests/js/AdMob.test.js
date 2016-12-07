import _ from 'lodash'

import {AD_SIZE} from '../../src/js/AdMob'

test('AD_SIZE keys match values', () => {
  _.each(AD_SIZE, (v, k) => {
    expect(k).toBe(v)
  })
})
