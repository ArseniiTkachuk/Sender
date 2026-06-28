using Sender.Application.Interfaces;
using Sender.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sender.Application
{
    public class MessageCRUD
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IEmailProvider _emailProvider;
        private readonly ITelegramlProvider _telegramlProvider;

        public MessageCRUD(IMessageRepository messageRepository, IContactRepository contactRepository, IEmailProvider emailProvider, ITelegramlProvider telegramlProvider)
        {
            _messageRepository = messageRepository;
            _contactRepository = contactRepository;
            _emailProvider = emailProvider;
            _telegramlProvider = telegramlProvider;
        }

        public async Task<Result<Message>> CreateMessageAsync(string subject, string body, Guid userId)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                Subject = subject,
                Body = body,
                UserId = userId
            };

            await _messageRepository.AddAsync(message);

            return Result<Message>.Success(message, 201);
        }

        public async Task<Result<Message>> GetOneMessageAsync(Guid userId, Guid messageId)
        {
            Message message = await _messageRepository.GetByIdAsync(messageId);

            if (message == null)
            {
                return Result<Message>.Failure("Не вдалося знайти шаблон повідомлення", 404);
            }

            if (message.UserId != userId)
            {
                return Result<Message>.Failure("Цей шаблон повідомлення не ваш", 403);
            }

            return Result<Message>.Success(message);
        }

        public async Task<Result<List<Message>>> GetAllMessageAsync(Guid userId)
        {
            List<Message> messages = await _messageRepository.GetAllByUserIdAsync(userId);

            return Result<List<Message>>.Success(messages);
        }

        public async Task<Result<Message>> UpdateMessageAsync(Guid messageId, string subject, string body, Guid userId)
        {
            Message message = await _messageRepository.GetByIdAsync(messageId);

            if (message == null)
            {
                return Result<Message>.Failure("Не вдалося знайти шаблон повідомлення", 404);
            }

            if (message.UserId != userId)
            {
                return Result<Message>.Failure("Цей шаблон повідомлення не ваш", 403);
            }

            message.Subject = subject ?? message.Subject;
            message.Body = body ?? message.Body;

            await _messageRepository.UpdateAsync(message);
            return Result<Message>.Success(message);
        }

        public async Task<Result<Unit>> DeleteMessageAsync(Guid messageId, Guid userId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);

            if (message == null)
            {
                return Result<Unit>.Failure("Не вдалося знайти шаблон повідомлення", 404);
            }

            if (message.UserId != userId)
            {
                return Result<Unit>.Failure("Цей шаблон повідомлення не ваш", 403);
            }

            await _messageRepository.RemoveAsync(message);
            return Result<Unit>.Success();

        }


        public async Task<Result<Unit>> SendMessageAsync(Guid userId, List<ContactTarget> contactTargets, MessageContent? messageContent, Guid? messageId)
        {
            MessageContent messageToSend;

            if (messageContent != null)
            {
                messageToSend = messageContent;
            }
            else if (messageId.HasValue)
            {
                var message = await _messageRepository.GetByIdAsync(messageId.Value);

                if (message == null)
                    return Result<Unit>.Failure("Не вдалося знайти шаблон повідомлення", 404);

                if (message.UserId != userId)
                    return Result<Unit>.Failure("Цей шаблон повідомлення не ваш", 403);

                messageToSend = new MessageContent
                {
                    Subject = message.Subject,
                    Body = message.Body
                };
            }
            else
            {
                return Result<Unit>.Failure("Потрібно вказати або messageId, або об'єкт Message", 400);
            }


            var failedContacts = new List<string>();

            foreach (var contactTarget in contactTargets)
            {
                foreach (string canal in contactTarget.Channels)
                {
                    try
                    {
                        if (canal.Equals("email", StringComparison.OrdinalIgnoreCase))
                        {
                            string? email = await _contactRepository.GetEmailAsync(contactTarget.ContactId);
                            if (string.IsNullOrEmpty(email)) throw new Exception("Email не знайдено");

                            await _emailProvider.SendAsync(messageToSend.Subject, messageToSend.Body, email);
                        }
                        else if (canal.Equals("telegram", StringComparison.OrdinalIgnoreCase))
                        {
                            string? username = await _contactRepository.GetTelegramUsernameAsync(contactTarget.ContactId);
                            if (string.IsNullOrEmpty(username)) throw new Exception("Telegram username не знайдено");

                            await _telegramlProvider.SendAsync(messageToSend.Subject, messageToSend.Body, username);
                        }
                    }
                    catch (Exception ex)
                    {
                        failedContacts.Add($"Контакт {contactTarget.ContactId} ({canal}): {ex.Message}");
                    }
                }
            }

            // Якщо є помилки, повертаємо їх користувачу
            if (failedContacts.Any())
            {
                string errorMessage = "Повідомлення відправлено частково. Помилки: " + string.Join("; ", failedContacts);
                return Result<Unit>.Failure(errorMessage, 207);
            }

            return Result<Unit>.Success();
        }


    }
}