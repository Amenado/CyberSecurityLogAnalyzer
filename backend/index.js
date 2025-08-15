const express = require('express');
const cors = require('cors');

const app = express();
const port = 5000; // Backend portu

app.use(cors());
app.use(express.json());

// Örnek log verileri
let logs = [
    {
        id: 1,
        timestamp: new Date(),
        sourceIP: '192.168.1.2',
        destinationIP: '10.0.0.5',
        action: 'Login',
        status: 'Success',
        riskScore: 0.2,
        server: 'DemoServer1'
    },
    {
        id: 2,
        timestamp: new Date(),
        sourceIP: '192.168.1.3',
        destinationIP: '10.0.0.8',
        action: 'FileUpload',
        status: 'Failed',
        riskScore: 0.7,
        server: 'DemoServer2'
    }
];

// /logs endpoint'i
app.get('/logs', (req, res) => {
    res.json(logs);
});

app.listen(port, () => {
    console.log(`Backend çalışıyor! Port: ${port}`);
});
