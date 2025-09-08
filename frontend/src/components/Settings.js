import React, { useState } from 'react';
import Sidebar from './Sidebar';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBell, faUserCog, faCogs } from '@fortawesome/free-solid-svg-icons';
import './Settings.css';

const Settings = () => {
    // Şifre formunun state'i
    const [passwordForm, setPasswordForm] = useState({
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
    });

    // Bildirim ayarlarının state'i
    const [notifications, setNotifications] = useState({
        emailNotifications: true,
        smsNotifications: false
    });

    // Şifre formundaki değişiklikleri yönetme
    const handlePasswordChange = (e) => {
        const { id, value } = e.target;
        setPasswordForm(prevForm => ({ ...prevForm, [id]: value }));
    };

    // Şifre değiştirme formunu gönderme
    const handlePasswordSubmit = async (e) => {
        e.preventDefault();
        
        // Basit validasyon
        if (passwordForm.newPassword !== passwordForm.confirmPassword) {
            alert("Yeni şifreler eşleşmiyor!");
            return;
        }

        try {
            const response = await fetch("http://localhost:5254/api/Settings/change-password", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(passwordForm)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.title || "Şifre değiştirme başarısız.");
            }

            alert("Şifreniz başarıyla değiştirildi!");
            // Formu temizle
            setPasswordForm({ currentPassword: '', newPassword: '', confirmPassword: '' });

        } catch (error) {
            console.error("Hata:", error);
            alert("Hata: " + error.message);
        }
    };

    // Bildirim ayarlarındaki değişiklikleri yönetme ve backend'e gönderme
    const handleNotificationChange = async (e) => {
        const { id, checked } = e.target;
        
        // Önce local state'i güncelle
        const updatedNotifications = { ...notifications, [id]: checked };
        setNotifications(updatedNotifications);

        try {
            const response = await fetch("http://localhost:5254/api/Settings/notifications", {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedNotifications)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.title || "Bildirim ayarı güncellenemedi.");
            }

            console.log("Bildirim ayarları başarıyla güncellendi.");

        } catch (error) {
            console.error("Hata:", error);
            alert("Bildirim ayarları kaydedilirken hata oluştu: " + error.message);
        }
    };

    return (
        <div className="settings-container">
            <Sidebar />
            <div className="main-content">
                <header className="header">
                    <h1>Ayarlar</h1>
                </header>

                {/* Kullanıcı Ayarları Bölümü */}
                <div className="settings-section section">
                    <h2><FontAwesomeIcon icon={faUserCog} /> Kullanıcı Ayarları</h2>
                    <form className="settings-form" onSubmit={handlePasswordSubmit}>
                        <div className="form-group">
                            <label htmlFor="username">Kullanıcı Adı</label>
                            <input type="text" id="username" defaultValue="admin" disabled />
                        </div>
                        <div className="form-group">
                            <label htmlFor="currentPassword">Mevcut Şifre</label>
                            <input 
                                type="password" 
                                id="currentPassword" 
                                value={passwordForm.currentPassword} 
                                onChange={handlePasswordChange}
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="newPassword">Yeni Şifre</label>
                            <input 
                                type="password" 
                                id="newPassword" 
                                value={passwordForm.newPassword} 
                                onChange={handlePasswordChange}
                            />
                        </div>
                        <div className="form-group">
                            <label htmlFor="confirmPassword">Yeni Şifreyi Onayla</label>
                            <input 
                                type="password" 
                                id="confirmPassword" 
                                value={passwordForm.confirmPassword} 
                                onChange={handlePasswordChange}
                            />
                        </div>
                        <button type="submit">Şifreyi Değiştir</button>
                    </form>
                </div>

                {/* Bildirim Ayarları Bölümü */}
                <div className="settings-section section">
                    <h2><FontAwesomeIcon icon={faBell} /> Bildirim Ayarları</h2>
                    <div className="toggle-group">
                        <label htmlFor="emailNotifications">E-posta Bildirimleri</label>
                        <label className="toggle-switch">
                            <input 
                                type="checkbox" 
                                id="emailNotifications" 
                                checked={notifications.emailNotifications}
                                onChange={handleNotificationChange}
                            />
                            <span className="slider"></span>
                        </label>
                    </div>
                    <div className="toggle-group">
                        <label htmlFor="smsNotifications">SMS Bildirimleri</label>
                        <label className="toggle-switch">
                            <input 
                                type="checkbox" 
                                id="smsNotifications" 
                                checked={notifications.smsNotifications}
                                onChange={handleNotificationChange} 
                            />
                            <span className="slider"></span>
                        </label>
                    </div>
                    <p className="description">Yüksek riskli aktiviteler için bildirim alın.</p>
                </div>
            </div>
        </div>
    );
};

export default Settings;