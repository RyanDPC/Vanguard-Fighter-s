using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InputManager
{
    private KeyboardState currentKeyState;
    private KeyboardState previousKeyState;

    public void Update()
    {
        previousKeyState = currentKeyState;
        currentKeyState = Keyboard.GetState();
    }

    // Vérifie si une touche est enfoncée
    public bool IsKeyDown(Keys key)
    {
        return currentKeyState.IsKeyDown(key);
    }

    // Vérifie si une touche a été pressée cette frame (pas enfoncée lors de la frame précédente)
    public bool IsKeyPressed(Keys key)
    {
        return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
    }

    // Vérifie si une touche a été relâchée cette frame (était enfoncée lors de la frame précédente)
    public bool IsKeyReleased(Keys key)
    {
        return !currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyDown(key);
    }

    // Détecte le mouvement du joueur
    public Vector2 GetMovement()
    {
        Vector2 movement = Vector2.Zero;

        if (IsKeyDown(Keys.W)) movement.Y -= 1;  // Déplacement vers le haut
        if (IsKeyDown(Keys.S)) movement.Y += 1;  // Déplacement vers le bas
        if (IsKeyDown(Keys.A)) movement.X -= 1;  // Déplacement vers la gauche
        if (IsKeyDown(Keys.D)) movement.X += 1;  // Déplacement vers la droite

        return movement;
    }

    // Vérifie si la touche Espace est pressée pour sauter
    public bool IsJumpRequested()
    {
        return IsKeyPressed(Keys.Space);
    }
}
