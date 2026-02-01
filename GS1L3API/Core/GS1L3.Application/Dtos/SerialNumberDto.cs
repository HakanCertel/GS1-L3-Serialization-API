namespace GS1L3.Application.Dtos
{
    public class SerialNumberDto
    {
        public string SN { get; set; }
        public string FullGs1Code { get; set; } // (01)GTIN(21)SN...
    }
}
