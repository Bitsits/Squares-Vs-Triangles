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

namespace BitSits_Framework
{
    struct Tile
    {
        public const int Width = 50;
        public const int Height = Width;

        public static Rectangle GetBounds(Vector2 position)
        {
            int y = (int)Math.Floor(position.Y / Height);

            // Hex packing
            //if (y % 2 == 0) position.X -= Width / 2;
            //int x = (int)Math.Floor(position.X / Width);
            //return new Rectangle(x * Width + (y % 2 == 0 ? Width / 2 : 0), y * Height, Width, Height);

            int x = (int)Math.Floor(position.X / Width);
            return new Rectangle(x * Width, y * Height, Width, Height);
        }
    }
}
