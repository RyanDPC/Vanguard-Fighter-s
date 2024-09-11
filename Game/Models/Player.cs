using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Player
{
    private List<Texture2D> _skins;
    private int _currentSkinIndex;
    public Vector2 Position { get; set; }
    public float Speed { get; set; } = 300.0f;

    // Taille du sprite en pixels (dimensions approximatives pour Smash Bros. Ultimate)
    private const int PlayerWidth = 64;   // Largeur du sprite
    private const int PlayerHeight = 128; // Hauteur du sprite

    public Player(List<Texture2D> skins, Vector2 initialPosition)
    {
        _skins = skins;
        _currentSkinIndex = 0; // Index du skin principal
        Position = initialPosition;
    }

    // Propriété publique pour accéder à la texture actuelle
    public Texture2D CurrentTexture => _skins[_currentSkinIndex];

    // Propriété pour obtenir le rectangle du joueur (utile pour les collisions)
    public Rectangle GetPlayerRectangle()
    {
        return new Rectangle((int)Position.X, (int)Position.Y, PlayerWidth, PlayerHeight);
    }

    public void Move(Vector2 movement, GameTime gameTime, GraphicsDevice graphicsDevice)
    {
        if (movement != Vector2.Zero)
        {
            movement.Normalize();
            Position += movement * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        // Obtenir les limites de l'écran
        int screenWidth = graphicsDevice.Viewport.Width;
        int screenHeight = graphicsDevice.Viewport.Height;

        // Créer un nouveau Vector2 pour Position avec le clamping
        Position = new Vector2(
            MathHelper.Clamp(Position.X, 0, screenWidth - PlayerWidth),
            MathHelper.Clamp(Position.Y, 0, screenHeight - PlayerHeight)
        );
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle destinationRectangle = new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            PlayerWidth,   // Largeur du sprite définie ici
            PlayerHeight   // Hauteur du sprite définie ici
        );

        spriteBatch.Draw(CurrentTexture, destinationRectangle, Color.White);
    }

    public void ChangeSkin(int newIndex)
    {
        if (newIndex >= 0 && newIndex < _skins.Count)
        {
            _currentSkinIndex = newIndex;
        }
    }
}
