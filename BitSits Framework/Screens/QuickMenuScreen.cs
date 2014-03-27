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

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    class QuickMenuScreen : GameScreen
    {
        Camera2D camera = new Camera2D();


        public override void HandleInput(InputState input)
        {
            if (input.IsLeftClicked())
            {
                LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, 
                    new GameplayScreen(), new PauseMenuScreen());

                ExitScreen();
            }
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 1 / 3);


            // Fade the popup alpha during transitions.
            Color color = new Color(Color.White, TransitionAlpha);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
                SaveStateMode.SaveState, camera.Transform);

            spriteBatch.Draw(ScreenManager.GameContent.menuBackground, Vector2.Zero, color);

            spriteBatch.End();
        }
    }
}
