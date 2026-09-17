# Adr Unit Tests

Büyük çaplı projelerde *(kurumsal çözümlerde diyebiliriz)* bazı şeylerin garanti altına alınması gerekir. Örneğin uygulamanın mimarisinde geliştirici kaynaklı yapılabilecek bir takım hatalar derleme aşamalarında fark edilmez. Kod bir anda üretim ortamına kadar çıkabilir. Bu tip istenmeyen durumların önüne geçmek için kullanılabilecek farklı yollar var. Kodun kontrollü şekilde çıkartılması için bir review sürecinden geçmesi ve pull request kullanımı bu yöntemlerden birisidir. Ancak birde genel mimariyi etkileyebilecek, statik kod tarama araçları tarafından değerlendirilmeyen hususlar da vardır. Bir katmanın erişmemesi gereken bir katmandan nesne kullanmaya çalışması, geliştirilen bir adaptörün olması gereken katmanda durmaması vb

İşte bu gibi senaryolarda ADR *(Architecture Decision Record)* belgelerince kayıt altına alınan bir takım garantilerin kodun derlenmesi sırasında en azından birim testler yoluyla denetlenmesi çok işe yarar. Özellikle Java dünyasında bu amaçla kullanılan [ArchUnit](https://archunitnet.readthedocs.io/en/stable/) paketleri .Net tarafında da ele alınabilir. Bu repodaki çalışmanın amacı ArcUnitNet kütüphanesinin nasıl kullanıldığına dair bir demo sunmaktır.

## Solution Hakkında

Çözümde çok iddialı bir mimari tasarımı ele almıyoruz. Sadece üzerinde ADR kuralları işleteceğimiz bir solution olması yeterli. Kabaca aşağıdaki bağımlıkların söz konusu olduğunu söyleyebiliriz.

![Layers Overview](Layers.png)

Api çalıştırılabilir giriş noktası ve composition root rolündedir. Application, use case ile repository portlarını içerir. Infrastructure, portların adaptörlerini sağlar. Domain katmanı malum business içerisindeki taban nesneleri taşır ve en önemlisi dış katmanları bilmez *(Business'ı koruma ilkesi)*

Proje referansların yönü aşağıdaki tabloda olduğu gibidir;

| **Proje** | **Referans verdiği uygulama projeleri** |
| --- | --- |
| `OrderManagement.Domain` | Yok |
| `OrderManagement.Application` | `Domain` |
| `OrderManagement.Infrastructure` | `Application` |
| `OrderManagement.Api` | `Application`, `Infrastructure` |
| `OrderManagement.ArchitectureTests` | Dört uygulama projesine de |

Infrastructure, Application üzerinden kullanılan Domain türlerini geçişli referanslarla görebilir. Application katmanının Infrastructure'a referans vermemesi bizim için kritik sınırdır.

## Solution İskeletinin Oluşturulması

Aşağıdaki adımları izleyerek .NET 10 tabanlı solution ve içeriğini oluşturabiliriz.

```bash
# Solution ve projelerin oluşturulması
dotnet new sln -n OrderManagement
dotnet new classlib -n OrderManagement.Domain -o src/OrderManagement.Domain -f net10.0
dotnet new classlib -n OrderManagement.Application -o src/OrderManagement.Application -f net10.0
dotnet new classlib -n OrderManagement.Infrastructure -o src/OrderManagement.Infrastructure -f net10.0
dotnet new web -n OrderManagement.Api -o src/OrderManagement.Api -f net10.0
dotnet new xunit -n OrderManagement.ArchitectureTests -o tests/OrderManagement.ArchitectureTests -f net10.0

# Projelerin solution'a eklenmesi
dotnet sln add src/OrderManagement.Domain/OrderManagement.Domain.csproj
dotnet sln add src/OrderManagement.Application/OrderManagement.Application.csproj
dotnet sln add src/OrderManagement.Infrastructure/OrderManagement.Infrastructure.csproj
dotnet sln add src/OrderManagement.Api/OrderManagement.Api.csproj
dotnet sln add tests/OrderManagement.ArchitectureTests/OrderManagement.ArchitectureTests.csproj

# Projelerin ihtiyaçı olan referans projelerin eklenmesi
dotnet add src/OrderManagement.Application reference src/OrderManagement.Domain
dotnet add src/OrderManagement.Infrastructure reference src/OrderManagement.Application
dotnet add src/OrderManagement.Api reference src/OrderManagement.Application src/OrderManagement.Infrastructure
dotnet add tests/OrderManagement.ArchitectureTests reference src/OrderManagement.Domain src/OrderManagement.Application src/OrderManagement.Infrastructure src/OrderManagement.Api

# Gereksiz Class1'lerin silinmesi
rm src/OrderManagement.Domain/Class1.cs
rm src/OrderManagement.Application/Class1.cs
rm src/OrderManagement.Infrastructure/Class1.cs 

# ve mimari test paketinin eklenmesi
dotnet add tests/OrderManagement.ArchitectureTests package TngTech.ArchUnitNET.xUnit

# Infrastructure projesine aşağıdaki Nuget bağımlılığı eklenmeli
# Microsoft.Extensions.DependencyInjection.Abstractions
```

Bundan sonraki kısımları kodlardan takip edebiliriz. Odaklanmamız gereken yer test projesi içeriği ve ADR dokümanları olacak.

## ADR Dokümanları

Projede ele alacağımız ADR dökümanları docs klasörü altında yer alacaktır.

- [ADR 001 Layered Dependency Direction](docs/ADR-001-layered-dependency-direction.md)
- [ADR 002 Ports and Adapters](docs/ADR-002-ports-and-adapters.md)
- [ADR 003 Api as Composition Root](docs/ADR-003-api-as-composition-root.md)
- [ADR 004 Naming and Placement](docs/ADR-004-naming-and-placement.md)

## Testler

Tüm mimari testler tamamlandığında projeyi normal şekilde test edebiliriz. İster komut satırından ister Visual Studio gibi IDE ortamlarından.

```bash
dotnet test
```

## Örnek İhlal Vakaları

//EKLENECEK
