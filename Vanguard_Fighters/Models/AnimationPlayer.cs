using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A helper class for handling animated textures based on different states.
/// </summary>
public class AnimationPlayer
{
    private Texture2D texture;
    private int frameCount;
    private int rowCount;
    private float timePerFrame;
    private int currentFrame;
    private double totalElapsed;
    private bool isPaused;
    public float Rotation, Scale, Depth;
    public Vector2 Origin;

    // Animation state management
    public enum AnimationState
    {
        Idle = 0,
        LookingRight = 1,
        Armed = 2,
        WalkingArmedRight = 3,
        WalkingUnarmedRight = 4,
        Crouching = 9
    }

    private AnimationState currentState;
    private SpriteEffects spriteEffect;

    public AnimationPlayer(Vector2 origin, float rotation, float scale, float depth)
    {
        Origin = origin;
        Rotation = rotation;
        Scale = scale;
        Depth = depth;
    }

    public void Load(ContentManager content, string asset, int frameCount, int rowCount, int framesPerSec)
    {
        texture = content.Load<Texture2D>(asset);
        this.frameCount = frameCount;
        this.rowCount = rowCount;
        timePerFrame = 1.0f / framesPerSec;
        currentFrame = 0;
        totalElapsed = 0;
        isPaused = false;
        currentState = AnimationState.Idle;
        spriteEffect = SpriteEffects.None;
    }

    public void UpdateFrame(float elapsed, AnimationState newState, bool facingRight)
    {
        if (isPaused) return;

        // Update the state and sprite effect based on facing direction
        if (currentState != newState)
        {
            currentState = newState;
            currentFrame = 0;  // Reset frame on state change
            totalElapsed = 0;
        }
        spriteEffect = facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        // Update the animation frame
        totalElapsed += elapsed;
        if (totalElapsed > timePerFrame)
        {
            currentFrame = (currentFrame + 1) % frameCount;
            totalElapsed -= timePerFrame;
        }
    }

    public void DrawFrame(SpriteBatch spriteBatch, Vector2 screenPos)
    {
        int FrameWidth = texture.Width / frameCount;
        int FrameHeight = texture.Height / rowCount;
        Rectangle sourceRectangle = new Rectangle(FrameWidth * currentFrame, FrameHeight * (int)currentState, FrameWidth, FrameHeight);
        spriteBatch.Draw(texture, screenPos, sourceRectangle, Color.White, Rotation, Origin, Scale, spriteEffect, Depth);
    }

    public void Play()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Reset()
    {
        currentFrame = 0;
        totalElapsed = 0;
    }
}
