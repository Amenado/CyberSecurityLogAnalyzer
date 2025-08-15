// src/components/Dashboard.js veya log tablosu olan component

import React from "react";
import axios from "axios";

const Dashboard = ({ logs, setLogs }) => {

  const addRandomLog = async () => {
    try {
      const res = await axios.post("http://localhost:5000/api/logs/random");
      setLogs(prevLogs => [...prevLogs, res.data]);
    } catch (err) {
      console.error("Log eklenemedi:", err);
    }
  };

  return (
    <div>
      <h1>Cyber Security Log Analyzer</h1>
      <button onClick={addRandomLog}>Add Random Log</button>
      
      <table>
        <thead>
          <tr>
            <th>Timestamp</th>
            <th>Source IP</th>
            <th>Destination IP</th>
            <th>Action</th>
            <th>Status</th>
            <th>RiskScore</th>
            <th>Server</th>
          </tr>
        </thead>
        <tbody>
          {logs.map((log, idx) => (
            <tr key={idx}>
              <td>{log.Timestamp}</td>
              <td>{log.SourceIP}</td>
              <td>{log.DestinationIP}</td>
              <td>{log.Action}</td>
              <td>{log.Status}</td>
              <td>{log.RiskScore}</td>
              <td>{log.Server}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Dashboard;
