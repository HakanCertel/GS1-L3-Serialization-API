# GS1-L3-Serialization-API
## 🌟 Konfigürasyon
<table>
  <tr>
    <td align="center">
      <h3>Veritabanı Bağlantı Ayarı</h3>
      <p>Projeye ait appsettings.json dosyasında MssqlConnectionString anahtarına kendi bağlandı cümlenizi yazın<>
      <img width="739" height="218" alt="image" src="https://github.com/user-attachments/assets/c81e58ed-542d-4788-b8df-da507e3c0924" />
    </td>
    <td align="center">
      <h3>Migration' ların uygulanması</h3>
      <p>Projede Package Manager Console açın. Default Project alnında görseldeki projeyi seçili hale getirin ve update-database diyerek veritabanı ve tabloların oluşmasını sağlayın</p>
      <img width="752" height="81" alt="image" src="https://github.com/user-attachments/assets/45a1eb89-ad24-42f4-b1a3-dc559a87eb41" />
    </td>
  </tr>
</table>

## 🌟 Swagger , Endpoint ve Script

<table>
  <tr>
    <td align="center">
      <h3>Swagger ' in açılması</h3>
      <p>Projeyi çalıştırın ve port aderesi  https://localhost:7032/index.html olarak tarayıcıda çağırın </p>
      <img width="455" height="69" alt="image" src="https://github.com/user-attachments/assets/f7740307-17c7-4707-85d2-f373688110d7" />
      <img width="690" height="348" alt="image" src="https://github.com/user-attachments/assets/8d9f6215-3047-48f5-b490-9af9aa66ed90" />
    </td>
  </tr>
</table>
<table>
  <tr>
    <td align="center">
      <h3>Müşteri Kaydı Oluşturma</h3>
      <p>POST /api/customer endpoint request body'sine aşağıdaki script'i yazın ve çalıştrın</p> <p> {
    "Name": "BAYER",
    "GLN": "14 Haneli GLN numarası",
    "Description": "Açıklama"
} </p>
      <p>Dönen Sonuç Aşağıdaki gibi olacaktır. Dönen script' den Id değerini ürün oluştururken customerId değeri olarak kullanmak üzere kopyalayın</p>
     <img width="401" height="212" alt="image" src="https://github.com/user-attachments/assets/587375e8-ab84-4fa1-8f58-581cebfd1557" />

    </td>
  </tr>
</table>
<table>
  <tr>
    <td align="center">
      <h3>Ürün Kaydı Oluşturma</h3>
      <p>
        POST /api/product endpoint request body'sine aşağıdaki script'i yazın ve çalıştrın GTIN numarası 14 karakteri geçemez. Validation mekanizması oluşturlmadığı için hata alırsınız         </p>
      <p>
        {
          "Name": "ASPIRIN",
          "GTIN": "14 haneli GTIN",
          "CustomerId": "fb77a9c5-b33b-466e-af11-08de6178ae06"
        }
      </p>
      <p>Dönen Sonuç aşağıdaki gibidir. Busonuçta iş emri oluşturmak iiçin Id kopyalamır ve workorder request body' sinde productId alanına yağıştırılır</p>
      <img width="454" height="221" alt="image" src="https://github.com/user-attachments/assets/e782120d-6dc4-4eff-bff1-6bf1d2df0e3f" />
    </td>
  </tr>
</table>
<table>
  <tr>
    <td align="center">
      <h3>İş Emri Oluşturma</h3>
      <p>
        POST /api/workOrders endpoint request body'sine aşağıdaki script'i yazın ve çalıştrın. Sürecin bu aşamasında iş emri ile birlikte hedef miktar kadar seri numarası üretilip veri tabanına kaydedilecekti. Yani mevcut örneğimiz için 100 adet seri numarası üretilecektir. Gerçek hayatta senaryo muhtemelen seri numaraları veritabanında daha önceden tanımlanmış ve üretim aşamasında her bir ürün için bir seri numarası kullanılacak ve kullanılacak bu seri numarası pasife alınacaktır, birkez daha kullanılmaması için.
      </p>
      <p>
        {
            "ProductId":"84df8851-05aa-4194-6936-08de617b8e56",
            "LotNo":"BYRASP30012026",
            "ExpiryDate":"2026-01-30",
            "TargetQuantity":100,
            "Status":"Active",
            "SerialStartValue":"BYR"
        }
      </p>
      <p>Dönen Sonuç aşağıdaki gibidir. Birsonraki aşama iş emrini üretme aşamasıdır o yüzden dönen sunucun Id si kopyalanmalı ve üretim isteğinin body sindeki workorderId alanına yapıştırılmalıdır</p>
      <img width="547" height="116" alt="image" src="https://github.com/user-attachments/assets/bd6a983b-7590-48f6-a020-a8d1b0ed2412" />

    </td>
  </tr>
</table>
<table>
  <tr>
    <td align="center">
      <h3>Üretim Gerçekleştirme</h3>
      <p>
        POST /api/workOrders/produceWorkOrder endpoint request body'sine aşağıdaki script'i yazın üretilecek miktarı ve iş emri id sini girerek çalıştrın. Bu işlemlem sonucunda üretilecek her bir ürün için GS1 barkod numarası oluşacak ,varsayılan olarak koli içi adet 10 ve her bir palet 10 koli olacak şekil belirlenmiş olup üretilen her 10 adet ürün için bir koli SSCC kodu ve her 10 koli için bir palet SSCC kodu üretecektir
      </p>
      <p>
        {
            "ProducedQuantity": 100,
            "WorkOrderId": "49897c45-56e9-45b9-2581-08de617d77bd"
        }
      </p>
      <p>Dönen Sonuç aşağıdaki gibidir.</p>
     <img width="680" height="359" alt="image" src="https://github.com/user-attachments/assets/879808b9-42f9-4cb0-a25f-1c2f433887bb" />
      <img width="427" height="425" alt="image" src="https://github.com/user-attachments/assets/0deb321b-1684-47a0-b248-b23966dc5d6a" />
      <img width="332" height="235" alt="image" src="https://github.com/user-attachments/assets/63597124-744d-409d-8ca5-461291416c93" />


    </td>
  </tr>
</table>