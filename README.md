# TaskFlow - Modern Proje Yönetim Aracı

TaskFlow, takımların projelerini yönetmesini sağlayan, Kanban tabanlı, modern arayüze sahip bir masaüstü ve web uygulamasıdır.

## 🚀 Özellikler

*   **Kanban Panosu:** Yapılacak, Devam Eden ve Tamamlanan görevlerinizi sürükle-bırak ile yönetin.
*   **Takım Çalışması:** Arkadaşlarınızı boardlarınıza davet edin, görev atayın.
*   **Yetkilendirme:** Sadece board üyeleri içeriği görebilir ve düzenleyebilir.
*   **Bildirimler:** Katılma istekleri ve görev atamaları için anlık bildirimler.
*   **Etiketler:** Görevleri renklendirilmiş etiketlerle kategorize edin.
*   **Masaüstü Uygulaması:** Windows için tek bir `.exe` olarak kurulabilir.

## 🛠️ Teknolojiler

*   **Backend:** .NET 8, EF Core, SQL Server, JWT Auth
*   **Frontend:** Vue 3, TypeScript, Pinia, TailwindCSS
*   **Desktop:** Electron.js (Backend process management)

---

## 👨‍💻 Geliştiriciler İçin Kurulum

Projeyi bilgisayarınızda geliştirmek için aşağıdaki adımları izleyin.

### Gereksinimler
*   .NET 8 SDK
*   Node.js (v18+)
*   SQL Server (Express veya Developer)

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/KULLANICI_ADINIZ/TaskFlow.git
cd TaskFlow
```

### 2. Backend Kurulumu
1.  `backend/TaskFlow.API` klasörüne gidin.
2.  `appsettings.Development.json` dosyasını oluşturun (veya `appsettings.json`'u kopyalayın) ve SQL bağlantı cümlenizi girin:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.\\SQLEXPRESS;Initial Catalog=TaskFlowDb;Integrated Security=True;TrustServerCertificate=True;"
    }
    ```
3.  Veritabanını oluşturun:
    ```bash
    dotnet tool install --global dotnet-ef
    dotnet ef database update
    ```
4.  Projeyi başlatın:
    ```bash
    dotnet run
    ```

### 3. Frontend Kurulumu
1.  Yeni bir terminalde `frontend` klasörüne gidin.
2.  Bağımlılıkları yükleyin:
    ```bash
    npm install
    ```
3.  Uygulamayı başlatın:
    ```bash
    npm run dev
    ```

---

## 📦 Masaüstü Uygulaması (Kullanım)

GitHub Releases üzerinden indirdiğiniz `TaskFlow-Setup.exe` dosyasını kurduktan sonra yapmanız gerekenler:

1.  Uygulamayı kurun.
2.  Uygulamanın kurulu olduğu klasöre gidin (Genellikle: `C:\Users\Kullanici\AppData\Local\Programs\TaskFlow\resources\backend` veya benzeri).
3.  `appsettings.json` dosyasını Not Defteri ile açın.
4.  `"DefaultConnection"` kısmına **kendi SQL Server** bağlantı cümlenizi yazın.
    *   Örnek: `"Server=LOCALHOST\\SQLEXPRESS;Initial Catalog=TaskFlowDb;..."`
5.  Uygulamayı çalıştırın!


