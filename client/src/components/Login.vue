<template>
  <div id="login">    
    <g-signin-button :params="googleSignInParams"
                     @success="onSignInSuccess"
                     @error="onSignInError">
      Sign in with Google
    </g-signin-button>
  </div>
</template>

<script>
  import GoogleSignInButton from './GoogleSignInButton'
  import { authenticationService } from '../_services/authentication.service'
export default {
  data () {
    return {
      /**
       * The Auth2 parameters, as seen on
       * https://developers.google.com/identity/sign-in/web/reference#gapiauth2initparams.
       * As the very least, a valid client_id must present.
       * @type {Object}
       */
      googleSignInParams: {
        client_id: '1056158958261-u9o1s4i9srb3tr6ksqk1jk3o17h358pc.apps.googleusercontent.com'
      }
    }
  },
  components: {
    'g-signin-button' : GoogleSignInButton
  },
  methods: {
    async onSignInSuccess (googleUser) {
      let userId = await authenticationService.login(googleUser)
      this.$router.push({ name: 'user', params: { userId: userId } })
    },
    onSignInError (error) {
      // `error` contains any error occurred.
      console.log('OH NOES', error)
    }
  }
}
</script>

<style>

</style>
