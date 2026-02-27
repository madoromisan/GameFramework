# SampleGame

`GameFramework` を利用する最小サンプルです。

## 構成
- `Program.cs`: `GameMain` / `Scene` を継承した最小実装。
- `SampleGame.csproj`: `GameFramework.csproj` への参照を持つ実行プロジェクト。

## 実行の流れ
1. `DemoGameMain.Initialize()` で `DxLib` 初期化。
2. `DemoScene` を `AddScene` して `ChangeScene` で遷移。
3. `MainLoop()` で更新・描画ループを開始。

## ビルド
このコンテナには `msbuild`/`dotnet` がないため、ローカル環境（.NET Framework 4.8 対応）でビルドしてください。


## 推奨ビルド手順（CS0006対策）
- Visual Studioでは `SampleGame.sln` を開いて **ソリューション全体** をビルドしてください。
- 単体プロジェクトのみをビルドすると、`GameFramework.dll` が先に生成されず `CS0006` になる場合があります。
- プラットフォームは `Debug|Any CPU` か `Debug|x86` を選択してください。

- もし `Any CPU` で失敗する場合は `AnyCPU` との差異吸収を入れてあるので、ソリューションの再読み込み後に再ビルドしてください。

- リポジトリ直下にも `sampleGame.sln` を配置しているため、そこから開いてもビルドできます。
