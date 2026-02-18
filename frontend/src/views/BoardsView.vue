<template>
  <div class="boards-page">
    <!-- Navbar -->
    <nav class="navbar">
      <div class="navbar-inner">
        <div class="navbar-brand">
          <div class="brand-icon">
            <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4" />
            </svg>
          </div>
          <span class="brand-text">TaskFlow</span>
        </div>
        <div class="navbar-right">
          <NotificationBell />
          <div class="user-badge">
            <div class="user-avatar">
              {{ authStore.user?.fullName?.charAt(0).toUpperCase() }}
            </div>
            <span class="user-name">{{ authStore.user?.fullName }}</span>
          </div>
          <button @click="handleLogout" class="logout-btn" title="Çıkış Yap">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M9 21H5a2 2 0 01-2-2V5a2 2 0 012-2h4" />
              <polyline points="16 17 21 12 16 7" />
              <line x1="21" y1="12" x2="9" y2="12" />
            </svg>
          </button>
        </div>
      </div>
    </nav>

    <!-- Main Content -->
    <div class="main-content">
      <!-- Header -->
      <div class="page-header">
        <div class="header-left">
          <h2 class="page-title">{{ activeTab === 'all' ? 'Tüm Boardlar' : 'Boardlarım' }}</h2>
          <p class="page-subtitle">{{ activeTab === 'all' ? 'Mevcut tüm projeleri görüntüle' : 'Projelerinizi yönetin ve organize edin' }}</p>
        </div>
        <button @click="showCreateModal = true" class="create-btn">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="12" y1="5" x2="12" y2="19" />
            <line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          <span>Yeni Board</span>
        </button>
      </div>

      <!-- Tabs -->
      <div class="tabs-container">
        <button
          @click="activeTab = 'all'"
          :class="['tab-btn', { active: activeTab === 'all' }]"
        >
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <rect x="3" y="3" width="7" height="7" />
            <rect x="14" y="3" width="7" height="7" />
            <rect x="14" y="14" width="7" height="7" />
            <rect x="3" y="14" width="7" height="7" />
          </svg>
          Tüm Boardlar
        </button>
        <button
          @click="activeTab = 'my'"
          :class="['tab-btn', { active: activeTab === 'my' }]"
        >
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2" />
            <circle cx="12" cy="7" r="4" />
          </svg>
          Boardlarım
        </button>
      </div>

      <!-- Loading State -->
      <div v-if="boardStore.loading" class="state-container">
        <div class="loading-spinner"></div>
        <p class="state-text">Boardlar yükleniyor...</p>
      </div>

      <!-- Error State -->
      <div v-else-if="boardStore.error" class="state-container">
        <div class="state-icon error-icon">
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="10" />
            <line x1="15" y1="9" x2="9" y2="15" />
            <line x1="9" y1="9" x2="15" y2="15" />
          </svg>
        </div>
        <p class="state-text error">{{ boardStore.error }}</p>
      </div>

      <!-- Empty State -->
      <div v-else-if="filteredBoards.length === 0" class="state-container">
        <div class="state-icon empty-icon">
          <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
            <path d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2" />
            <path d="M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
          </svg>
        </div>
        <h3 class="empty-title">{{ activeTab === 'my' ? 'Henüz board yok' : 'Board bulunamadı' }}</h3>
        <p class="state-text">{{ activeTab === 'my' ? 'İlk board\'unuzu oluşturarak başlayın!' : 'Henüz hiçbir board oluşturulmamış' }}</p>
        <button v-if="activeTab === 'my'" @click="showCreateModal = true" class="create-btn" style="margin-top: 1.5rem;">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="12" y1="5" x2="12" y2="19" />
            <line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          <span>Board Oluştur</span>
        </button>
      </div>

      <!-- Boards Grid -->
      <div v-else class="boards-grid">
        <div
          v-for="board in filteredBoards"
          :key="board.id"
          class="board-card"
          @click="router.push(`/boards/${board.id}`)"
        >
          <!-- Card accent bar -->
          <div class="card-accent" :style="{ background: getBoardColor(board.id) }"></div>

          <div class="card-body">
            <!-- Title & Description -->
            <div class="card-header">
              <h3 class="card-title">{{ board.title }}</h3>
              <p class="card-desc">{{ board.description || 'Açıklama yok' }}</p>
            </div>

            <!-- Members -->
            <div class="card-members">
              <div class="member-avatars">
                <div
                  v-for="member in (boardMembersMap[board.id] || []).slice(0, 4)"
                  :key="member.userId"
                  class="member-avatar"
                  :class="{ 'owner': member.role === 0 }"
                  :title="member.userName + (member.role === 0 ? ' (Sahip)' : '')"
                >
                  {{ member.userName?.charAt(0).toUpperCase() }}
                </div>
              </div>
              <span v-if="(boardMembersMap[board.id] || []).length > 4" class="members-more">
                +{{ (boardMembersMap[board.id] || []).length - 4 }}
              </span>
              <span v-else-if="(boardMembersMap[board.id] || []).length > 0" class="members-count">
                {{ (boardMembersMap[board.id] || []).length }} üye
              </span>
            </div>

            <!-- Footer: Owner + Actions -->
            <div class="card-footer">
              <div class="owner-info">
                <div class="owner-avatar">{{ board.ownerName?.charAt(0).toUpperCase() }}</div>
                <span class="owner-name">{{ board.ownerName }}</span>
              </div>

              <!-- Owner: Delete -->
              <button
                v-if="board.ownerId === authStore.user?.id"
                @click.stop="handleDeleteBoard(board.id)"
                class="action-btn delete-btn"
                title="Board'u Sil"
              >
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <polyline points="3 6 5 6 21 6" />
                  <path d="M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6m3 0V4a2 2 0 012-2h4a2 2 0 012 2v2" />
                </svg>
              </button>

              <!-- Member badge -->
              <span v-else-if="isMember(board.id)" class="badge member-badge">
                <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
                  <polyline points="20 6 9 17 4 12" />
                </svg>
                Üye
              </span>

              <!-- Request Access -->
              <button
                v-else
                @click.stop="handleRequestAccess(board.id)"
                class="action-btn request-btn"
              >
                Erişim İste
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Create Board Modal -->
    <Transition name="modal">
      <div v-if="showCreateModal" class="modal-overlay" @click.self="showCreateModal = false">
        <div class="modal-card">
          <div class="modal-header">
            <h3>Yeni Board Oluştur</h3>
            <button @click="showCreateModal = false" class="modal-close">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <line x1="18" y1="6" x2="6" y2="18" />
                <line x1="6" y1="6" x2="18" y2="18" />
              </svg>
            </button>
          </div>

          <form @submit.prevent="handleCreateBoard" class="modal-form">
            <div class="form-group">
              <label for="boardTitle">Board Başlığı</label>
              <input
                id="boardTitle"
                v-model="newBoard.title"
                type="text"
                required
                placeholder="Proje adını girin"
              />
            </div>

            <div class="form-group">
              <label for="boardDesc">Açıklama</label>
              <textarea
                id="boardDesc"
                v-model="newBoard.description"
                rows="3"
                placeholder="Bu board ne hakkında? (isteğe bağlı)"
              ></textarea>
            </div>

            <div class="modal-actions">
              <button type="button" @click="showCreateModal = false" class="btn-cancel">
                İptal
              </button>
              <button type="submit" class="btn-submit">
                Oluştur
              </button>
            </div>
          </form>
        </div>
      </div>
    </Transition>

  </div>
</template>

<style scoped>
/* ============================================
   BOARDS PAGE - PROFESSIONAL DARK THEME
   ============================================ */

.boards-page {
  min-height: 100vh;
  background: #0f1117;
  color: #e2e8f0;
  font-family: 'Inter', 'Segoe UI', system-ui, -apple-system, sans-serif;
}

/* ---- NAVBAR ---- */
.navbar {
  position: sticky;
  top: 0;
  z-index: 50;
  background: rgba(15, 17, 23, 0.85);
  backdrop-filter: blur(16px);
  border-bottom: 1px solid rgba(255, 255, 255, 0.06);
}

.navbar-inner {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1.5rem;
  height: 64px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.navbar-brand {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.brand-icon {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 10px;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
}

.brand-text {
  font-size: 1.2rem;
  font-weight: 800;
  color: #f1f5f9;
  letter-spacing: -0.3px;
}

.navbar-right {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.user-badge {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.35rem 0.75rem 0.35rem 0.35rem;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.06);
}

.user-avatar {
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.75rem;
  font-weight: 700;
}

.user-name {
  font-size: 0.825rem;
  font-weight: 500;
  color: #cbd5e1;
}

.logout-btn {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.06);
  background: transparent;
  color: #64748b;
  cursor: pointer;
  transition: all 0.2s;
}

.logout-btn:hover {
  color: #ef4444;
  background: rgba(239, 68, 68, 0.1);
  border-color: rgba(239, 68, 68, 0.2);
}

/* ---- MAIN CONTENT ---- */
.main-content {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1.5rem 3rem;
}

/* ---- PAGE HEADER ---- */
.page-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.page-title {
  font-size: 1.75rem;
  font-weight: 800;
  color: #f1f5f9;
  margin-bottom: 0.25rem;
  letter-spacing: -0.3px;
}

.page-subtitle {
  font-size: 0.875rem;
  color: #64748b;
}

.create-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.65rem 1.25rem;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.85rem;
  font-weight: 600;
  border: none;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.2s;
  box-shadow: 0 4px 16px rgba(99, 102, 241, 0.25);
}

.create-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 6px 24px rgba(99, 102, 241, 0.35);
}

/* ---- TABS ---- */
.tabs-container {
  display: flex;
  gap: 0.25rem;
  padding: 0.25rem;
  background: rgba(255, 255, 255, 0.03);
  border-radius: 14px;
  border: 1px solid rgba(255, 255, 255, 0.06);
  margin-bottom: 2rem;
}

.tab-btn {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 0.6rem 1rem;
  border: none;
  border-radius: 11px;
  font-size: 0.825rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  background: transparent;
  color: #64748b;
}

.tab-btn:hover:not(.active) {
  color: #94a3b8;
  background: rgba(255, 255, 255, 0.03);
}

.tab-btn.active {
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  box-shadow: 0 2px 12px rgba(99, 102, 241, 0.3);
}

/* ---- STATES ---- */
.state-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 5rem 2rem;
  text-align: center;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 3px solid rgba(255, 255, 255, 0.06);
  border-top-color: #6366f1;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
  margin-bottom: 1rem;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.state-icon {
  width: 64px;
  height: 64px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 16px;
  margin-bottom: 1rem;
}

.error-icon {
  background: rgba(239, 68, 68, 0.1);
  color: #f87171;
}

.empty-icon {
  background: rgba(99, 102, 241, 0.1);
  color: #818cf8;
}

.empty-title {
  font-size: 1.15rem;
  font-weight: 700;
  color: #e2e8f0;
  margin-bottom: 0.35rem;
}

.state-text {
  font-size: 0.875rem;
  color: #64748b;
}

.state-text.error {
  color: #f87171;
}

/* ---- BOARDS GRID ---- */
.boards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 1.25rem;
}

/* ---- BOARD CARD ---- */
.board-card {
  position: relative;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.06);
  border-radius: 16px;
  overflow: hidden;
  cursor: pointer;
  transition: all 0.25s ease;
}

.board-card:hover {
  background: rgba(255, 255, 255, 0.05);
  border-color: rgba(99, 102, 241, 0.2);
  transform: translateY(-2px);
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
}

.card-accent {
  height: 4px;
  width: 100%;
}

.card-body {
  padding: 1.25rem 1.5rem 1.25rem;
}

/* Card Header */
.card-header {
  margin-bottom: 1rem;
}

.card-title {
  font-size: 1.05rem;
  font-weight: 700;
  color: #f1f5f9;
  margin-bottom: 0.35rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.card-desc {
  font-size: 0.8rem;
  color: #64748b;
  line-height: 1.5;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  min-height: 2.4em;
}

/* Card Members */
.card-members {
  display: flex;
  align-items: center;
  margin-bottom: 1rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

.member-avatars {
  display: flex;
}

.member-avatar {
  width: 28px;
  height: 28px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.65rem;
  font-weight: 700;
  color: white;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  border: 2px solid #13141f;
  margin-left: -6px;
  transition: transform 0.15s;
}

.member-avatar:first-child {
  margin-left: 0;
}

.member-avatar.owner {
  background: linear-gradient(135deg, #f59e0b, #ef4444);
}

.member-avatar:hover {
  transform: translateY(-2px);
  z-index: 1;
}

.members-more,
.members-count {
  font-size: 0.7rem;
  color: #64748b;
  margin-left: 0.5rem;
  font-weight: 500;
}

/* Card Footer */
.card-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.owner-info {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.owner-avatar {
  width: 24px;
  height: 24px;
  border-radius: 6px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.6rem;
  font-weight: 700;
  color: white;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
}

.owner-name {
  font-size: 0.75rem;
  color: #94a3b8;
  font-weight: 500;
}

/* Action Buttons */
.action-btn {
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s;
}

.delete-btn {
  width: 34px;
  height: 34px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(239, 68, 68, 0.08);
  color: #64748b;
}

.delete-btn:hover {
  background: rgba(239, 68, 68, 0.15);
  color: #f87171;
}

.badge {
  display: inline-flex;
  align-items: center;
  gap: 0.3rem;
  padding: 0.3rem 0.7rem;
  border-radius: 8px;
  font-size: 0.7rem;
  font-weight: 600;
}

.member-badge {
  background: rgba(34, 197, 94, 0.1);
  color: #4ade80;
  border: 1px solid rgba(34, 197, 94, 0.15);
}

.request-btn {
  padding: 0.35rem 0.85rem;
  font-size: 0.725rem;
  font-weight: 600;
  background: rgba(99, 102, 241, 0.1);
  color: #818cf8;
  border: 1px solid rgba(99, 102, 241, 0.15);
}

.request-btn:hover {
  background: rgba(99, 102, 241, 0.2);
  color: #a5b4fc;
}

/* ---- MODAL ---- */
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  backdrop-filter: blur(8px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
  padding: 1rem;
}

.modal-card {
  width: 100%;
  max-width: 460px;
  background: #1a1b2e;
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 20px;
  padding: 2rem;
  box-shadow: 0 25px 50px rgba(0, 0, 0, 0.5);
}

.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.modal-header h3 {
  font-size: 1.25rem;
  font-weight: 700;
  color: #f1f5f9;
}

.modal-close {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 10px;
  border: none;
  background: rgba(255, 255, 255, 0.05);
  color: #64748b;
  cursor: pointer;
  transition: all 0.2s;
}

.modal-close:hover {
  background: rgba(255, 255, 255, 0.1);
  color: #e2e8f0;
}

.modal-form {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
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

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 0.75rem 1rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 12px;
  color: #f1f5f9;
  font-size: 0.875rem;
  font-family: inherit;
  outline: none;
  transition: all 0.2s;
  box-sizing: border-box;
  resize: none;
}

.form-group input::placeholder,
.form-group textarea::placeholder {
  color: #475569;
}

.form-group input:focus,
.form-group textarea:focus {
  border-color: #6366f1;
  background: rgba(99, 102, 241, 0.05);
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.modal-actions {
  display: flex;
  gap: 0.75rem;
  margin-top: 0.5rem;
}

.btn-cancel {
  flex: 1;
  padding: 0.7rem;
  border-radius: 12px;
  border: 1px solid rgba(255, 255, 255, 0.08);
  background: transparent;
  color: #94a3b8;
  font-size: 0.85rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-cancel:hover {
  background: rgba(255, 255, 255, 0.05);
  color: #e2e8f0;
}

.btn-submit {
  flex: 1;
  padding: 0.7rem;
  border-radius: 12px;
  border: none;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.85rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  box-shadow: 0 4px 12px rgba(99, 102, 241, 0.25);
}

.btn-submit:hover {
  box-shadow: 0 6px 20px rgba(99, 102, 241, 0.35);
  transform: translateY(-1px);
}

/* Modal transition */
.modal-enter-active { transition: all 0.25s ease-out; }
.modal-leave-active { transition: all 0.2s ease-in; }
.modal-enter-from, .modal-leave-to { opacity: 0; }
.modal-enter-from .modal-card { transform: scale(0.95) translateY(10px); }
.modal-leave-to .modal-card { transform: scale(0.97) translateY(5px); }

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
  z-index: 200;
  box-shadow: 0 12px 32px rgba(0, 0, 0, 0.4);
  max-width: 440px;
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

.toast-icon {
  flex-shrink: 0;
}

.toast-msg {
  font-size: 0.825rem;
  font-weight: 500;
  line-height: 1.4;
}

.toast-close {
  flex-shrink: 0;
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(255, 255, 255, 0.06);
  border: none;
  border-radius: 6px;
  color: inherit;
  opacity: 0.6;
  cursor: pointer;
  transition: opacity 0.15s;
}

.toast-close:hover {
  opacity: 1;
}

/* Toast transition */
.toast-enter-active { transition: all 0.3s ease-out; }
.toast-leave-active { transition: all 0.25s ease-in; }
.toast-enter-from { opacity: 0; transform: translateX(-50%) translateY(20px); }
.toast-leave-to { opacity: 0; transform: translateX(-50%) translateY(10px); }

/* ---- RESPONSIVE ---- */
@media (max-width: 768px) {
  .main-content {
    padding: 1.25rem 1rem 2rem;
  }

  .page-header {
    flex-direction: column;
    gap: 1rem;
  }

  .create-btn {
    width: 100%;
    justify-content: center;
  }

  .boards-grid {
    grid-template-columns: 1fr;
  }

  .user-name {
    display: none;
  }
}
</style>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useBoardStore } from '@/stores/board'
import NotificationBell from '@/components/NotificationBell.vue'
import { joinRequestService } from '@/services/join-request.service'
import { boardMemberService } from '@/services/board-member.service'
import type { Board } from '@/types/board'
import type { BoardMember } from '@/types/board-member'

import { useToast } from '@/composables/useToast'

const router = useRouter()
const authStore = useAuthStore()
const boardStore = useBoardStore()
const { show: showToast } = useToast()

const activeTab = ref<'all' | 'my'>('all')
const showCreateModal = ref(false)
const myBoardIds = ref<Set<number>>(new Set())
const boardMembersMap = ref<Record<number, BoardMember[]>>({})
const newBoard = reactive({
  title: '',
  description: ''
})


// Color palette for board accent bars
const boardColors = [
  'linear-gradient(135deg, #6366f1, #8b5cf6)',
  'linear-gradient(135deg, #3b82f6, #06b6d4)',
  'linear-gradient(135deg, #8b5cf6, #ec4899)',
  'linear-gradient(135deg, #f59e0b, #ef4444)',
  'linear-gradient(135deg, #10b981, #3b82f6)',
  'linear-gradient(135deg, #ec4899, #f97316)',
  'linear-gradient(135deg, #14b8a6, #8b5cf6)',
]

function getBoardColor(boardId: number): string {
  return boardColors[boardId % boardColors.length] ?? 'linear-gradient(135deg, #6366f1, #8b5cf6)'
}

function isMember(boardId: number): boolean {
  return myBoardIds.value.has(boardId)
}

const filteredBoards = computed(() => {
  if (activeTab.value === 'my') {
    return boardStore.boards.filter(board => isMember(board.id))
  }
  return boardStore.boards
})

async function loadMyBoards() {
  try {
    const boards: Board[] = await boardMemberService.getMyBoards()
    myBoardIds.value = new Set(boards.map(b => b.id))
  } catch (error) {
    console.error('Failed to load my boards:', error)
  }
}

async function loadAllBoardMembers() {
  try {
    for (const board of boardStore.boards) {
      const members = await boardMemberService.getBoardMembers(board.id)
      boardMembersMap.value[board.id] = members
    }
  } catch (error) {
    console.error('Failed to load board members:', error)
  }
}

onMounted(async () => {
  authStore.loadUserFromStorage()
  await boardStore.fetchBoards()
  loadMyBoards()
  loadAllBoardMembers()
})

async function handleCreateBoard() {
  if (!authStore.user) return

  await boardStore.createBoard({
    title: newBoard.title,
    description: newBoard.description,
    ownerId: authStore.user.id
  })

  newBoard.title = ''
  newBoard.description = ''
  showCreateModal.value = false
  await loadMyBoards()
}

async function handleDeleteBoard(id: number) {
  if (confirm('Bu board\'u silmek istediğinize emin misiniz?')) {
    await boardStore.deleteBoard(id)
  }
}

async function handleRequestAccess(boardId: number) {
  if (!authStore.user) return

  try {
    await joinRequestService.createRequest({
      boardId,
      requesterId: authStore.user.id
    })
    showToast('Katılım isteği başarıyla gönderildi! Board sahibi bilgilendirilecek.', 'success')
  } catch (error: any) {
    const msg = error.response?.data || ''
    if (typeof msg === 'string' && msg.toLowerCase().includes('pending')) {
      showToast('Bu board için zaten bekleyen bir katılım isteğiniz var.', 'warning')
    } else {
      showToast('İstek gönderilemedi. Daha önce istek göndermiş olabilirsiniz.', 'error')
    }
  }
}

function handleLogout() {
  authStore.logout()
  router.push('/login')
}
</script>
