using System;

namespace T2DUploader.Utility.Drebedengi
{
    public record Record
    {
        public decimal Sum { get; set; }
        public long CurrencyId { get; set; }
        
        /// <summary>
        /// Ид категории расходов или источника доходов
        /// Для перемещений ид счета "из которого" если сумма > 0,
        /// или "в который" если < 0. Для начальных остатков -1.
        /// </summary>
        public long ObjectId { get; set; }
        
        public long AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; } = "";
        public long? UserId { get; set; }
        public long? GroupId { get; set; }
    }
}