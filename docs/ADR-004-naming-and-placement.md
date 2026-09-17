# ADR-004: Adlandırma ve Namespace Yerleşimi

- **Durum *(Status)*** : Kabul edildi.
- **Tarih :** 2026-09-17
- **Karar Veren :** Ekip

## Bağlam

Mimari roller yalnız proje referanslarından anlaşılmaz. Portların, adapter'ların ve use case handler'larının tutarlı adlandırılması kodun taranmasını ve otomatik mimari doğrulamayı kolaylaştırır.

## Karar

- Application portları `OrderManagement.Application.Ports` namespace'inde bulunur ve isimlerinde `Repository` kelimesini içerir.
- Persistence adapter'ları `OrderManagement.Infrastructure.Persistence` namespace'inde bulunur ve isimlerinde `Repository` kelimesini içerir.
- Sipariş handler'ları `OrderManagement.Application.Orders.Handlers` namespace'inde bulunur ve isimlerinde `Handler` kelimesini içerir.
- API request tipleri `OrderManagement.Api.Contracts` namespace'inde bulunur.

## Sonuçlar

- Yeni tiplerin rolü adından ve yerinden anlaşılır.
- Kurala uymayan bir tip test aşamasında görünür olur.
- Bu standart, farklı teknik roller doğduğunda yeni ADR belgeleri ile genişletilmelidir.

## Doğrulama

Karar `ConventionTests` sınıfındaki üç test ve `BoundaryTests.Api_Requests_Should_Reside_In_Contracts_Namespace` testiyle doğrulanır.
