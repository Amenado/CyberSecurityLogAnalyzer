import React from 'react';
// Login sayfası için stil dosyası
import './Login.css';

function Login() {
  return (
    <div className="login-container">
      <h2>Giriş Yap</h2>
      <form>
        <div className="form-group">
          <label>Kullanıcı Adı:</label>
          <input type="text" />
        </div>
        <div className="form-group">
          <label>Şifre:</label>
          <input type="password" />
        </div>
        <button type="submit">Giriş Yap</button>
      </form>
    </div>
  );
}

export default Login;