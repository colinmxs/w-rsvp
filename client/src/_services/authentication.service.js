import axios from 'axios'

export const authenticationService = {
  login,
  logout,
  get currentUser () { return JSON.parse(localStorage.getItem('currentUser')) }
}

async function login (googleUser) {
  const profile = googleUser.getBasicProfile()
  const idToken = googleUser.getAuthResponse().id_token
  let id = '';
  
  const response = await axios.post('http://localhost:56215/api/account', {
      idToken: idToken
  })
  profile.altId = response.data.altId
  id = response.data.altId
  
  
  localStorage.setItem('currentUser', JSON.stringify(profile))
  localStorage.setItem('id_token', JSON.stringify(idToken))
  return id;
}

function logout () {
  // remove user from local storage to log user out
  localStorage.removeItem('currentUser')
  localStorage.removeItem('id_token')
}
