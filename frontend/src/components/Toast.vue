<template>
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
          <line x1="12" y1="16" x2="12" y2="12" />
          <line x1="12" y1="8" x2="12.01" y2="8" />
        </svg>
      </div>
      <span class="toast-msg">{{ toast.message }}</span>
      <button @click="close" class="toast-close">
        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
        </svg>
      </button>
    </div>
  </Transition>
</template>

<script setup lang="ts">
import { useToast } from '@/composables/useToast'
const { toast, close } = useToast()
</script>

<style scoped>
.toast {
  position: fixed;
  bottom: 2rem;
  right: 2rem;
  z-index: 9999;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.85rem 1.25rem;
  background: #1a1b2e;
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 12px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
  min-width: 300px;
  max-width: 400px;
}

.toast.success { border-color: rgba(34, 197, 94, 0.2); }
.toast.error { border-color: rgba(239, 68, 68, 0.2); }
.toast.info { border-color: rgba(99, 102, 241, 0.2); }

.toast-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border-radius: 8px;
  background: rgba(255, 255, 255, 0.05);
}

.toast.success .toast-icon { color: #4ade80; background: rgba(34, 197, 94, 0.1); }
.toast.error .toast-icon { color: #f87171; background: rgba(239, 68, 68, 0.1); }
.toast.info .toast-icon { color: #818cf8; background: rgba(99, 102, 241, 0.1); }

.toast-msg {
  flex: 1;
  font-size: 0.85rem;
  color: #e2e8f0;
  font-weight: 500;
}

.toast-close {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 24px;
  height: 24px;
  border: none;
  background: transparent;
  color: #64748b;
  cursor: pointer;
  border-radius: 4px;
  transition: all 0.2s;
}

.toast-close:hover {
  background: rgba(255, 255, 255, 0.05);
  color: #e2e8f0;
}

/* Animation */
.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.toast-enter-from {
  opacity: 0;
  transform: translateY(20px) scale(0.95);
}

.toast-leave-to {
  opacity: 0;
  transform: translateY(-10px) scale(0.95);
}
</style>
