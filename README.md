# TELERADIOLOGY

![SR Badge](https://img.shields.io/badge/SR-95%25-blue)  

Kurumsal teleradyoloji yönetim sistemi. .NET 8 tabanlı çok katmanlı mimari, MediatR ile CQRS, EF Core + PostgreSQL, ASP.NET Core Identity (JWT), Serilog, hız sınırlama, önbellekleme ve yerelleştirme özelliklerini içerir.

- .NET 8 Web API (Swagger ile)
- Katmanlar: Domain, Application, Infrastructure, WebAPI, Tests
- Kimlik Doğrulama: ASP.NET Core Identity + JWT
- Veri Erişimi: EF Core (PostgreSQL)
- Günlükleme: Serilog (PostgreSQL sink)
- Önbellek: In-Memory Cache
- Rate limiting (FixedWindow)
- Yerelleştirme (en, tr)
- E-posta ve SMS gönderimi

## SR Oranı
- Bu proje için SR: %95

## Mimari
- Domain: Entity, Enum ve Repository arayüzleri
- Application: CQRS (MediatR), Validator/Behavior, DTO/Mapping (AutoMapper)
- Infrastructure: EF Core, Repository implementasyonları, Identity, Serilog, servisler (JwtProvider, EmailSender, SmsService)
- WebAPI: Program, Middleware’lar, Controller’lar, Swagger
- Tests: xUnit, Moq, FluentAssertions ile birim testleri

## Gereksinimler
- .NET SDK 8.x
- PostgreSQL 14+ (veya uyumlu sürüm)
- (İsteğe bağlı) EF Core CLI: `dotnet tool install --global dotnet-ef`

## Hızlı Başlangıç
1) Depoyu klonlayın ve bağımlılıkları geri yükleyin
- `dotnet restore`

2) Veritabanı
- appsettings.Development.json içinde ConnectionStrings.PostgreSql’i yapılandırın
- Migration’ları uygulayın: `dotnet ef database update --project src/TELERADIOLOGY.Infrastructure --startup-project src/TELERADIOLOGY.WebAPI`

3) Çalıştırma
- `dotnet build`
- `dotnet run --project src/TELERADIOLOGY.WebAPI`
- Swagger: https://localhost:{port}/swagger

İlk kullanıcı oluşturma: Uygulama başlarken `ExtensionsMiddleware.CreateFirstUser(app)` ile varsayılan bir kullanıcı oluşturulur (gerekli konfigürasyonları sağlayın).

## Yapılandırma (appsettings örneği)
Aşağıdaki örneği `src/TELERADIOLOGY.WebAPI/appsettings.Development.json` içine uyarlayın:
{
  "ConnectionStrings": {
    "PostgreSql": "Host=localhost;Port=5432;Database=ata;Username=postgres;Password=1"
  },
  "Jwt": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "Key": "your-very-strong-secret-key",
    "AccessTokenExpirationMinutes": 60
  },
  "EmailSettings": {
    "Host": "smtp.yourhost.com",
    "Port": 587,
    "EnableSSL": true,
    "UserName": "no-reply@yourhost.com",
    "Password": "smtp-password",
    "From": "no-reply@yourhost.com"
  },
  "Serilog": {
    "MinimumLevel": "Information"
  },
  "AllowedHosts": "*"
}
Notlar:
- Serilog PostgreSQL sink’i Program.cs içinde tabloyu otomatik oluşturur (public.logs).
- CORS, `DefaultCorsPolicyNugetPackage` ile varsayılan olarak etkindir.
- Yerelleştirme: varsayılan kültür `en`, desteklenenler `en`, `tr`.

## Proje Yapısı
- src/TELERADIOLOGY.Domain: Entity/Enum/Repository arayüzleri
- src/TELERADIOLOGY.Application: Features (MediatR), DTO, Validator, Mapping, Behaviors
- src/TELERADIOLOGY.Infrastructure: DbContext, Migrations, Repository implementasyonları, Options/Services
- src/TELERADIOLOGY.WebAPI: Controllers, Middlewares, Program.cs
- tests/TELERADIOLOGY.Test: Birim testleri

## Öne Çıkan Modüller
- Auth: Login (JWT), Identity ile kullanıcı yönetimi
- Users/Roles/Permissions: Rol ve izin atama/senkronizasyon
- Companies/Hospitals/Partitions/CompanyUsers/Members
- Reports/Templates/DoctorSignatures
- Notifications: Email (NetMail), SMS

## Swagger ve Kimlik Doğrulama
- Swagger’da Authorize’a tıklayın ve sadece token girin (Bearer şeması otomatik eklenir):
  - Örnek: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
- Controller’lar JWT ile korunur; uygun rol/izin gerektirebilir.

## Günlükleme ve İzleme
- Serilog Console + PostgreSQL sink
- public.logs tablosu otomatik oluşur ve sütunlar Program.cs’de tanımlıdır.

## Rate Limiting
- FixedWindow (“fixed”) politikası Program.cs’de kayıtlıdır
- API endpoint’leri `RequireRateLimiting("fixed")` ile korunur

## Testler
- xUnit, Moq, FluentAssertions kullanılır
- Çalıştırma: `dotnet test`

## Sık Karşılaşılan Hatalar
- “Kullanıcı/rol/izin bulunamadı”: Seed verilerini ve CreateFirstUser akışını doğrulayın
- “Şifreler eşleşmiyor”: UpdateUser akışındaki doğrulamayı kontrol edin
- “Geçersiz CompanyType değeri”: SmartEnum değerini doğru gönderin
- DB bağlantı hataları: ConnectionStrings/PostgreSQL servis durumunu kontrol edin

## Katkı
- Issue/PR açmadan önce ilgili testleri ekleyin/güncelleyin
- Kod stili ve katman sınırlarına dikkat edin (Domain bağımsız, Application saf, Infrastructure dış bağımlılık)

## Lisans
- Lisans bilgisi ekleyin (örn. MIT).
