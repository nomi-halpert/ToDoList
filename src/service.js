import axios from 'axios';
// import axiosConfig
const apiUrl = "http://localhost:5139"
axios.defaults.baseURL = apiUrl;
axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';

axios.interceptors.request.use(request => {
  console.log(request);
  // Edit request config
  return request;
}, error => {
  console.log(error);
  return Promise.reject(error);
});

axios.interceptors.response.use(response => {
  console.log(response);
  // Edit response config
  return response;
}, error => {
  console.log(error);
  return Promise.reject(error);
});

export default {
  getTasks: async () => {
    const result = await axios.get(`/items`)
    return result.data;
  },

  addTask: async (name) => {
    const result = await axios.post(`/items/?name=${name}` )
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    isComplete=isComplete?0:1;
    const result =  await axios.put(`/items/${id}?IsComplete=${isComplete}`);
    return result.data;
  },

  deleteTask: async (id) => {
    return await axios.delete(`/items/${id}`)
  }
};
