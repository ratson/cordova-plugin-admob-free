import { translateOptions } from '../../src/js/utils'

describe('translateOptions', () => {
  it('translate forChild from boolean/null to string', () => {
    expect(translateOptions({ forChild: true })).toEqual({ forChild: 'yes' })
    expect(translateOptions({ forChild: false })).toEqual({ forChild: 'no' })
    expect(translateOptions({ forChild: null })).toEqual({ forChild: '' })
  })

  it('translate forFamily from boolean/null to string', () => {
    expect(translateOptions({ forFamily: true })).toEqual({ forFamily: 'yes' })
    expect(translateOptions({ forFamily: false })).toEqual({ forFamily: 'no' })
    expect(translateOptions({ forFamily: null })).toEqual({ forFamily: '' })
  })

  it('do not translate string', () => {
    expect(translateOptions({ forChild: 'yes' })).toEqual({ forChild: 'yes' })
    expect(translateOptions({ forChild: 'no' })).toEqual({ forChild: 'no' })
    expect(translateOptions({ forChild: '' })).toEqual({ forChild: '' })

    expect(translateOptions({ forFamily: 'yes' })).toEqual({ forFamily: 'yes' })
    expect(translateOptions({ forFamily: 'no' })).toEqual({ forFamily: 'no' })
    expect(translateOptions({ forFamily: '' })).toEqual({ forFamily: '' })
  })
})
