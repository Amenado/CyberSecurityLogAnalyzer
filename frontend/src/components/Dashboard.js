import React from 'react';
import './Dashboard.css';

function Dashboard() {
  return (
    <div className="dashboard-container">
      <h2>Dashboard</h2>
      <div className="summary-section">
        <div className="summary-card">Günlük Log Sayısı: 1245</div>
        <div className="summary-card">Anormal Aktivite Sayısı: 4</div>
        <div className="summary-card">Canlı Uyarılar: 1</div>
      </div>

      <div className="graphs-section">
        <h3>Anormal Girişlerin Trend Grafiği</h3>
        {/* Buraya bir grafik bileşeni eklenecek */}
        <div className="graph-placeholder">Grafik Alanı</div>
      </div>
      
      {/* İleride diğer bölümler buraya eklenecek */}
    </div>
  );
}

export default Dashboard;