#!/bin/bash
# ============================================================
# Skrypt do rozdzielenia commita 708cb36 na 3 commity
# Uruchom w katalogu głównym repozytorium
# ============================================================

set -e

COMMIT="708cb367f3b2d2f8ebce7d75552229679cc3ef31"

# Autorzy
KUBA_AUTHOR="kulabin <kulabin555@gmail.com>"
FILIP_AUTHOR="FWookowiak <s27961@gmail.com>"
JULIAN_AUTHOR="iilatt <ilatt@users.noreply.github.com>"

echo "=== Backup branch ==="
git branch -f backup-before-split HEAD

echo "=== Soft reset do rodzica commita ==="
git reset --soft "${COMMIT}~1"

echo ""
echo "=== Commit 1/3: Kuba (GameEvents + BuildManager + Node + HUDPresenter) ==="
git reset HEAD -- .

# Kuba: nowy plik
git add Assets/Scripts/Core/GameEvents.cs

# Kuba: modyfikowane pliki
git add Assets/Scripts/Towers/BuildManager.cs
git add Assets/Scripts/Towers/Node.cs
git add Assets/Scripts/UI/Presenters/HUDPresenter.cs

# Dodaj też .meta jeśli istnieją
git add Assets/Scripts/Core/GameEvents.cs.meta 2>/dev/null || true

git commit \
  --author="$KUBA_AUTHOR" \
  -m "feat(S-4.2): Observer — GameEvents + BuildManager + Node + HUDPresenter

- GameEvents.cs: statyczny event bus (wzorzec Observer)
- BuildManager: SellTowerOn(), DeselectTower(), emituje eventy
- Node: towerConfig, kolory feedbacku, zaznaczanie wieży
- HUDPresenter: subskrypcja GameEvents zamiast pollingu w Update()

Realizuje: S-4.2 (Observer — reakcje wież na zmiany stanu gry)"

echo ""
echo "=== Commit 2/3: Filip (TowerBase Observer + EnemyBase eventy + Sprzedaż) ==="

git add Assets/Scripts/Towers/TowerBase.cs
git add Assets/Scripts/Towers/TowerConfig.cs
git add Assets/Scripts/Enemies/EnemyBase.cs
git add Assets/Scripts/UI/Views/ITowerUpgradeView.cs
git add Assets/Scripts/UI/Views/TowerUpgradeView.cs
git add Assets/Scripts/UI/Presenters/TowerUpgradePresenter.cs

git commit \
  --author="$FILIP_AUTHOR" \
  -m "feat(S-5.2): Sprzedaż wież + Observer w TowerBase/EnemyBase

- TowerBase: Observer na GameStateChanged/WaveStarted/WaveCompleted
- EnemyBase: emituje GameEvents.EnemyKilled/EnemyReachedGoal
- TowerConfig: +sellRefundPercent (50% zwrotu)
- TowerUpgradeView: przycisk Sprzedaj + UpdateSellButton()
- TowerUpgradePresenter: HandleSell() -> BuildManager.SellTowerOn()
- ITowerUpgradeView: +OnSellClicked, +UpdateSellButton()

Realizuje: S-5.2 (sprzedaż wież)"

echo ""
echo "=== Commit 3/3: Julian (PlayerStats + WaveManager + AudioManager + Tutorial) ==="

git add Assets/Scripts/Core/PlayerStats.cs
git add Assets/Scripts/Core/WaveManager.cs
git add Assets/Scripts/Core/AudioManager.cs
git add Assets/Scripts/Core/GameManager.cs
git add Assets/Scripts/Tutorial/TutorialManager.cs

# Dodaj wszystko co zostało (np. usunięte pliki modeli, które widać w Twoim logu)
git add -A

git commit \
  --author="$JULIAN_AUTHOR" \
  -m "feat(S-5.3): Tutorial sprzedaży + eventy w WaveManager/AudioManager

- PlayerStats: emituje MoneyChanged/LivesChanged na Start()
- WaveManager: emituje WaveStarted/WaveCompleted przez GameEvents
- AudioManager: Observer — subskrybuje eventy, gra SFX
- GameManager: +GameEvents.GameStateChanged w ChangeState()
- TutorialManager: nowy krok 'sell_tower' (sprzedawanie wież)

Realizuje: S-5.3 (samouczek — sprzedawanie wież)"

echo ""
echo "============================================================"
echo "GOTOWE! Sprawdź wynik:"
echo "  git log --oneline -5"
echo "============================================================"