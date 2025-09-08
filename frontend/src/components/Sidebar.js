import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import './Sidebar.css';

const Sidebar = () => {
  const location = useLocation();

  return (
    <aside className="sidebar">
      <div className="logo">Siber Güvenlik</div>
      <nav className="nav-menu">
        <Link to="/dashboard" className={location.pathname === "/dashboard" ? "active" : ""}>Dashboard</Link>
        <Link to="/live-logs" className={location.pathname === "/live-logs" ? "active" : ""}>Canlı Loglar</Link>
        <Link to="/search-logs" className={location.pathname === "/search-logs" ? "active" : ""}>Log Arama</Link>
        <Link to="/reports" className={location.pathname === "/reports" ? "active" : ""}>Raporlar</Link>
        <Link to="/settings" className={location.pathname === "/settings" ? "active" : ""}>Ayarlar</Link>
      </nav>
    </aside>
  );
};

export default Sidebar;