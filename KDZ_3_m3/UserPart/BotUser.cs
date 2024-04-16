using AdditionalLibrary;
namespace UserPart
{
    public class BotUser
    {
        private readonly long _chatId;
        public List<GasStation> Data;
        // Загрузил ли пользователь файл с корректными данными.
        public bool IsThereDataOrNot = false;
        // Нужно ли получить данные для фильтра.
        public bool NeedToGetFilterDataOrNot = false;
        // По какому поля должна проходить сортировка.
        public string NameFieldForSort;
        // Данные для фильтра по полю AdmArea при выборе типа сортировки AdmArea и Owner.
        public string DataForSort = "";

        public long ChatId => _chatId;
        public BotUser(long chatId)
        {
            _chatId = chatId;
        }

        public BotUser() { }
    }
}
