# ADR-005: Domain ve Application Katmanlarının Framework Bağımsızlığı

- **Durum *(Status)*** : Kabul edildi.
- **Tarih :** 2026-09-18
- **Karar Veren :** Ekip
- **İlgili :** [ADR-001](ADR-001-layered-dependency-direction.md) kararını tamamlar.

## Bağlam

ADR-001 bağımlılıkların içeriye doğru aktığını garanti eder ve bunu katmanlar arası
referanslarla doğrular. Ancak bir katmanın temiz *(clean)* kalması yalnızca diğer katmanlara
bağımlı olmamakla ölçülemez. Domain katmanı hiçbir proje referansı almadan da
serileştirme attribute'ları, logging arayüzleri veya ORM tipleriyle kirletilebilir.
Bunlar shared framework üzerinden veya tek satırlık bir NuGet paketiyle gelir ve
derleyici hiçbir uyarı vermez.

Pratikte en sık görülen sızıntı yolları şunlardır:

- Serileştirme kolaylığı için domain entity'sine `System.Text.Json` attribute'u eklenmesi.
- "Nasılsa her yerde lazım" gerekçesiyle `Microsoft.Extensions.Logging` bağımlılığı.
- Use case handler'ına doğrudan `DbContext` veya `HttpClient` taşınması.

## Karar

- `OrderManagement.Domain` katmanındaki hiçbir tip; web *(`Microsoft.AspNetCore.*`)*,
  dependency injection ve konfigürasyon *(`Microsoft.Extensions.*`)*, ORM
  *(`Microsoft.EntityFrameworkCore.*`)*, ADO.NET *(`System.Data.*`)*, HTTP istemcisi
  *(`System.Net.Http.*`)* ve serileştirme *(`System.Text.Json.*`, `Newtonsoft.Json.*`)*
  namespace'lerine bağımlı olamaz.
- `OrderManagement.Application` katmanı; web, ORM, ADO.NET ve HTTP istemcisi
  namespace'lerine bağımlı olamaz. Bu katmanda soyutlama paketlerine izin verilir,
  taşıma ve kalıcılık teknolojilerine izin verilmez.
- Bu teknolojilere duyulan ihtiyaç, ADR-002 uyarınca `Application.Ports` altında bir
  port tanımlanarak ve adaptörü `Infrastructure` katmanında uygulanarak karşılanır.
- Domain ve Application projeleri, yasaklı teknolojilere karşılık gelen NuGet
  paketlerini **kodda kullanılmasa dahi** referans edemez. Kullanılmayan bir
  referans, sonraki geliştirici için açık bir davettir ve ilgili assembly
  build çıktısına taşınır.

## Sonuçlar

- Domain nesneleri serileştirme biçiminden bağımsızdır; wire format değiştiğinde
  iş kuralları değişmez.
- Kolaylık gerekçesiyle eklenen bağımlılıklar, ekleyen kişiye test aşamasında geri döner.
- Yeni bir teknoloji sınıfı *(örneğin bir mesajlaşma kütüphanesi)* yasaklanacaksa,
  yasaklı namespace listesi bu ADR üzerinden genişletilir.
- Yasak liste tabanlıdır; listede olmayan bir framework sessizce geçer. Liste, yeni
  bağımlılık türleri ortaya çıktıkça gözden geçirilmelidir.
- Kullanım denetimi assembly metadata'sı üzerinden ArchUnitNET ile, referans denetimi
ise proje dosyaları okunarak yapılır. İkincisi ArchUnitNET'in görev alanı dışındadır:
derleyici, kullanılmayan bir paketten assembly'ye hiçbir iz bırakmaz.

## Doğrulama

Karar `FrameworkIndependenceTests` sınıfındaki iki ArchUnitNET testiyle doğrulanır;

- `Domain_Should_Not_Depend_On_Frameworks`
- `Application_Should_Not_Depend_On_Web_Or_Orm_Frameworks`
