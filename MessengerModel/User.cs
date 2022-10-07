﻿namespace MessengerModel
{
    public class User
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<UserChats>? UserChats { get; set; }
        public List<Message>? Messages { get; set; }
        public List<UserContacts>? Contacts { get; set; }
    }
}