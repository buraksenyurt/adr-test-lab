# ADR-003: API Composition Root'tur

- **Durum *(Status)*** : Kabul edildi.
- **Tarih :** 2026-09-17
- **Karar Veren :** Ekip

## Bağlam

HTTP sözleşmeleri ve dependency injection kayıtlarının iş katmanlarına sızması Domain'i ASP.NET Core ortamına bağlar. Bununla birlikte çalışma zamanına sahip uygulamanın use case ve adapter seçimlerini bir noktada birleştirmesi gerekir.

## Karar

`OrderManagement.Api` çalıştırılabilir giriş noktası ve composition root olarak davranır. HTTP request tipleri `OrderManagement.Api.Contracts` namespace'inde tutulur. `Program.cs`, Application handler'ını ve Infrastructure kayıtlarını birleştirir. Domain ve Application HTTP kavramlarını bilmez.

## Sonuçlar

- Framework bağımlılıkları dışarıda kalır.
- API'nin Application ve Infrastructure projelerine referans vermesi bilinçli bir istisnadır.
- HTTP sözleşmeleri domain nesneleri yerine ayrı request tipleriyle ifade edilir.

## Doğrulama

Katmanın içeriye sızmaması `LayerDependencyTests` ile, request tiplerinin sınırda kalması `BoundaryTests.Api_Requests_Should_Reside_In_Contracts_Namespace` testiyle doğrulanır.
