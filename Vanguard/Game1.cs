
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGameProjectComplete.Models;
using MyGameProjectComplete.View;
using MyGameProjectComplete.Services;

namespace MyGameProjectComplete
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Enemy enemy;
        Map map;
        Weapon weapon;
        Bullet bullet;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Charger les textures et initialiser les objets de jeu
            Texture2D playerTexture = Content.Load<Texture2D>("player");
            Texture2D enemyTexture = Content.Load<Texture2D>("enemy");
            Texture2D backgroundTexture = Content.Load<Texture2D>("background");
            Texture2D weaponTexture = Content.Load<Texture2D>("weapon");
            Texture2D bulletTexture = Content.Load<Texture2D>("bullet");

            player = new Player(playerTexture, new Vector2(100, 100));
            enemy = new Enemy(enemyTexture, new Vector2(300, 100), player);
            map = new Map(backgroundTexture);
            weapon = new Weapon(weaponTexture, new Vector2(150, 150));
            bullet = new Bullet(bulletTexture, new Vector2(150, 150), new Vector2(1, 0), 5.0f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputService.IsKeyPressed(Keys.Escape))
                Exit();

            player.Update(gameTime);
            enemy.Update(gameTime);
            bullet.Update(gameTime);

            // Gestion des collisions entre le bullet et l'ennemi
            if (CollisionService.CheckCollision(bullet.GetBounds(), enemy.GetBounds()))
            {
                // Logique à appliquer en cas de collision
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            map.Draw(spriteBatch, Vector2.Zero);
            player.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            weapon.Draw(spriteBatch);
            bullet.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
