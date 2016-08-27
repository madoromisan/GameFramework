using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
	//================================================================================
	//	GUI連携用
	//================================================================================
	/// <summary>クライアントインスタンスの連携用デリゲート</summary>
	public delegate void InnerPostEventHandler( object sender, InnerPostEventArgs e );
	//================================================================================
	//	クライアントインスタンスの連携用イベントパラメータ
	//================================================================================
	public class InnerPostEventArgs : EventArgs
	{
		private object _oGenericObject = null;
		private object _oArgsObject = null;
		//----------------------------------------------------------------------
		//	プロパティ
		//----------------------------------------------------------------------
		/// <summary>汎用授受オブジェクト</summary>
		public object oGenericObject { set { _oGenericObject = value; } get { return _oGenericObject; } }
		public object oArgsObject { get { return _oArgsObject; } }
		//----------------------------------------------------------------------
		//	コンストラクタ
		//----------------------------------------------------------------------
		/// <summary>引数にはゲーム固有のメッセージのEnum定義などを渡すと利用しやすい</summary>
		public InnerPostEventArgs( object oArgsObject )
		{
			_oArgsObject = oArgsObject;
		}
	}
}
