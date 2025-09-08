import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';

const Login = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch("http://localhost:5254/api/LiveLogs/login", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            });

            if (response.ok) {
                // Backend'den gelen yanıt 200 OK ise
                onLogin();
                navigate('/dashboard');
            } else {
                // Backend'den gelen yanıt 401 Unauthorized ise veya başka bir hata varsa
                alert('Kullanıcı adı veya şifre hatalı!');
            }
        } catch (error) {
            // Ağ hatası veya API'ye ulaşılamama durumu
            console.error("Giriş yapılırken hata oluştu:", error);
            alert('Giriş yapılırken bir sorun oluştu. Lütfen daha sonra tekrar deneyin.');
        }
    };

    return (
        <div className="login-container">
            <div className="login-box">
                <h1 className="login-title">Log Analiz Paneli</h1>
                <p className="login-subtitle">Devam etmek için giriş yapın.</p>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="username">Kullanıcı Adı</label>
                        <input
                            type="text"
                            id="username"
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="password">Şifre</label>
                        <input
                            type="password"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>
                    <button type="submit" className="login-button">Giriş Yap</button>
                </form>
            </div>
        </div>
    );
};

export default Login;