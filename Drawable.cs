using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using System.Drawing;

namespace GameFramework
{
	//================================================================================
	//	描画オブジェクト基底クラス
	//================================================================================
	public class Drawable
	{
		//--------------------------------------------------------------------------------
		//	静的プロパティ
		//--------------------------------------------------------------------------------
		static protected int COLOR_WHITE = 0;
		static protected int COLOR_RED = 0;
		static protected int COLOR_BLACK = 0;
		static protected int COLOR_YELLOW = 0;
		static protected int WINDOW_SIZE_X = 800;
		static protected int WINDOW_SIZE_Y = 600;
		/// <summary>マウスオーバー処理を行ったウィンドウがあるかどうか</summary>
		static protected Drawable s_win_OnRectMouseOver;
		/// <summary>マウスクリック処理を行ったウィンドウがあるかどうか</summary>
		static protected Drawable s_win_OnRectMouseClick;
		/// <summary>ウィンドウにドラッグ中のものがあるかどうか</summary>
		static protected Drawable s_win_DraggingWindow;
		/// <summary>最前面要求のウィンドウがあるかどうか</summary>
		static protected Drawable s_win_BringToTop;
		/// <summary>ドラッグ終了したウィンドウ</summary>
		static protected Drawable s_win_DragOutWindow;
		/// <summary>ドラッグウィンドウの元々の親ウィンドウ</summary>
		static protected Drawable s_win_DraggingOriginalParentWindow;
		/// <summary>ドラッグアウトを処理したウィンドウ</summary>
		static protected Drawable s_win_ExecDragOutWindow;
		/// <summary>メインゲームオブジェクトへの参照</summary>
		static protected GameMain s_GameMain = null;
		/// <summary>メインゲームオブジェクトへの参照</summary>
		static public GameMain GameMain { set { s_GameMain = value; } }
		/// <summary>マウス処理の排他制御用フラグ</summary>
		static bool s_bMouseAlready = false;
		/// <summary>マウス処理の排他制御用フラグ</summary>
		static public bool bMouseAlready { set { s_bMouseAlready = value; } get { return s_bMouseAlready; } }
		//--------------------------------------------------------------------------------
		//	プロパティ
		//--------------------------------------------------------------------------------
		/// <summary>子描画オブジェクトのリスト</summary>
		protected List<Drawable> m_list_Child_drawable = null;
		/// <summary>親に投げるイベント</summary>
		public event InnerPostEventHandler InnerPostEvent;
		/// <summary>子供から投げられるイベントの関数テーブル</summary>
		protected Dictionary<object, InnerPostEventHandler> m_Dictionary_Event_Func;
		/// <summary>派生クラスから親にイベントを投げる際のブリッジ</summary>
		protected InnerPostEventArgs m_InnerPostEventArgs;
		/// <summary>派生クラスから親にイベントを投げる際のブリッジ</summary>
		public InnerPostEventArgs InnerPostEventArgs { set { m_InnerPostEventArgs = value; } }
		/// <summary>左クリックのデフォルトイベントが設定されているかどうか</summary>
		protected bool m_bDefaultMouseLeftClickEvent = false;
		/// <summary>左クリックのデフォルトイベントが設定されているかどうか</summary>
		public bool bDefaultMouseLeftClickEvent { set { m_bDefaultMouseLeftClickEvent = value; } get { return m_bDefaultMouseLeftClickEvent; } }
		/// <summary>右クリックのデフォルトイベントが設定されているかどうか</summary>
		protected bool m_bDefaultMouseRightClickEvent = false;
		/// <summary>描画座標を親との相対で行うかどうか</summary>
		protected bool m_bPosOnParent = true;
		/// <summary> true なら描画座標を表示する</summary>
		protected static bool ___bDEBUG_ON___ = true;
		/// <summary>グラフィックハンドル</summary>
		protected int m_nHandle_Graphic = -1;
		/// <summary>マウス左ボタンクリックチェック用</summary>
		protected bool m_bMouseButtonLeftClickCheck = false;
		/// <summary>マウス右ボタンクリックチェック用</summary>
		protected bool m_bMouseButtonRightClickCheck = false;
		/// <summary>画像サイズ X</summary>
		protected int m_nSize_x = 0;
		/// <summary>画像サイズ Y</summary>
		protected int m_nSize_y = 0;
		/// <summary>画像矩形内ぶマウスカーソルがあるかどうか</summary>
		protected bool m_bMouseOnRect = false;
		/// <summary>ドラッグ開始時の座標</summary>
		protected int m_nDragBeginX, m_nDragBeginY;
		/// <summary>ドラッグ時の限界矩形（この範囲内に描画座標が収まらなければならない）</summary>
		protected int m_nDragTopLeftX, m_nDragTopLeftY, m_nDragBottomRightX, m_nDragBottomRightY;
		/// <summary>ドラッグ中かどうか</summary>
		protected bool m_bDraggingNow = false;
		/// <summary>ドラッグ開始マウス座標</summary>
		protected int m_nPrevMouseX, m_nPrevMouseY;
		/// <summary>ドラッグ開始スクリーン座標</summary>
		protected int m_fBeginDragX, m_fBeginDragY;
		/// <summary>ドラッグを受け入れるかどうか</summary>
		protected bool m_bEnableDragOut = false;
		/// <summary>ドラッグを受け入れる処理を必須とするかどうか。この値がtrueの場合、ドラッグアウト処理が行われなければ元の親元へ帰る
		/// （ドラッグ可能なのにこれがfalseだとドラッグ時に親がシーンになってしまう）</summary>
		protected bool m_bNeedDragOutExec = false;
		/// <summary>デフォルトのドラッグアウトイベントが設定されているかどうか</summary>
		protected bool m_bDefaultDragOutEvent = false;
		/// <summary>デフォルトドラッグアウトイベント用データオブジェクト</summary>
		protected object m_oDragOutEventObject = null;
		/// <summary>マウス左ボタンクリック時にウィンドウをトップにするかどうか</summary>
		protected bool m_bTopDraw = false;
		/// <summary>現在トップで表示されているかどうか</summary>
		protected bool m_bCurrentTop = false;
		//--------------------------------------------------------------------------------
		//	プロパティ
		//--------------------------------------------------------------------------------
		/// <summary>描画するかどうか</summary>
		protected bool m_bDoDraw = true;
		/// <summary>描画するかどうか</summary>
		public bool bDoDraw { set { m_bDoDraw = value; } get { return m_bDoDraw; } }
		/// <summary>親描画オブジェクト</summary>
		protected Drawable m_Drawable_Parent = null;
		/// <summary>親描画オブジェクト</summary>
		public Drawable Drawable_Parent { set { m_Drawable_Parent = value; } get { return m_Drawable_Parent; } }
		/// <summary>マウス検知するかどうか</summary>
		protected bool m_bMouseAct = false;
		/// <summary>マウス検知するかどうか</summary>
		public bool bMouseAct { set { m_bMouseAct = value; } get { return m_bMouseAct; } }
		/// <summary>マウス左ボタンクリック処理するかどうか</summary>
		protected bool m_bMouseButtonLeftClickEnable = false;
		/// <summary>マウス左ボタンクリック処理するかどうか</summary>
		public bool bMouseButtonLeftClickEnable { set { m_bMouseButtonLeftClickEnable = value; } get { return m_bMouseButtonLeftClickEnable; } }
		/// <summary>マウス右ボタンクリック処理するかどうか</summary>
		protected bool m_bMouseButtonRightClickEnable = false;
		/// <summary>マウス右ボタンクリック処理するかどうか</summary>
		public bool bMouseButtonRightClickEnable { set { m_bMouseButtonRightClickEnable = value; } get { return m_bMouseButtonRightClickEnable; } }
		/// <summary>マウス左ボタンクリックされたかどうか</summary>
		protected bool m_bMouseButtonLeftClicked = false;
		/// <summary>マウス左ボタンクリックされたかどうか</summary>
		public bool bMouseButtonLeftClicked { get { return m_bMouseButtonLeftClicked; } }
		/// <summary>マウス右ボタンクリックされたかどうか</summary>
		protected bool m_bMouseButtonRightClicked = false;
		/// <summary>マウス右ボタンクリックされたかどうか</summary>
		public bool bMouseButtonRightClicked { get { return m_bMouseButtonRightClicked; } }
		/// <summary>ドラッグ可能にするかどうか</summary>
		protected bool m_bDragEnable = false;
		/// <summary>ドラッグ可能にするかどうか</summary>
		public bool bDragEnable { set { m_bDragEnable = value; } get { return m_bDragEnable; } }
		/// <summary>ドラッグステート管理フラグ</summary>
		protected bool m_bDragging = false;
		/// <summary>通常時グラフィックハンドル</summary>
		protected int m_nHandle_Graphic_OFF = -1;
		/// <summary>通常時グラフィックハンドル</summary>
		public int nHandle_Graphic_OFF { get { return m_nHandle_Graphic_OFF; } }
		/// <summary>クリック時グラフィックハンドル</summary>
		protected int m_nHandle_Graphic_ON = -1;
		/// <summary>クリック時グラフィックハンドル</summary>
		public int nHandle_Graphic_ON { get { return m_nHandle_Graphic_ON; } }
		/// <summary>マウスオーバー時グラフィックハンドル</summary>
		protected int m_nHandle_Graphic_OVER = -1;
		/// <summary>マウスオーバー時グラフィックハンドル</summary>
		public int nHandle_Graphic_OVER { get { return m_nHandle_Graphic_OVER; } }
		/// <summary>親に対する相対座標</summary>
		protected int m_nDrawPos_X = 0;
		/// <summary>親に対する相対座標</summary>
		public int nDrawPos_X { set { m_nDrawPos_X = value; } get { return m_nDrawPos_X; } }
		/// <summary>親に対する相対座標</summary>
		protected int m_nDrawPos_Y = 0;
		/// <summary>親に対する相対座標</summary>
		public int nDrawPos_Y { set { m_nDrawPos_Y = value; } get { return m_nDrawPos_Y; } }
		/// <summary>スクリーン座標</summary>
		protected int m_nDrawPosScreen_X = 0;
		/// <summary>スクリーン座標</summary>
		public int nDrawPosScreen_X { set { m_nDrawPosScreen_X = value; } get { return m_nDrawPosScreen_X; } }
		/// <summary>スクリーン座標</summary>
		protected int m_nDrawPosScreen_Y = 0;
		/// <summary>スクリーン座標</summary>
		public int nDrawPosScreen_Y { set { m_nDrawPosScreen_Y = value; } get { return m_nDrawPosScreen_Y; } }
		/// <summary>アニメーション情報</summary>
		protected Animation m_animation = null;
		/// <summary>透過フラグ</summary>
		protected int m_nTransFlag = 1;
		/// <summary>透過フラグ</summary>
		public int nTransFlag { set { m_nTransFlag = value; } get { return m_nTransFlag; } }
		/// <summary>ドラッグを受け入れる処理を必須とするかどうか。この値がtrueの場合、ドラッグアウト処理が行われなければ元の親元へ帰る</summary>
		public bool bNeedDragOutExec { get { return m_bNeedDragOutExec; } }
		//--------------------------------------------------------------------------------
		//	コンストラクタ
		//--------------------------------------------------------------------------------
		public Drawable( )
		{
			m_list_Child_drawable = new List<Drawable>( );
			m_Dictionary_Event_Func = new Dictionary<object, InnerPostEventHandler>( );
		}
		static Drawable( )
		{
			COLOR_WHITE = DX.GetColor( 255, 255, 255 );
			COLOR_RED = DX.GetColor( 255, 0, 0 );
			COLOR_YELLOW = DX.GetColor( 255, 255, 0 );
		}
		//----------------------------------------------------------------------
		//	子供のイベントハンドラ
		//----------------------------------------------------------------------
		virtual public void Invoke_InnerPostEvent( object sender, InnerPostEventArgs e )
		{
<<<<<<< HEAD
			if ( e == null ) return;
=======
			if ( e == null )
			{
				GameMain.DebugLog( "InnerPostEventArgs オブジェクトがnullですわ\n" );
				return;
			}
			if ( e.oArgsObject == null )
			{
				GameMain.DebugLog( "InnerPostEventArgsのイベント定義がnullですわ\n" );
			}
>>>>>>> origin/codex/explain-codebase-structure-to-newcomers-toliwo
			//	このウィンドウでハンドルするイベントかどうかチェック
			if ( e.oArgsObject != null && m_Dictionary_Event_Func.ContainsKey( e.oArgsObject ) == true )
			{
				//	処理関数実行
				( ( InnerPostEventHandler )m_Dictionary_Event_Func[ e.oArgsObject ] )( sender, e );
			}
			//	親（シーン）に渡す
			if ( InnerPostEvent != null ) InnerPostEvent( sender, e );
		}
		//----------------------------------------------------------------------
		/// <summary>派生先のクラスでイベントを起こすためのブリッジ。
		/// 使用の際はあらかじめ _InnerPostEventArgs を割り当てておくこと</summary>
		//----------------------------------------------------------------------
		protected void BridgeEvent( )
		{
			if ( InnerPostEvent != null && m_InnerPostEventArgs != null ) InnerPostEvent( this, m_InnerPostEventArgs );
		}
		//--------------------------------------------------------------------------------
		/// <summary>基底では子供のInitialize()を呼ぶだけ</summary>
		//--------------------------------------------------------------------------------
		public virtual void Initialize( )
		{
			for ( int i = 0; i < m_list_Child_drawable.Count; i++ )
			{
				m_list_Child_drawable[ i ].Initialize( );
			}
		}
		//--------------------------------------------------------------------------------
		/// <summary>グラフィックの設定 mode =1 通常時 =2 マウスクリック時 =3 マウスオーバー時</summary>
		//--------------------------------------------------------------------------------
		public virtual void SetGraphicHandle( int nGraphicHandle,int nMode )
		{
			if ( nMode == 1 )
			{
				m_nHandle_Graphic_OFF = nGraphicHandle;
				m_nHandle_Graphic = m_nHandle_Graphic_OFF;
				DX.GetGraphSize( nGraphicHandle, out m_nSize_x, out m_nSize_y );
			}
			if ( nMode == 2 )
			{
				m_nHandle_Graphic_ON = nGraphicHandle;
			}
			if ( nMode == 3 )
			{
				m_nHandle_Graphic_OVER = nGraphicHandle;
			}
		}
		//--------------------------------------------------------------------------------
		/// <summary>描画子オブジェクトの追加。先に追加されたものから順に描画される</summary>
		//--------------------------------------------------------------------------------
		public virtual void AddChildDrawable( Drawable drawable, Drawable parent )
		{
			//	すでに存在する子供かチェック
			if ( m_list_Child_drawable.Contains( drawable ) == true ) return;
			//	親を設定
			drawable.m_Drawable_Parent = parent;
			//	リストに追加
			m_list_Child_drawable.Add( drawable );
			//	追加した子ウィンドウのイベントハンドラを設定する
			drawable.InnerPostEvent += new InnerPostEventHandler( Invoke_InnerPostEvent );
			//	最後に追加されたウィンドウの最前面表示フラグをオンにする
			for ( int i = 0; i < m_list_Child_drawable.Count; i++ ) {
				m_list_Child_drawable[ i ].m_bCurrentTop = false;
				if ( i == m_list_Child_drawable.Count - 1 ) {
					m_list_Child_drawable[ i ].m_bCurrentTop = true;
				}
			}
		}
		//--------------------------------------------------------------------------------
		/// <summary>描画子オブジェクトの削除</summary>
		//--------------------------------------------------------------------------------
		public virtual void RemoveChildDrawable( Drawable drawable )
		{
			//	存在チェック
			if ( m_list_Child_drawable.Contains( drawable ) == false ) return;
			drawable.m_Drawable_Parent = null;
			m_list_Child_drawable.Remove( drawable );
			drawable.InnerPostEvent -= new InnerPostEventHandler( Invoke_InnerPostEvent );
		}
		//--------------------------------------------------------------------------------
		/// <summary>アニメーション情報の追加</summary>
		//--------------------------------------------------------------------------------
		public virtual void AddAnimation( int nGraphicHandle, int nShowCount )
		{
			if ( m_animation == null ) m_animation = new Animation( );
			m_animation.AddAnimate( nGraphicHandle, nShowCount );
		}
		//================================================================================
		/// <summary>アニメーションの設定</summary>
		//================================================================================
		public virtual void SetAnimation( Animation anime )
		{
			m_animation = anime;
		}
		//--------------------------------------------------------------------------------
		//	マウス検知
		//--------------------------------------------------------------------------------
		protected virtual void MouseAct( )
		{
			//	非表示の場合処理しない
			if ( m_bDoDraw == false ) return;
			//	親が非表示の場合処理しない
			if ( m_Drawable_Parent != null ) {
				if ( m_Drawable_Parent.bDoDraw == false ) return;
			}
			//	マウス排他制御によってマウスオーバーが外れないことがあるので、いったんOFFにしておく
			m_nHandle_Graphic = m_nHandle_Graphic_OFF;
			//	ドラッグアウトされたウィンドウがあれば受け入れチェック
			if ( s_win_DragOutWindow != null && m_bEnableDragOut == true ) {
				if ( IsDragOutCheck( ) == true ) return;
			}
			//	ドラッグ中のウィンドウがあれば以降処理しない
			if ( s_win_DraggingWindow != null && s_win_DraggingWindow != this ) return;
			//	ドラッグチェック（このウィンドウがドラッグ中ならドラッグ処理を行う）
			if ( Draging( ) == true ) {
				return;
			}
			//	マウスの状態取得
			int nMouseX,nMouseY;
			DX.GetMousePoint( out nMouseX, out nMouseY );
			//	マウスが描画矩形範囲にあるかどうかチェックするフラグをリセット
			m_bMouseOnRect = false;
			//	マウスが描画矩形範囲にあるかどうかチェック
			if ( nMouseX >= m_nDrawPosScreen_X && nMouseX <= m_nDrawPosScreen_X + m_nSize_x && nMouseY >= m_nDrawPosScreen_Y && nMouseY <= m_nDrawPosScreen_Y + m_nSize_y ) {
				m_bMouseOnRect = true;
			}
			//	ウィンドウ矩形内部にマウスカーソルがなければ処理しない
			if ( m_bMouseOnRect == false ) return;
			//	マウスオーバーレイ処理を実行したウィンドウがすでにあれば処理しない
			if ( s_win_OnRectMouseOver != null ) return;
			//	マウスオーバーレイ処理実行
			if ( m_nHandle_Graphic_OVER != -1 ) m_nHandle_Graphic = m_nHandle_Graphic_OVER;
			//	親にマウスオーバーレイ処理を実行したことを通知する
			s_win_OnRectMouseOver = this;
			//	クリック処理を受け入れるならクリックチェックを実行
			if ( m_bMouseButtonLeftClickEnable == true ) {
				//	クリック処理をすでに実行したウィンドウがあるならば処理しない
				if ( s_win_OnRectMouseClick != null ) return;
				//	マウスクリックチェックを実行
				if ( IsMouseClickCheck( ) == true ) {
					//	クリック処理を実行したことを親に通知
					s_win_OnRectMouseClick = this;
				}
			}
			
		}
		//--------------------------------------------------------------------------------
		//	ドラッグ処理
		//--------------------------------------------------------------------------------
		private bool Draging( )
		{
			//	ドラッグを許可するかチェック
			if ( m_bDragEnable == false ) return false;
			//	ドラッグ開始をチェック
			if ( m_bMouseButtonLeftClickCheck == false ) return false;
			//	ドラッギングフラグが立っていなければドラッグ開始処理
			if ( m_bDraggingNow == false ) {
				//	ボタンチェック。左ボタンが押されていなければそもそも処理は開始しない
				if ( ( DX.GetMouseInput( ) & DX.MOUSE_INPUT_LEFT ) == 0 ) return false;
				//	現在のマウス座標を保存
				int nMouseX, nMouseY;
				DX.GetMousePoint( out nMouseX, out nMouseY );
				m_nPrevMouseX = nMouseX;
				m_nPrevMouseY = nMouseY;
				m_fBeginDragX = m_nDrawPosScreen_X;
				m_fBeginDragY = m_nDrawPosScreen_Y;
				//	ドラッグ開始フラグオン
				m_bDraggingNow = true;
				//	ドラッグ中の描画オブジェクトとして自分を保存
				s_win_DraggingWindow = this;
				//	ドラッグ開始時の親オブジェクトを保存
				s_win_DraggingOriginalParentWindow = m_Drawable_Parent;
				GameMain.DebugLog( "ドラッグ開始\n" );
				//	まず現在の親から外して
				m_Drawable_Parent.RemoveChildDrawable( this );
				//	シーンに渡す
				s_GameMain.GetCurrentScene( ).AddChildDrawable( this, s_GameMain.GetCurrentScene( ) );
				//	オフセット調整
				FixOffset( );
			}
			else {
				//	ドラッギングフラグが立っていればドラッグ処理
				//	マウスの移動量を取得
				int nMouseX, nMouseY;
				DX.GetMousePoint( out nMouseX, out nMouseY );
				int nDisX = nMouseX - m_nPrevMouseX;
				int nDisY = nMouseY - m_nPrevMouseY;
				//	オフセット座標を更新
				m_nDrawPos_X += nDisX;
				m_nDrawPos_Y += nDisY;
				//	現在のマウス座標を保存
				m_nPrevMouseX = nMouseX;
				m_nPrevMouseY = nMouseY;
				//	ドラッグが終了したかどうかチェック
				//	マウスボタンが離されていればドラッグ中状態を解除
				if ( ( DX.GetMouseInput( ) & DX.MOUSE_INPUT_LEFT ) == 0 ) {
					m_bMouseButtonLeftClickCheck = false;
					m_bDraggingNow = false;
					s_win_DraggingWindow = null;
					s_win_DragOutWindow = this;
				}
			}
			return true;
		}
		//--------------------------------------------------------------------------------
		/// <summary>何かが自分にドラッグされようとしているとき</summary>
		//--------------------------------------------------------------------------------
		protected virtual bool IsDragOutCheck( )
		{
			//	範囲チェック
			int nPosX = s_win_DragOutWindow.nDrawPos_X + s_win_DragOutWindow.m_nSize_x / 2;
			int nPosY = s_win_DragOutWindow.nDrawPos_Y + s_win_DragOutWindow.m_nSize_y / 2;
			Rectangle rect = new Rectangle(
				m_nDrawPos_X,
				m_nDrawPos_Y,
				m_nSize_x,
				m_nSize_y);
			if ( rect.Contains( nPosX, nPosY ) == false ) return false;
			GameMain.DebugLog( "ドラッグアウト処理を行います.\n" );
			//	親から外して自分の子供にする
			s_win_DragOutWindow.m_Drawable_Parent.RemoveChildDrawable( s_win_DragOutWindow );
			AddChildDrawable( s_win_DragOutWindow, this );
			//	イベント処理
			DragOut( );
			s_win_DragOutWindow.FixOffset( );
			s_win_ExecDragOutWindow = this;
			s_win_DragOutWindow = null;
			return true;
		}
		//--------------------------------------------------------------------------------
		//	マウスクリックチェック
		//--------------------------------------------------------------------------------
		private bool IsMouseClickCheck( )
		{
			//	リリースされており、かつ押された形跡があればクリックとみなす
			if(( DX.GetMouseInput( ) & DX.MOUSE_INPUT_LEFT ) == 0 && m_bMouseButtonLeftClickCheck == true){
				//	左クリックチェック用フラグをリセット
				m_bMouseButtonLeftClickCheck = false;
				//	左クリック処理実行
				MouseLeftClick( );
				return true;
			}
			//	押されているか
			else if ( ( DX.GetMouseInput( ) & DX.MOUSE_INPUT_LEFT ) != 0 ) {
				//	左ボタンが押されていればクリックチェック用フラグをセット
				m_bMouseButtonLeftClickCheck = true;
				//	最前面フラグがあれば最前面要求する
				if ( m_bTopDraw == true ) s_win_BringToTop = this;
				return true;
			}
			return false;
		}
		//--------------------------------------------------------------------------------
		//	他のウィンドウがドラッグされた時処理
		//--------------------------------------------------------------------------------
		protected virtual void DragOut( )
		{
			//	標準ドラッグアウトイベントが設定されている場合はイベントを投げる
			if ( m_bDefaultDragOutEvent == true ) {
				m_oDragOutEventObject = s_win_DragOutWindow;
				m_InnerPostEventArgs.oGenericObject = m_oDragOutEventObject;
				BridgeEvent( );
			}
		}
		//--------------------------------------------------------------------------------
		//	描画オフセット値を、現在の座標から逆算して設定する
		//--------------------------------------------------------------------------------
		public void FixOffset( )
		{
			m_nDrawPos_X = m_nDrawPosScreen_X - m_Drawable_Parent.nDrawPosScreen_X;
			m_nDrawPos_Y = m_nDrawPosScreen_Y - m_Drawable_Parent.nDrawPosScreen_Y;
		}
		//--------------------------------------------------------------------------------
		/// <summary>ドラッグをなかったことにする</summary>
		//--------------------------------------------------------------------------------
		public void UnDoDrag( )
		{
			m_nDrawPosScreen_X = m_fBeginDragX;
			m_nDrawPosScreen_Y = m_fBeginDragY;
		}
		//--------------------------------------------------------------------------------
		//	左クリック実行時処理
		//--------------------------------------------------------------------------------
		protected virtual void MouseLeftClick( )
		{
			//	標準イベントが設定されている場合はイベントを投げる
			if ( m_bDefaultMouseLeftClickEvent == true )
			{
				BridgeEvent( );
			}
		}
		//--------------------------------------------------------------------------------
		//	右クリック実行時処理
		//--------------------------------------------------------------------------------
		protected virtual void MouseRightClick( )
		{
			//	標準イベントが設定されている場合はイベントを投げる
			if ( m_bDefaultMouseRightClickEvent == true )
			{
				BridgeEvent( );
			}
		}
		//--------------------------------------------------------------------------------
		//	更新
		//--------------------------------------------------------------------------------
		public virtual void Update( )
		{
			//	所有ウィンドウの更新（最前面から処理する）
			for ( int i = m_list_Child_drawable.Count - 1; i >= 0; i-- ) {
				//	所有ウィンドウのUpdate
				m_list_Child_drawable[ i ].Update( );
			}
			//	最前面更新要求のあるウィンドウが自分の子供なら並べ替える
			if ( m_list_Child_drawable.Contains( s_win_BringToTop ) == true ) {
				if ( s_win_BringToTop != null ) {
					m_list_Child_drawable.Remove( s_win_BringToTop );
					m_list_Child_drawable.Add( s_win_BringToTop );
					//	最後に追加されたウィンドウの最前面表示フラグをオンにする
					for ( int i = 0; i < m_list_Child_drawable.Count; i++ ) {
						m_list_Child_drawable[ i ].m_bCurrentTop = false;
						if ( i == m_list_Child_drawable.Count - 1 ) {
							m_list_Child_drawable[ i ].m_bCurrentTop = true;
						}
					}
				}
			}
			//	マウス検知が true ならばマウス検知実行
			if ( m_bMouseAct == true ) {
				MouseAct( );
			}
			//	マウス検知が false ならば通常テクスチャを使用
			else {
				m_nHandle_Graphic = m_nHandle_Graphic_OFF;
			}
		}
		//--------------------------------------------------------------------------------
		//	描画
		//--------------------------------------------------------------------------------
		public virtual void Draw( )
		{
			//	描画フラグが立っていない場合は何もしない
			if ( m_bDoDraw == false ) return;
			//	アニメーション情報がある場合は更新する
			if ( m_animation != null ) {
				m_nHandle_Graphic = m_animation.GetGraphicHandle( );
			}
			//	グラフィックハンドルが設定されていない場合は何もしない
			if ( m_nHandle_Graphic == -1 ) return;
			//	親がある場合は相対座標から描画座標（スクリーン座標）を更新
			if ( m_Drawable_Parent != null && m_bPosOnParent == true )
			{
				m_nDrawPosScreen_X = m_Drawable_Parent.nDrawPosScreen_X + m_nDrawPos_X;
				m_nDrawPosScreen_Y = m_Drawable_Parent.nDrawPosScreen_Y + m_nDrawPos_Y;
			}
			//	親がない場合はスクリーン座標に直接変換
			else
			{
				m_nDrawPosScreen_X = m_nDrawPos_X;
				m_nDrawPosScreen_Y = m_nDrawPos_Y;
			}
			DX.DrawGraph( m_nDrawPosScreen_X, m_nDrawPosScreen_Y, m_nHandle_Graphic, m_nTransFlag );
			//	子描画オブジェクトの描画
			for ( int i = 0; i < m_list_Child_drawable.Count; i++ )
			{
				m_list_Child_drawable[ i ].Draw( );
			}
		}
	}
}
