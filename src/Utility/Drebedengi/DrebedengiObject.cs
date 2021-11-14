namespace T2DUploader.Utility.Drebedengi
{
    public record DrebedengiObject
    {
        /// <summary>
        /// Ид категории/источника/счета
        /// </summary>
        public long Id { get; set; }
        public long ParentId { get; set; }
        public ObjectType Type { get; set; }
        public string Name { get; set; } = "";
        
        /// <summary>
        /// Для импорта из других программ, всегда пустая строка 
        /// </summary>
        public string UserId { get; set; } = "";
        
        /// <summary>
        /// Not used yet
        /// </summary>
        public bool IsCreditCard { get; set; }
        public bool IsHidden { get; set; }
        public bool IsForDuty { get; set; }
        public long Sort { get; set; }

        /// <summary>
        /// Не обязательный, ид иконки
        /// </summary>
        public long? IconId { get; set; }
        
        /// <summary>
        /// Не обязательный, автоскрытие при нулевом балансе для долговых счетов
        /// </summary>
        public bool? IsAutohide { get; set; }
    }
}