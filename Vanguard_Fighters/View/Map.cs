using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System.Collections.Generic;

namespace Vanguard_Fighters.View
{
    public class Map
    {
        private List<TiledMap> _maps;            // Liste des cartes Tiled
        private List<Texture2D> _backgrounds;    // Liste des fonds associés aux cartes
        private int _currentMapIndex;            // Index de la carte actuelle
        private TiledMapRenderer _mapRenderer;   // Renderer pour afficher la carte
        private Texture2D _currentBackground;    // Le fond actuel
        private GraphicsDevice _graphicsDevice;  // Référence au GraphicsDevice

        public Map(GraphicsDevice graphicsDevice, List<TiledMap> maps, List<Texture2D> backgrounds)
        {
            _graphicsDevice = graphicsDevice;
            _maps = maps;
            _backgrounds = backgrounds;
            _currentMapIndex = 0; // Commence avec la première carte et le premier fond
            _currentBackground = _backgrounds[_currentMapIndex];
            _mapRenderer = new TiledMapRenderer(graphicsDevice, _maps[_currentMapIndex]);
        }

        // Méthode pour obtenir la carte TiledMap actuelle
        public TiledMap GetCurrentTiledMap()
        {
            return _maps[_currentMapIndex];
        }

        // Méthode pour changer de carte
        public void ChangeMap(int mapIndex)
        {
            if (mapIndex >= 0 && mapIndex < _maps.Count)
            {
                _currentMapIndex = mapIndex;
                _currentBackground = _backgrounds[mapIndex]; // Charger le fond correspondant
                _mapRenderer = new TiledMapRenderer(_graphicsDevice, _maps[mapIndex]);
            }
        }

        // Mise à jour de la carte (TiledMap)
        public void Update(GameTime gameTime)
        {
            _mapRenderer.Update(gameTime);
        }

        // Afficher le fond et la carte
        public void Draw(SpriteBatch spriteBatch)
        {
            // Commencer un batch avant de dessiner le background
            spriteBatch.Begin();  // Assurez-vous que Begin est appelé ici avant tout appel à Draw

            // Dessiner le fond
            spriteBatch.Draw(_currentBackground, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);

            // Terminer le batch pour le background
            spriteBatch.End(); // Fin du premier bloc de dessin

            // Maintenant, dessiner la carte Tiled
            _mapRenderer.Draw();  // Le TiledMapRenderer gère son propre pipeline de rendu
        }

    }
}
