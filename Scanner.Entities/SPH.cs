namespace Scanner.Entities
{
    public class SPH
    {
        public int SphId { get; set; }
        public int SymbolId { get; set; }
        public int CandleId { get; set; }
        public string Date { get; set; }
        public decimal Price { get; set; }
        public int StabilityInHours { get; set; }
        public decimal PanicPercentage { get; set; }
        public int PanicHours { get; set; }
        public int RecoveryHours { get; set; }

        public bool Expired { get; set; }
        public bool Success { get; set; }

        public SPH()
        {
        }
    }
}