# ADR-001: Katmanlı Bağımlılık Yönü

- **Durum *(Status)*** : Kabul edildi.
- **Tarih :** 2026-09-17
- **Karar Veren :** Ekip

## Bağlam *(Context)*

Sipariş yönetimi örneğinin iş kuralları HTTP, dependency injection ve veri saklama ayrıntılarından bağımsız kalmalıdır. Yalnız proje referanslarına güvenmek, ileride namespace veya dolaylı bağımlılıklarla sınırların aşılmasını engellemez.

## Karar *(Decision)*

Bağımlılıklar içeriye doğru akar:

- `Domain` katmanı hiçbir dış uygulama katmanına bağımlı olamaz.
- `Application` katmanı yalnızca `Domain` katmanına bağımlı olabilir.
- `Infrastructure` katmanı, `Application` tarafından tanımlanan portları uygular ve `Api` katmanına bağımlı olamaz.
- `Api`, composition root olarak `Application` ile `Infrastructure` katmanlarını birleştirebilir.

## Sonuçlar *(Consequences)*

- İş kuralları *(business rules)* framework ve adapter değişikliklerinden korunur.
- Ters bir bağımlılık eklenmeye çalışıldığında, mimari testlerde açık bir ihlal olarak raporlanır.
- Katmanlar arasında veri taşımak için sözleşmelerin doğru katmanda tanımlanması gerekir.

## Doğrulama *(How this is enforced)*

Karar `LayerDependencyTests` sınıfındaki üç ArchUnitNET testiyle doğrulanmalıdır;

- `Domain_Should_Not_Depend_On_Outer_Layers`
- `Application_Should_Not_Depend_On_Adapters_Or_Api`
- `Infrastructure_Should_Not_Depend_On_Api`
