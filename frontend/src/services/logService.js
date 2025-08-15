import axios from 'axios';

const API_URL = 'http://localhost:5000/logs';

export const fetchLogs = async () => {
    try {
        const response = await axios.get(API_URL);
        return response.data;
    } catch (error) {
        console.error('Logları çekerken hata oluştu:', error);
        return [];
    }
};
