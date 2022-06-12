namespace PersianCalendar.Data.Entities.Reponses
{
    public class OneApiResult<T>
    {
        public string Status { get; set; }

        public T Result { get; set; }
    }
}
