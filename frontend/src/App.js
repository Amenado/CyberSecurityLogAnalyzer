import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Dashboard from './components/Dashboard';
import LiveLogs from './components/LiveLogs';
import LogSearch from './components/LogSearch';
import Reports from './components/Reports';
import Settings from './components/Settings';
import './App.css';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  const handleLogin = () => {
    setIsAuthenticated(true);
  };

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login onLogin={handleLogin} />} />
        {}
        <Route 
          path="/dashboard" 
          element={isAuthenticated ? <Dashboard /> : <Navigate to="/login" replace />} 
        />
        <Route 
          path="/live-logs" 
          element={isAuthenticated ? <LiveLogs /> : <Navigate to="/login" replace />} 
        />
        <Route 
          path="/search-logs" 
          element={isAuthenticated ? <LogSearch /> : <Navigate to="/login" replace />}
        />
        <Route 
          path="/reports" 
          element={isAuthenticated ? <Reports /> : <Navigate to="/login" replace />} 
        />
        <Route 
          path="/settings" 
          element={isAuthenticated ? <Settings /> : <Navigate to="/login" replace />} 
        />
        {}
        <Route path="/" element={<Navigate to="/login" replace />} />
      </Routes>
    </Router>
  );
}

export default App;