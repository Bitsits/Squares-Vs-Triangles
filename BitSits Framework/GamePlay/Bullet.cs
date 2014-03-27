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
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    class Bullet
    {
        Rank rank;

        public float totalDistance;
        float rotation = 0, speed = 3, direction;
        Vector2 position;

        Texture2D texture;

        public Bullet(Texture2D texture, Rank rank, Vector2 position, float direction)
        {
            this.position = position;
            this.direction = direction;
            this.texture = texture;
            this.rank = rank;
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                int halfSize = 10 / 2;
                return new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize,
                    2 * halfSize, 2 * halfSize);
            }
        }

        public void Update(GameTime gameTime)
        {
            position += new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction)) * speed;
            totalDistance += speed;

            if (rank == Rank.ninja) rotation += 10f / 180 * (float)Math.PI;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, direction + rotation,
                new Vector2(Tile.Width) / 2, 1,
                Math.Abs(direction) > (float)Math.PI / 2 ? SpriteEffects.FlipVertically : SpriteEffects.None, 1);
        }
    }
}
