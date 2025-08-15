import React, { useEffect, useState } from "react";
import "./App.css";

function App() {
  const [logs, setLogs] = useState([]);

  // Logları çekmek için fonksiyon
  const fetchLogs = async () => {
    try {
      const response = await fetch("https://localhost:5000/api/LiveLogs/random?count=1");
      if (!response.ok) throw new Error("Network response was not ok");
      const data = await response.json();

      // Yeni logu en üste ekle, max 20 log göster
      setLogs(prevLogs => [data[0], ...prevLogs].slice(0, 20));
    } catch (error) {
      console.error("Error fetching logs:", error);
    }
  };

  // Component mount olduğunda ve her 2 saniyede bir fetch et
  useEffect(() => {
    fetchLogs(); // İlk yüklemede
    const interval = setInterval(fetchLogs, 2000); // Her 2 saniyede
    return () => clearInterval(interval); // Cleanup
  }, []);

  return (
    <div className="App">
      <h1>Live Network Logs</h1>
      <table>
        <thead>
          <tr>
            <th>Timestamp</th>
            <th>Source IP</th>
            <th>Destination IP</th>
            <th>Action</th>
            <th>Status</th>
            <th>Risk Score</th>
            <th>Server</th>
          </tr>
        </thead>
        <tbody>
          {logs.map((log, index) => (
            <tr key={index} className="log-row">
              <td>{new Date(log.Timestamp).toLocaleTimeString()}</td>
              <td>{log.SourceIP}</td>
              <td>{log.DestinationIP}</td>
              <td>{log.Action}</td>
              <td>{log.Status}</td>
              <td>{log.RiskScore.toFixed(2)}</td>
              <td>{log.Server}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default App;
