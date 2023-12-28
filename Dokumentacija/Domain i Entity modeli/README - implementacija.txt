U folderu za Entity Modele se nalaze svi oni entiteti koji se cuvaju u bazi podataka.
U folderu za Domain Modele se nalaze svi oni modeli podataka sa kojima radi biznis logika, odnosno
transformise odgovarajuce entity modele u business modele i vraca korisniku ili dalje transformise 
na odgovarajuci nacin.

ORM koji mapira Entity Modele(OOP objekte) u relacionu bazu je Entity Framework Core.
Na Data Access Layeru se koristi Repository pattern, gde za svaki Entity Model(entitet u bazi) se 
nalazi po jedan repozitorijum koji uzima podatke iz te tabele.

Mapiranje se nalazi na lokaciji:
Back-End/Models/Context

Entity modeli se nalaze na lokaciji:
Back-End/Models/

Business modeli se nalaze na lokaciji:
Back-End/Models/

Repozitorijumi se nalaze na lokaciji:
Back-End/Repository

Interfejsi koji repozitorijumi implementiraju se nalaze na lokaciji:
Back-End/IRepository

Implementacije repozitorijuma su registrovane u Service Containeru, a sam ASP.NET Core framework
ih nam dostavlja kroz Dependecy Injection u konstruktore klasa kontrolera.