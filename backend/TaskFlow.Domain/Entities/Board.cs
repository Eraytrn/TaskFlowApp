using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities
{
    public class Board : BaseEntity
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int OwnerId { get; private set; }

        // Navigation Properties
        public User Owner { get; private set; }
        public ICollection<TaskItem> Tasks { get; private set; } = new List<TaskItem>();
        public ICollection<BoardMember> Members { get; private set; } = new List<BoardMember>();
        public ICollection<BoardJoinRequest> JoinRequests { get; private set; } = new List<BoardJoinRequest>();
        public ICollection<Label> Labels { get; private set; } = new List<Label>();

        private Board() { }

        public Board(string title, string? description, int ownerId)
        {
            Title = title;
            Description = description;
            OwnerId = ownerId;
        }

        // Domain method: Board bilgilerini güncelle
        public void UpdateDetails(string title, string? description)
        {
            Title = title;
            Description = description;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
