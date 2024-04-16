using UserPart;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using AdditionalLibrary;
using Microsoft.Extensions.Logging;
namespace TelegramBot
{
    class Program
    {
        // Пользователи, которые взаимодействуют с ботом.
        private static List<BotUser> users = new List<BotUser>();

        static void Main(string[] args)
        {
            string token = "6996254690:AAHwntNx_-FjBQFCBJURZFdJ9kc_G_nf7QI";
            TelegramBotClient client = new TelegramBotClient(token);
            LoggingMethods.Log("Main", "Бот запущен", LogLevel.Information);
            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        private async static Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var message = update.Message;
            BotUser currentUser = new BotUser();
            // Проверяем, есть ли текущий пользователь в листе.
            List<BotUser> checkList = (from p in users where p.ChatId == message.Chat.Id select p).ToList();
            // Если нету, то добавляем в лист.
            if (checkList.Count == 0)
            {
                await client.SendTextMessageAsync(message.Chat.Id, $"Вас приветствует бот по работе с CSV и JSON файлами! {Environment.NewLine}{Environment.NewLine}Для обработки файла пришлите его в формате CSV или JSON.");
                LoggingMethods.Log("Main", "Был добавлен новый пользователь", LogLevel.Information);
                try
                {
                    Stream stickerStream = System.IO.File.OpenRead("../../../../sticker/st.webp");
                    await client.SendStickerAsync(message.Chat.Id, InputFile.FromStream(stickerStream));
                    stickerStream.Close();
                }
                catch { }
                #region Реализация кнопок меню
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                    new KeyboardButton[] { "Произвести выборку по District" },
                    new KeyboardButton[] { "Произвести выборку по Owner" },
                    new KeyboardButton[] { "Произвести выборку по AdmArea и Owner" },
                    new KeyboardButton[] { "Отсортировать TestDate по возрастанию" },
                    new KeyboardButton[] { "Отсортировать TestDate по убыванию" },
                    new KeyboardButton[] { "Скачать обработанный файл" }
                    })
                {
                    ResizeKeyboard = true
                };
                await client.SendTextMessageAsync(message.Chat.Id, "После отправки файла выберите пункт меню.", replyMarkup: replyKeyboardMarkup);
                #endregion
                currentUser = new BotUser(message.Chat.Id);
                users.Add(currentUser);
                // Если количество пользователей > 50, убираем первого пользователя.
                if (users.Count > 50)
                {
                    users.Remove(users[0]);
                    LoggingMethods.Log("Main", "Был удален пользователь", LogLevel.Information);
                }
            }
            else
            {
                currentUser = checkList[0];
            }
            WorkWithUser workWithUser = new WorkWithUser(currentUser, client);
            if (message != null)
            {
                if (message.Text != null)
                {
                    SwithcMenu(message.Text, currentUser, workWithUser, client);
                }
                if (message.Document != null)
                {
                    workWithUser.GetFile(update);
                }
            }
        }
        // Выбор пункта меню.
        private async static void SwithcMenu(string message, BotUser currentUser, WorkWithUser workWithUser, ITelegramBotClient client)
        {
            // Если для текущего пользователю нужно получить данные для фильтра, фильтруем данные.
            if (currentUser.NeedToGetFilterDataOrNot)
            {
                workWithUser.Filter(message);
            }
            else
            {
                switch (message)
                {
                    case "/start":
                        await client.SendTextMessageAsync(currentUser.ChatId, "Вас приветствует бот по работе с CSV и JSON файлами!");
                        break;
                    case "Произвести выборку по District":
                        workWithUser.GetFilterField("District");
                        break;
                    case "Произвести выборку по Owner":
                        workWithUser.GetFilterField("Owner");
                        break;
                    case "Произвести выборку по AdmArea и Owner":
                        workWithUser.GetFilterField("AdmAreaOwner");
                        break;
                    case "Отсортировать TestDate по возрастанию":
                        workWithUser.SortTestData(true);
                        break;
                    case "Отсортировать TestDate по убыванию":
                        workWithUser.SortTestData(false);
                        break;
                    case "Скачать обработанный файл":
                        workWithUser.SendFile();
                        break;
                };
            }
        }
        private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception.Message);
            LoggingMethods.Log("Main", $"Возникла ошибка - {exception.Message}", LogLevel.Information);
            return Task.CompletedTask;
        }
    }
}