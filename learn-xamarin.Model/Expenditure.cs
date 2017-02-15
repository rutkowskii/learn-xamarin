using System;

namespace learn_xamarin.Model
{
    public class Expenditure
    {
        public Guid Id { get; set; }
        public decimal Sum { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}