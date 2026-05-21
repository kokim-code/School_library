Миграции EF Core создаются командой:

dotnet ef migrations add InitialCreate --project SchoolLibrary.Web

При запуске приложение автоматически применяет миграции через Database.Migrate().
