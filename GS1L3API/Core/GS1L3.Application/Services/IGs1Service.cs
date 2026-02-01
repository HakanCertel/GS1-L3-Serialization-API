namespace GS1L3.Application.Services
{
    public interface IGs1Service
    {
        public string GenerateGs1String(string gtin, string sn, DateTime expiry, string lot);
        public string CreateSscc(string companyPrefix, int referenceNumber);
    }
}
