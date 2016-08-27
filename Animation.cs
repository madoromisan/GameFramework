using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;

namespace GameFramework
{
	//================================================================================
	//	アニメーション管理クラス
	//================================================================================
	public class Animation
	{
		/// <summary>アニメーションインデックス</summary>
		private int m_nIndex = 0;
		/// <summary>アニメーション用グラフィックハンドルを格納するリスト</summary>
		private List<int> m_list_graphic_handle = null;
		/// <summary>アニメーション用表示カウンターを格納するリスト</summary>
		private List<int> m_list_animate_counter = null;
		/// <summary>アニメーション管理用カウンター</summary>
		private int m_nCounter = 0;
		//--------------------------------------------------------------------------------
		//	コンストラクタ
		//--------------------------------------------------------------------------------
		public Animation( )
		{
			m_list_graphic_handle = new List<int>( );
			m_list_animate_counter = new List<int>( );
		}
		//--------------------------------------------------------------------------------
		//	アニメーションカットの追加
		//--------------------------------------------------------------------------------
		public void AddAnimate( int nGraphicHandle, int nShowCount )
		{
			m_list_graphic_handle.Add( nGraphicHandle );
			m_list_animate_counter.Add( nShowCount );
		}
		//--------------------------------------------------------------------------------
		//	アニメーションに応じたグラフィックハンドルを取得
		//--------------------------------------------------------------------------------
		public int GetGraphicHandle( )
		{
			m_nCounter++;
			if ( m_nCounter > m_list_animate_counter[ m_nIndex ] )
			{
				m_nCounter = 0;
				m_nIndex++;
				if ( m_nIndex >= m_list_animate_counter.Count ) m_nIndex = 0;
			}
			return m_list_graphic_handle[ m_nIndex ];
		}
	}
}
