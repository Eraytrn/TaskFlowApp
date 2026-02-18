namespace TaskFlow.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    //Burada tüm Repository'ler (User, Board, Task vb.) tek bir çatı altında toplanmış.
    //Bu sayede kodun içinde her seferinde ayrı ayrı Repository'ler oluşturmak yerine, 
    //sadece IUnitOfWork üzerinden hepsine erişebilirsin.
    IUserRepository Users { get; }
    IBoardRepository Boards { get; }
    ITaskItemRepository TaskItems { get; }
    ICommentRepository Comments { get; }
    IBoardMemberRepository BoardMembers { get; }
    IBoardJoinRequestRepository JoinRequests { get; }
    INotificationRepository Notifications { get; }
    ILabelRepository Labels { get; }

    //Diyelim ki aynı anda hem yeni bir User oluşturdun hem de ona bir Task atadın. Eğer User kaydedilir ama Task kaydedilirken
    //elektrik kesilirse veri bütünlüğü bozulur. SaveChangesAsync metodunu çağırdığında,
    //o ana kadar yapılan tüm değişiklikler veritabanına tek bir transaction (işlem) olarak gönderilir. Ya hepsi kaydedilir ya da hiçbiri.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    //Begin: "Bir işlem başlatıyorum, her şeyi izle" der.
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    //Commit: "Her şey yolunda, tüm değişiklikleri veritabanına kalıcı olarak işle."
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    //Rollback: "Bir hata oluştu! Hiçbir şeyi kaydetme, her şeyi eski haline döndür (Geri al)."
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
