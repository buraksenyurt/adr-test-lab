# ADR-002: Repository İçin Ports and Adapters

- **Durum *(Status)*** : Kabul edildi.
- **Tarih :** 2026-09-17
- **Karar Veren :** Ekip

## Bağlam

Sipariş oluşturma use case'i siparişi depolamak zorundadır ancak depolama teknolojisini bilmemelidir. API'nin somut bir repository sınıfını kullanması *(concrete class)*, HTTP katmanını doğrudan persistence detaylarına bağlar.

## Karar

`IOrderRepository` arayüzü ile tanımlanan port bileşeni `OrderManagement.Application.Ports` namespace'inde yer alır. Adapter bu portu `OrderManagement.Infrastructure.Persistence` namespace'inde implemente eder. API, somut adaptörü doğrudan oluşturmaz veya kullanmaz. Eşleme Infrastructure katmanında dependency injection genişletmesinde yapılır.

## Sonuçlar

- Use case depolama teknolojisinden bağımsızdır ve adaptör değiştirilebilir. 
- Composition root'un Infrastructure katmanındaki DI  extension'ını çağırmasına izin verilir fakat API kodunun persistence namespace'indeki tiplere bağımlılığı yasaktır.

## Doğrulama

Karar `BoundaryTests.Api_Should_Not_Depend_On_Persistence_Adapters` testiyle korunur. Port ve adapter yerleşimlerinin adlandırılması ayrıca `ConventionTests` içinde doğrulanır.
