# Steampunk-Moba

## Kto jest za co odpowiedzialny / Who is responsible for which part?

Filip Krężel - większość kodu sieciowego / most of the netcode, matchmaking, bazowe statystyki / basic stats, <br />
Piotr Duliński - UI, sklep / shop, klient gry / game client, <br />
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
 

# 2021.3.10f1

## Wymagana paczka do serwera (z asset store)
https://assetstore.unity.com/packages/tools/network/mirror-129321
https://assetstore.unity.com/packages/3d/environments/landscapes/rpg-poly-pack-lite-148410
https://github.com/PlayFab/UnitySDK

## Wykorzystywane assety (niewymagane do pobrania, dostępne na gicie)
https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488
https://assetstore.unity.com/packages/2d/gui/icons/fantasy-inventory-icons-free-143805
https://ambientcg.com/view?id=Ground037
https://ambientcg.com/view?id=Rock030

## propozycje prefabow
https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-lich-pbr-hp-polyart-91497

## Struktura folderów (ważne)
Jeżeli coś tworzycie to prosze zachowac nowa strukture folderow:
(Przykład dla serwera, mapy i klienta)
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
itp...
