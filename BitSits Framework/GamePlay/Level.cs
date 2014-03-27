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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.IO;
using GameDataLibrary;

namespace BitSits_Framework
{
    class Level : IDisposable
    {
        #region Fields

        public int Score { get; private set; }

        public bool IsLevelUp { get; private set; }
        public bool ReloadLevel { get; private set; }
        int levelIndex;

        GameContent gameContent;

        List<Army> armies = new List<Army>();
        List<Vector2> armyPosition = new List<Vector2>();
        Army mouseOverArmy, selectedArmy;

        Castle[] castles = new Castle[2];
        float MaxEnemyWaitTime = 2f; float enemyWaitTime;

        #endregion

        #region Initialization


        public Level(GameContent gameContent, int levelIndex)
        {
            this.gameContent = gameContent;
            this.levelIndex = levelIndex;

            LevelDetails ld = gameContent.content.Load<LevelDetails>("Levels/level" + levelIndex.ToString("00"));

            Vector2 castlePos = new Vector2(1.5f, 6.5f) *Tile.Width;
            armyPosition.Add(castlePos);
            castles[(int)Shape.square] = new Castle(gameContent, Shape.square, castlePos);
            castles[(int)Shape.triangle] = new Castle(gameContent, Shape.triangle, new Vector2(800, 600)
                - castlePos);

            for (int i = 0; i < ld.ArmyNumber.Count; i++)
                for (int j = 0; j < ld.ArmyNumber[i]; j++)
                {
                    Vector2 v = GetPosition();
                    armies.Add(new Army(gameContent, Shape.square, (Rank)i, v));
                    armies.Add(new Army(gameContent, Shape.triangle, (Rank)i, new Vector2(800, 600) - v));
                }

            enemyWaitTime = 0;
            MaxEnemyWaitTime = MaxEnemyWaitTime / (float)Math.Pow(10, ld.GameMode);
        }


        Vector2 GetPosition()
        {
            while (true)
            {
                int i = gameContent.random.Next(5);
                int j = gameContent.random.Next(11);

                Point p = Tile.GetBounds(new Vector2(i, j) * Tile.Width).Center;
                Vector2 v = new Vector2(p.X, p.Y);

                if (!armyPosition.Contains(v))
                {
                    armyPosition.Add(v); return v;
                }
            }
        }


        public void Dispose() { }


        #endregion

        #region Update and HandleInput


        public void Update(GameTime gameTime)
        {
            enemyWaitTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            int enemyAIindex = gameContent.random.Next(armies.Count / 2) * 2 + 1;

            bool noneAlive = true;
            for (int i = armies.Count - 1; i >= 0; i--)
            {
                armies[i].Update(gameTime);

                if (armies[i].IsAlive)
                {
                    armies[i].HandleEnemy(armies);
                    if (armies[i].Shape == Shape.triangle && enemyWaitTime > MaxEnemyWaitTime
                        && i == enemyAIindex) // probability it will be moved or not
                    {
                        enemyWaitTime = 0;
                        armies[i].EnemyAI(castles[(int)Shape.square].AttackRectangle);
                    }
                }

                if (armies[i].Shape == Shape.square
                    && castles[(int)Shape.triangle].position == armies[i].position) 
                    IsLevelUp = true;
                if (armies[i].Shape == Shape.triangle
                    && castles[(int)Shape.square].position == armies[i].position)
                    ReloadLevel = true;

                if (armies[i].IsAlive && armies[i].Shape == Shape.square) noneAlive = false;
            }

            if (noneAlive) ReloadLevel = true;
        }


        public void HandleInput(InputState input, int playerIndex)
        {
            Vector2 mousePosition = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y) 
                / BitSitsGames.PhoneScale;
            Point mouseP = new Point((int)mousePosition.X, (int)mousePosition.Y);

            mouseOverArmy = null;
            for (int i = 0; i < armies.Count; i++)
            {
                if (armies[i].MouseBounds.Contains(mouseP) && armies[i].IsAlive && selectedArmy == null
                    && armies[i].Shape == Shape.square)
                {
                    mouseOverArmy = armies[i]; break;
                }
            }

            if (input.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (input.IsLeftClicked() && mouseOverArmy != null)
                {
                    selectedArmy = mouseOverArmy; mouseOverArmy = null;
                    selectedArmy.Reset();
                }

                if(selectedArmy!= null) selectedArmy.AddPosition(mousePosition);
            }

            if ((selectedArmy != null && input.LastMouseState.LeftButton == ButtonState.Pressed
                && input.CurrentMouseState.LeftButton == ButtonState.Released)
                || (selectedArmy != null && !selectedArmy.IsAlive))
                selectedArmy = null;
        }


        #endregion

        #region Draw


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameContent.background, Vector2.Zero, Color.White);

            for (int i = 0; i < armies.Count; i++) armies[i].DrawPath(gameTime, spriteBatch);

            if (mouseOverArmy != null) mouseOverArmy.DrawMouseOver(spriteBatch);
            if (selectedArmy != null) selectedArmy.DrawMouseOver(spriteBatch);

            for (int i = 0; i < armies.Count; i++) armies[i].Draw(gameTime, spriteBatch);

            foreach (Castle c in castles) c.Draw(spriteBatch);
        }


        #endregion
    }
}
