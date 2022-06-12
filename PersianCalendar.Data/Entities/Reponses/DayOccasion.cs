namespace PersianCalendar.Data.Entities.Reponses
{
    public class DayOccasion
    {
        public int? Year { get; set; }

        public string Type { get; set; }

        public string Occasion { get; set; }

        public override string ToString()
        {
            var result = $"{Occasion}\n";

            if (Year != null)
            {
                result = $"{Occasion} , سال : {Year}\n";
            }

            return result;
        }
    }
}
