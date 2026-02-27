using System;
using GameFramework;

namespace SampleGame
{
	internal enum DemoEvent
	{
		SampleClick,
	}

	internal class DemoScene : Scene
	{
		public override void EnterScene( )
		{
			GameMain.DebugLog( "DemoScene.EnterScene\n" );
		}

		public override void ExitScene( )
		{
			GameMain.DebugLog( "DemoScene.ExitScene\n" );
		}
	}

	internal class DemoGameMain : GameMain
	{
		private DemoScene m_demoScene;

		public override void Initialize( )
		{
			base.Initialize( );
			InitDebugWindow( );

			m_demoScene = new DemoScene( );
			AddScene( m_demoScene );
			AddEventFunc( DemoEvent.SampleClick, OnSampleClick );
			ChangeScene( m_demoScene );
		}

		private void OnSampleClick( object sender, InnerPostEventArgs e )
		{
			GameMain.DebugLog( "Sample event received\n" );
		}
	}

	internal static class Program
	{
		[STAThread]
		private static void Main( )
		{
			DemoGameMain app = new DemoGameMain( );
			app.Initialize( );
			app.MainLoop( );
		}
	}
}
