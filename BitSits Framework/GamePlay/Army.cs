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
using Microsoft.Xna.Framework.Audio;
using GameDataLibrary;

namespace BitSits_Framework
{
    enum Rank { archer, ninja, bazooka, tribal, bomber, sword, }

    enum Shape { square, triangle }

    class Army
    {
        GameContent gameContent;

        Rank Rank;
        public readonly Shape Shape;

        const float MaxRemoveTime = 5f, MaxPathVisibleTime = 2f;

        readonly float Damage, MaxHealth, MaxReloadTime, Range, MoveSpeed, defaultRotaion;
        float health, reloadTime, pathVisibleTime, rotation, removeTime = MaxRemoveTime + 2;

        public Vector2 position;

        Animation walkAnimation, idleAnimation, dieAnimation;
        AnimationPlayer animationPlayer = new AnimationPlayer();

        List<Vector2> destination = new List<Vector2>();
        List<Bullet> bullets = new List<Bullet>();

        public Army(GameContent gameContent, Shape shape, Rank rank, Vector2 position)
        {
            this.gameContent = gameContent;
            Shape = shape; Rank = rank;
            this.position = position;

            walkAnimation = new Animation(gameContent.walk[(int)shape], 2, 0.1f, true, new Vector2(0.5f));
            idleAnimation = new Animation(gameContent.idle[(int)shape], 1, 0.1f, true, new Vector2(0.5f));
            dieAnimation = new Animation(gameContent.die[(int)shape], 3, 0.1f, false, new Vector2(0.5f));
            animationPlayer.PlayAnimation(idleAnimation);

            ArmyDetails details = gameContent.content.Load<ArmyDetails>("Graphics/" + rank);
            Damage = details.Damage;
            health = MaxHealth = details.Health;
            MaxReloadTime = details.ReloadTime;
            Range = details.Range;
            MoveSpeed = details.MoveSpeed;

            rotation = defaultRotaion = (shape == Shape.square) ? 0 : (float)Math.PI;
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                int halfSize = 25 / 2;
                return new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize,
                    2 * halfSize, 2 * halfSize);
            }
        }

        public Rectangle MouseBounds
        {
            get
            {
                int halfSize = Tile.Width / 2;
                return new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize,
                    2 * halfSize, 2 * halfSize);
            }
        }

        public void Reset() { destination.Clear(); }

        public bool IsAlive { get { return health > 0; } }

        public void AddPosition(Vector2 nextDestPosition)
        {
            nextDestPosition.Y = MathHelper.Clamp(nextDestPosition.Y, 0, gameContent.GameplayArea.Y - 1);
            nextDestPosition.X = MathHelper.Clamp(nextDestPosition.X, 0, gameContent.GameplayArea.X - 1);

            while (true)
            {
                Vector2 nextPos = nextDestPosition, prevPos = position;

                if (destination.Count != 0) prevPos = destination[destination.Count - 1];

                float theta = (float)Math.Atan2(nextDestPosition.Y - prevPos.Y, nextDestPosition.X - prevPos.X);
                if (Vector2.Distance(prevPos, nextDestPosition) > Tile.Width)
                    nextPos = prevPos + Tile.Width * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));

                Point p = Tile.GetBounds(nextPos).Center;
                Vector2 v = new Vector2(p.X, p.Y);

                if (!destination.Contains(v)) destination.Add(v);
                else
                {
                    int i = destination.IndexOf(v);
                    destination.RemoveRange(i + 1, destination.Count - i - 1);
                    break;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime);
                if (bullets[i].totalDistance > Range) bullets.Remove(bullets[i]);
            }

            if (!IsAlive) return;

            if (destination.Count > 0)
            {
                Vector2 next = destination[0];
                float distanceLeft = Vector2.Distance(next, position);

                float theta = (float)Math.Atan2(next.Y - position.Y, next.X - position.X);
                rotation = theta;
                position += MoveSpeed * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));

                if (distanceLeft <= MoveSpeed)
                {
                    position = next; destination.RemoveAt(0);
                }

                animationPlayer.PlayAnimation(walkAnimation);
            }
            else animationPlayer.PlayAnimation(idleAnimation);


            reloadTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //HandleEnemy(armies);
            //EnemyAI();
        }

        public void EnemyAI(Rectangle enemyCastleBounds)
        {
            if (destination.Count == 0)
            {
                if (position.X < 5 * Tile.Width)
                {
                    Point p = enemyCastleBounds.Center;
                    AddPosition(new Vector2(p.X, p.Y));
                }
                else
                {
                    int x = gameContent.random.Next((int)gameContent.GameplayArea.X - 1);
                    int y = gameContent.random.Next((int)gameContent.GameplayArea.Y - 1);

                    AddPosition(new Vector2(x, y));
                }
            }
        }

        public void HandleEnemy(List<Army> armies)
        {
            Army nearestEnemy = null;
            float D = float.PositiveInfinity;

            for (int i = 0; i < armies.Count; i++)
                if (armies[i] != this && armies[i].Shape != Shape)
                {
                    float dx = Vector2.Distance(armies[i].position, position);
                    if (D > dx && armies[i].IsAlive)
                    {
                        D = dx; nearestEnemy = armies[i];
                    }

                    for (int j = armies[i].bullets.Count - 1; j >= 0; j--)
                    {
                        if (BoundingRectangle.Intersects(armies[i].bullets[j].BoundingRectangle))
                        {
                            health = Math.Max(health - armies[i].Damage, 0); armies[i].bullets.RemoveAt(j);
                            if (!IsAlive)
                            {
                                gameContent.soundBank.PlayCue("die");
                                animationPlayer.PlayAnimation(dieAnimation);
                            }
                        }
                    }
                }

            //rotation = defaultRotaion;
            if (nearestEnemy != null)
                rotation = (float)Math.Atan2(nearestEnemy.position.Y - position.Y,
                    nearestEnemy.position.X - position.X);

            if (Rank == Rank.bazooka) rotation = defaultRotaion;

            if (nearestEnemy != null && D < Range && reloadTime > MaxReloadTime)
            {
                reloadTime = 0;
                bullets.Add(new Bullet(gameContent.bullet[(int)Rank], Rank, position, rotation));
                gameContent.soundBank.PlayCue(Rank + "WeaponNoise");
            }
        }

        public void DrawPath(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color c = new Color(Color.White, pathVisibleTime / MaxPathVisibleTime);
            for (int i = 0; i < destination.Count - 1; i++)
            {
                float theta = (float)Math.Atan2(destination[i + 1].Y - destination[i].Y,
                    destination[i + 1].X - destination[i].X);
                spriteBatch.Draw(gameContent.pathArrow, destination[i], null, c, theta,
                    new Vector2(Tile.Width) / 2, 1, SpriteEffects.None, 1);
            }

            if (destination.Count > 0)
                spriteBatch.Draw(gameContent.pathCross, destination[destination.Count - 1], null, c, 0,
                        new Vector2(Tile.Width) / 2, 1, SpriteEffects.None, 1);

            pathVisibleTime = Math.Max(pathVisibleTime - (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets) b.Draw(spriteBatch);

            if (!IsAlive) removeTime = Math.Max(removeTime - (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            Color c = new Color(Color.White, removeTime / MaxRemoveTime);

            animationPlayer.Draw(gameTime, spriteBatch, position, c, 0, 1,
                Math.Abs(rotation) > (float)Math.PI / 2 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

            spriteBatch.Draw(gameContent.weapon[(int)Rank], position, null, c, rotation,
                new Vector2(Tile.Width) / 2, 1,
                Math.Abs(rotation) > (float)Math.PI / 2 ? SpriteEffects.FlipVertically : SpriteEffects.None, 1);

            if (IsAlive)
            {
                spriteBatch.Draw(gameContent.healthBar, position - new Vector2(gameContent.healthBar.Width / 2, 20),
                    Color.White);
                spriteBatch.Draw(gameContent.healthBar, position - new Vector2(gameContent.healthBar.Width / 2, 20),
                    new Rectangle(0, 0, (int)(health / MaxHealth * gameContent.healthBar.Width),
                        gameContent.healthBar.Height), Color.Red);
            }
        }

        public void DrawMouseOver(SpriteBatch spriteBatch)
        {
            pathVisibleTime = MaxPathVisibleTime;
            spriteBatch.Draw(gameContent.mouseOver, position - gameContent.mouseOverOrigin,
                new Color(Color.Yellow, 0.8f));
        }
    }
}
