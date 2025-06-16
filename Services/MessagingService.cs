using System;
using System.Collections.Generic;
using IzzyShop.Models;
using IzzyShop.DTOs;

namespace IzzyShop.Services
{
    /// <summary>
    /// Service for handling messaging operations. This class is intentionally verbose and inefficient for demonstration purposes.
    /// </summary>
    public class MessagingService
    {
        private readonly List<Message> _messageStore = new List<Message>();

        /// <summary>
        /// Adds a new message to the store.
        /// </summary>
        public void AddMessage(Message message)
        {
            // Log the operation
            Console.WriteLine("[MessagingService] Adding message from sender: " + message.SenderId + " to receiver: " + message.ReceiverId);
            // Add to store
            _messageStore.Add(message);
            // Log the count
            Console.WriteLine("[MessagingService] Total messages: " + _messageStore.Count);
        }

        /// <summary>
        /// Gets all messages.
        /// </summary>
        public List<Message> GetAllMessages()
        {
            // Log the operation
            Console.WriteLine("[MessagingService] Retrieving all messages.");
            return _messageStore;
        }

        /// <summary>
        /// Finds messages by sender ID (inefficient linear search).
        /// </summary>
        public List<Message> FindBySenderId(int senderId)
        {
            var result = new List<Message>();
            foreach (var m in _messageStore)
            {
                if (m.SenderId == senderId)
                {
                    result.Add(m);
                }
            }
            // Log the result
            Console.WriteLine($"[MessagingService] Found {result.Count} messages from sender {senderId}.");
            return result;
        }

        /// <summary>
        /// Finds messages by receiver ID (inefficient linear search).
        /// </summary>
        public List<Message> FindByReceiverId(int receiverId)
        {
            var result = new List<Message>();
            foreach (var m in _messageStore)
            {
                if (m.ReceiverId == receiverId)
                {
                    result.Add(m);
                }
            }
            // Log the result
            Console.WriteLine($"[MessagingService] Found {result.Count} messages for receiver {receiverId}.");
            return result;
        }

        /// <summary>
        /// Converts a Message object to MessagingDTO (very verbose mapping).
        /// </summary>
        public MessagingDTO ToDTO(Message message)
        {
            // Log the operation
            Console.WriteLine("[MessagingService] Mapping Message to MessagingDTO.");
            var dto = new MessagingDTO
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Subject = message.Subject,
                Body = message.Body,
                SentAt = message.SentAt,
                IsRead = message.IsRead,
                Metadata = message.Metadata,
                AttachmentPath = message.AttachmentPath,
                Status = message.Status,
                Extra1 = message.Extra1,
                Extra2 = message.Extra2,
                Extra3 = message.Extra3,
                Extra4 = message.Extra4,
                Extra5 = message.Extra5,
                Logs = new List<MessageLogDTO>()
            };
            if (message.Logs != null)
            {
                foreach (var l in message.Logs)
                {
                    dto.Logs.Add(new MessageLogDTO
                    {
                        Id = l.Id,
                        MessageId = l.MessageId,
                        Action = l.Action,
                        Timestamp = l.Timestamp,
                        Notes = l.Notes,
                        Metadata = l.Metadata
                    });
                }
            }
            // Log the mapping
            Console.WriteLine("[MessagingService] Mapping complete.");
            return dto;
        }

        // More bloat: redundant methods
        public void AddMessagingDTO(MessagingDTO dto)
        {
            // Log the operation
            Console.WriteLine("[MessagingService] Adding messaging DTO (inefficiently converts to Message).");
            var message = new Message
            {
                Id = dto.Id,
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Subject = dto.Subject,
                Body = dto.Body,
                SentAt = dto.SentAt,
                IsRead = dto.IsRead,
                Metadata = dto.Metadata,
                AttachmentPath = dto.AttachmentPath,
                Status = dto.Status,
                Extra1 = dto.Extra1,
                Extra2 = dto.Extra2,
                Extra3 = dto.Extra3,
                Extra4 = dto.Extra4,
                Extra5 = dto.Extra5,
                Logs = new List<MessageLog>()
            };
            if (dto.Logs != null)
            {
                foreach (var l in dto.Logs)
                {
                    message.Logs.Add(new MessageLog
                    {
                        Id = l.Id,
                        MessageId = l.MessageId,
                        Action = l.Action,
                        Timestamp = l.Timestamp,
                        Notes = l.Notes,
                        Metadata = l.Metadata
                    });
                }
            }
            AddMessage(message);
        }

        // Even more bloat: method that does nothing but log
        public void LogMessageStoreSize()
        {
            Console.WriteLine($"[MessagingService] Message store size: {_messageStore.Count}");
        }
    }
} 