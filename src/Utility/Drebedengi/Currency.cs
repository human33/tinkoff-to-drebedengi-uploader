namespace T2DUploader.Utility.Drebedengi
{
    public record Currency
    {
        public long Id { get; set; }
        
        public string Name { get; set; } = "";

        /// <summary>
        /// Поставить 1
        /// </summary>
        public decimal Course { get; set; } = 1;
        
        /// <summary>
        /// International currency code
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// false - don't update
        /// true - update every day at noon (UTC+3)
        /// </summary>
        public bool IsAutoUpdate { get; set; } = false;
        
        public bool IsHidden { get; set; } = false;
        
        /// <summary>
        /// true - for default currency
        /// talse - for other
        /// </summary>
        public bool IsDefault { get; set; }


    }
}