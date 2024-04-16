using System.Text.Json.Serialization;
namespace AdditionalLibrary
{
    [Serializable]
    public class GasStation
    {
        #region [private fields]
        private int? _id;
        private string? _fullName;
        private long? _globalId;
        private string? _shortName;
        private string? _admArea;
        private string? _district;
        private string? _address;
        private string? _owner;
        private DateTime? _testDate;
        private string? _geodataCenter;
        private string? _geoarea;
        #endregion

        // Публичные свойства для обращения к полям и работы с JSON файлом.
        #region [public properties]
        [JsonPropertyName("ID")]
        public int? Id
        {
            get => _id;
            set => _id = value != null || value <= 0 ? value : throw new ArgumentException();
        }

        public string? FullName
        {
            get => _fullName;
            set => _fullName = value != null ? value : throw new ArgumentException();
        }

        [JsonPropertyName("global_id")]
        public long? GlobalId
        {
            get => _globalId;
            set => _globalId = value != null || value <= 0 ? value : throw new ArgumentException();
        }

        public string? ShortName
        {
            get => _shortName;
            set => _shortName = value != null ? value : throw new ArgumentException();
        }

        public string? AdmArea
        {
            get => _admArea;
            set => _admArea = value != null ? value : throw new ArgumentException();
        }

        public string? District
        {
            get => _district;
            set => _district = value != null ? value : throw new ArgumentException();
        }

        public string? Address
        {
            get => _address;
            set => _address = value != null ? value : throw new ArgumentException();
        }

        public string? Owner
        {
            get => _owner;
            set => _owner = value != null ? value : throw new ArgumentException();
        }

        public DateTime? TestDate
        {
            get => _testDate;
            set => _testDate = value != null ? value : throw new ArgumentException();

        }

        [JsonPropertyName("geodata_center")]
        public string? GeodataCenter
        {
            get => _geodataCenter;
            set => _geodataCenter = value;
        }

        [JsonPropertyName("geoarea")]
        public string? Geoarea
        {
            get => _geoarea;
            set => _geoarea = value;
        }
        #endregion

        public GasStation() { }

        public GasStation(int? id, string? fullName, long? globalId, string? shortName, string? admArea, string? district, string? address, string? owner, DateTime? testDate, string? geodataCenter, string? geoarea)
        {
            _id = id > 0 ? id : throw new ArgumentException();
            _fullName = fullName != null && fullName != String.Empty ? fullName : throw new ArgumentException();
            _globalId = globalId > 0 ? globalId : throw new ArgumentException();
            _shortName = shortName != null && shortName != String.Empty ? shortName : throw new ArgumentException();
            _admArea = admArea != null && admArea != String.Empty ? admArea : throw new ArgumentException();
            _district = district != null && district != String.Empty ? district : throw new ArgumentException();
            _address = address != null && address != String.Empty ? address : throw new ArgumentException();
            _owner = owner != null && owner != String.Empty ? owner : throw new ArgumentException();
            _testDate = testDate != null ? testDate : throw new ArgumentException();

            _geodataCenter = geodataCenter;
            _geoarea = geoarea;
        }
    }
}

