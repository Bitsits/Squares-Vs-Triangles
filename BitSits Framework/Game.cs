/*
 * Copyright (c) 2011 BitSits Games
 *  
 * Shubhajit Saha    http://bitsits.blogspot.com/
 * Maya Agarwal      http://bitsitsgames.blogspot.com/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class BitSitsGames : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public static Vector2 viewportSize = new Vector2(800, 600);
        public static float PhoneScale = 1;

        #endregion

        #region Initialization


        /// <summary>
        /// The main game constructor.
        /// </summary>
        public BitSitsGames()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

#if WINDOWS_PHONE
            viewportSize = new Vector2(640, 480);
            PhoneScale = 0.8f;
#endif

            graphics.PreferredBackBufferWidth = (int)viewportSize.X;
            graphics.PreferredBackBufferHeight = (int)viewportSize.Y;  

            IsMouseVisible = true;
            graphics.IsFullScreen = true;

            // Create the screen manager component.
            screenManager = new ScreenManager(this, graphics);

            Components.Add(screenManager);

            LoadingScreen.Load(screenManager, false, null, new QuickMenuScreen());

            // TEST LEVELS
            //LoadingScreen.Load(screenManager, false, PlayerIndex.One, new GameplayScreen());
        }


        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #endregion
    }


    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (BitSitsGames game = new BitSitsGames())
            {
                game.Run();
            }
        }
    }

    #endregion
}
