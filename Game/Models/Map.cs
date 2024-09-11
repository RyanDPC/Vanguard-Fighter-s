using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Map
{
    private Texture2D _mapTexture;
    private Vector2 _position;

    // Constructeur pour charger la carte
    public Map(Texture2D mapTexture)
    {
        _mapTexture = mapTexture;
        _position = Vector2.Zero; // La carte commence à la position (0, 0)
    }

    // Méthode pour dessiner la carte
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_mapTexture, _position, Color.White); // Dessiner la carte à la position (0, 0)
    }
}
