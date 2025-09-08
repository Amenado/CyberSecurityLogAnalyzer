import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5254/api' // Backend'in genel API adresi
});

// Logları çeken endpoint
export const getLogs = () => api.get('/LiveLogs/logs');

// Log ekleyen endpoint
export const createLog = (log) => api.post('/LiveLogs/create', log);

// Gelişmiş filtreleme için yeni endpoint
export const searchLogs = (filters) => {
    return api.get('/LiveLogs/search', { params: filters });
};

export default api;