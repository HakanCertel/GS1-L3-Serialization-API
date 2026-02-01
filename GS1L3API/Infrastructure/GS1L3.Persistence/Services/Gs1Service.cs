using GS1L3.Application.Services;

namespace GS1L3.Persistence.Services
{
    public class Gs1Service : IGs1Service
    {
        public string GenerateGs1String(string gtin, string sn, DateTime expiry, string lot)
        {
            // AI(01) + AI(17) + AI(10) + AI(21)
            // Not: Sabit uzunluklu olmayan AI'lar (Lot ve SN gibi) genelde sona eklenir.
            return $"(01){gtin}(17){expiry:yyMMdd}(10){lot}(21){sn}";
        }

        public string CreateSscc(string companyPrefix, int referenceNumber)
        {
            string baseCode = $"0{companyPrefix}{referenceNumber.ToString().PadLeft(16 - companyPrefix.Length, '0')}";
            return baseCode + CalculateCheckDigit(baseCode);
        }

        private int CalculateCheckDigit(string data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                int digit = int.Parse(data[i].ToString());
                sum += (i % 2 == 0) ? digit * 3 : digit * 1; // 3-1-3-1 kuralı
            }
            return (10 - (sum % 10)) % 10;
        }
    }
}
