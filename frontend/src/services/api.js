import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5254'  // Backend adresi
});

export const getLogs = () => api.get('/logs');
export const createLog = (log) => api.post('/logs', log);

export default api;
