using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player player;
    private InputManager inputManager;
    private CollisionManager collisionManager;
    private Map map;
    private bool isFullScreen = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Configurer la fenêtre pour le mode fenêtré
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.IsFullScreen = false;
        Window.IsBorderless = false;
        _graphics.ApplyChanges();

        inputManager = new InputManager();
        collisionManager = new CollisionManager();
    }

    protected override void LoadContent()
    {

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D mapTexture = Content.Load<Texture2D>("Map/TREE/Background/Background"); // Assurez-vous que le fichier est dans le dossier Content/Map
        map = new Map(mapTexture); // Créer la carte avec la texture

        // Charger les textures des skins
        var skins = new List<Texture2D>
        {
            Content.Load<Texture2D>("Player/SpecialistFace"),
            //Content.Load<Texture2D>("Player/AnotherSkin"),
            //Content.Load<Texture2D>("Player/AdditionalSkin")
        };

        // Calculer la position initiale du joueur
        Vector2 playerInitialPosition = new Vector2(
            (GraphicsDevice.PresentationParameters.BackBufferWidth - 64) / 2,
            GraphicsDevice.PresentationParameters.BackBufferHeight - 128
        );

        player = new Player(skins, playerInitialPosition);
    }

    protected override void Update(GameTime gameTime)
    {
        inputManager.Update();
        Vector2 movement = inputManager.GetMovement();
        player.Move(movement, gameTime, GraphicsDevice);

        // Gestion de l'échappement du plein écran avec "Échap"
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            ToggleFullScreen();
        }

        // Obtenir le rectangle de la texture du joueur
        Rectangle playerRectangle = player.GetPlayerRectangle();

        Vector2 playerPosition = player.Position;

        // Contrainte du joueur aux limites de l'écran
        collisionManager.ConstrainToScreen(playerRectangle, GraphicsDevice, ref playerPosition);

        player.Position = playerPosition;

        base.Update(gameTime);
    }

    private void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen;
        _graphics.IsFullScreen = isFullScreen;
        Window.IsBorderless = !isFullScreen; // Mode fenêtré sans bordures si plein écran
        _graphics.ApplyChanges();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Dessiner la carte en premier (en arrière-plan)
        map.Draw(_spriteBatch);

        // Dessiner le joueur par-dessus la carte
        player.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}