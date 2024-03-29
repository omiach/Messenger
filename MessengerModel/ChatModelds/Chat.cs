﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerModel.MessageModels;

namespace MessengerModel.ChatModelds
{
    public class Chat
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Public { get; set; } = false;
        public ICollection<UserChats> ChatUsers { get; set; } = new List<UserChats>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<DeletedMessage> DeletedMessages { get; set; } = new List<DeletedMessage>();
    }
}
