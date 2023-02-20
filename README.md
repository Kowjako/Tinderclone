# 👩‍❤️‍💋‍👨 Tinderclone
Aplikacja webowa przedstawiająca klon Tinder'a. Pozwala wyświetlać różne osoby, osoby z którymi masz match, również jest zawarty moduł chatu, który pozwala na prowadzenie konwersacji z wybranymi osobami. Projekt napisany przy użyciu kombo .NET 6 / Angular 14.

## Użyte narzędzia
- **Angular 14, HTML, CSS, RxJS, Bootstrap, Typescript** - frontend  
- **.NET 6, ASP.NET Core Web API, ASP.NET Core SignalR, ASP.NET Core Identity, Entity Framework Core, SQLite** - backend  

Cała aplikacja została zaimplementowana zgodnie ze wzorcem **CQRS** - Command and Query Responsibility Segregation, dla implementacji podobnej segregacji został wykorzystany
dodatek MediatR.

## Zaimplementowane rzeczy  
- Autentykacja na podstawie JWT-tokenów
- Autoryzacja za pomocą ról - w powiązaniu z ASP.NET Core Identity
- Zarządzanie skrzynką wiadomości, chat na żywo za pomocą SignalR
- Widoczność osób online - za pomocą SignalR
- Możliwość dodawania zdjęć profilowych oraz ustawiania głównych
- Panel admina który zarządza rolami użytkowników oraz zatwierdza/odrzuca nowe zdjęcia użytkowników.
- Paginacja strony wszystkich użytkowników oraz użytkowników polubionych
- Cachowanie po stronie serwisów Angular'a
- Edycja całego profilu osoby
- CQRS + MediatR
- Angular Route Guards do przeciwdziałania przypadkowym akcjom
- Angular Interceptors do wysłania zapytań z nagłówkiem autentykacji  

I wiele innych :)

## Jak zainstalować
1️⃣ Pobrać kod źrodłowy backend + frontend  
2️⃣ Uruchomić API (automatycznie zrobi migrację bazy SQLite).  
3️⃣ Uruchomić stronę kliencką: ng serve  

## Screenshoty
![Screenshot_2](https://user-images.githubusercontent.com/19534189/207607383-0123e6b4-bbce-4766-8259-e69da83151f2.png)
![Screenshot_3](https://user-images.githubusercontent.com/19534189/207607388-07f1781a-e0e4-4a86-8bfa-7cacc978721c.png)
![Screenshot_4](https://user-images.githubusercontent.com/19534189/207607392-c67b01d0-0730-4ff3-b133-608de196cb24.png)
![Screenshot_5](https://user-images.githubusercontent.com/19534189/207607396-9cf0f00a-78b2-43dd-bc2b-dea185094248.png)
![Screenshot_6](https://user-images.githubusercontent.com/19534189/207607399-2833b1e6-7b50-4a36-a125-3a569ee6e06c.png)
![Screenshot_7](https://user-images.githubusercontent.com/19534189/207607408-07f3e8a1-2a5f-45ed-8c25-494da1f7d960.png)
![Screenshot_8](https://user-images.githubusercontent.com/19534189/207607424-e716c839-bc6f-405f-9dd2-5561e29d53f3.png)
![Screenshot_9](https://user-images.githubusercontent.com/19534189/207607430-b024c4ec-2187-4353-9593-b495c1e51bc1.png)
