using AdditionalLibrary;
namespace DataProcessing
{
    public class CSVProcessing
    {
        // Лист объектов, полученных при считывании.
        public List<GasStation> Stations { get; set; }

        // Удалось ли получить данные.
        public bool GetDataOrNot { get; set; } = false;

        /// <summary>
        /// Чтение файла CSV.
        /// </summary>
        /// <param name="fileStream"> Поток, где находится файл. </param>
        /// <returns> Лист объектов из файла. </returns>
        /// <exception cref="FormatException"> Ошибка, которая возникает при неправильном количестве строк. </exception>
        public List<GasStation> Read(FileStream fileStream)
        {
            List<GasStation> objects = new List<GasStation>();

            try
            {
                objects = new List<GasStation>();
                string[] data = File.ReadAllLines(fileStream.Name);
                if (data.Length <= 2)
                {
                    throw new FormatException();
                }
                CheckCap(data[0], data[1]);
                for (int i = 2; i < data.Length; i++)
                {
                    try
                    {
                        string[] row = data[i].Split(';');
                        int? id = int.Parse(row[0].Trim('\"'));
                        string? fullName = row[1].Trim('\"');
                        long? globalId = long.Parse(row[2].Trim('\"'));
                        string? shortName = row[3].Trim('\"');
                        string? admArea = row[4].Trim('\"');
                        string? district = row[5].Trim('\"');
                        string address = row[6].Trim('\"');
                        string? owner = row[7].Trim('\"');
                        DateTime? testDate = DateTime.Parse(row[8].Trim('\"'));
                        string? geodataCenter = row[9].Trim('\"');
                        string? geoarea = row[10].Trim('\"');
                        GasStation s = new GasStation(id, fullName, globalId, shortName, admArea, district, address, owner, testDate, geodataCenter, geoarea);
                        objects.Add(s);
                    }
                    catch
                    {
                        // Если не удалось создать объект, пропускаем строку.
                        continue;
                    }

                }
                // Присваиваем переменной значение - лист считанных объектов.
                Stations = objects;
                // true - удалось получить данные.
                GetDataOrNot = true;
                fileStream.Close();
                return objects;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // false - не удалось получить данные.
                GetDataOrNot = false;
                fileStream.Close();
                return new List<GasStation>();
            }
        }

        /// <summary>
        /// Запись файла CSV.
        /// </summary>
        /// <param name="objects"> Объекты для записи. </param>
        /// <returns> Поток, где находится записанный файл. </returns>
        public Stream? Write(List<GasStation> objects)
        {
            Stream fileStream = null;
            try
            {
                // На рабочем столе создаем новый файл с рандомным именем.
                string path = "../../../../dataFromUser/" + String.Format("{0}.csv", Path.GetRandomFileName().Replace(".", string.Empty));
                string[] data = new string[objects.Count + 2];
                // Записываем шапку.
                data[0] = "\"ID\";\"FullName\";\"global_id\";\"ShortName\";\"AdmArea\";\"District\";\"Address\";\"Owner\";\"TestDate\";\"geodata_center\";\"geoarea\";";
                data[1] = "\"Код\";\"Полное официальное наименование\";\"global_id\";\"Сокращенное наименование\";\"Административный округ\";\"Район\";\"Адрес\";\"Наименование компании\";\"Дата проверки\";\"geodata_center\";\"geoarea\";";
                // Записываем данные.
                for (int i = 0; i < objects.Count; i++)
                {
                    var obj = objects[i];
                    string row = $"\"{obj.Id}\";\"{obj.FullName}\";\"{obj.GlobalId}\";\"{obj.ShortName}\";\"{obj.AdmArea}\";\"{obj.District}\";\"{obj.Address}\";\"{obj.Owner}\";\"{obj.TestDate}\";\"{obj.GeodataCenter}\";\"{obj.Geoarea}\";";
                    data[i + 2] = row;
                }
                File.WriteAllLines(path, data);
                fileStream = File.OpenRead(path);
                return fileStream;
            }
            catch
            {
                return fileStream;
            }
        }
        /// <summary>
        /// Проверка шапки CSV файла для чтения.
        /// </summary>
        /// <param name="first"> Первая строка файла. </param>
        /// <param name="second"> Вторая строка файла. </param>
        private void CheckCap(string first, string second)
        {
            if (first != "\"ID\";\"FullName\";\"global_id\";\"ShortName\";\"AdmArea\";\"District\";\"Address\";\"Owner\";\"TestDate\";\"geodata_center\";\"geoarea\";")
            {
                throw new ArgumentException();
            }
            if (second != "\"Код\";\"Полное официальное наименование\";\"global_id\";\"Сокращенное наименование\";\"Административный округ\";\"Район\";\"Адрес\";\"Наименование компании\";\"Дата проверки\";\"geodata_center\";\"geoarea\";")
            {
                throw new ArgumentException();
            }
        }
        public CSVProcessing() { }
    }
}
