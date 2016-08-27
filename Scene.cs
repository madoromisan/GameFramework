using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
	//================================================================================
	//	シーンクラス
	//================================================================================
	public class Scene : Drawable
	{
		//--------------------------------------------------------------------------------
		//	コンストラクタ
		//--------------------------------------------------------------------------------
		public Scene( )
		{
		}
		//--------------------------------------------------------------------------------
		/// <summary>シーン開始時の処理</summary>
		//--------------------------------------------------------------------------------
		public virtual void EnterScene( )
		{

		}
		//--------------------------------------------------------------------------------
		/// <summary>シーン終了時の処理</summary>
		//--------------------------------------------------------------------------------
		public virtual void ExitScene( )
		{

		}
		//--------------------------------------------------------------------------------
		//	マウス検知
		//--------------------------------------------------------------------------------
		protected override void MouseAct( )
		{
			base.MouseAct( );
		}
		//--------------------------------------------------------------------------------
		//	左クリック実行時処理
		//--------------------------------------------------------------------------------
		protected override void MouseLeftClick( )
		{
			base.MouseLeftClick( );
		}
		//--------------------------------------------------------------------------------
		//	右クリック実行時処理
		//--------------------------------------------------------------------------------
		protected override void MouseRightClick( )
		{
			base.MouseRightClick( );
		}
		//--------------------------------------------------------------------------------
		//	更新処理
		//--------------------------------------------------------------------------------
		public override void Update( )
		{
			//	マウス処理関係の初期化
			s_win_BringToTop = null;
			s_win_OnRectMouseClick = null;
			s_win_OnRectMouseOver = null;
			s_win_DragOutWindow = null;
			s_win_ExecDragOutWindow = null;
			base.Update( );
			//	処理の必要なドラッグアウトウィンドウが処理されないまま終わった場合
			if ( s_win_DragOutWindow != null && s_win_DragOutWindow.bNeedDragOutExec == true && s_win_ExecDragOutWindow == null ) {
				//	ドラッグをなかったことにする
				//	親から外して自分の子供にする
				s_win_DragOutWindow.Drawable_Parent.RemoveChildDrawable( s_win_DragOutWindow );
				s_win_DraggingOriginalParentWindow.AddChildDrawable( s_win_DragOutWindow, s_win_DraggingOriginalParentWindow );
				s_win_DragOutWindow.UnDoDrag( );
				s_win_DragOutWindow.FixOffset( );
				GameMain.DebugLog( "ドラッグアウトを無効とします.\n" );
			}
		}
	}
}
