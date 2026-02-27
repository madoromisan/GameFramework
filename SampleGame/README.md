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
このコンテナには `msbuild`/`dotnet` がないため、ローカル環境（.NET Framework 3.5 対応）でビルドしてください。
