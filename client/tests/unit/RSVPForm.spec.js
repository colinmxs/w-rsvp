import { shallowMount } from '@vue/test-utils'
import RSVPForm from '@/components/RSVPForm.vue'

describe('RSVPForm.vue', () => {
  it('renders props.msg when passed', () => {
    const msg = 'new message'
    const wrapper = shallowMount(RSVPForm, {
      propsData: { msg }
    })
    expect(wrapper.text()).toMatch(msg)
  })
})
