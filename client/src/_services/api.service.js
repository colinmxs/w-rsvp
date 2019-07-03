import axios from 'axios'

export const apiService = {
    getUserData
}

async function getUserData (userId) {
    const idToken = localStorage.getItem('id_token')
    const response = await axios.post('http://localhost:56215/api/user/' + userId, {
        idToken: idToken
    })
    return response
}