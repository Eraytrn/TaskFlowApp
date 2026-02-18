using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; private set; }
        public string Email{ get; private set; }
        public string PasswordHash{ get; private set; }

        // Navigation Properties
        public ICollection<Board> OwnedBoards { get; private set; } = new List<Board>();
        public ICollection<TaskItem> AssignedTasks { get; private set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
        public ICollection<BoardMember> BoardMemberships { get; private set; } = new List<BoardMember>();
        public ICollection<Notification> Notifications { get; private set; } = new List<Notification>();

        //veritabanından veri çekerken (örneğin bir ToList() attığında), Entity Framework Core (EF) bu boş constructor'ı kullanır.
        //private olmasıının nedeni dısarıdan birisi bos bir user nesnesi olusturmasın diye (parametresiz cunku)
        private User() { }

        public User(string fullName, string email, string password)
        {
            FullName = fullName;
            Email = email;
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }
}
