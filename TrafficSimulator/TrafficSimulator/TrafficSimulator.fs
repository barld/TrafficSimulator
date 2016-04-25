module Simulation

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type TrafficSimulator() as this =
    inherit Game()
        
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    let mutable spritebatch = Unchecked.defaultof<SpriteBatch>

    override this.Initialize() = 
        do base.Initialize()
   
    override this.LoadContent() =
        do spritebatch <- new SpriteBatch(this.GraphicsDevice)

    override this.Update(gameTime) =
        let deltaTime = gameTime.ElapsedGameTime.TotalSeconds |> float32
        ()

  
    override this.Draw(gameTime) =
        do this.GraphicsDevice.Clear Color.LightGreen       
        spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        spritebatch.End() 
        ()