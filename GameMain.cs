using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using System.Diagnostics;

namespace GameFramework
{
	//================================================================================
	//
	//================================================================================
	public class GameMain
	{
		static public int WINDOW_SIZE_X = 800;
		static public int WINDOW_SIZE_Y = 600;
		//	DXLib関連
		protected string m_szGAME_TITLE = "GAME TITLE";
		protected int WINDOW_COLOR_DEPTH = 16;
		protected int WINDOW_MODE = 1;
		protected int COLOR_WHITE = 0;
		protected int COLOR_RED = 0;
		/// <summary>デバッグ用フラグ</summary>
		static protected bool ___bDEBUG___ = true;
		/// <summary>デバッグウインドウ</summary>
		static public DebugWindow __debug_window__;
		protected int m_nLoopCounter = 0;
		//
		protected int FPS = 60;
		protected int FPS_WAIT = 0;
		protected int m_nPrevDrawTime = 0;
		protected int m_nFPS_counter = 0;
		protected int m_nFPS_prev_time = 0;
		protected int m_nFPS_prev = 0;
		protected string m_szFPS = "";
		//
		/// <summary>シーンリスト</summary>
		protected List<Scene> m_list_Scene = null;
		/// <summary>現在のシーン</summary>
		protected Scene m_scene_current = null;
		/// <summary>イベントシーケンス用関数ハッシュ</summary>
		protected Dictionary<object, InnerPostEventHandler> m_dic_Event_Func = null;

		//--------------------------------------------------------------------------------
		//	コンストラクタ
		//--------------------------------------------------------------------------------
		public GameMain( )
		{
			//	最低限の初期化
			m_list_Scene = new List<Scene>( );
			m_dic_Event_Func = new Dictionary<object, InnerPostEventHandler>( );
		}
		//--------------------------------------------------------------------------------
		//	デストラクタ
		//--------------------------------------------------------------------------------
		~GameMain( )
		{
			DX.DxLib_End( );
		}
		//--------------------------------------------------------------------------------
		/// <summary>現在のシーンを返す</summary>
		//--------------------------------------------------------------------------------
		virtual public Scene GetCurrentScene( )
		{
			return m_scene_current;
		}
		//--------------------------------------------------------------------------------
		/// <summary><para>ここでは以下のようなことをしておく</para>
		/// <para>・ゲームのロジック部分の生成</para>
		/// <para>・シーンの生成（ウィンドウなどの描画オブジェクトはシーン内部のイニシャライザで行う）</para>
		/// ・GUIイベントハンドラの関数テーブルを AddEventFunc() で初期化
		/// </summary>
		//--------------------------------------------------------------------------------
		virtual public void Initialize( )
		{
			//	DX関連初期化
			DX.ChangeWindowMode( WINDOW_MODE );
			DX.SetGraphMode( WINDOW_SIZE_X, WINDOW_SIZE_Y, WINDOW_COLOR_DEPTH );
			DX.SetMainWindowText( m_szGAME_TITLE );
			if ( DX.DxLib_Init( ) == -1 ) return;
			DX.SetDrawScreen( DX.DX_SCREEN_BACK );
			FPS_WAIT = 1000 / FPS;
			m_nPrevDrawTime = DX.GetNowCount( );
			m_nFPS_prev_time = DX.GetNowCount( );
			COLOR_WHITE = DX.GetColor( 255, 255, 255 );
			COLOR_RED = DX.GetColor( 255, 0, 0 );
			DX.SetAlwaysRunFlag( 1 );
			Drawable.GameMain = this;
		}
		//--------------------------------------------------------------------------------
		/// <summary>デバッグモードがONのとき、デバッグウィンドウを生成する</summary>
		//--------------------------------------------------------------------------------
		virtual public void InitDebugWindow( )
		{
			//	デバッグウィンドウ
			if ( ___bDEBUG___ == true )
			{
				__debug_window__ = new DebugWindow( );
				__debug_window__.Show( );
			}
		}
		//--------------------------------------------------------------------------------
		/// <summary>デバッグモードがONのとき、デバッグウィンドウにメッセージを表示する</summary>
		//--------------------------------------------------------------------------------
		static public void DebugLog( string szLogMessage )
		{
			if ( ___bDEBUG___ == false ) return;
			if ( __debug_window__ != null ) __debug_window__.WriteLog( szLogMessage );
			else Debug.Write( szLogMessage );
		}
		//--------------------------------------------------------------------------------
		/// <summary>他のシーンへ移行する。あらかじめAddScene()で登録していないと失敗する</summary>
		//--------------------------------------------------------------------------------
		protected void ChangeScene( Scene scene )
		{
			if ( m_list_Scene.Contains( scene ) == false )
			{
				DebugLog( "無効なシーンへチェンジしようとしました。\n" );
				return;
			}
			//	現在のシーンが null なら ExitScene()は実行しない
			if ( m_scene_current != null ) m_scene_current.ExitScene( );
			//	シーン変更
			m_scene_current = scene;
			//	シーン開始処理
			m_scene_current.EnterScene( );
		}
		//--------------------------------------------------------------------------------
		//	イベントハンドラにシーンを追加
		//--------------------------------------------------------------------------------
		/// <summary>シーンを作成したら必ずこの関数でシーンを追加すること。
		/// でないとシーンからのイベントが受け取れない</summary>
		protected void AddScene( Scene scene )
		{
			if ( m_list_Scene.Contains( scene ) == true ) return;
			m_list_Scene.Add( scene );
			scene.InnerPostEvent += new InnerPostEventHandler( Invoke_InnerPostEvent );
		}
		//--------------------------------------------------------------------------------
		//	イベントハンドラにイベント別処理関数を追加
		//--------------------------------------------------------------------------------
		/// <summary>GUIメッセージ振り分け用の ArgsObject を定義したら、
		/// 各々のメッセージに応じた処理関数をこれで登録する</summary>
		protected void AddEventFunc( object oArgsObject, InnerPostEventHandler func )
		{
			if ( m_dic_Event_Func.ContainsKey( oArgsObject ) == true ) return;
			m_dic_Event_Func.Add( oArgsObject, func );
		}
		//--------------------------------------------------------------------------------
		//	シーンのイベントハンドラ
		//--------------------------------------------------------------------------------
		protected void Invoke_InnerPostEvent( object sender, InnerPostEventArgs e )
		{
			if ( e == null )
			{
				DebugLog( "InnerPostEventArgs オブジェクトがnullですわ\n" );
				return;
			}
			if ( e.oArgsObject == null )
			{
				DebugLog( "InnerPostEventArgsのイベント定義がnullですわ\n" );
				return;
			}
			//	メッセージに対応する関数がテーブルに定義されているかチェック
			if ( m_dic_Event_Func.ContainsKey( e.oArgsObject ) == false )
			{
				DebugLog( "未登録のイベント" + e.oArgsObject.ToString( ) + "を受け取りました\n" );
				return;
			}
			//	存在すれば実行
			( ( InnerPostEventHandler )m_dic_Event_Func[ e.oArgsObject ] )( sender, e );
		}

		//--------------------------------------------------------------------------------
		//	メインループ
		//--------------------------------------------------------------------------------
		virtual public void MainLoop( )
		{
			while ( DX.ProcessMessage( ) == 0 )
			{
				m_nLoopCounter++;
				Update( );
				//	描画時間が経過していれば描画実行
				if ( DX.GetNowCount( ) >= m_nPrevDrawTime + FPS_WAIT )
				{
					m_nPrevDrawTime = DX.GetNowCount( );
					DX.ClearDrawScreen( );
					Draw( );
					DX.ScreenFlip( );
				}
			}
		}
		//--------------------------------------------------------------------------------
		//	描画
		//--------------------------------------------------------------------------------
		virtual public void Draw( )
		{
			//	シーンがnullでなければシーンのDrawを実行
			if ( m_scene_current != null ) m_scene_current.Draw( );
			if ( ___bDEBUG___ == true )
			{
				m_nFPS_counter++;
				if ( DX.GetNowCount( ) >= m_nFPS_prev_time + 1000 )
				{
					m_nFPS_prev_time = DX.GetNowCount( );
					m_nFPS_prev = m_nFPS_counter;
					m_szFPS = m_nFPS_counter + " fps";
					m_nFPS_counter = 0;
				}
				DX.DrawString( 0, 580, m_szFPS, COLOR_WHITE );
			}
		}
		//--------------------------------------------------------------------------------
		//	更新
		//--------------------------------------------------------------------------------
		virtual public void Update( )
		{
			//	シーンがnullでなければシーンのUpdateを実行
			if ( m_scene_current != null ) m_scene_current.Update( );
		}
	}
}
