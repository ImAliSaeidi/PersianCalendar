namespace PersianCalendar.Data.Entities.Reponses
{
    public class HafezOmen
    {
        [JsonProperty("TITLE")]
        public string Title { get; set; }

        [JsonProperty("RHYME")]
        public string Poetry { get; set; }

        [JsonProperty("MEANING")]
        public string Meaning { get; set; }

        [JsonProperty("SHOMARE")]
        public string Number { get; set; }

        public override string ToString()
        {
            return
                $"غزل شماره {Number} : {Title}\n" +
                $"{new string('-', 100)}\n" +
                $"{Poetry}\n" +
                $"{new string('-', 100)}\n" +
                $"تفسیر : \n" +
                $"{Meaning}";
        }
    }
}
