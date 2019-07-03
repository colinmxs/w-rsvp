<template>
  <div>{{userId}}</div>
</template>

<script>
import { apiService } from '../_services/api.service' 
  export default {
    name: 'user',
    data() {
      return {
        userId: "unknown"
      }
    },
    async created () {
      console.log('User component created')
      await this.getUserData(this.$route.params.userId)
    },
    async beforeRouteUpdate (to, from, next) {
      await this.getUserData(to.params.userId)
      next()
    },
    methods: {
      async getUserData (userId) {
        console.log('getUserData(' + userId + ')')
        const userData = await apiService.getUserData(userId, )
        console.log(userData)
        this.userId = userId
      }
    }
  }
</script>

<style scoped>

</style>
