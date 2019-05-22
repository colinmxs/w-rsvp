<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png">
    <RSVPForm msg=""/>
  </div>
</template>

<script>
import RSVPForm from './components/RSVPForm.vue'

export default {
  name: 'app',
  components: {
    RSVPForm
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
