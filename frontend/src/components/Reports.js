import React, { useState, useEffect } from 'react';
import Sidebar from './Sidebar';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import './Reports.css';

const Reports = () => {
    const [logs, setLogs] = useState([]);
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [loading, setLoading] = useState(false);
    const [trendData, setTrendData] = useState([]);

    const API_URL = "http://localhost:5254/api/LiveLogs/search";

    const fetchReports = async () => {
        setLoading(true);
        try {
            const response = await fetch(`${API_URL}?startDate=${startDate}&endDate=${endDate}`);
            
            if (!response.ok) {
                throw new Error(`API hatası: ${response.statusText}`);
            }

            const data = await response.json();
            setLogs(data);

            // Grafik verilerini hazırla
            const processedData = data.reduce((acc, log) => {
                const date = new Date(log.timestamp).toLocaleDateString('tr-TR', { year: 'numeric', month: 'numeric', day: 'numeric' });
                const existingEntry = acc.find(item => item.date === date);

                if (existingEntry) {
                    if (log.riskScore > 0.5) {
                        existingEntry.abnormalCount += 1;
                    }
                } else {
                    acc.push({
                        date,
                        abnormalCount: log.riskScore > 0.5 ? 1 : 0
                    });
                }
                return acc;
            }, []).sort((a, b) => new Date(a.date) - new Date(b.date));

            setTrendData(processedData);

        } catch (error) {
            console.error("Rapor verileri çekilirken hata oluştu:", error);
            setLogs([]);
            setTrendData([]);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        const today = new Date();
        const thirtyDaysAgo = new Date(today);
        thirtyDaysAgo.setDate(today.getDate() - 30);

        setStartDate(thirtyDaysAgo.toISOString().split('T')[0]);
        setEndDate(today.toISOString().split('T')[0]);
    }, []);

    const handleButtonClick = () => {
        if (startDate && endDate) {
            fetchReports();
        } else {
            alert("Lütfen başlangıç ve bitiş tarihlerini seçin.");
        }
    };

    const handleDownloadPdf = () => {
        const doc = new jsPDF();
        doc.text("Sistem Log Raporu", 14, 15);

        const tableColumn = ["Zaman", "Kaynak IP", "Hedef IP", "Eylem", "Durum", "Risk Skoru"];
        const tableRows = logs.map(log => [
            new Date(log.timestamp).toLocaleString(),
            log.sourceIP,
            log.destinationIP,
            log.action,
            log.status,
            log.riskScore.toFixed(7)
        ]);

        doc.autoTable({
            head: [tableColumn],
            body: tableRows,
            startY: 20
        });
        
        doc.save('log_raporu.pdf');
    };

    return (
        <div className="reports-container">
            <Sidebar />
            <div className="main-content">
                <header className="header">
                    <h1>Raporlar</h1>
                </header>
                
                <div className="report-controls">
                    <div className="date-filter">
                        <label>Başlangıç Tarihi: </label>
                        <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
                        <label>Bitiş Tarihi: </label>
                        <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
                        <button onClick={handleButtonClick} disabled={loading}>
                            {loading ? "Rapor Yükleniyor..." : "Raporu Getir"}
                        </button>
                    </div>
                    <button onClick={handleDownloadPdf} className="download-btn" disabled={logs.length === 0}>
                        Raporu İndir (PDF)
                    </button>
                </div>

                <div className="graphs-section section">
                    <h2>Anormal Aktivite Trendi</h2>
                    <div className="graph-placeholder">
                        <ResponsiveContainer width="100%" height={300}>
                            <LineChart data={trendData}>
                                <CartesianGrid strokeDasharray="3 3" />
                                <XAxis dataKey="date" />
                                <YAxis allowDecimals={false} />
                                <Tooltip />
                                <Legend />
                                <Line type="monotone" dataKey="abnormalCount" stroke="#8884d8" activeDot={{ r: 8 }} name="Anormal Aktivite Sayısı" />
                            </LineChart>
                        </ResponsiveContainer>
                    </div>
                </div>

                <div className="log-table-section section">
                    <h2>Detaylı Log Tablosu</h2>
                    {loading ? (
                        <p className="loading-text">Veriler yükleniyor...</p>
                    ) : logs.length > 0 ? (
                        <table>
                            <thead>
                                <tr>
                                    <th>Zaman Damgası</th>
                                    <th>Kaynak IP</th>
                                    <th>Hedef IP</th>
                                    <th>Eylem</th>
                                    <th>Durum</th>
                                    <th>Risk Skoru</th>
                                </tr>
                            </thead>
                            <tbody>
                                {logs.map((log, index) => (
                                    <tr key={index}>
                                        <td>{new Date(log.timestamp).toLocaleString()}</td>
                                        <td>{log.sourceIP}</td>
                                        <td>{log.destinationIP}</td>
                                        <td>{log.action}</td>
                                        <td>{log.status}</td>
                                        <td>{log.riskScore.toFixed(7)}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    ) : (
                        <p className="no-results-text">Seçtiğiniz tarihler arasında kayıt bulunamadı.</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Reports;