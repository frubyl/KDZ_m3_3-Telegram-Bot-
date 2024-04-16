using Telegram.Bot;
using DataProcessing;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging;
using AdditionalLibrary;
namespace UserPart
{
    public class WorkWithUser
    {
        private BotUser _user;

        private ITelegramBotClient client;

        // Отправка файла пользователю.
        public async void SendFile()
        {
            // Проверка есть данные или нет.
            if (!_user.IsThereDataOrNot)
            {
                await client.SendTextMessageAsync(_user.ChatId, "Сначала отправьте файл!");
            }
            else
            {
                try
                {
                    JSONProcessing JP = new JSONProcessing();
                    CSVProcessing CP = new CSVProcessing();
                    using Stream? streamCSV = CP.Write(_user.Data);
                    using Stream? streamJSON = JP.Write(_user.Data);
                    // Далее отправляем JSON и CSV файлы, если при отправки какого-то произошла ошибка, оповещаем пользователя.
                    // Отправляем только те файлы, при работе с которыми не возникла ошибка.
                    if (streamJSON == null & streamCSV == null)
                    {
                        LoggingMethods.Log("WorkWithUser", "Не удалось отправить JSON и CSV", LogLevel.Error);
                        await client.SendTextMessageAsync(_user.ChatId, "Возникла непредвиденная ошибка при отправке файлов, повторите попытку!");
                    }
                    else if (streamJSON == null)
                    {
                        LoggingMethods.Log("WorkWithUser", "Не удалось отправить CSV", LogLevel.Error);
                        await client.SendTextMessageAsync(_user.ChatId, "Возникла непредвиденная ошибка при отправке json файла, повторите попытку!");
                        await client.SendDocumentAsync(_user.ChatId, InputFile.FromStream(streamCSV, "data.csv"));
                    }
                    else if (streamCSV == null)
                    {
                        LoggingMethods.Log("WorkWithUser", "Не удалось отправить CSV", LogLevel.Error);
                        await client.SendTextMessageAsync(_user.ChatId, "Возникла непредвиденная ошибка при отправке csv файла, повторите попытку!");
                        await client.SendDocumentAsync(_user.ChatId, InputFile.FromStream(streamJSON, "data.json"));
                    }
                    else
                    {
                        LoggingMethods.Log("WorkWithUser", "Отправлены JSON и CSV", LogLevel.Information);
                        await client.SendDocumentAsync(_user.ChatId, InputFile.FromStream(streamJSON, "data.json"));
                        await client.SendDocumentAsync(_user.ChatId, InputFile.FromStream(streamCSV, "data.csv"));
                    }
                }
                catch
                {
                    LoggingMethods.Log("WorkWithUser", "Не удалось отправить JSON и CSV", LogLevel.Error);
                    await client.SendTextMessageAsync(_user.ChatId, "Возникла непредвиденная ошибка, повторите попытку!");
                }
            }
        }
        // Скачиваем данные.
        public async void GetFile(Update update)
        {        var message = update.Message;
            string[] pathName = message.Document.FileName.Split('.');
            // Проверяем разрешение файла.
            if (!(pathName[pathName.Length - 1] == "json" || pathName[pathName.Length - 1] == "csv"))
            {
                LoggingMethods.Log("WorkWithUser", "Не удалось считать файл - неправильное расширение", LogLevel.Error);
                await client.SendTextMessageAsync(message.Chat.Id, "Неправильное расширение файла! Вам нужно загрузить файл с расширением: CSV или JSON");
            }
            else
            {
                try
                {
                    var fileId = update.Message.Document.FileId;
                    var fileInfo = await client.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;
                    // Скачиваем файл на рабочий стол, генерируем рандомное имя.
                    string path = "../../../../dataFromUser/" + String.Format("{0}.{1}", Path.GetRandomFileName().Replace(".", string.Empty), pathName[pathName.Length - 1]); 
                    await using Stream fileStream = System.IO.File.OpenWrite(path);
                    await client.DownloadFileAsync(filePath, fileStream);
                    fileStream.Close();
                    // В зависимости от расширения используем соответствующий метод.
                    await using FileStream newStream = System.IO.File.OpenRead(path);
                    if (pathName[pathName.Length - 1] == "json")
                    {
                        JSONProcessing JP = new JSONProcessing();
                        _user.Data = JP.Read(newStream);
                        if (!JP.GetDataOrNot)
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, "Возникла ошибка, проверьте формат и повторите попытку!");
                            LoggingMethods.Log("WorkWithUser", "Не удалось считать файл", LogLevel.Error);
                        }
                        else
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, "Данные получены!");
                            _user.IsThereDataOrNot = true;
                            LoggingMethods.Log("WorkWithUser", "Файл считан", LogLevel.Information);

                        }
                    }
                    else
                    {
                        CSVProcessing CP = new CSVProcessing();
                        _user.Data = CP.Read(newStream);
                        if (!CP.GetDataOrNot)
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, "Возникла ошибка, проверьте формат и повторите попытку!");
                            LoggingMethods.Log("WorkWithUser", "Не удалось считать файл", LogLevel.Error);
                        }
                        else
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, "Данные получены!");
                            _user.IsThereDataOrNot = true;
                            LoggingMethods.Log("WorkWithUser", "Файл считан", LogLevel.Information);

                        }
                    }
                    newStream.Close();
                }
                catch
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Возникла ошибка, проверьте формат и повторите попытку!");
                    LoggingMethods.Log("WorkWithUser", "Не удалось считать файл", LogLevel.Error);

                }
            }
        }
        // Оповещаем пользователя, что в следующих сообщениях нужно ввести данные для фильтра.
        public async void GetFilterField(string fieldName)
        {
            string message;
            switch (fieldName)
            {
                case "AdmAreaOwner":
                    message = "В следующих двух сообщениях введите данные для фильтра, в первом для AdmArea, во втором для Owner";
                    break;
                default:
                    message = "В следующем сообщении введите данные для фильтра";
                    break;
            }
            if (!_user.IsThereDataOrNot)
            {
                await client.SendTextMessageAsync(_user.ChatId, "Сначала отправьте файл!");
            }
            else
            {
                await client.SendTextMessageAsync(_user.ChatId, message);
                // Меняем свойство на true, так как нужно получить данные.
                _user.NeedToGetFilterDataOrNot = true;
                _user.NameFieldForSort = fieldName;
            }
        }
        // Реализация фильтра.
        public async void Filter(string data)
        {
            List<GasStation> newData = new List<GasStation>();
            // Сортировка по полю.
            switch (_user.NameFieldForSort)
            {
                case "AdmAreaOwner":
                    if (_user.DataForSort == "")
                    {
                        _user.DataForSort = data;
                    }
                    else
                    {
                        newData = MethodsForWorkWithData.Filter(_user.Data, _user.DataForSort, data, "AdmAreaOwner");
                        _user.DataForSort = "";
                        _user.NeedToGetFilterDataOrNot = false;
                    }
                    break;
                case "District":
                    newData = MethodsForWorkWithData.Filter(_user.Data, data, "", "District");
                    _user.NeedToGetFilterDataOrNot = false;
                    break;
                case "Owner":
                    newData = MethodsForWorkWithData.Filter(_user.Data, data, "", "Owner");
                    _user.NeedToGetFilterDataOrNot = false;
                    break;
            }
            if (newData.Count != 0 & _user.NeedToGetFilterDataOrNot == false)
            {
                _user.Data = newData;
                await client.SendTextMessageAsync(_user.ChatId, "Данные отфильтрованы!");
                LoggingMethods.Log("WorkWithUser", $"Данные отфильтрованы по полю {_user.NameFieldForSort}", LogLevel.Information);

            }
            // Если подходящих данных нет, не меняем данные.
            else if (newData.Count == 0 & _user.NeedToGetFilterDataOrNot == false)
            {
                await client.SendTextMessageAsync(_user.ChatId, "Таких объектов не существует, данные остались без изменений!");
            }
        }

        // Сортировка по полю TestDate.
        public async void SortTestData(bool ascendingOrNot)
        {
            if (!_user.IsThereDataOrNot)
            {
                await client.SendTextMessageAsync(_user.ChatId, "Сначала отправьте файл!");
            }
            else
            {
                _user.Data = MethodsForWorkWithData.Sort(_user.Data, ascendingOrNot);
                await client.SendTextMessageAsync(_user.ChatId, "Данные отсортированы!");
                LoggingMethods.Log("WorkWithUser", "Данные отсортированы", LogLevel.Information);

            }
        }

        public WorkWithUser(BotUser user, ITelegramBotClient client)
        {
            _user = user;
            this.client = client;
        }

        public WorkWithUser() { }
    }
}

