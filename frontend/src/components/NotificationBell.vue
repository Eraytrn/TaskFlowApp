<template>
  <div class="notification-bell">
    <button @click="toggleDropdown" class="bell-button" :class="{ 'has-unread': unreadCount > 0 }">
      <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path>
        <path d="M13.73 21a2 2 0 0 1-3.46 0"></path>
      </svg>
      <span v-if="unreadCount > 0" class="badge">{{ unreadCount }}</span>
    </button>

    <div v-if="showDropdown" class="dropdown">
      <div class="dropdown-header">
        <h3>Bildirimler</h3>
        <button @click="closeDropdown" class="close-btn">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18" />
            <line x1="6" y1="6" x2="18" y2="18" />
          </svg>
        </button>
      </div>

      <div v-if="loading" class="state-msg">
        <div class="mini-spinner"></div>
        <span>Yükleniyor...</span>
      </div>

      <div v-else-if="notifications.length === 0" class="state-msg">
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" style="color: #475569;">
          <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path>
          <path d="M13.73 21a2 2 0 0 1-3.46 0"></path>
        </svg>
        <span>Bildirim yok</span>
      </div>

      <div v-else class="notifications-list">
        <div
          v-for="notification in notifications"
          :key="notification.id"
          class="notification-item"
          :class="{ 'unread': !notification.isRead }"
        >
          <div class="notif-icon" :class="getNotifIconClass(notification.type)">
            <svg v-if="notification.type === 0" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <path d="M16 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2" />
              <circle cx="8.5" cy="7" r="4" />
              <line x1="20" y1="8" x2="20" y2="14" />
              <line x1="23" y1="11" x2="17" y2="11" />
            </svg>
            <svg v-else-if="notification.type === 1" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
              <polyline points="20 6 9 17 4 12" />
            </svg>
            <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path>
            </svg>
          </div>

          <div class="notification-content" @click="handleNotificationClick(notification)">
            <h4>{{ notification.title }}</h4>
            <p>{{ notification.message }}</p>
            <span class="time">{{ formatDate(notification.createdDate) }}</span>
          </div>

          <!-- Approve/Reject buttons for join requests -->
          <div v-if="notification.type === 0 && !notification.isRead" class="notification-actions">
            <button @click.stop="handleApprove(notification)" class="btn-approve" title="Onayla">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="20 6 9 17 4 12" />
              </svg>
            </button>
            <button @click.stop="handleReject(notification)" class="btn-reject" title="Reddet">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
                <line x1="18" y1="6" x2="6" y2="18" />
                <line x1="6" y1="6" x2="18" y2="18" />
              </svg>
            </button>
          </div>

          <div v-else-if="!notification.isRead" class="unread-dot"></div>
        </div>
      </div>
    </div>

    <!-- Toast Notification -->
    <Transition name="toast">
      <div v-if="toast.show" class="toast" :class="toast.type">
        <div class="toast-icon">
          <svg v-if="toast.type === 'success'" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12" />
          </svg>
          <svg v-else-if="toast.type === 'error'" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10" />
            <line x1="12" y1="8" x2="12" y2="12" />
            <line x1="12" y1="16" x2="12.01" y2="16" />
          </svg>
           <svg v-else width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10" />
            <line x1="12" y1="8" x2="12" y2="12" />
            <line x1="12" y1="16" x2="12.01" y2="16" />
          </svg>
        </div>
        <span class="toast-msg">{{ toast.message }}</span>
      </div>
    </Transition>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { notificationService } from '@/services/notification.service'
import type { Notification } from '@/types/notification'

const notifications = ref<Notification[]>([])
const showDropdown = ref(false)
const loading = ref(false)

// Toast state
const toast = ref({
  show: false,
  message: '',
  type: 'success' as 'success' | 'warning' | 'error'
})
let toastTimer: ReturnType<typeof setTimeout> | null = null

function showToast(message: string, type: 'success' | 'warning' | 'error' = 'success') {
  if (toastTimer) clearTimeout(toastTimer)
  toast.value = { show: true, message, type }
  toastTimer = setTimeout(() => { toast.value.show = false }, 4000)
}

const unreadCount = computed(() => {
  return notifications.value.filter(n => !n.isRead).length
})

function getNotifIconClass(type: number): string {
  if (type === 0) return 'icon-request'
  if (type === 1) return 'icon-approved'
  return 'icon-default'
}

const toggleDropdown = async () => {
  showDropdown.value = !showDropdown.value
  if (showDropdown.value && notifications.value.length === 0) {
    await loadNotifications()
  }
}

const closeDropdown = () => {
  showDropdown.value = false
}

const loadNotifications = async () => {
  try {
    loading.value = true
    notifications.value = await notificationService.getUnread()
  } catch (error) {
    console.error('Failed to load notifications:', error)
  } finally {
    loading.value = false
  }
}

const handleNotificationClick = async (notification: Notification) => {
  if (!notification.isRead) {
    try {
      await notificationService.markAsRead(notification.id)
      notification.isRead = true
    } catch (error) {
      console.error('Failed to mark notification as read:', error)
    }
  }
}

const handleApprove = async (notification: Notification) => {
  if (!notification.relatedEntityId) {
    showToast('İlgili istek bulunamadı', 'error')
    return
  }

  try {
    const { joinRequestService } = await import('@/services/join-request.service')
    await joinRequestService.approveRequest(notification.relatedEntityId)
    await notificationService.markAsRead(notification.id)
    notification.isRead = true
    showToast('Katılım isteği onaylandı!', 'success')
    await loadNotifications()
  } catch (error: any) {
    console.error('Failed to approve request:', error)
    const msg = error.response?.data || error.message || ''
    if (msg.includes('not found') || msg.includes('exists')) {
      showToast('Hata: Board veya istek bulunamadı (silinmiş olabilir).', 'error')
    } else {
      showToast(`Onaylama başarısız: ${msg}`, 'error')
    }
  }
}

const handleReject = async (notification: Notification) => {
  if (!notification.relatedEntityId) {
    showToast('İlgili istek bulunamadı', 'error')
    return
  }

  try {
    const { joinRequestService } = await import('@/services/join-request.service')
    await joinRequestService.rejectRequest(notification.relatedEntityId)
    await notificationService.markAsRead(notification.id)
    notification.isRead = true
    showToast('Katılım isteği reddedildi', 'success')
    await loadNotifications()
  } catch (error: any) {
    console.error('Failed to reject request:', error)
    const msg = error.response?.data || error.message || ''
    if (msg.includes('not found') || msg.includes('exists')) {
       showToast('Hata: Board veya istek bulunamadı (silinmiş olabilir).', 'error')
    } else {
       showToast(`Reddetme başarısız: ${msg}`, 'error')
    }
  }
}

const formatDate = (dateString: string) => {
  if (!dateString) return ''
  // Backend sends UTC but might miss 'Z'. Ensure we treat it as UTC.
  const dateToParse = dateString.endsWith('Z') || dateString.includes('+') ? dateString : dateString + 'Z'
  const date = new Date(dateToParse)
  const now = new Date()
  const diffMs = now.getTime() - date.getTime()
  const diffMins = Math.floor(diffMs / 60000)

  if (diffMins < 1) return 'Az önce'
  if (diffMins < 60) return `${diffMins}dk önce`

  const diffHours = Math.floor(diffMins / 60)
  if (diffHours < 24) return `${diffHours}sa önce`

  const diffDays = Math.floor(diffHours / 24)
  if (diffDays < 7) return `${diffDays}g önce`

  return date.toLocaleDateString('tr-TR')
}

onMounted(() => {
  loadNotifications()
  setInterval(loadNotifications, 30000)
})
</script>

<style scoped>
.notification-bell {
  position: relative;
}

.bell-button {
  position: relative;
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: transparent;
  border: 1px solid rgba(255, 255, 255, 0.06);
  border-radius: 10px;
  cursor: pointer;
  color: #64748b;
  transition: all 0.2s;
}

.bell-button:hover {
  color: #94a3b8;
  background: rgba(255, 255, 255, 0.05);
}

.bell-button.has-unread {
  color: #818cf8;
  border-color: rgba(99, 102, 241, 0.2);
}

.badge {
  position: absolute;
  top: -4px;
  right: -4px;
  background: #ef4444;
  color: white;
  border-radius: 8px;
  padding: 1px 5px;
  font-size: 0.65rem;
  font-weight: 700;
  min-width: 16px;
  text-align: center;
  line-height: 1.4;
  border: 2px solid #0f1117;
}

.dropdown {
  position: absolute;
  top: calc(100% + 8px);
  right: 0;
  width: 380px;
  max-height: 500px;
  background: #1a1b2e;
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 16px;
  overflow: hidden;
  z-index: 1000;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.4);
}

.dropdown-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.06);
}

.dropdown-header h3 {
  margin: 0;
  font-size: 0.9rem;
  font-weight: 700;
  color: #f1f5f9;
}

.close-btn {
  width: 28px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(255, 255, 255, 0.05);
  border: none;
  border-radius: 8px;
  cursor: pointer;
  color: #64748b;
  transition: all 0.2s;
}

.close-btn:hover {
  background: rgba(255, 255, 255, 0.1);
  color: #e2e8f0;
}

.state-msg {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  padding: 2.5rem 1rem;
  color: #64748b;
  font-size: 0.8rem;
}

.mini-spinner {
  width: 20px;
  height: 20px;
  border: 2px solid rgba(255, 255, 255, 0.06);
  border-top-color: #6366f1;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.notifications-list {
  max-height: 420px;
  overflow-y: auto;
}

.notifications-list::-webkit-scrollbar {
  width: 4px;
}

.notifications-list::-webkit-scrollbar-track {
  background: transparent;
}

.notifications-list::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, 0.1);
  border-radius: 2px;
}

.notification-item {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.04);
  cursor: pointer;
  transition: background 0.15s;
}

.notification-item:last-child {
  border-bottom: none;
}

.notification-item:hover {
  background: rgba(255, 255, 255, 0.03);
}

.notification-item.unread {
  background: rgba(99, 102, 241, 0.06);
}

.notification-item.unread:hover {
  background: rgba(99, 102, 241, 0.1);
}

.notif-icon {
  width: 32px;
  height: 32px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  margin-top: 2px;
}

.icon-request {
  background: rgba(99, 102, 241, 0.15);
  color: #818cf8;
}

.icon-approved {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
}

.icon-default {
  background: rgba(255, 255, 255, 0.06);
  color: #64748b;
}

.notification-content {
  flex: 1;
  min-width: 0;
}

.notification-content h4 {
  margin: 0 0 3px 0;
  font-size: 0.8rem;
  font-weight: 600;
  color: #e2e8f0;
}

.notification-content p {
  margin: 0 0 6px 0;
  font-size: 0.75rem;
  color: #94a3b8;
  line-height: 1.4;
}

.notification-content .time {
  font-size: 0.675rem;
  color: #475569;
}

.unread-dot {
  width: 8px;
  height: 8px;
  background: #818cf8;
  border-radius: 50%;
  margin-top: 8px;
  flex-shrink: 0;
}

.notification-actions {
  display: flex;
  gap: 6px;
  flex-shrink: 0;
  margin-top: 4px;
}

.btn-approve,
.btn-reject {
  width: 30px;
  height: 30px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s;
}

.btn-approve {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
}

.btn-approve:hover {
  background: rgba(34, 197, 94, 0.25);
  transform: scale(1.05);
}

.btn-reject {
  background: rgba(239, 68, 68, 0.15);
  color: #f87171;
}

.btn-reject:hover {
  background: rgba(239, 68, 68, 0.25);
  transform: scale(1.05);
}

/* ---- TOAST ---- */
.toast {
  position: fixed;
  bottom: 2rem;
  left: 50%;
  transform: translateX(-50%);
  display: flex;
  align-items: center;
  gap: 0.6rem;
  padding: 0.75rem 1.25rem;
  border-radius: 14px;
  z-index: 2000; /* Higher than dropdown */
  box-shadow: 0 12px 32px rgba(0, 0, 0, 0.4);
  max-width: 440px;
  pointer-events: none;
}

.toast.success {
  background: rgba(34, 197, 94, 0.15);
  border: 1px solid rgba(34, 197, 94, 0.25);
  color: #4ade80;
}

.toast.warning {
  background: rgba(245, 158, 11, 0.15);
  border: 1px solid rgba(245, 158, 11, 0.25);
  color: #fbbf24;
}

.toast.error {
  background: rgba(239, 68, 68, 0.15);
  border: 1px solid rgba(239, 68, 68, 0.25);
  color: #f87171;
}

.toast-icon {
  flex-shrink: 0;
}

.toast-msg {
  font-size: 0.825rem;
  font-weight: 500;
  line-height: 1.4;
}

/* Toast transition */
.toast-enter-active { transition: all 0.3s ease-out; }
.toast-leave-active { transition: all 0.25s ease-in; }
.toast-enter-from { opacity: 0; transform: translateX(-50%) translateY(20px); }
.toast-leave-to { opacity: 0; transform: translateX(-50%) translateY(10px); }
</style>
