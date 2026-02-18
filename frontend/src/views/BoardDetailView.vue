<template>
  <div class="board-detail-page">
    <!-- Navbar -->
    <nav class="navbar">
      <div class="navbar-inner">
        <div class="navbar-left">
          <button @click="router.push('/boards')" class="back-btn">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <polyline points="15 18 9 12 15 6" />
            </svg>
            <span>Geri</span>
          </button>
          <div class="board-info">
            <div class="board-icon">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4" />
              </svg>
            </div>
            <div>
              <h1 class="board-title">{{ boardStore.currentBoard?.title }}</h1>
              <p class="board-desc">{{ boardStore.currentBoard?.description }}</p>
            </div>
          </div>
        </div>
        <div class="navbar-right">
          <div class="user-badge">
            <div class="user-avatar">{{ authStore.user?.fullName?.charAt(0).toUpperCase() }}</div>
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

    <!-- Content -->
    <div class="main-content">
      <!-- Header bar -->
      <div v-if="!loading" class="content-header">
        <div class="task-count-badge">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M9 11l3 3L22 4" />
            <path d="M21 12v7a2 2 0 01-2 2H5a2 2 0 01-2-2V5a2 2 0 012-2h11" />
          </svg>
          <span>{{ taskStore.tasks.length }} Görev</span>
        </div>
        <button @click="showCreateModal = true" class="create-btn">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="12" y1="5" x2="12" y2="19" />
            <line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          <span>Yeni Görev</span>
        </button>
      </div>

      <!-- Loading -->
      <div v-if="taskStore.loading" class="state-container">
        <div class="loading-spinner"></div>
        <p class="state-text">Görevler yükleniyor...</p>
      </div>

      <!-- Kanban Board -->
      <div v-else class="kanban-board">
        <!-- Todo Column -->
        <div class="kanban-column">
          <div class="column-header col-todo">
            <div class="col-header-left">
              <div class="col-dot dot-todo"></div>
              <h3>Yapılacak</h3>
            </div>
            <span class="col-count">{{ todoTasks.length }}</span>
          </div>
          <draggable
            v-model="todoTasks"
            group="tasks"
            item-key="id"
            class="column-body"
            @change="(evt: any) => onTaskChange(evt, TaskStatus.Todo)"
          >
            <template #item="{ element: task }">
              <div class="task-card">
                <h4 @click.stop="openTaskDetail(task)" class="task-title">{{ task.title }}</h4>
                <!-- Labels on Card -->
                <div v-if="task.labels && task.labels.length > 0" class="card-labels">
                  <span v-for="label in task.labels" :key="label.id" class="card-label-bar" :style="{ backgroundColor: label.colorHex }" :title="label.name"></span>
                </div>

                <p class="task-desc">{{ task.description }}</p>
                <div class="task-footer">
                  <div v-if="task.assignedUserName" class="assignee">
                    <div class="assignee-avatar">{{ task.assignedUserName.charAt(0).toUpperCase() }}</div>
                    <span>{{ task.assignedUserName }}</span>
                  </div>
                  <div v-else class="assignee-empty"></div>
                  <div class="task-actions">
                    <button @click="moveTask(task.id, 1)" class="act-btn act-forward" title="Devam Edene Taşı">
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="9 18 15 12 9 6" /></svg>
                    </button>
                    <button @click="deleteTask(task.id)" class="act-btn act-delete" title="Sil">
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6" /><path d="M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6m3 0V4a2 2 0 012-2h4a2 2 0 012 2v2" /></svg>
                    </button>
                  </div>
                </div>
              </div>
            </template>
          </draggable>
        </div>

        <!-- In Progress Column -->
        <div class="kanban-column">
          <div class="column-header col-progress">
            <div class="col-header-left">
              <div class="col-dot dot-progress"></div>
              <h3>Devam Eden</h3>
            </div>
            <span class="col-count">{{ inProgressTasks.length }}</span>
          </div>
          <draggable
            v-model="inProgressTasks"
            group="tasks"
            item-key="id"
            class="column-body"
            @change="(evt: any) => onTaskChange(evt, TaskStatus.InProgress)"
          >
            <template #item="{ element: task }">
              <div class="task-card card-progress">
                <h4 @click.stop="openTaskDetail(task)" class="task-title">{{ task.title }}</h4>
                <p class="task-desc">{{ task.description }}</p>
                <div class="task-footer">
                  <div v-if="task.assignedUserName" class="assignee">
                    <div class="assignee-avatar">{{ task.assignedUserName.charAt(0).toUpperCase() }}</div>
                    <span>{{ task.assignedUserName }}</span>
                  </div>
                  <div v-else class="assignee-empty"></div>
                  <div class="task-actions">
                    <button @click="moveTask(task.id, 0)" class="act-btn act-back" title="Yapılacak'a Geri Taşı">
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="15 18 9 12 15 6" /></svg>
                    </button>
                    <button @click="moveTask(task.id, 2)" class="act-btn act-forward" title="Tamamlanan'a Taşı">
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="9 18 15 12 9 6" /></svg>
                    </button>
                    <button @click="deleteTask(task.id)" class="act-btn act-delete" title="Sil">
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6" /><path d="M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6m3 0V4a2 2 0 012-2h4a2 2 0 012 2v2" /></svg>
                    </button>
                  </div>
                </div>
              </div>
            </template>
          </draggable>
        </div>

        <!-- Done Column -->
        <div class="kanban-column">
          <div class="column-header col-done">
            <div class="col-header-left">
              <div class="col-dot dot-done"></div>
              <h3>Tamamlanan</h3>
            </div>
            <span class="col-count">{{ doneTasks.length }}</span>
          </div>
          <draggable
            v-model="doneTasks"
            group="tasks"
            item-key="id"
            class="column-body"
            @change="(evt: any) => onTaskChange(evt, TaskStatus.Done)"
          >
            <template #item="{ element: task }">
              <div class="task-card card-done">
                <h4 @click.stop="openTaskDetail(task)" class="task-title">{{ task.title }}</h4>
                <p class="task-desc">{{ task.description }}</p>
                <div class="task-footer">
                  <div v-if="task.assignedUserName" class="assignee">
                    <div class="assignee-avatar">{{ task.assignedUserName.charAt(0).toUpperCase() }}</div>
                    <span>{{ task.assignedUserName }}</span>
                  </div>
                  <div v-else class="assignee-empty"></div>
                  <div class="task-actions">
                    <button @click="moveTask(task.id, 1)" class="act-btn act-back" title="Devam Eden'e Geri Taşı">
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="15 18 9 12 15 6" /></svg>
                    </button>
                    <button @click="deleteTask(task.id)" class="act-btn act-delete" title="Sil">
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6" /><path d="M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6m3 0V4a2 2 0 012-2h4a2 2 0 012 2v2" /></svg>
                    </button>
                  </div>
                </div>
              </div>
            </template>
          </draggable>
        </div>
      </div>
      <div v-if="loading" class="full-page-loading">
        <div class="loading-spinner"></div>
        <p>Pano yükleniyor...</p>
      </div>
    </div>

    <!-- Create Task Modal -->
    <Transition name="modal">
      <div v-if="showCreateModal" class="modal-overlay" @click.self="showCreateModal = false">
        <div class="modal-card">
          <div class="modal-header">
            <h3>Yeni Görev Oluştur</h3>
            <button @click="showCreateModal = false" class="modal-close">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
              </svg>
            </button>
          </div>

          <form @submit.prevent="handleCreateTask" class="modal-form">
            <div class="form-group">
              <label for="taskTitle">Görev Başlığı</label>
              <input id="taskTitle" v-model="newTask.title" type="text" required placeholder="Görev adını girin" />
            </div>

            <div class="form-group">
              <label for="taskDesc">Açıklama</label>
              <textarea id="taskDesc" v-model="newTask.description" required rows="3" placeholder="Görevi açıklayın..."></textarea>
            </div>

            <div class="form-group">
              <label for="taskAssign">Atanan Kişi</label>
              <select id="taskAssign" v-model="newTask.assignedUserId">
                <option :value="undefined">Atanmamış</option>
                <option v-for="member in boardMembers" :key="member.userId" :value="member.userId">
                  {{ member.userName }} ({{ member.role === 0 ? 'Sahip' : 'Üye' }})
                </option>
              </select>
            </div>

            <div class="modal-actions">
              <button type="button" @click="showCreateModal = false" class="btn-cancel">İptal</button>
              <button type="submit" class="btn-submit">Oluştur</button>
            </div>
          </form>
        </div>
      </div>
    </Transition>

    <!-- Task Detail / Comments Modal -->
    <Transition name="modal">
      <div v-if="showDetailModal && selectedTask" class="modal-overlay" @click.self="closeTaskDetail">
        <div class="modal-card modal-detail">
          <!-- Header -->
          <div class="detail-header">
            <div class="detail-header-left">
              <div class="status-icon" :class="getStatusClass(selectedTask.status)">
                <svg v-if="selectedTask.status === TaskStatus.Done" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                  <polyline points="20 6 9 17 4 12" />
                </svg>
                <svg v-else-if="selectedTask.status === TaskStatus.InProgress" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <polygon points="13 2 3 14 12 14 11 22 21 10 12 10 13 2" />
                </svg>
                <svg v-else width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <circle cx="12" cy="12" r="10" />
                </svg>
              </div>
              <div>
                <div v-if="isEditingTitle" class="edit-title-wrapper">
                    <input 
                        v-model="selectedTask.title" 
                        @blur="handleUpdateTaskDetails" 
                        @keyup.enter="handleUpdateTaskDetails"
                        class="edit-title-input" 
                        autoFocus
                    />
                </div>
                <h3 v-else @click="isEditingTitle = true" class="detail-title editable" title="Düzenlemek için tıklayın">{{ selectedTask.title }}</h3>
                <span class="status-badge" :class="getStatusClass(selectedTask.status)">{{ getTaskStatusText(selectedTask.status) }}</span>
              </div>
            </div>
            <button @click="closeTaskDetail" class="modal-close">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
              </svg>
            </button>
          </div>

          <!-- Body -->
          <div class="detail-body">
            <!-- Description -->
            <div class="detail-section">
              <h4 class="section-label">Açıklama <span class="edit-hint">(Düzenlemek için tıklayın)</span></h4>
              <div v-if="isEditingDesc">
                  <textarea 
                    v-model="selectedTask.description" 
                    @blur="handleUpdateTaskDetails" 
                    class="edit-desc-input" 
                    rows="3"
                  ></textarea>
              </div>
              <p v-else @click="isEditingDesc = true" class="detail-desc editable">{{ selectedTask.description || 'Açıklama yok' }}</p>
            </div>

            <!-- Labels & Due Date Row -->
            <div class="detail-row">
                <!-- Labels -->
                <div class="detail-section flex-1">
                    <h4 class="section-label">Etiketler</h4>
                    <div class="labels-container">
                        <div v-for="label in taskLabels" :key="label.id" class="label-chip" :style="{ backgroundColor: label.colorHex }">
                            {{ label.name }}
                            <button @click="handleRemoveLabel(label.id)" class="remove-label-btn">×</button>
                        </div>
                        
                        <div class="add-label-wrapper">
                            <button v-if="!showLabelCreator" @click="showLabelCreator = true" class="add-label-btn">+ Etiket</button>
                            <div v-else class="label-creator-popover">
                                <h6>Etiket Ekle/Oluştur</h6>
                                <div class="available-labels">
                                    <button v-for="label in availableLabels" :key="label.id" @click="handleAddLabel(label.id)" class="avail-label-item">
                                        <span class="color-dot" :style="{ backgroundColor: label.colorHex }"></span>
                                        {{ label.name }}
                                    </button>
                                </div>
                                <div class="divider"></div>
                                <input v-model="newLabelName" placeholder="Yeni etiket adı" class="mini-input" />
                                <div class="color-picker">
                                    <button v-for="color in ['#ef4444', '#f97316', '#eab308', '#22c55e', '#3b82f6', '#8b5cf6', '#ec4899']" 
                                            :key="color" 
                                            @click="newLabelColor = color"
                                            class="color-circle" 
                                            :class="{ active: newLabelColor === color }"
                                            :style="{ backgroundColor: color }">
                                    </button>
                                </div>
                                <div class="creator-actions">
                                    <button @click="handleCreateLabel" :disabled="!newLabelName" class="btn-mini-primary">Oluştur</button>
                                    <button @click="showLabelCreator = false" class="btn-mini-text">İptal</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



            <!-- Assigned -->
            <div v-if="selectedTask.assignedUserName" class="detail-section">
              <h4 class="section-label">Atanan Kişi</h4>
              <div class="detail-assignee">
                <div class="assignee-avatar lg">{{ selectedTask.assignedUserName.charAt(0).toUpperCase() }}</div>
                <span>{{ selectedTask.assignedUserName }}</span>
              </div>
            </div>

            <!-- Comments -->
            <div class="detail-section">
              <h4 class="section-label">Yorumlar ({{ comments.length }})</h4>

              <div v-if="commentsLoading" class="comments-loading">
                <div class="mini-spinner"></div>
              </div>

              <div v-else class="comments-list">
                <div v-if="comments.length === 0" class="comments-empty">
                  <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" style="color: #475569;">
                    <path d="M21 15a2 2 0 01-2 2H7l-4 4V5a2 2 0 012-2h14a2 2 0 012 2z" />
                  </svg>
                  <span>Henüz yorum yok</span>
                </div>

                <div v-for="comment in comments" :key="comment.id" class="comment-item">
                  <div class="comment-left">
                    <div class="comment-avatar">{{ comment.userName?.charAt(0)?.toUpperCase() || '?' }}</div>
                    <div class="comment-body">
                      <div class="comment-meta">
                        <span class="comment-author">{{ comment.userName }}</span>
                        <span class="comment-time">{{ formatDate(comment.createdDate) }}</span>
                      </div>
                      <p class="comment-text">{{ comment.content }}</p>
                    </div>
                  </div>
                  <button
                    v-if="comment.userId === authStore.user?.id"
                    @click="handleDeleteComment(comment.id)"
                    class="comment-delete"
                    title="Yorumu Sil"
                  >
                    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                      <polyline points="3 6 5 6 21 6" /><path d="M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6m3 0V4a2 2 0 012-2h4a2 2 0 012 2v2" />
                    </svg>
                  </button>
                </div>
              </div>

              <!-- Add Comment -->
              <form @submit.prevent="handleAddComment" class="comment-form">
                <div class="comment-avatar sm">{{ authStore.user?.fullName?.charAt(0)?.toUpperCase() || '?' }}</div>
                <input
                  v-model="newComment"
                  type="text"
                  placeholder="Yorum yazın..."
                  :disabled="commentSubmitting"
                />
                <button type="submit" :disabled="!newComment.trim() || commentSubmitting" class="send-btn">
                  <svg v-if="commentSubmitting" class="spin" width="16" height="16" viewBox="0 0 24 24" fill="none">
                    <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="3" opacity="0.25" />
                    <path d="M4 12a8 8 0 018-8" stroke="currentColor" stroke-width="3" stroke-linecap="round" opacity="0.75" />
                  </svg>
                  <span v-else>Gönder</span>
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
      </div>
    </Transition>
  </div>
</template>

<style scoped>
/* ============================================
   BOARD DETAIL - DARK THEME
   ============================================ */

.board-detail-page {
  min-height: 100vh;
  background: #0f1117;
  color: #e2e8f0;
  font-family: 'Inter', 'Segoe UI', system-ui, -apple-system, sans-serif;
}

.full-page-loading {
  position: absolute;
  inset: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: #0f1117;
  z-index: 40;
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
  max-width: 100%;
  margin: 0 auto;
  padding: 0 1.5rem;
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.navbar-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.back-btn {
  display: flex;
  align-items: center;
  gap: 0.3rem;
  padding: 0.4rem 0.75rem;
  background: transparent;
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 10px;
  color: #94a3b8;
  font-size: 0.8rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.back-btn:hover {
  color: #e2e8f0;
  background: rgba(255, 255, 255, 0.05);
  border-color: rgba(255, 255, 255, 0.12);
}

.board-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.board-icon {
  width: 36px;
  height: 36px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 10px;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
}

.board-title {
  font-size: 1rem;
  font-weight: 700;
  color: #f1f5f9;
  margin: 0;
}

.board-desc {
  font-size: 0.7rem;
  color: #64748b;
  margin: 0;
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
  padding: 0.3rem 0.7rem 0.3rem 0.3rem;
  background: rgba(255, 255, 255, 0.05);
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.06);
}

.user-avatar {
  width: 28px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 7px;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.7rem;
  font-weight: 700;
}

.user-name {
  font-size: 0.8rem;
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
  padding: 1.5rem;
}

.content-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.task-count-badge {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.06);
  border-radius: 10px;
  color: #94a3b8;
  font-size: 0.8rem;
  font-weight: 500;
}

.create-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.55rem 1.1rem;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.8rem;
  font-weight: 600;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  transition: all 0.2s;
  box-shadow: 0 4px 12px rgba(99, 102, 241, 0.25);
}

.create-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 6px 20px rgba(99, 102, 241, 0.35);
}

/* ---- STATES ---- */
.state-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 5rem 2rem;
}

.loading-spinner {
  width: 36px;
  height: 36px;
  border: 3px solid rgba(255, 255, 255, 0.06);
  border-top-color: #6366f1;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
  margin-bottom: 1rem;
}

.state-text {
  font-size: 0.85rem;
  color: #64748b;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* ---- KANBAN BOARD ---- */
.kanban-board {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.25rem;
}

/* ---- KANBAN COLUMN ---- */
.kanban-column {
  display: flex;
  flex-direction: column;
  border-radius: 16px;
  overflow: hidden;
  background: rgba(255, 255, 255, 0.02);
  border: 1px solid rgba(255, 255, 255, 0.05);
}

.column-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.85rem 1rem;
}

.col-header-left {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.col-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
}

.dot-todo { background: #64748b; }
.dot-progress { background: #3b82f6; }
.dot-done { background: #22c55e; }

.column-header h3 {
  font-size: 0.825rem;
  font-weight: 700;
  color: #e2e8f0;
  margin: 0;
}

.col-count {
  padding: 0.15rem 0.55rem;
  border-radius: 8px;
  font-size: 0.7rem;
  font-weight: 700;
  background: rgba(255, 255, 255, 0.06);
  color: #94a3b8;
}

.col-todo { border-bottom: 2px solid rgba(100, 116, 139, 0.3); }
.col-progress { border-bottom: 2px solid rgba(59, 130, 246, 0.3); }
.col-done { border-bottom: 2px solid rgba(34, 197, 94, 0.3); }

.column-body {
  flex: 1;
  padding: 0.75rem;
  min-height: 350px;
  display: flex;
  flex-direction: column;
  gap: 0.6rem;
}

/* ---- TASK CARD ---- */
.task-card {
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.06);
  border-radius: 12px;
  padding: 1rem;
  cursor: grab;
  transition: all 0.2s;
}

.task-card:hover {
  background: rgba(255, 255, 255, 0.06);
  border-color: rgba(255, 255, 255, 0.1);
  transform: translateY(-1px);
}

.task-card:active {
  cursor: grabbing;
}

.card-progress {
  border-left: 3px solid rgba(59, 130, 246, 0.5);
}

.card-done {
  border-left: 3px solid rgba(34, 197, 94, 0.5);
}

.task-title {
  font-size: 0.85rem;
  font-weight: 600;
  color: #f1f5f9;
  margin: 0 0 0.35rem 0;
  cursor: pointer;
  transition: color 0.15s;
}

.task-title:hover {
  color: #818cf8;
}

.task-desc {
  font-size: 0.75rem;
  color: #64748b;
  line-height: 1.4;
  margin: 0 0 0.75rem 0;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.task-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.assignee {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  font-size: 0.7rem;
  color: #94a3b8;
}

.assignee-avatar {
  width: 22px;
  height: 22px;
  border-radius: 6px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.6rem;
  font-weight: 700;
}

.assignee-avatar.lg {
  width: 28px;
  height: 28px;
  font-size: 0.7rem;
}

.assignee-empty {
  height: 22px;
}

.task-actions {
  display: flex;
  gap: 4px;
  opacity: 0;
  transition: opacity 0.15s;
}

.task-card:hover .task-actions {
  opacity: 1;
}

.act-btn {
  width: 26px;
  height: 26px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 6px;
  border: none;
  cursor: pointer;
  transition: all 0.15s;
  background: transparent;
}

.act-forward {
  color: #818cf8;
}

.act-forward:hover {
  background: rgba(99, 102, 241, 0.15);
}

.act-back {
  color: #94a3b8;
}

.act-back:hover {
  background: rgba(255, 255, 255, 0.08);
}

.act-delete {
  color: #64748b;
}

.act-delete:hover {
  color: #f87171;
  background: rgba(239, 68, 68, 0.1);
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

.modal-detail {
  max-width: 520px;
  max-height: 85vh;
  display: flex;
  flex-direction: column;
  padding: 0;
}

.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.modal-header h3 {
  font-size: 1.15rem;
  font-weight: 700;
  color: #f1f5f9;
  margin: 0;
}

.modal-close {
  width: 34px;
  height: 34px;
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
  gap: 1.1rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.form-group label {
  font-size: 0.75rem;
  font-weight: 600;
  color: #94a3b8;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.form-group input,
.form-group textarea,
.form-group select {
  width: 100%;
  padding: 0.7rem 0.9rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 10px;
  color: #f1f5f9;
  font-size: 0.85rem;
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
.form-group textarea:focus,
.form-group select:focus {
  border-color: #6366f1;
  background: rgba(99, 102, 241, 0.05);
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.form-group select {
  appearance: none;
  background-image: url("data:image/svg+xml,%3Csvg width='12' height='12' viewBox='0 0 24 24' fill='none' stroke='%2364748b' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' xmlns='http://www.w3.org/2000/svg'%3E%3Cpolyline points='6 9 12 15 18 9'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 0.75rem center;
  padding-right: 2.25rem;
}

.form-group select option {
  background: #1a1b2e;
  color: #e2e8f0;
}

.modal-actions {
  display: flex;
  gap: 0.75rem;
  margin-top: 0.5rem;
}

.btn-cancel {
  flex: 1;
  padding: 0.65rem;
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.08);
  background: transparent;
  color: #94a3b8;
  font-size: 0.825rem;
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
  padding: 0.65rem;
  border-radius: 10px;
  border: none;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.825rem;
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

/* ---- DETAIL MODAL ---- */
.detail-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  padding: 1.5rem 1.5rem 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.06);
}

.detail-header-left {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
}

.status-icon {
  width: 36px;
  height: 36px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  margin-top: 2px;
}

.status-icon.status-todo {
  background: rgba(100, 116, 139, 0.15);
  color: #94a3b8;
}

.status-icon.status-progress {
  background: rgba(59, 130, 246, 0.15);
  color: #60a5fa;
}

.status-icon.status-done {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
}

.detail-title {
  font-size: 1.1rem;
  font-weight: 700;
  color: #f1f5f9;
  margin: 0 0 0.3rem 0;
}

.status-badge {
  display: inline-block;
  padding: 0.15rem 0.55rem;
  border-radius: 6px;
  font-size: 0.65rem;
  font-weight: 600;
}

.status-badge.status-todo {
  background: rgba(100, 116, 139, 0.15);
  color: #94a3b8;
}

.status-badge.status-progress {
  background: rgba(59, 130, 246, 0.15);
  color: #60a5fa;
}

.status-badge.status-done {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
}

.detail-body {
  flex: 1;
  overflow-y: auto;
  padding: 1.25rem 1.5rem;
}

.detail-section {
  margin-bottom: 1.5rem;
}

.section-label {
  font-size: 0.7rem;
  font-weight: 600;
  color: #64748b;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin: 0 0 0.5rem 0;
}

.detail-desc {
  font-size: 0.85rem;
  color: #94a3b8;
  line-height: 1.6;
  margin: 0;
}

.detail-assignee {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.85rem;
  color: #e2e8f0;
  font-weight: 500;
}

/* ---- COMMENTS ---- */
.comments-loading {
  display: flex;
  justify-content: center;
  padding: 2rem;
}

.mini-spinner {
  width: 20px;
  height: 20px;
  border: 2px solid rgba(255, 255, 255, 0.06);
  border-top-color: #6366f1;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

.comments-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.comments-empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.4rem;
  padding: 2rem;
  font-size: 0.8rem;
  color: #475569;
}

.comment-item {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  padding: 0.75rem;
  border-radius: 10px;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid rgba(255, 255, 255, 0.04);
}

.comment-item:hover .comment-delete {
  opacity: 1;
}

.comment-left {
  display: flex;
  align-items: flex-start;
  gap: 0.6rem;
  flex: 1;
}

.comment-avatar {
  width: 26px;
  height: 26px;
  border-radius: 7px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.6rem;
  font-weight: 700;
  flex-shrink: 0;
  margin-top: 1px;
}

.comment-avatar.sm {
  width: 24px;
  height: 24px;
  font-size: 0.55rem;
}

.comment-body {
  flex: 1;
}

.comment-meta {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.2rem;
}

.comment-author {
  font-size: 0.75rem;
  font-weight: 600;
  color: #e2e8f0;
}

.comment-time {
  font-size: 0.65rem;
  color: #475569;
}

.comment-text {
  font-size: 0.8rem;
  color: #94a3b8;
  line-height: 1.4;
  margin: 0;
}

.comment-delete {
  width: 26px;
  height: 26px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 6px;
  border: none;
  background: transparent;
  color: #475569;
  cursor: pointer;
  opacity: 0;
  transition: all 0.15s;
}

.comment-delete:hover {
  color: #f87171;
  background: rgba(239, 68, 68, 0.1);
}

.comment-form {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.comment-form input {
  flex: 1;
  padding: 0.6rem 0.85rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 10px;
  color: #f1f5f9;
  font-size: 0.8rem;
  font-family: inherit;
  outline: none;
  transition: all 0.2s;
}

.comment-form input::placeholder {
  color: #475569;
}

.comment-form input:focus {
  border-color: #6366f1;
  background: rgba(99, 102, 241, 0.05);
  box-shadow: 0 0 0 2px rgba(99, 102, 241, 0.1);
}

.send-btn {
  padding: 0.55rem 1rem;
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: white;
  font-size: 0.75rem;
  font-weight: 600;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  transition: all 0.2s;
  white-space: nowrap;
}

.send-btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.send-btn:hover:not(:disabled) {
  box-shadow: 0 4px 12px rgba(99, 102, 241, 0.3);
}

.spin {
  animation: spin 1s linear infinite;
}

/* ---- RESPONSIVE ---- */
@media (max-width: 1024px) {
  .kanban-board {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .column-body {
    min-height: 200px;
  }
}

@media (max-width: 640px) {
  .user-name {
    display: none;
  }

  .main-content {
    padding: 1rem;
  }
}
/* ---- TASK LABELS & DATE ---- */
.card-labels {
  display: flex;
  flex-wrap: wrap;
  gap: 4px;
  margin-bottom: 6px;
}

.card-label-bar {
  width: 32px;
  height: 6px;
  border-radius: 3px;
  display: inline-block;
}

.card-due-date {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 0.7rem;
  color: #94a3b8;
  margin-bottom: 6px;
  padding: 2px 6px;
  background: rgba(255, 255, 255, 0.03);
  border-radius: 4px;
  width: fit-content;
}

.card-due-date.overdue {
  color: #f87171;
  background: rgba(248, 113, 113, 0.1);
}

/* ---- DETAIL EDITING ---- */
.detail-title.editable {
  cursor: pointer;
  border: 1px solid transparent;
  border-radius: 6px;
  padding: 2px 6px;
  margin-left: -6px;
  transition: all 0.2s;
}

.detail-title.editable:hover {
  background: rgba(255, 255, 255, 0.05);
  border-color: rgba(255, 255, 255, 0.1);
}

.edit-title-wrapper {
  margin-bottom: 0.3rem; 
}

.edit-title-input {
  width: 100%;
  font-size: 1.1rem;
  font-weight: 700;
  color: #f1f5f9;
  background: rgba(0, 0, 0, 0.2);
  border: 1px solid #6366f1;
  border-radius: 6px;
  padding: 4px 8px;
  outline: none;
}

.edit-hint {
  font-size: 0.6rem;
  color: #64748b;
  font-weight: 400;
  margin-left: 0.5rem;
  text-transform: none;
  opacity: 0.7;
}

.detail-desc.editable {
    cursor: pointer;
    border: 1px solid transparent;
    padding: 6px;
    border-radius: 6px;
    margin: -6px;
    transition: all 0.2s;
}

.detail-desc.editable:hover {
    background: rgba(255, 255, 255, 0.03);
    border-color: rgba(255, 255, 255, 0.08);
}

.edit-desc-input {
    width: 100%;
    background: rgba(0, 0, 0, 0.2);
    border: 1px solid #6366f1;
    border-radius: 8px;
    padding: 8px;
    color: #f1f5f9;
    font-family: inherit;
    font-size: 0.85rem;
    line-height: 1.6;
    outline: none;
    resize: vertical;
}

/* ---- DETAIL ROW (Labels & Date) ---- */
.detail-row {
    display: flex;
    gap: 1.5rem;
    margin-bottom: 1.5rem;
}

.flex-1 {
    flex: 1;
}

.detail-row .detail-section {
    margin-bottom: 0;
}

/* ---- LABELS UI ---- */
.labels-container {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
    align-items: center;
    position: relative;
}

.label-chip {
    display: flex;
    align-items: center;
    gap: 6px;
    padding: 4px 8px;
    border-radius: 6px;
    font-size: 0.75rem;
    font-weight: 600;
    color: white;
    text-shadow: 0 1px 2px rgba(0,0,0,0.3);
}

.remove-label-btn {
    background: none;
    border: none;
    color: rgba(255,255,255,0.7);
    cursor: pointer;
    padding: 0;
    font-size: 1rem;
    line-height: 1;
    display: flex;
    align-items: center;
}

.remove-label-btn:hover {
    color: white;
}

.add-label-btn {
    padding: 4px 10px;
    background: rgba(255, 255, 255, 0.05);
    border: 1px solid rgba(255, 255, 255, 0.1);
    border-radius: 6px;
    color: #94a3b8;
    font-size: 0.75rem;
    cursor: pointer;
    transition: all 0.2s;
}

.add-label-btn:hover {
    background: rgba(255, 255, 255, 0.1);
    color: #e2e8f0;
}

.add-label-wrapper {
    position: relative;
}

.label-creator-popover {
    position: absolute;
    top: 100%;
    left: 0;
    margin-top: 8px;
    background: #1e293b;
    border: 1px solid rgba(255, 255, 255, 0.1);
    border-radius: 12px;
    padding: 12px;
    width: 220px;
    z-index: 50;
    box-shadow: 0 10px 25px rgba(0,0,0,0.5);
}

.label-creator-popover h6 {
    margin: 0 0 8px 0;
    font-size: 0.75rem;
    color: #94a3b8;
    text-transform: uppercase;
}

.available-labels {
    display: flex;
    flex-wrap: wrap;
    gap: 6px;
    margin-bottom: 10px;
    max-height: 120px;
    overflow-y: auto;
}

.avail-label-item {
    display: flex;
    align-items: center;
    gap: 6px;
    background: rgba(255, 255, 255, 0.05);
    border: none;
    padding: 4px 8px;
    border-radius: 4px;
    color: #e2e8f0;
    font-size: 0.75rem;
    cursor: pointer;
    transition: background 0.2s;
}

.avail-label-item:hover {
    background: rgba(255, 255, 255, 0.1);
}

.color-dot {
    width: 8px;
    height: 8px;
    border-radius: 50%;
}

.divider {
    height: 1px;
    background: rgba(255, 255, 255, 0.1);
    margin: 8px 0;
}

.mini-input {
    width: 100%;
    background: rgba(0, 0, 0, 0.3);
    border: 1px solid rgba(255, 255, 255, 0.1);
    border-radius: 6px;
    padding: 6px;
    color: #f1f5f9;
    font-size: 0.8rem;
    margin-bottom: 8px;
    outline: none;
}

.mini-input:focus {
    border-color: #6366f1;
}

.color-picker {
    display: flex;
    gap: 6px;
    margin-bottom: 10px;
}

.color-circle {
    width: 18px;
    height: 18px;
    border-radius: 50%;
    border: 2px solid transparent;
    cursor: pointer;
    transition: transform 0.2s;
}

.color-circle.active {
    border-color: white;
    transform: scale(1.1);
}

.creator-actions {
    display: flex;
    gap: 8px;
}

.btn-mini-primary {
    flex: 1;
    background: #6366f1;
    color: white;
    border: none;
    padding: 4px;
    border-radius: 4px;
    font-size: 0.75rem;
    cursor: pointer;
}

.btn-mini-primary:disabled {
    opacity: 0.5;
    cursor: not-allowed;
}

.btn-mini-text {
    flex: 1;
    background: transparent;
    color: #94a3b8;
    border: none;
    font-size: 0.75rem;
    cursor: pointer;
}

.btn-mini-text:hover {
    color: #e2e8f0;
}

/* ---- DATE INPUT ---- */
.date-input {
    padding: 6px 10px;
    background: rgba(255, 255, 255, 0.05);
    border: 1px solid rgba(255, 255, 255, 0.1);
    border-radius: 8px;
    color: #f1f5f9;
    font-family: inherit;
    font-size: 0.85rem;
    outline: none;
    cursor: pointer;
}

.date-input:focus {
    border-color: #6366f1;
}

/* Adjust for mobile */
@media (max-width: 640px) {
    .detail-row {
        flex-direction: column;
        gap: 1rem;
    }
}
</style>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useBoardStore } from '@/stores/board'
import { useTaskStore } from '@/stores/task'
import { TaskStatus } from '@/types/task'
import type { TaskItem } from '@/types/task'
import type { Comment } from '@/types/comment'
import type { BoardMember } from '@/types/board-member'
import type { Label } from '@/types/label'
import { commentService } from '@/services/comment.service'
import { boardMemberService } from '@/services/board-member.service'
import { labelService } from '@/services/label.service'
import { taskService } from '@/services/task.service'
import draggable from 'vuedraggable'
import { useToast } from '@/composables/useToast'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()
const boardStore = useBoardStore()
const taskStore = useTaskStore()
const { show: showToast } = useToast()

const loading = ref(true) // Full page loading
const showCreateModal = ref(false)
const boardMembers = ref<BoardMember[]>([])
const newTask = reactive({
  title: '',
  description: '',
  assignedUserId: undefined as number | undefined
})

// Task Detail & Comments state
const showDetailModal = ref(false)
const selectedTask = ref<TaskItem | null>(null)
const comments = ref<Comment[]>([])
const commentsLoading = ref(false)
const newComment = ref('')
const commentSubmitting = ref(false)

// Editing State
const isEditingTitle = ref(false)
const isEditingDesc = ref(false)
const taskLabels = ref<Label[]>([])
const availableLabels = computed(() => boardStore.currentBoard?.labels || [])
const newLabelName = ref('')
const newLabelColor = ref('#3b82f6')
const showLabelCreator = ref(false)

function getStatusClass(status: TaskStatus): string {
  switch (status) {
    case TaskStatus.Todo: return 'status-todo'
    case TaskStatus.InProgress: return 'status-progress'
    case TaskStatus.Done: return 'status-done'
    default: return 'status-todo'
  }
}

function getTaskStatusText(status: TaskStatus): string {
  switch (status) {
    case TaskStatus.Todo: return 'Yapılacak'
    case TaskStatus.InProgress: return 'Devam Eden'
    case TaskStatus.Done: return 'Tamamlanan'
    default: return 'Bilinmiyor'
  }
}

const todoTasks = computed({
  get: () => taskStore.tasks.filter(t => t.status === TaskStatus.Todo),
  set: (_value) => {
    // Required by vuedraggable
  }
})

const inProgressTasks = computed({
  get: () => taskStore.tasks.filter(t => t.status === TaskStatus.InProgress),
  set: (_value) => {
    // Required by vuedraggable
  }
})

const doneTasks = computed({
  get: () => taskStore.tasks.filter(t => t.status === TaskStatus.Done),
  set: (_value) => {
    // Required by vuedraggable
  }
})

async function loadBoardMembers() {
  try {
    const boardId = Number(route.params.id)
    boardMembers.value = await boardMemberService.getBoardMembers(boardId)
  } catch (error) {
    console.error('Failed to load board members:', error)
  }
}

onMounted(async () => {
  authStore.loadUserFromStorage()
  const boardId = Number(route.params.id)
  loading.value = true
  try {
      await boardStore.fetchBoard(boardId) // This might throw 403/404
      await taskStore.fetchTasksByBoard(boardId) // This might throw 403
      loadBoardMembers()
  } catch (err: any) {
      if (err.response && (err.response.status === 403 || err.response.status === 404)) {
          // Expected error for access denied, no need to log stack trace
          console.warn('Access denied or board not found (Redirecting...)')
      } else {
          console.error('Failed to load board:', err)
      }
      showToast("Bu panoya erişim yetkiniz yok!", 'error')
      router.push('/boards')
  } finally {
      loading.value = false
  }
})

async function handleCreateTask() {
  const boardId = Number(route.params.id)

  await taskStore.createTask({
    title: newTask.title,
    description: newTask.description,
    boardId,
    assignedUserId: newTask.assignedUserId || undefined
  })

  newTask.title = ''
  newTask.description = ''
  newTask.assignedUserId = undefined
  showCreateModal.value = false
}

async function moveTask(taskId: number, newStatus: number) {
  try {
    await taskStore.updateTaskStatus(taskId, newStatus)
  } catch (err) {
    console.error('Failed to move task:', err)
  }
}

async function deleteTask(taskId: number) {
  if (confirm('Bu görevi silmek istediğinize emin misiniz?')) {
    try {
      await taskStore.deleteTask(taskId)
    } catch (err) {
      console.error('Failed to delete task:', err)
    }
  }
}

async function onTaskChange(evt: any, targetStatus: TaskStatus) {
  if (evt.added) {
    const task = evt.added.element
    if (task && task.id) {
      try {
        await taskStore.updateTaskStatus(task.id, targetStatus)
      } catch (err) {
        console.error('Failed to update task status via drag:', err)
      }
    }
  }
}

function handleLogout() {
  authStore.logout()
  router.push('/login')
}

async function openTaskDetail(task: TaskItem) {
  selectedTask.value = task
  showDetailModal.value = true
  commentsLoading.value = true
  try {
    comments.value = await commentService.getByTaskId(task.id)
    // Init editing state
    taskLabels.value = task.labels ? [...task.labels] : []
    isEditingTitle.value = false
    isEditingDesc.value = false
  } catch (err) {
    console.error('Failed to load comments:', err)
    comments.value = []
  } finally {
    commentsLoading.value = false
  }
}

function closeTaskDetail() {
  showDetailModal.value = false
  selectedTask.value = null
  comments.value = []
  newComment.value = ''
}

async function handleAddComment() {
  if (!newComment.value.trim() || !selectedTask.value || !authStore.user) return
  commentSubmitting.value = true
  try {
    const created = await commentService.create({
      content: newComment.value.trim(),
      taskItemId: selectedTask.value.id,
      userId: authStore.user.id
    })
    comments.value.push(created)
    newComment.value = ''
  } catch (err) {
    console.error('Failed to add comment:', err)
  } finally {
    commentSubmitting.value = false
  }
}

async function handleDeleteComment(commentId: number) {
  try {
    await commentService.delete(commentId)
    comments.value = comments.value.filter(c => c.id !== commentId)
  } catch (err) {
    console.error('Failed to delete comment:', err)
  }
}

function formatDate(dateStr: string): string {
  const date = new Date(dateStr)
  const now = new Date()
  const diffMs = now.getTime() - date.getTime()
  const diffMin = Math.floor(diffMs / 60000)
  if (diffMin < 1) return 'az önce'
  if (diffMin < 60) return `${diffMin}dk önce`
  const diffHr = Math.floor(diffMin / 60)
  if (diffHr < 24) return `${diffHr}sa önce`
  const diffDay = Math.floor(diffHr / 24)
  return `${diffDay}g önce`
}




// --- Editing Handlers ---

async function handleUpdateTaskDetails() {
  if (!selectedTask.value) return
  
  if (!selectedTask.value.title.trim()) return

  try {
    // Optimistic update in store (or wait for reload)
    // We'll update backend first
    await taskStore.updateTask({
       id: selectedTask.value.id,
       title: selectedTask.value.title,
       description: selectedTask.value.description,
       assignedUserId: selectedTask.value.assignedUserId
    })
    
    // Update local task object to reflect new date format from backend (if needed) or just trust local
    // For now, taskStore handles fetching valid list? No, updateTask usually updates local state.
    
    isEditingTitle.value = false
    isEditingDesc.value = false
  } catch (err) {
    console.error('Failed to update task:', err)
  }
}

async function handleAddLabel(labelId: number) {
    if(!selectedTask.value) return;
    const label = availableLabels.value.find(l => l.id === labelId)
    if(!label) return;
    if(taskLabels.value.some(l => l.id === labelId)) return;

    try {
        await taskService.assignLabel(selectedTask.value.id, labelId)
        taskLabels.value.push(label)
        
        // Update task in store to show on card
        const task = taskStore.tasks.find(t => t.id === selectedTask.value?.id)
        if(task) {
            if(!task.labels) task.labels = []
            task.labels.push(label)
        }
    } catch(err) { console.error(err) }
}

async function handleRemoveLabel(labelId: number) {
    if(!selectedTask.value) return;
    try {
        await taskService.removeLabel(selectedTask.value.id, labelId)
        taskLabels.value = taskLabels.value.filter(l => l.id !== labelId)
        
        // Update task in store
        const task = taskStore.tasks.find(t => t.id === selectedTask.value?.id)
        if(task && task.labels) {
            task.labels = task.labels.filter(l => l.id !== labelId)
        }
    } catch(err) { console.error(err) }
}

async function handleCreateLabel() {
    if(!boardStore.currentBoard || !newLabelName.value) return;
    try {
        const newLabel = await labelService.create(boardStore.currentBoard.id, newLabelName.value, newLabelColor.value)
        if(!boardStore.currentBoard.labels) boardStore.currentBoard.labels = []
        boardStore.currentBoard.labels.push(newLabel)
        
        // Add to current task immediately
        await handleAddLabel(newLabel.id)
        
        newLabelName.value = ''
        showLabelCreator.value = false
    } catch(err) { console.error(err) }
}
</script>
