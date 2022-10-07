﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerModel
{
    public class Chat
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<UserChats> ChatUsers { get; set; } = null!;
        public List<Message>? Messages { get; set; }
    }
}
