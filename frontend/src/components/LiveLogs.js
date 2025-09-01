import React, { useEffect, useState } from "react";
import "./aa.css";

function App() {
  const [logs, setLogs] = useState([]);

  const fetchLogs = async () => {
    try {
      const response = await fetch("http://localhost:5254/api/LiveLogs/logs?count=1");
      if (!response.ok) throw new Error("Network response was not ok");
      const data = await response.json();

      setLogs(prevLogs => [data[0], ...prevLogs].slice(0, 20));
    } catch (error) {
      console.error("Error fetching logs:", error);
    }
  };

  useEffect(() => {
    fetchLogs();
    const interval = setInterval(fetchLogs, 2000); // Her 2 saniyede
    return () => clearInterval(interval); // Dolunca temizle
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
              <td>{new Date(log.timestamp).toLocaleTimeString()}</td>
              <td>{log.sourceIP}</td>
              <td>{log.destinationIP}</td>
              <td>{log.action}</td>
              <td>{log.status}</td>
              <td>{log.riskScore.toFixed(7)}</td>
              <td>{log.server}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default App;
