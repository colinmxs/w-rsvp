export const authenticationService = {
  login,
  logout,
  get currentUser () { return JSON.parse(localStorage.getItem('currentUser')) }
}

function login (googleUser) {
  const profile = googleUser.getBasicProfile() // etc etc
  const idToken = googleUser.getAuthResponse().id_token
  localStorage.setItem('currentUser', JSON.stringify(profile))
  localStorage.setItem('id_token', JSON.stringify(idToken))
}

function logout () {
  // remove user from local storage to log user out
  localStorage.removeItem('currentUser')
  localStorage.removeItem('id_token')
}
