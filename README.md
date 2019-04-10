# BlogWebApplication

Proje Tanımı
ASP.Net Core çatısı altında geliştirilen, site sahibi (Admin)’in makalelerini yazıp yayınlayabildiği ve yorumlar ile diğer kişilerle etkileşimde olduğu, kişisel blog sitesi olarak kullanılan MVC projesi.

Kullanılan Teknolojiler

• C# (OOP)
• ASP.NET Core
• ASP.NET MVC
• ASP.NET Core Identity
• Entity Framework Core (Code First)
• HTML
• CSS
• Javascript
• JQuery
• Ajax
• Bootstrap
• N-Tier Architecture
• Repository Entity Pattern
• Linq To Sql
• Dependency Injection



Öngereklilikler
• Visual Studio 2017
• Sql Server 2017 Local Db
• .Net Framework ^4.5



Çalıştırma
1. Solution’ı sağ tıklayıp Restore Nuget Packages’ı tıklayınız.
2. Nuget Package Manager Console’dan Default Project’i Blog.DAL yaptıktan sonra öncelikle “ add-migration blog “ komutunu sonrasında ise “ update-database ” komutunu çalıştırınız.
a. Hata vermesi durumunda “Rebuilt Solution” yapıp projeyi kapatıp tekrar açabilirsiniz.
3. Blog.Web > Controllers > AccountController’da ki yorum halinde olan Register ve CreateRoles action’larını yorum halinden çıkarıp projeyi başlattığımızda URL’e …/Account/Register yazıp adrese gidiniz böylece “Site Yöneticisi”nin kaydı oluşturulmuş olacaktır. İsteğe göre actionlardaki bilgiler değiştirilerek istediğiniz şekilde kayıt yapabilirsiniz. Kayıt işlemi bittikten sonra farklı kişilerin erişmesini engellemek için komutları tekrar yorum haline getirmeyi unutmayınız.


Uygulamada Bulunan Özellikler
• Site Yöneticisi (Admin) URL’e …/Account/Login adresini yazarak giriş sayfasına gidip oradan bilgileri ile giriş yapabilir.
• Giriş Sayfasında giriş olayının yanı sıra “Şifremi Unuttum” butonuna tıklayarak veritabanında kayıtlı olan Email adresini girip, sonrasında gelen şifremi unuttum mailinin içerisindeki yeni şifresi ile sisteme giriş yapabilir.
• Giriş işlemi başarılı sonuçlandığında otomatik olarak “Admin Panel” sayfasına yönelendirilir.
• Panelde makale oluşturma, makale için kategori oluşturma, mevcut makalelerin bilgilerini değiştirebilme ve silme, makalelere yapılan yorumları silme gibi işlemler yapılabilir.
• Makale oluştururken çoklu resim eklemesi yapılabilir.
• Panelde aynı zamanda Kişisel Bilgilerde değiştirilebilir.
• Web sitesinin Ana sayfasında yani ziyaretçilerin gördüğü kısımda makaleler görüntülenebilir ve istendiğinde yorum yapılabilir. Yapılan yorumlar ilk olarak Site yöneticisinin mail adresine gider ve eğer onaylanırsa sayfada görüntülenebilir.
• Hakkımda sayfasına giderek site yöneticisi hakkında bilgilere ulaşabilir.
NOT: Responsive Design kullanılmadığı için çözünürlüğü farklı olan ekranlarda siteyi görüntülerken sayfa üzerindeki elemanlarda kaymalar olabilir. Proje 1920x1080 çözünürlüğe göre yapılmıştır.

