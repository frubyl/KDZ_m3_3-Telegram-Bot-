using System.Text.Encodings.Web;
using System.Text.Json;
using AdditionalLibrary;
namespace DataProcessing
{
    public class JSONProcessing
    {
        // Список объектов, полученных при чтении.
        public List<GasStation> Stations { get; set; }

        // Получилось ли считать данные.
        public bool GetDataOrNot { get; set; } = false;

        /// <summary>
        /// Чтение файла JSON из потока.
        /// </summary>
        /// <param name="fileStream"> Поток для чтения. </param>
        /// <returns> Лист объектов из файла. </returns>
        /// 
        public List<GasStation> Read(FileStream fileStream)
        {
            try
            {
                var objects = JsonSerializer.Deserialize<List<GasStation>>(fileStream);
                Stations = objects;
                GetDataOrNot = true;
                fileStream.Close();
                return objects;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                GetDataOrNot = false;
                fileStream.Close();
                return new List<GasStation>();
            }
        }
        /// <summary>
        /// Запись файла JSON.
        /// </summary>
        /// <param name="objects"> Объекты для записи. </param>
        /// <returns> Поток, где находится записанный файл. </returns>
        public Stream? Write(List<GasStation> objects)
        {
            Stream fileStream = null;
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                // На рабочем столе создаем новый файл с рандомным именем.
                string path = "../../../../dataFromUser/" + String.Format("{0}.json", Path.GetRandomFileName().Replace(".", string.Empty)); ;
                string jsonData = JsonSerializer.Serialize(objects, options);
                File.WriteAllText(path, jsonData);
                fileStream = File.OpenRead(path);
                return fileStream;
            }
            catch
            {
                return fileStream;
            }
        }

        public JSONProcessing() { }
    }
}

