module Simulation

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open TrafficLight

type TrafficSimulator() as this =
    inherit Game()
        
    let graphics = new GraphicsDeviceManager(this)
    let mutable spritebatch = Unchecked.defaultof<SpriteBatch>
    let mutable plainTexture = Unchecked.defaultof<Texture2D>


    override this.Initialize() = 
        do base.Initialize()
   

    override this.LoadContent() =
        do spritebatch <- new SpriteBatch(this.GraphicsDevice)
        do plainTexture <- new Texture2D(this.GraphicsDevice, 1, 1)
        plainTexture.SetData([|Color.White|])
        ()


    override this.Update(gameTime) =
        let deltaTime = gameTime.ElapsedGameTime.TotalSeconds |> float32
        ()

  
    override this.Draw(gameTime) =
        do this.GraphicsDevice.Clear Color.LightGreen       
        spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        spritebatch.End()
        ()