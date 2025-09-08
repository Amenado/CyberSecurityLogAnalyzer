import React, { useEffect, useState } from 'react';
import Sidebar from './Sidebar';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChartLine, faExclamationTriangle, faListAlt } from '@fortawesome/free-solid-svg-icons';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, BarChart, Bar } from 'recharts';
import './Dashboard.css';

const API_URL = "http://localhost:5254/api/LiveLogs/trends?days=7";

const Dashboard = () => {
    const [totalLogs, setTotalLogs] = useState(0);
    const [abnormalLogs, setAbnormalLogs] = useState(0);
    const [criticalAlerts, setCriticalAlerts] = useState([]);
    const [trendData, setTrendData] = useState([]);
    const [ipData, setIpData] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                const response = await fetch(API_URL);
                if (!response.ok) throw new Error("Network response was not ok");
                const logs = await response.json();

                const total = logs.length;
                const abnormal = logs.filter(log => log.riskScore > 0.5).length;
                
                const alerts = logs
                    .filter(log => log.riskScore > 0.7)
                    .sort((a, b) => new Date(b.timestamp) - new Date(a.timestamp))
                    .slice(0, 5);

                setTotalLogs(total);
                setAbnormalLogs(abnormal);
                setCriticalAlerts(alerts);
                
                // Anormal Log Trendi için veriyi hazırla (günlük bazda)
                const logTrend = logs.reduce((acc, log) => {
                    const date = new Date(log.timestamp).toLocaleDateString('tr-TR', { year: 'numeric', month: 'numeric', day: 'numeric' });
                    const entry = acc.find(item => item.date === date);

                    if (entry) {
                        if (log.riskScore > 0.5) {
                            entry.abnormalCount += 1;
                        }
                    } else {
                        acc.push({ 
                            date, 
                            abnormalCount: log.riskScore > 0.5 ? 1 : 0 
                        });
                    }
                    return acc;
                }, []).sort((a, b) => new Date(a.date) - new Date(b.date));
                setTrendData(logTrend);
                
                // En Çok Saldırı Alan IP'ler için veriyi hazırla
                const ipCounts = logs
                    .filter(log => log.riskScore > 0.7)
                    .reduce((acc, log) => {
                        acc[log.sourceIP] = (acc[log.sourceIP] || 0) + 1;
                        return acc;
                    }, {});
                
                const sortedIps = Object.entries(ipCounts)
                    .sort(([, a], [, b]) => b - a)
                    .slice(0, 5)
                    .map(([ip, count]) => ({ ip, count }));
                setIpData(sortedIps);

            } catch (error) {
                console.error("Error fetching dashboard data:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
        // Artık canlı akış simülasyonu yerine, daha uzun vadeli trend analizi yaptığımız için
        // 5 saniyelik setInterval döngüsüne ihtiyacımız yok.
        // const interval = setInterval(fetchData, 5000);
        // return () => clearInterval(interval);
    }, []);

    return (
        <div className="dashboard-container">
            <Sidebar />
            <div className="main-content">
                <header className="header">
                    <h1>Dashboard</h1>
                </header>
                
                {loading ? (
                    <p className="loading-text">Veriler yükleniyor...</p>
                ) : (
                    <>
                        {/* 1. Özet Kartları */}
                        <div className="summary-cards">
                            <div className="card">
                                <FontAwesomeIcon icon={faListAlt} className="card-icon" />
                                <div className="card-info">
                                    <h3>Toplam Log Sayısı</h3>
                                    <span>{totalLogs}</span>
                                </div>
                            </div>
                            <div className="card">
                                <FontAwesomeIcon icon={faExclamationTriangle} className="card-icon warning" />
                                <div className="card-info">
                                    <h3>Anormal Aktivite</h3>
                                    <span>{abnormalLogs}</span>
                                </div>
                            </div>
                            <div className="card">
                                <FontAwesomeIcon icon={faChartLine} className="card-icon" />
                                <div className="card-info">
                                    <h3>Ortalama Risk Skoru</h3>
                                    <span>{(abnormalLogs / totalLogs * 100).toFixed(2)}%</span>
                                </div>
                            </div>
                        </div>

                        {/* 2. Kritik Uyarılar */}
                        <div className="critical-alerts section">
                            <h2>Kritik Uyarılar</h2>
                            {criticalAlerts.length > 0 ? (
                                <table>
                                    <thead>
                                        <tr>
                                            <th>Zaman</th>
                                            <th>Kaynak IP</th>
                                            <th>Hedef IP</th>
                                            <th>Risk Skoru</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {criticalAlerts.map((alert, index) => (
                                            <tr key={index}>
                                                <td>{new Date(alert.timestamp).toLocaleTimeString()}</td>
                                                <td>{alert.sourceIP}</td>
                                                <td>{alert.destinationIP}</td>
                                                <td>{alert.riskScore.toFixed(7)}</td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            ) : (
                                <p>Belirlenen aralıkta kritik uyarı bulunamadı.</p>
                            )}
                        </div>

                        {/* 3. Grafik Alanları */}
                        <div className="graphs-section section">
                            <h2>Veri Analiz Grafikleri</h2>
                            <div className="graph-container">
                                <div className="graph-box">
                                    <h3>Anormal Log Trendi</h3>
                                    <ResponsiveContainer width="100%" height={300}>
                                        <LineChart data={trendData}>
                                            <CartesianGrid strokeDasharray="3 3" />
                                            <XAxis dataKey="date" />
                                            <YAxis />
                                            <Tooltip />
                                            <Legend />
                                            <Line type="monotone" dataKey="abnormalCount" stroke="#8884d8" name="Anormal Aktivite" />
                                        </LineChart>
                                    </ResponsiveContainer>
                                </div>

                                <div className="graph-box">
                                    <h3>En Çok Saldırı Alan IP'ler</h3>
                                    <ResponsiveContainer width="100%" height={300}>
                                        <BarChart data={ipData}>
                                            <CartesianGrid strokeDasharray="3 3" />
                                            <XAxis dataKey="ip" />
                                            <YAxis />
                                            <Tooltip />
                                            <Legend />
                                            <Bar dataKey="count" fill="#82ca9d" name="Saldırı Sayısı" />
                                        </BarChart>
                                    </ResponsiveContainer>
                                </div>
                            </div>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
};

export default Dashboard;