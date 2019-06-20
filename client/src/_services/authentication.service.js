import axios from 'axios'

export const authenticationService = {
  login,
  logout,
  get currentUser () { return JSON.parse(localStorage.getItem('currentUser')) }
}

async function login (googleUser) {
  const profile = googleUser.getBasicProfile()
  const idToken = googleUser.getAuthResponse().id_token

  try{
    const response = await axios.post('http://localhost:56215/api/account', {
        idToken: idToken
    })
    console.log(response);
  } catch (e) {
    //handle
  }

  localStorage.setItem('currentUser', JSON.stringify(profile))
  localStorage.setItem('id_token', JSON.stringify(idToken))
}

function logout () {
  // remove user from local storage to log user out
  localStorage.removeItem('currentUser')
  localStorage.removeItem('id_token')
}
