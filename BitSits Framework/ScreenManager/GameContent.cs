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
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameDataLibrary;

namespace BitSits_Framework
{
    /// <summary>
    /// All the Contents of the Game is loaded and stored here
    /// so that all other screen can copy from here
    /// </summary>
    public class GameContent
    {
        public ContentManager content;
        public Vector2 GameplayArea = new Vector2(800, 600);
        
        public Random random = new Random();

        public int levelIndex;

        ///GenerateData gd = new GenerateData();

        // Textures
        public Texture2D blank, gradient;
        public Texture2D menuBackground;

        public Texture2D background;
        public Texture2D[] walk = new Texture2D[2], idle = new Texture2D[2], die = new Texture2D[2];

        public Texture2D mouseOver;
        public Vector2 mouseOverOrigin;

        public Texture2D[] castle = new Texture2D[2];
        public Vector2 castleOrigin;

        public Texture2D pathArrow, pathCross;

        public Texture2D healthBar;

        const int MaxRank = 3;
        public Texture2D[] weapon = new Texture2D[MaxRank], armyOverlay = new Texture2D[MaxRank],
            bullet = new Texture2D[MaxRank];

        public Texture2D[] tutorial = new Texture2D[3];
        public Texture2D levelUp, retry;

        // Fonts
        public SpriteFont debugFont, gameFont;

        // Audio objects
        public AudioEngine audioEngine;
        public SoundBank soundBank;
        public WaveBank waveBank;
        

        /// <summary>
        /// Load GameContents
        /// </summary>
        public GameContent(GameComponent screenManager)
        {
            content = screenManager.Game.Content;
            Viewport viewport = screenManager.Game.GraphicsDevice.Viewport;
            //viewportSize = new Vector2(800, 600);

            blank = content.Load<Texture2D>("Graphics/blank");
            gradient = content.Load<Texture2D>("Graphics/gradient");
            menuBackground = content.Load<Texture2D>("Graphics/menuBackground2");

            background = content.Load<Texture2D>("Graphics/background");

            for (int i = 0; i < walk.Length; i++)
            {
                walk[i] = content.Load<Texture2D>("Graphics/" + (Shape)i + "Walk");
                idle[i] = content.Load<Texture2D>("Graphics/" + (Shape)i + "Idle");
                die[i] = content.Load<Texture2D>("Graphics/" + (Shape)i + "Die");
            }

            mouseOver = content.Load<Texture2D>("Graphics/mouseOver");
            mouseOverOrigin = new Vector2(mouseOver.Width, mouseOver.Height) / 2;

            for (int i = 0; i < castle.Length; i++)
                castle[i] = content.Load<Texture2D>("Graphics/" + (Shape)i + "Castle");
            castleOrigin = new Vector2(castle[0].Width / 2, castle[0].Height);

            pathArrow = content.Load<Texture2D>("Graphics/pathArrow");
            pathCross = content.Load<Texture2D>("Graphics/pathCross");

            healthBar = content.Load<Texture2D>("Graphics/healthBar");

            for (int i = 0; i < MaxRank; i++)
            {
                weapon[i] = content.Load<Texture2D>("Graphics/" + (Rank)i + "Weapon");
                //armyOverlay[i] = content.Load<Texture2D>("Graphics/" + ((Rank)(i)).ToString() + "Overlay");
                bullet[i] = content.Load<Texture2D>("Graphics/" + (Rank)i + "Bullet");
            }

            for (int i = 0; i < tutorial.Length; i++)
                tutorial[i] = content.Load<Texture2D>("Graphics/tutotrial" + i);

            levelUp = content.Load<Texture2D>("Graphics/levelUp");
            retry = content.Load<Texture2D>("Graphics/retry");

            debugFont = content.Load<SpriteFont>("Fonts/debugFont");
            gameFont = content.Load<SpriteFont>("Fonts/bicho_plumon50");

            audioEngine = new AudioEngine("Content/Audio/Audio.xgs");
            soundBank = new SoundBank(audioEngine, "Content/Audio/Sound Bank.xsb");
            waveBank = new WaveBank(audioEngine, "Content/Audio/Wave Bank.xwb");

            soundBank.PlayCue("Marching by Pill");

            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            screenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload GameContents
        /// </summary>
        public void UnloadContent() { content.Unload(); }
    }
}
