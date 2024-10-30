﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanguard_Fighters.Services
{
    internal class Cercle
    {
        public Texture2D CreateCircleTexture(GraphicsDevice graphicsDevice, int radius, Color color)
        {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
            Color[] data = new Color[diameter * diameter];

            float radiusSquared = radius * radius;

            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    int index = x + y * diameter;
                    Vector2 position = new Vector2(x - radius, y - radius);
                    if (position.LengthSquared() <= radiusSquared)
                        data[index] = color;
                    else
                        data[index] = Color.Transparent;
                }
            }

            texture.SetData(data);
            return texture;
        }

    }
}