import React, { useEffect, useState } from "react";
import axios from "axios";

const LiveLogsTable = () => {
  const [logs, setLogs] = useState([]);

  useEffect(() => {
    let count = 0;
    const maxCount = 50; // maksimum log sayısı
    const interval = setInterval(async () => {
      try {
        const response = await axios.get("http://localhost:5000/api/livelogs/start?count=1");
        const newLog = response.data[0];

        setLogs(prevLogs => {
          const updatedLogs = [newLog, ...prevLogs];
          if (updatedLogs.length > maxCount) updatedLogs.pop(); // eski logları sil
          return updatedLogs;
        });

        count++;
        if (count >= maxCount) clearInterval(interval); // belirlenen sayıya ulaştığında dur
      } catch (err) {
        console.error("API hatası:", err);
        clearInterval(interval);
      }
    }, 1000); // 1 saniyede bir yeni log

    return () => clearInterval(interval); // bileşen kapandığında interval temizle
  }, []);

  return (
    <div>
      <h2>Live Network Logs</h2>
      <table border="1" cellPadding="5">
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
            <tr key={index}>
              <td>{log.timestamp}</td>
              <td>{log.sourceIP}</td>
              <td>{log.destinationIP}</td>
              <td>{log.action}</td>
              <td>{log.status}</td>
              <td>{log.riskScore.toFixed(2)}</td>
              <td>{log.server}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default LiveLogsTable;
