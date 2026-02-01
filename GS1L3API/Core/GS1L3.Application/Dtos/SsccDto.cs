namespace GS1L3.Application.Dtos
{
    public class SsccDto
    {
        public string SSCCCode { get; set; }
        public string Level { get; set; }
        public List<string> ChildCodes { get; set; } // İçindeki SN'ler veya alt SSCC'ler
    }
}
