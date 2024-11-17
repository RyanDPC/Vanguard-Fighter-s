using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Vanguard.Menu;
using Vanguard.Models;
using Vanguard.Service;
using Vanguard.View;

namespace Vanguard
{
    public class Game1 : Game
    {
        // Champs principaux
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Dimensions de l'écran
        private int screenWidth;
        private int screenHeight;

        // Gestion des états du jeu
        private enum GameState
        {
            MainMenu,
            Playing,
            GameOver
        }

        private GameState currentGameState;

        // Objets du jeu
        private Player player;
        private InputManager inputManager;
        private WeaponView weaponView;
        private Weapon playerWeapon;
        private Weapon enemyWeapon;
        private List<Bullet> playerBullets;
        private List<Bullet> enemyBullets;
        private List<Platform> platforms;
        private Texture2D playerTexture;
        private Texture2D enemyTexture;
        private Texture2D platformTexture;
        private Texture2D debugTexture;
        private List<Enemy> enemies; // Liste des ennemis actifs
        private Random random; // Générateur de positions aléatoires
        private int totalEnemies; // Nombre total d'ennemis actifs


        // Vues
        private PlayerView playerView;
        private EnemyView enemyView;

        // Menus
        private GameMenu gameMenu;
        private GameOverMenu gameOverMenu;

        // Ressources pour les menus
        private Texture2D menuBackgroundTexture;
        private Texture2D menuTitleTexture;
        private Texture2D playButtonTexture;
        private Texture2D optionButtonTexture;
        private Texture2D editButtonTexture;
        private Texture2D quitButtonTexture;

        // Autres variables
        private bool isGameOver;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Initialisation des dimensions de l'écran
            screenWidth = 800;  // Remplacez par la largeur souhaitée
            screenHeight = 600; // Remplacez par la hauteur souhaitée

            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
        }

        protected override void Initialize()
        {
            inputManager = new InputManager();
            inputManager.OnToggleFullScreen += ToggleFullScreen;
            enemies = new List<Enemy>();
            random = new Random();
            totalEnemies = 1; // Commencer avec un ennemi
            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            WeaponLibrary.InitializeWeapons();
            debugTexture = new Texture2D(GraphicsDevice, 1, 1);
            debugTexture.SetData(new[] { Color.White });
            // Charger les ressources (textures, polices, etc.)
            SpriteFont font = Content.Load<SpriteFont>("Font");
            // Charger les textures pour le menu principal
            menuBackgroundTexture = Content.Load<Texture2D>("Menu/BackGroundMenu");
            menuTitleTexture = Content.Load<Texture2D>("Menu/Title");
            playButtonTexture = Content.Load<Texture2D>("Menu/Play");
            optionButtonTexture = Content.Load<Texture2D>("Menu/Option");
            editButtonTexture = Content.Load<Texture2D>("Menu/EDIT");
            quitButtonTexture = Content.Load<Texture2D>("Menu/Quit");

            // Initialiser le menu principal
            gameMenu = new GameMenu(
                menuBackgroundTexture,
                menuTitleTexture,
                playButtonTexture,
                optionButtonTexture,
                editButtonTexture,
                quitButtonTexture,
                screenWidth,
                screenHeight);

            // Souscrire aux événements du menu principal
            gameMenu.OnPlayClicked += StartGame;
            gameMenu.OnOptionClicked += OpenOptions;
            gameMenu.OnEditClicked += OpenEdit;
            gameMenu.OnQuitClicked += ExitGame;

            // Initialiser le menu Game Over
            gameOverMenu = new GameOverMenu(menuBackgroundTexture, font, screenWidth, screenHeight);

            // Souscrire aux événements du menu Game Over
            gameOverMenu.OnPlayAgainClicked += RestartGame;
            gameOverMenu.OnQuitClicked += ExitGame;

            // Charger les textures pour le jeu
            playerTexture = Content.Load<Texture2D>("PLayers/SpecialistFace"); // Remplacez par le nom réel de votre texture joueur
            enemyTexture = Content.Load<Texture2D>("PLayers/WukongEntier");   // Remplacez par le nom réel de votre texture ennemi
            platformTexture = Content.Load<Texture2D>("Maps/Background"); // Remplacez par le nom réel de votre texture plateforme

            // Initialiser le gestionnaire d'entrée
            inputManager = new InputManager();

            // Initialiser les armes
            Texture2D weaponTexture = Content.Load<Texture2D>("Weapons/TacticalPistol"); // Remplacez par le nom réel de votre texture d'arme
            // Initialiser l'arme du joueur à partir de la bibliothèque
            Weapon basePlayerWeapon = WeaponLibrary.GetWeapon("TacticalPistol");
            playerWeapon = new Weapon(
                basePlayerWeapon.Name,
                basePlayerWeapon.Damage,
                basePlayerWeapon.FireRate,
                basePlayerWeapon.AmmoCapacity,
                basePlayerWeapon.ReloadTime,
                basePlayerWeapon.InitialReloadCooldown,
                basePlayerWeapon.InitialFireCooldown,
                basePlayerWeapon.BulletSpeed,
                basePlayerWeapon.Range,
                weaponTexture // Assurez-vous de fournir une texture spécifique
            );

            // Initialiser l'arme de l'ennemi à partir de la bibliothèque
            Weapon baseEnemyWeapon = WeaponLibrary.GetWeapon("TacticalPistol");
            enemyWeapon = new Weapon(
                baseEnemyWeapon.Name,
                baseEnemyWeapon.Damage,
                baseEnemyWeapon.FireRate,
                baseEnemyWeapon.AmmoCapacity,
                baseEnemyWeapon.ReloadTime,
                baseEnemyWeapon.InitialReloadCooldown,
                baseEnemyWeapon.InitialFireCooldown,
                baseEnemyWeapon.BulletSpeed,
                baseEnemyWeapon.Range,
                weaponTexture // Assurez-vous de fournir une texture spécifique
            );


            // Initialiser la vue des armes
            weaponView = new WeaponView(new List<Weapon> { playerWeapon });

            // Initialiser les vues du joueur et de l'ennemi
            playerView = new PlayerView(playerTexture, weaponView, GraphicsDevice);
            enemyView = new EnemyView(enemyTexture, weaponView, GraphicsDevice);

            // Initialiser l'état du jeu
            currentGameState = GameState.MainMenu;
            isGameOver = false;
        }

        private void InitializeGameObjects()
        {
            // Initialiser les listes de balles
            playerBullets = new List<Bullet>();
            enemyBullets = new List<Bullet>();


            // Initialiser les plateformes
            platforms = new List<Platform>();

            // Ajouter une plateforme au sol (par exemple, une grande plateforme en bas de l'écran)
            platforms.Add(new Platform(new Rectangle(0, screenHeight - 50, screenWidth, 50)));

            // Ajouter des plateformes supplémentaires (par exemple, des plateformes suspendues)
            platforms.Add(new Platform(new Rectangle(200, screenHeight - 200, 200, 20)));
            platforms.Add(new Platform(new Rectangle(500, screenHeight - 300, 200, 20)));
            platforms.Add(new Platform(new Rectangle(100, screenHeight - 400, 200, 20)));

            // Initialiser le joueur
            player = new Player(
                new Vector2(100, screenHeight - 150),
                inputManager,
                playerBullets,
                platforms,
                screenWidth,
                screenHeight
            );
            player.EquippedWeapon = playerWeapon;

            // Initialiser les ennemis
            for (int i = 0; i < totalEnemies; i++)
            {
                InitializeEnemy();
            }
        }
        private void InitializeEnemy()
        {
            // Générer une position aléatoire pour le nouvel ennemi
            Vector2 randomPosition = new Vector2(
                random.Next(100, screenWidth - 100), // Position X aléatoire
                screenHeight - 150 // Position Y au sol
            );

            // Créer un nouvel ennemi
            Enemy newEnemy = new Enemy(
                randomPosition,
                weaponView,
                enemyBullets, // Passer la liste des balles de l'ennemi
                player,
                platforms,
                screenWidth,
                screenHeight,
                GraphicsDevice
            );

            // Assigner une arme
            newEnemy.EquippedWeapon = WeaponLibrary.GetWeapon("TacticalPistol");
            newEnemy.EquippedWeapon.Reload();
            // Souscrire à l'événement de mort
            newEnemy.EnemyDied += () => OnEnemyDied(newEnemy);

            // Ajouter l'ennemi à la liste
            enemies.Add(newEnemy);
        }

        private void OnEnemyDied(Enemy deadEnemy)
        {
            // Supprimer l'ennemi mort de la liste
            enemies.Remove(deadEnemy);

            // Ajouter un nouvel ennemi pour le remplacer
            for (int i = 0; i < totalEnemies; i++)
            {
                InitializeEnemy();
            }

            // Augmenter le total d'ennemis actifs
        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    gameMenu.Update(gameTime);
                    break;

                case GameState.Playing:
                    inputManager.Update(gameTime);
                    player.Update(gameTime);

                    UpdateEnemies(gameTime); // Gérer tous les ennemis

                    foreach (var bullet in playerBullets)
                    {
                        bullet.Update(gameTime);
                    }

                    UpdateBullets(gameTime); // Gérer toutes les balles

                    if (player.Health <= 0)
                    {
                        currentGameState = GameState.GameOver;
                        isGameOver = true;
                    }
                    break;

                case GameState.GameOver:
                    gameOverMenu.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateBullets(GameTime gameTime)
        {
            // Mise à jour des balles du joueur
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                var bullet = playerBullets[i];
                bullet.Update(gameTime);

                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    var enemy = enemies[j];

                    if (bullet.IsActive && enemy.Health > 0 && bullet.Bounds.Intersects(enemy.GetHitbox()))
                    {
                        bullet.IsActive = false;
                        enemy.TakeDamage(bullet.Damage); // Appliquer les dégâts de la balle
                        Console.WriteLine($"Bullet hit enemy: Damage={bullet.Damage}, EnemyHealth={enemy.Health}");
                    }
                }

                if (!bullet.IsActive)
                {
                    playerBullets.RemoveAt(i);
                }
            }

            // Mise à jour des balles de l'ennemi
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                var bullet = enemyBullets[i];
                bullet.Update(gameTime);

                // Vérifier la collision avec le joueur
                if (bullet.IsActive && bullet.Bounds.Intersects(player.GetHitbox()))
                {
                    // Infliger des dégâts au joueur
                    bullet.IsActive = false;
                    player.TakeDamage(bullet.Damage);
                    
                }

                if (!bullet.IsActive)
                {
                    enemyBullets.RemoveAt(i);
                }
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];

                if (enemy.Health <= 0)
                {
                    enemies.RemoveAt(i); // Supprimer l'ennemi mort
                    InitializeEnemy();   // Ajouter un nouvel ennemi
                    totalEnemies++;      // Augmenter le compteur
                }
                else
                {
                    enemy.Update(gameTime); // Mettre à jour les ennemis vivants
                }
            }
        }

        private void DrawBullets(SpriteBatch spriteBatch, List<Bullet> bullets, Texture2D debugTexture, Color color)
        {
            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch, debugTexture, color);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    gameMenu.Draw(_spriteBatch);
                    break;

                case GameState.Playing:
                    // Dessiner les plateformes
                    foreach (var platform in platforms)
                    {
                        platform.Draw(_spriteBatch, platformTexture);
                    }

                    // Dessiner le joueur
                    playerView.Draw(_spriteBatch, player);

                    foreach (var enemy in enemies)
                    {
                        enemyView.Draw(_spriteBatch, enemy);
                    }

                    // Dessiner les balles du joueur en rouge
                    DrawBullets(_spriteBatch, playerBullets, debugTexture, Color.Red);

                    // Dessiner les balles de l'ennemi en bleu
                    DrawBullets(_spriteBatch, enemyBullets, debugTexture, Color.Blue);

                    // Dessiner les plateformes
                    foreach (var platform in platforms)
                    {
                        platform.Draw(_spriteBatch, platformTexture);
                    }

                    break;

                case GameState.GameOver:
                    gameOverMenu.Draw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void StartGame()
        {
            // Démarrer le jeu
            currentGameState = GameState.Playing;
            InitializeGameObjects();
        }

        private void RestartGame()
        {
            // Redémarrer le jeu depuis le menu Game Over
            currentGameState = GameState.Playing;
            isGameOver = false;
            InitializeGameObjects();
        }

        private void OpenOptions()
        {
            // Ouvrir le menu des options (à implémenter)
            // Pour l'instant, vous pouvez afficher un Fmessage ou laisser cette méthode vide
            System.Diagnostics.Debug.WriteLine("Le menu des options n'est pas encore implémenté.");
        }

        private void OpenEdit()
        {
            // Ouvrir l'éditeur (à implémenter)
            // Pour l'instant, vous pouvez afficher un message ou laisser cette méthode vide
            System.Diagnostics.Debug.WriteLine("L'éditeur n'est pas encore implémenté.");
        }
        public void ToggleFullScreen()
        {
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();

            // Mettre à jour les dimensions de l'écran
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            // Mettre à jour le layout des menus
            gameMenu?.UpdateLayout(screenWidth, screenHeight);
            gameOverMenu?.UpdateLayout(screenWidth, screenHeight);
        }

        private void ExitGame()
        {
            Exit();
        }
    }
}
