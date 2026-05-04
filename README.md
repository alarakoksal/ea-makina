# Makine Hizmetleri Web Platformu

Bu proje, makine hizmetlerini, ekipman listelerini ve yedek parça bilgilerini modern ve dinamik bir arayüz üzerinden sunmak amacıyla geliştirilmiş web tabanlı bir platformdur.

## Özellikler

- Dinamik ürün listeleme
- Yedek parça görüntüleme sistemi
- İçerik yönetimi için admin paneli
- Görsel yükleme desteği
- RESTful API entegrasyonu
- Tam kapsamlı CRUD işlemleri

## Kullanılan Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- Bootstrap

## Mimari

Uygulama katmanlı bir yapı ile geliştirilmiştir:

- MVC (Kullanıcı arayüzü)
- REST API (Veri iletişimi)
- Veritabanı (Kalıcı veri saklama)
- Admin Panel (İçerik yönetimi)

## Yapılandırma

Admin giriş bilgileri gibi hassas veriler .NET User Secrets kullanılarak yönetilmekte olup, kaynak kod içerisinde tutulmamaktadır.

## Notlar

Bu proje, hem portföy amacıyla hem de gerçek kullanım senaryoları göz önünde bulundurularak geliştirilmiştir. Geliştirme sürecinde modern web uygulama mimarisi, veri yönetimi ve kullanıcı arayüzü deneyimi ön planda tutulmuştur