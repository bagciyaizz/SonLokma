# SonLokma

Yiyecek ve içecek işletmelerinin gün sonunda israf olabilecek sürpriz paketlerini listeleyip kullanıcıların ayırtabildiği web ve mobil ürün temeli.

## Stack

- Backend: .NET 9 Web API
- Frontend: Angular 21
- Database: PostgreSQL + PostGIS

## Lokal Çalıştırma

```powershell
docker compose up -d
dotnet ef database update --project backend\SonLokma.Infrastructure --startup-project backend\SonLokma.Api
dotnet run --project backend\SonLokma.Api
```

Başka bir terminalde:

```powershell
cd frontend
npm start
```

API varsayılan olarak `http://localhost:5106`, frontend `http://localhost:4200`, PostgreSQL host portu `5433` üzerinden çalışır.

Development admin hesabı:

```text
Email: admin@sonlokma.local
Password: Admin1234!
```

## İlk MVP Sınırı

- Kullanıcı kayıt/giriş ve JWT rol temeli
- Customer / Business / Admin rol modeli
- PostgreSQL + PostGIS konum altyapısı
- Listing ve reservation entity temeli
- Sonraki aşama: işletme başvurusu ve admin onayı
