using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GameFramework
{
	public class DebugWindow : Form
	{
		private TextBox textBox1 = null;
		// Win32 APIのインポート
		[DllImport( "USER32.DLL" )]
		private static extern IntPtr GetSystemMenu( IntPtr hWnd, UInt32 bRevert );
		[DllImport( "USER32.DLL" )]
		private static extern UInt32 RemoveMenu( IntPtr hMenu, UInt32 nPosition, UInt32 wFlags );
		// ［閉じる］ボタンを無効化するための値
		private const UInt32 SC_CLOSE = 0x0000F060;
		private const UInt32 MF_BYCOMMAND = 0x00000000;
		public DebugWindow( )
		{
			textBox1 = new TextBox( );
			textBox1.ReadOnly = true;
			textBox1.Multiline = true;
			textBox1.ScrollBars = ScrollBars.Both;
			textBox1.Dock = DockStyle.Fill;
			textBox1.BackColor = Color.Black;
			textBox1.ForeColor = Color.White;
			this.Controls.Add( textBox1 );
			this.Text = "DebugWindow";
			this.FormClosing += new FormClosingEventHandler( DebugWindow_FormClosing );
			// コントロールボックスの［閉じる］ボタンの無効化
			IntPtr hMenu = GetSystemMenu( this.Handle, 0 );
			RemoveMenu( hMenu, SC_CLOSE, MF_BYCOMMAND );
		}
		void DebugWindow_FormClosing( object sender, FormClosingEventArgs e )
		{
			e.Cancel = true;
		}
		public void WriteLog( string szText )
		{
			this.WriteLog( szText, Color.White );
		}
		public void WriteLog( string szText, Color col )
		{
			if ( textBox1.Text.Length > 30000 ) textBox1.Clear( );
			textBox1.AppendText( szText );
			textBox1.ScrollToCaret( );
		}

	}
}
