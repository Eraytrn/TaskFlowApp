<template>
  <div class="register-page">
    <!-- Subtle animated background -->
    <div class="bg-decoration">
      <div class="bg-orb bg-orb-1"></div>
      <div class="bg-orb bg-orb-2"></div>
      <div class="bg-orb bg-orb-3"></div>
    </div>

    <div class="register-container">
      <!-- Left side: Branding -->
      <div class="brand-side">
        <div class="brand-content">
          <div class="brand-icon">
            <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2" />
              <path d="M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
              <path d="M9 14l2 2 4-4" />
            </svg>
          </div>
          <h1 class="brand-title">TaskFlow</h1>
          <p class="brand-subtitle">Ekibinizle birlikte projelerinizi yönetmenin en kolay yolu.</p>
          <div class="brand-features">
            <div class="feature-item">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="9 11 12 14 22 4"></polyline>
                <path d="M21 12v7a2 2 0 01-2 2H5a2 2 0 01-2-2V5a2 2 0 012-2h11"></path>
              </svg>
              <span>Sürükle-bırak görev yönetimi</span>
            </div>
            <div class="feature-item">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2"></path>
                <circle cx="9" cy="7" r="4"></circle>
                <path d="M23 21v-2a4 4 0 00-3-3.87"></path>
                <path d="M16 3.13a4 4 0 010 7.75"></path>
              </svg>
              <span>Takım davetleri ve erişim kontrolü</span>
            </div>
            <div class="feature-item">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M21 15a2 2 0 01-2 2H7l-4 4V5a2 2 0 012-2h14a2 2 0 012 2z"></path>
              </svg>
              <span>Görev yorumları ve tartışmalar</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Right side: Register Form -->
      <div class="form-side">
        <div class="form-wrapper">
          <div class="form-header">
            <h2>Hesap Oluştur</h2>
            <p>Hemen ücretsiz kaydolun ve başlayın</p>
          </div>

          <form @submit.prevent="handleRegister" class="register-form">
            <div class="form-group">
              <label for="fullName">Ad Soyad</label>
              <div class="input-wrapper">
                <svg class="input-icon" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2"></path>
                  <circle cx="12" cy="7" r="4"></circle>
                </svg>
                <input
                  id="fullName"
                  v-model="form.fullName"
                  type="text"
                  required
                  placeholder="Adınız Soyadınız"
                  autocomplete="name"
                />
              </div>
            </div>

            <div class="form-group">
              <label for="email">E-posta Adresi</label>
              <div class="input-wrapper">
                <svg class="input-icon" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
                  <polyline points="22,6 12,13 2,6"></polyline>
                </svg>
                <input
                  id="email"
                  v-model="form.email"
                  type="email"
                  required
                  placeholder="ornek@email.com"
                  autocomplete="email"
                />
              </div>
            </div>

            <div class="form-group">
              <label for="password">Şifre</label>
              <div class="input-wrapper">
                <svg class="input-icon" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
                  <path d="M7 11V7a5 5 0 0110 0v4"></path>
                </svg>
                <input
                  id="password"
                  v-model="form.password"
                  type="password"
                  required
                  minlength="6"
                  placeholder="En az 6 karakter"
                  autocomplete="new-password"
                />
              </div>
            </div>

            <div v-if="authStore.error" class="error-alert">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <circle cx="12" cy="12" r="10"></circle>
                <line x1="15" y1="9" x2="9" y2="15"></line>
                <line x1="9" y1="9" x2="15" y2="15"></line>
              </svg>
              <span>{{ authStore.error }}</span>
            </div>

            <button type="submit" :disabled="authStore.loading" class="submit-btn">
              <span v-if="!authStore.loading">Kayıt Ol</span>
              <span v-else class="loading-state">
                <svg class="spinner" width="20" height="20" viewBox="0 0 24 24" fill="none">
                  <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="3" opacity="0.25"></circle>
                  <path d="M4 12a8 8 0 018-8" stroke="currentColor" stroke-width="3" stroke-linecap="round" opacity="0.75"></path>
                </svg>
                Hesap oluşturuluyor...
              </span>
            </button>
          </form>

          <div class="divider">
            <span>veya</span>
          </div>

          <router-link to="/login" class="login-btn">
            Zaten hesabınız var mı? <strong>Giriş Yapın</strong>
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.register-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #0f1117;
  position: relative;
  overflow: hidden;
  font-family: 'Inter', 'Segoe UI', system-ui, -apple-system, sans-serif;
}

/* Subtle background orbs */
.bg-decoration {
  position: absolute;
  inset: 0;
  z-index: 0;
  pointer-events: none;
}

.bg-orb {
  position: absolute;
  border-radius: 50%;
  filter: blur(120px);
  opacity: 0.15;
}

.bg-orb-1 {
  width: 600px;
  height: 600px;
  background: #8b5cf6;
  top: -200px;
  right: -100px;
  animation: float 22s ease-in-out infinite;
}

.bg-orb-2 {
  width: 500px;
  height: 500px;
  background: #6366f1;
  bottom: -150px;
  left: -100px;
  animation: float 26s ease-in-out infinite reverse;
}

.bg-orb-3 {
  width: 400px;
  height: 400px;
  background: #3b82f6;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  animation: float 20s ease-in-out infinite 3s;
}

@keyframes float {
  0%, 100% { transform: translate(0, 0); }
  33% { transform: translate(30px, -30px); }
  66% { transform: translate(-20px, 20px); }
}

/* Container */
.register-container {
  position: relative;
  z-index: 1;
  display: flex;
  width: 100%;
  max-width: 960px;
  min-height: 620px;
  margin: 1rem;
  border-radius: 24px;
  overflow: hidden;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.06);
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5);
}

/* Left side: Branding */
.brand-side {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  background: linear-gradient(135deg, #1a1b2e 0%, #16172b 100%);
  border-right: 1px solid rgba(255, 255, 255, 0.05);
}

.brand-content {
  max-width: 320px;
}

.brand-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 64px;
  height: 64px;
  border-radius: 16px;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  margin-bottom: 1.5rem;
  box-shadow: 0 8px 32px rgba(99, 102, 241, 0.3);
}

.brand-title {
  font-size: 2.25rem;
  font-weight: 800;
  color: #ffffff;
  margin-bottom: 0.5rem;
  letter-spacing: -0.5px;
}

.brand-subtitle {
  color: #94a3b8;
  font-size: 0.95rem;
  line-height: 1.6;
  margin-bottom: 2.5rem;
}

.brand-features {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.feature-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  color: #cbd5e1;
  font-size: 0.85rem;
}

.feature-item svg {
  color: #818cf8;
  flex-shrink: 0;
}

/* Right side: Form */
.form-side {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  background: #13141f;
}

.form-wrapper {
  width: 100%;
  max-width: 360px;
}

.form-header {
  margin-bottom: 2rem;
}

.form-header h2 {
  font-size: 1.5rem;
  font-weight: 700;
  color: #f1f5f9;
  margin-bottom: 0.4rem;
}

.form-header p {
  color: #64748b;
  font-size: 0.875rem;
}

/* Form */
.register-form {
  display: flex;
  flex-direction: column;
  gap: 1.1rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.form-group label {
  font-size: 0.8rem;
  font-weight: 600;
  color: #94a3b8;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.input-wrapper {
  position: relative;
}

.input-icon {
  position: absolute;
  left: 14px;
  top: 50%;
  transform: translateY(-50%);
  color: #475569;
  pointer-events: none;
  transition: color 0.2s;
}

.input-wrapper input {
  width: 100%;
  padding: 0.75rem 0.875rem 0.75rem 2.75rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 12px;
  color: #f1f5f9;
  font-size: 0.9rem;
  transition: all 0.2s ease;
  outline: none;
  box-sizing: border-box;
}

.input-wrapper input::placeholder {
  color: #475569;
}

.input-wrapper input:focus {
  border-color: #6366f1;
  background: rgba(99, 102, 241, 0.05);
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.input-wrapper:focus-within .input-icon {
  color: #818cf8;
}

/* Error */
.error-alert {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1rem;
  background: rgba(239, 68, 68, 0.1);
  border: 1px solid rgba(239, 68, 68, 0.2);
  border-radius: 10px;
  color: #fca5a5;
  font-size: 0.825rem;
}

/* Submit Button */
.submit-btn {
  width: 100%;
  padding: 0.8rem;
  border: none;
  border-radius: 12px;
  background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
  color: white;
  font-size: 0.925rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  box-shadow: 0 4px 16px rgba(99, 102, 241, 0.3);
  margin-top: 0.25rem;
}

.submit-btn:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 6px 24px rgba(99, 102, 241, 0.4);
}

.submit-btn:active:not(:disabled) {
  transform: translateY(0);
}

.submit-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.loading-state {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.spinner {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* Divider */
.divider {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin: 1.5rem 0;
}

.divider::before,
.divider::after {
  content: '';
  flex: 1;
  height: 1px;
  background: rgba(255, 255, 255, 0.06);
}

.divider span {
  color: #475569;
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 1px;
}

/* Login Link Button */
.login-btn {
  display: block;
  text-align: center;
  padding: 0.75rem;
  border-radius: 12px;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.06);
  color: #94a3b8;
  font-size: 0.85rem;
  text-decoration: none;
  transition: all 0.2s ease;
}

.login-btn strong {
  color: #818cf8;
  font-weight: 600;
}

.login-btn:hover {
  background: rgba(255, 255, 255, 0.06);
  border-color: rgba(99, 102, 241, 0.2);
  color: #c7d2fe;
}

/* Responsive */
@media (max-width: 768px) {
  .register-container {
    flex-direction: column;
    max-width: 440px;
    min-height: auto;
  }

  .brand-side {
    padding: 2rem;
    border-right: none;
    border-bottom: 1px solid rgba(255, 255, 255, 0.05);
  }

  .brand-features {
    display: none;
  }

  .brand-subtitle {
    margin-bottom: 0;
  }

  .form-side {
    padding: 2rem;
  }
}
</style>

<script setup lang="ts">
import { reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const form = reactive({
  fullName: '',
  email: '',
  password: ''
})

async function handleRegister() {
  const success = await authStore.register(form)
  if (success) {
    router.push('/boards')
  }
}
</script>
