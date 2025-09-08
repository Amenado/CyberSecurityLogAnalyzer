import React, { useState, useEffect } from 'react';
import Sidebar from './Sidebar';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch, faFilter } from '@fortawesome/free-solid-svg-icons';
import './LogSearch.css';

const LogSearch = () => {
    const [logs, setLogs] = useState([]);
    const [searchFilters, setSearchFilters] = useState({
        startDate: '',
        endDate: '',
        sourceIP: '',
        destinationIP: '',
        riskScore: '',
        action: '',
    });
    const [loading, setLoading] = useState(false);

    const API_URL = "http://localhost:5254/api/LiveLogs/search";

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setSearchFilters(prevFilters => ({
            ...prevFilters,
            [name]: value,
        }));
    };

    const handleSearch = async () => {
        setLoading(true);
        try {
            const queryParams = new URLSearchParams(searchFilters).toString();
            const response = await fetch(`${API_URL}?${queryParams}`);
            
            if (!response.ok) {
                throw new Error(`API hatası: ${response.statusText}`);
            }

            const data = await response.json();
            setLogs(data);
        } catch (error) {
            console.error("Logları çekerken hata oluştu:", error);
            setLogs([]);
        } finally {
            setLoading(false);
        }
    };

    // YENİ VE GÜNCELLENMİŞ useEffect
    useEffect(() => {
        const today = new Date();
        const sevenDaysAgo = new Date(today);
        sevenDaysAgo.setDate(today.getDate() - 7);

        // Varsayılan tarih filtrelerini ayarla
        setSearchFilters(prevFilters => ({
            ...prevFilters,
            startDate: sevenDaysAgo.toISOString().split('T')[0],
            endDate: today.toISOString().split('T')[0]
        }));
    }, []);

    // YENİ useEffect: Filtreler her değiştiğinde arama yap
    useEffect(() => {
        // searchFilters state'i güncellendiğinde arama fonksiyonunu çağır
        // Bu, sayfa ilk yüklendiğinde varsayılan filtrelerle arama yapmayı sağlar
        if (searchFilters.startDate && searchFilters.endDate) {
            handleSearch();
        }
    }, [searchFilters]);

    return (
        <div className="log-search-container">
            <Sidebar />
            <div className="main-content">
                <header className="header">
                    <h1><FontAwesomeIcon icon={faSearch} /> Gelişmiş Log Arama</h1>
                </header>

                <div className="filter-form section">
                    <h2><FontAwesomeIcon icon={faFilter} /> Filtreleme Seçenekleri</h2>
                    <div className="filter-grid">
                        <div className="form-group">
                            <label htmlFor="startDate">Başlangıç Tarihi</label>
                            <input type="date" name="startDate" value={searchFilters.startDate} onChange={handleFilterChange} />
                        </div>
                        <div className="form-group">
                            <label htmlFor="endDate">Bitiş Tarihi</label>
                            <input type="date" name="endDate" value={searchFilters.endDate} onChange={handleFilterChange} />
                        </div>
                        <div className="form-group">
                            <label htmlFor="sourceIP">Kaynak IP</label>
                            <input type="text" name="sourceIP" value={searchFilters.sourceIP} onChange={handleFilterChange} placeholder="örn. 192.168.1.1" />
                        </div>
                        <div className="form-group">
                            <label htmlFor="destinationIP">Hedef IP</label>
                            <input type="text" name="destinationIP" value={searchFilters.destinationIP} onChange={handleFilterChange} placeholder="örn. 8.8.8.8" />
                        </div>
                        <div className="form-group">
                            <label htmlFor="riskScore">Risk Skoru (min)</label>
                            <input type="number" name="riskScore" value={searchFilters.riskScore} onChange={handleFilterChange} placeholder="örn. 0.8" step="0.1" />
                        </div>
                        <div className="form-group">
                            <label htmlFor="action">Eylem</label>
                            <input type="text" name="action" value={searchFilters.action} onChange={handleFilterChange} placeholder="örn. Denied" />
                        </div>
                    </div>
                    <button onClick={handleSearch} className="search-btn" disabled={loading}>
                        <FontAwesomeIcon icon={faSearch} /> {loading ? "Aranıyor..." : "Logları Ara"}
                    </button>
                </div>

                <div className="results-section section">
                    <h2>Arama Sonuçları ({logs.length} Kayıt)</h2>
                    {loading ? (
                        <p className="loading-text">Loglar yükleniyor...</p>
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
                        <p className="no-results-text">Aradığınız kriterlere uygun log kaydı bulunamadı.</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default LogSearch;