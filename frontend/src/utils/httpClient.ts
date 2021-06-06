import { environment } from '../environments/environment'

const headers = {
  'Content-Type': 'application/json'
}

export const httpClient = {

  async get(url: string) {
    try {
      const resp = await fetch(`${environment.apiPath}${url}`, { method: 'GET', headers })
      return resp.json();
    } catch (err) {
      console.error(err);
    }
  },

  async post<Type>(url: string, body: Type) {
    try {
      const reqBody = JSON.stringify(body)
      return fetch(`${environment.apiPath}${url}`, {
        method: 'POST',
        headers,
        body: reqBody
      })
    } catch (err) {
      console.error(err);
    }
  },

  async delete(url: string) {
    try {
      return fetch(`${environment.apiPath}${url}`, { method: 'DELETE', headers })
    } catch (err) {
      console.error(err);
    }
  }

}
