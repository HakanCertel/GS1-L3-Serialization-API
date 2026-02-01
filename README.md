# GS1-L3-Serialization-API
## 🌟 Genel Bakış

**Proje**, Müşteri , Ürün ve İş Emri kayıtlarının oluşturulduğu ve üretim işleminin gerçekleştirilerek  GS1 serilazasyon ve agregasyon sürecin
n yapılandırıldığı bir projedir.

Bu proje **.NET Core Web API** ile geliştirilmiş olup **Serilog** ile log mimarisi oluşturulmuştur.

## 🛠️ Teknolojiler

| Kategori | Teknolojiler |
| :--- | :--- |
| **Backend** | C#/.NET 8, Asp.Net Core Web Api,MSSQL Server, Entity Framework Core, Serilog, DependencyInjection |

## 🌟 Konfigürasyon
<table>
  <tr>
    <td align="center">
      <h3>Veritabanı Bağlantı Ayarı</h3>
      <p>Projeye ait appsettings.json dosyasında DefaultConnectionString anahtarına kendi bağlandı cümlenizi yazın<>
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
      <h4>
        {
          "Name": "ASPIRIN",
          "GTIN": "14 haneli GTIN",
          "CustomerId": "fb77a9c5-b33b-466e-af11-08de6178ae06"
        }
      </h4>
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
      <h4>
        {
            "ProductId":"84df8851-05aa-4194-6936-08de617b8e56",
            "LotNo":"BYRASP30012026",
            "ExpiryDate":"2026-01-30",
            "TargetQuantity":100,
            "Status":"Active",
            "SerialStartValue":"BYR"
        }
      </h4>
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
        POST /api/workOrders/produceWorkOrder endpoint request body'sine aşağıdaki script'i yazın üretilecek miktarı ve iş emri id sini girerek çalıştrın. Bu işlemlem sonucunda üretilecek her bir ürün için GS1 barkod numarası oluşacak ,varsayılan olarak koli içi adet 10 ve her bir palet 10 koli olacak şekil belirlenmiş olup üretilen her 10 adet ürün için bir koli SSCC kodu ve her 10 koli için bir palet SSCC kodu üretecektir.
      </p>
      <p> 
      Gerçek hayat uygulamasında bu entpoint 4 aşamaya bölünmüş olacak. Birinci aşama ürün üretilecek sensör ürünü görecek ve GS1 kodu üretilecek. İkinc aşamada yazıcı barkodu ürün üstüne basar. Üçüncü aşama üretilen GS1 kodu ile yazıcının yazdığı barkodun eşleşmesinikontrol edecek olan endpoint olacaktır. Dördüncü aşama onaylanan her ürün barkodu okutularak bir koliye konulacak ve koli içi adet tamamlandığında okutulan ürünler için bir SSCC kodu üretecek endpoint oluşturulacak. Son aşamada ise koliler iin basılan SSCC kodları okutularak palet oluşturulacak ve palet dolduğunda okutulan bütün koli SSCC ' lerine bağlı bir Parent SSCC yani palet etiketi basılarak üretim süreci tamamlanacaktır
      </p>
      <h4>
        {
            "ProducedQuantity": 100,
            "WorkOrderId": "49897c45-56e9-45b9-2581-08de617d77bd"
        }
      </h4>
      <p>Dönen Sonuç aşağıdaki gibidir. Burada dönen sonuç üretilen iş emri ile ilgili istenilen rapor sonucunu döndermektedir. Bu sonucu bir sonraki endpoint' i uygulayarakda alabilirsiniz</p>
           <img width="601" height="408" alt="image" src="https://github.com/user-attachments/assets/e70d41d3-8eea-4e91-85bd-ab430c84e26a" />
          <img width="426" height="366" alt="image" src="https://github.com/user-attachments/assets/c35a8683-0626-48a7-9994-afbf15ecdb8f" />
              <img width="319" height="246" alt="image" src="https://github.com/user-attachments/assets/7b1c17e1-e687-4891-a098-100a13d06d7b" />
   </td>
  </tr>
</table>
<table>
  <tr>
    <td align="center">
      <h3>Rapor Sayfası</h3>
      <p> GET /api/WorkOrders/{id}/full-report endpoint' inde '{id}' yerine işleme alınmış yani üretimi başlatırlmış bir iş emrinin ID' si yazılırsa bir önceki aşamada yapmış olduğumuz üretime ilişkin olarak bu iş emrine bağlı nekadar üretim yapıldığı, durumu,kalan miktarı ve üretilen herbir ürüne ait GS1 kodu,hesaplanmış herbir koli ve palet sayısında SSCC kodu ve ilişkisellikleri raporlanmaktadır <h4> Oluşan Rapor bir önceki endpoint' in döndüğü sonuçla aynı olacaktır</h4></p>
           <img width="601" height="408" alt="image" src="https://github.com/user-attachments/assets/e70d41d3-8eea-4e91-85bd-ab430c84e26a" />
          <img width="426" height="366" alt="image" src="https://github.com/user-attachments/assets/c35a8683-0626-48a7-9994-afbf15ecdb8f" />
              <img width="319" height="246" alt="image" src="https://github.com/user-attachments/assets/7b1c17e1-e687-4891-a098-100a13d06d7b" />
    </td>
  </tr>
</table>