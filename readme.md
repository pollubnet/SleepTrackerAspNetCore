Sleep Tracker
=============

**Program demonstracyjny ze spotkania grupy z 26 kwietnia 2016.**

**Aktualizacja 3 marca 2018 - dostosowanie do .NET Core 2.0.**

Aplikacja jest stworzona dla ASP.NET Core 2.0. Pozwala na upload 
pliku tekstowego w określonym formacie, który to plik zawiera dane 
dotyczące snu. Przykładowy plik już jest zawarty w repozytorium, 
w `/src/SleepTracker/wwwroot/uploads/data.txt`.

Po wejściu na stronę główną aplikacja prezentuje wykres snu z ostatnio
załadowanego pliku.

Po ściągnięciu repozytorium, jeżeli używa się Visual Studio, samo
powinno wykonać pobranie niezbędnych dodatkowych bibliotek, zarówno
tych z ASP.NET, jak i tych pochodzących z Bowera (używamy Chart.js) 
i NPM. Jeżeli używa się Visual Studio Code również samo powinno
zaproponować pobranie niezbędnych dodatków, natomiast ręcznie należy
pamiętać o wydaniu polecenia `dotnet restore`.

## Uruchomienie pod Linuksem
Serwer na Ubuntu 17.10 (oraz innymi odmianami) można skonfigurować zgodnie z oficjalnym 
[poradnikiem](https://www.microsoft.com/net/learn/get-started/linux/ubuntu17-10).

W tym celu, należy:

Dodać feed paczek Microsoft:
```bash
curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-artful-prod artful main" > /etc/apt/sources.list.d/dotnetdev.list'
```

Zainstalować aktualną wersję .NET Core SDK:

```bash
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-2.1.101
```

Teraz możliwe będzie już uruchomienie projektu po przejściu do 
katalogu `/src/SleepTracker` i po wydaniu polecenia `dotnet run`.

Skonfigurowany w tym projekcie dotnet run uruchamia się na wszystkich
adresach IP na porcie 5000. Lepsze jednak będzie uruchomienie np. 
serwera Nginx tak, aby odbierał on informacje z serwera Kestrel 
i przekazywał do nas (reverse-proxy):

Aby to zrobić, należy zainstalować nginx:

```bash
sudo apt-get install nginx
```

Oraz zmienić zawartość pliku `/etc/nginx/sites-enabled/default` na:

```
server {
        listen 80 default_server;
        listen [::]:80 default_server ipv6only=on;

        root /usr/share/nginx/html;
        index index.html index.htm;        

        location / {                
                proxy_pass http://localhost:5000;
                proxy_redirect off;
                proxy_set_header HOST $host;
                proxy_buffering off;
        }
}
```

Spowoduje to, że wszystkie żądania HTTP skierowane do adresu IP naszej
maszyny zostaną przekazane do serwera ASP.NET działającego "pod spodem".

Potem wystarczy tylko restart nginxa: `sudo service nginx restart` oraz
uruchomienie naszej strony poprzez przejście do `/src/SleepTracker/` i
wydanie komendy `dotnet run` i po uruchomieniu serwera Kestrel nasz serwer
powinien zacząć odpowiadać na nasze żądania serwując nam stronę ASP.NET
Core pod Linuksem.