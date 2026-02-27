# Anti-Reality

RimWorld 向けのコンテンツ追加 MOD です。  
世界の外にある「虚構エネルギー」を利用した武器・施設・衣服を追加します。

## Overview

- Mod Name: `Anti Reality`
- Author: `HighElixir`
- PackageId: `HE.AntiReality`
- Supported Versions: `1.5`, `1.6`

## 追加コンテンツの方向性

- 虚構エネルギーを扱う建築・パイプ系設備
- 虚構エネルギー関連の資源
- 遠距離武器を含む戦闘装備
- 一部の衣服/特性/状態異常関連定義

## 依存関係（必須）

- `brrainz.harmony` (Harmony)  
  - Steam: `steam://url/CommunityFilePage/2009463077`
  - Download: `https://github.com/pardeike/HarmonyRimWorld/releases/latest`
- `Ludeon.RimWorld.Biotech` (Biotech DLC)
- `OskarPotocki.VanillaFactionsExpanded.Core` (Vanilla Expanded Framework)
  - Steam: `https://steamcommunity.com/workshop/filedetails/?id=2023507013`
  - Download: `https://github.com/Vanilla-Expanded/VanillaExpandedFramework`

## ロード順

`About/About.xml` の `loadAfter` に基づく推奨順:

1. `brrainz.harmony`
2. `Ludeon.RimWorld.Biotech`
3. `OskarPotocki.VanillaFactionsExpanded.Core`
4. `Ancot.KiiroRace`（環境にある場合）
5. `HE.AntiReality`

## 読み込みフォルダ構成

`LoadFolders.xml` の設定:

- RimWorld `1.5`: `1.6` と `Cont` を読み込み
- RimWorld `1.6`: `1.6` と `Cont` を読み込み

## インストール

1. 依存 MOD/DLC を先に導入
2. 本 MOD を有効化
3. ロード順を上記に合わせる
4. ゲーム起動後、研究タブ/建築カテゴリに Anti-Reality 系定義が表示されることを確認

## 開発メモ

- C# プロジェクト: `AntiReality.csproj`
- Release 出力先: `1.6/Assemblies/`
- 例:

```bash
dotnet build AntiReality.csproj -c Release
```

## 参照

- メタデータ: `About/About.xml`
- ロード設定: `LoadFolders.xml`
- Def要約: `1.6/defs/DefFlavorSummary.txt`
