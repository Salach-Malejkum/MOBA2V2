# Steampunk-Moba
Here is the video with demo -> https://www.youtube.com/watch?v=gOhHgcn6WKQ

## Kto jest za co odpowiedzialny / Who is responsible for which part?

Filip Krężel - większość kodu sieciowego / most of the netcode, matchmaking, bazowe statystyki / basic stats, <br />
Piotr Duliński - UI, sklep / shop, klient gry / game client, przedmioty / items<br />
Erwin Marysiok - surowce / resources, wieże / towers, stwory w dżungli / mobs in the jungle, ulepszenia / upgrades, mapa / map, <br />
Mateusz Salach - Team Leader, gracz / player, minions, their skills, interactions and animations, kamera / camera. <br />

## Jak odpalic gierke do testowania sieciówki
 - WAŻNE, zawsze przy testowaniu sprawdźcie czy obiekt BuildType jest ustawiony na Remote client (--SYSTEMS-- w scenie Lobby)
 - Podstawowa scena to Lobby (w Assets/Lobby/Scenes)
 - Robicie build klienta jeżeli jest Remote client ustawiony (nie zmieniajcie nigdy na remote server, to jest tylko do dedykowanego)
 - Logowanie przy pomocy: test13 (hasło i login), albo nowe konto można stworzyć (testXX@test.com), login hasło testXX, XX to cyferki, łatwo zapamiętać
 - W unity editor dajecie Host custom game
 - W odpalonej gierce z builda logujecie się innym hasłem (test12) albo tworzycie nowe konto 
 - W kliencie dołączacie do hosta dająć Join custom game i IP to localhost albo 127.0.0.1 (tylko jedno z tych działa, nie stosować localhost i 127.0.0.1 wymiennie)
 - Dajecie Ready na kliencie i hoscie
 - Start na hoscie
 - Sprawdzacie czy wasze dodane funkcjonalności sieciowe/klientowe działają jak powinny
 - ZAKAZ TWORZENIE WIĘCEJ NIŻ 2 KONTA NA OSOBE
 
## How to launch the game for network testing
 - IMPORTANT, always check if the BuildType object is set to Remote client when testing (--SYSTEMS-- in the Lobby scene)
 - The main scene is Lobby (in Assets/Lobby/Scenes)
 - Build the client if Remote client is set (never change it to remote server, that's for dedicated)
 - Log in using: test13 (password and username), or you can create a new account (testXX@test.com), login password testXX, XX are numbers, easy to remember
 - In Unity editor, host a custom game
 - In the launched game from the build, log in with a different password (test12) or create a new account
 - In the client, join the host by selecting Join custom game and the IP is localhost or 127.0.0.1 (only one of them works, do not interchange localhost and 127.0.0.1)
 - Ready on the client and host
 - Start on the host
 - Check if your added network/client functionalities work as they should
 - DO NOT CREATE MORE THAN 2 ACCOUNTS PER PERSON
# 2021.3.10f1

## Wymagana paczka do serwera (z asset store) / Required package for the server (from the asset store)
https://assetstore.unity.com/packages/tools/network/mirror-129321
https://assetstore.unity.com/packages/3d/environments/landscapes/rpg-poly-pack-lite-148410
https://github.com/PlayFab/UnitySDK

## Wykorzystywane assety (niewymagane do pobrania, dostępne na gicie) / Used assets (not required to download, available on GitHub)
https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488
https://assetstore.unity.com/packages/2d/gui/icons/fantasy-inventory-icons-free-143805
https://ambientcg.com/view?id=Ground037
https://ambientcg.com/view?id=Rock030

## propozycje prefabow / refab suggestions
https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-lich-pbr-hp-polyart-91497

## Struktura folderów (ważne) / Folder structure (important)
Jeżeli coś tworzycie to prosze zachowac nowa strukture folderow: / If you are creating something, please maintain a new folder structure:
(Przykład dla serwera, mapy i klienta) / (Example for server, map, and client)
Server/
- Prefabs
- Scripts
- Scenes
- Resources/
- - SpawnablePrefabs

Client/
- Prefabs
- Scripts
- Scenes
- Resources/

Map/
- Prefabs
- Scripts
- Scenes
- Resources/
- - Textures
etc...
