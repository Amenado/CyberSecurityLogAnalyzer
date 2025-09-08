import React from 'react';
import './Dashboard.css'; // Stil dosyasını dahil etmeyi unutmayın

const SummaryCards = () => {
  return (
    <div className="summary-section">
      <div className="summary-card">Günlük Log Sayısı: 1245</div>
      <div className="summary-card">Anormal Aktivite Sayısı: 4</div>
      <div className="summary-card">Canlı Uyarılar: 1</div>
    </div>
  );
};

export default SummaryCards; // Hatanın asıl kaynağı: Bileşeni varsayılan olarak dışa aktarın