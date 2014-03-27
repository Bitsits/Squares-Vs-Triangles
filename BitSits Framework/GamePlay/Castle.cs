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
    class Castle
    {
        public readonly Shape Shape;
        GameContent gameContent;

        public readonly Vector2 position;
        public readonly Rectangle BoundingRectangle;
        public readonly Rectangle AttackRectangle;

        public bool ChangeFlag = false;

        public Castle(GameContent gameContent, Shape shape, Vector2 tileCenter)
        {
            this.Shape = shape;
            this.gameContent = gameContent;

            this.position = tileCenter;

            int halfSize = Tile.Width / 2;
            BoundingRectangle = new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize,
                2 * halfSize, 2 * halfSize);

            AttackRectangle = BoundingRectangle; AttackRectangle.Inflate(200, 600);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameContent.castle[(int)Shape],
                position + new Vector2(0, Tile.Height / 2) - gameContent.castleOrigin,
                new Color(Color.White, .8f));
        }
    }
}
