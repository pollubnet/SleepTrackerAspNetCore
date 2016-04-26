Sleep Tracker
=============

**Program demonstracyjny ze spotkania grupy z 26 kwietnia 2016.**

Aplikacja jest stworzona dla ASP.NET Core 1.0 RC1. Pozwala na upload 
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
pamiętać o wydaniu polecenia `dnu restore`.

## Uruchomienie pod Linuksem
Serwer na Ubuntu 14.04 można skonfigurować zgodnie z oficjalnym 
[poradnikiem](https://docs.asp.net/en/latest/getting-started/installing-on-linux.html#installing-on-ubuntu-14-04).

W tym celu, należy:

Zainstalować DNVM:
```bash
sudo apt-get install unzip curl
curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
```

Zainstalować aktualną wersję DNX oraz CoreCLR:

```bash
sudo apt-get install libunwind8 gettext libssl-dev libcurl4-openssl-dev zlib1g libicu-dev uuid-dev
dnvm upgrade -r coreclr
```

Zainstalować (ze źródeł) libuv:

```bash
sudo apt-get install make automake libtool curl
curl -sSL https://github.com/libuv/libuv/archive/v1.8.0.tar.gz | sudo tar zxfv - -C /usr/local/src
cd /usr/local/src/libuv-1.8.0
sudo sh autogen.sh
sudo ./configure
sudo make
sudo make install
sudo rm -rf /usr/local/src/libuv-1.8.0 && cd ~/
sudo ldconfig
```

Teraz możliwe będzie już uruchomienie projektu po przejściu do 
katalogu `/src/SleepTracker` i po wydaniu polecenia `dnx web`.

Niestety, domyślnie dnx web uruchamia się tylko na localhost na
porcie 5000. Lepsze będzie uruchomienie np. serwera Nginx tak, aby
odbierał on informacje z serwera Kestrel i przekazywał do nas.

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

Potem wystarczy tylko restart nginxa: `sudo service nginx restart` i
serwer powinien zacząć odpowiadać na nasze żądania serwując nam stronę
ASP.NET Core pod Linuksem.