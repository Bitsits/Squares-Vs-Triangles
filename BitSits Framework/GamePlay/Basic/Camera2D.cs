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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    /// <summary>
    /// Very basic sample program for demonstrating a 2D Camera
    /// Controls are WASD for movement, QE for rotation, and ZC for zooming.
    /// </summary>
    class Camera2D
    {
        //Vector2 viewportSize;
        bool ManualCamera = false, isMovingUsingScreenAxis = true;

        public Vector2 Position, ScrollArea, ScrollBar, Origin;
        public float Rotation, Scale = 1, Speed = 10;

        public Camera2D()
        {
            ScrollArea = BitSitsGames.viewportSize;

            ScrollBar = new Vector2(10);

            Origin = Position = BitSitsGames.viewportSize / 2;
        }

        public Matrix Transform
        {
            get
            {
                float s = BitSitsGames.PhoneScale;
                return Matrix.CreateScale(new Vector3(s, s, 0))
                    * Matrix.CreateTranslation(new Vector3(-Position, 0))
                    * Matrix.CreateRotationZ(-Rotation) * Matrix.CreateScale(new Vector3(Scale, Scale, 0))
                    * Matrix.CreateTranslation(new Vector3(Origin, 0));
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void HandleInput(InputState input, int playerIndex)
        {
            if (ManualCamera)
            {
                //translation controls WASD
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.A)) MoveCamera(new Vector2(-1, 0));
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.D)) MoveCamera(new Vector2(1, 0));
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.S)) MoveCamera(new Vector2(0, 1));
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.W)) MoveCamera(new Vector2(0, -1));

                //rotation controls QE
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Q)) Rotation += 0.01f;
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.E)) Rotation -= 0.01f;

                //zoom/scale controls CX
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.C)) Scale += 0.001f;
                if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.X)) Scale -= 0.001f;
            }

            /*
            Vector2 mousePos = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y);
            if (mousePos.X < ScrollBar.X) Position.X -= Speed;
            else if (mousePos.X > viewportSize.X - ScrollBar.X) Position.X += Speed;

            if (mousePos.Y < ScrollBar.Y) Position.Y -= Speed;
            else if (mousePos.Y > viewportSize.Y - ScrollBar.Y) Position.Y += Speed;
            

            // Clamp
            Position.X = MathHelper.Clamp(Position.X, viewportSize.X / 2 / Scale,
                (ScrollArea.X - viewportSize.X / 2 / Scale));
            Position.Y = MathHelper.Clamp(Position.Y, viewportSize.Y / 2 / Scale,
                (ScrollArea.Y - viewportSize.Y / 2 / Scale));
            */
        }

        void MoveCamera(Vector2 direction)
        {
            if (isMovingUsingScreenAxis)
            {
                float theta = (float)Math.Atan2(direction.Y, direction.X);
                theta += -Rotation;
                Position += new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * Speed;
            }
            else
                Position += direction * Speed;
        }
    }
}
