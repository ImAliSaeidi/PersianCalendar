namespace PersianCalendar.Data.Entities.Reponses
{
    public class PrayerTime
    {
        public string City { get; set; }

        [JsonProperty("azan_sobh")]
        public string AzanSobh { get; set; }

        [JsonProperty("toloe_aftab")]
        public string Sunrise { get; set; }

        [JsonProperty("azan_zohre")]
        public string AzanZohr { get; set; }

        [JsonProperty("ghorob_aftab")]
        public string Sunset { get; set; }

        [JsonProperty("azan_maghreb")]
        public string AzanMaghreb { get; set; }

        [JsonProperty("nime_shabe_sharie")]
        public string NimeShabSharee { get; set; }

        public override string ToString()
        {
            return $"اوقات شرعی به افق {City}\n" +
                $"اذان صبح : {AzanSobh}\n" +
                $"طلوع آفتاب : {Sunrise}\n" +
                $"اذان ظهر : {AzanZohr}\n" +
                $"غروب آفتاب : {Sunset}\n" +
                $"اذان مغرب : {AzanMaghreb}\n" +
                $"نیمه شب شرعی : {NimeShabSharee}";
        }
    }
}
