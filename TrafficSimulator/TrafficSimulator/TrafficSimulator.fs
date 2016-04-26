module Simulation

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open TrafficLight
open Vehicle
open SimulationState

type TrafficSimulator() as this =
    inherit Game()
    
    let graphics = new GraphicsDeviceManager(this)
    let mutable spritebatch = Unchecked.defaultof<SpriteBatch>
    let mutable state = Unchecked.defaultof<SimulationState>

    override this.Initialize() = 
        graphics.PreferredBackBufferHeight <- 600
        graphics.PreferredBackBufferWidth <- 800
        do base.Initialize()
   

    override this.LoadContent() =
        do spritebatch <- new SpriteBatch(this.GraphicsDevice)
        let plainTexture = new Texture2D(this.GraphicsDevice, 1, 1)
        let background = this.Content.Load<Texture2D> "background.jpg"
        plainTexture.SetData([|Color.White|])
        do state <- {
            trafficlights = [
                {status = Green(6.f); position = new Vector2(375.f, 230.f)}
                {status = Green(6.f); position = new Vector2(425.f, 370.f)}
                {status = Red(11.f); position = new Vector2(470.f, 275.f)}
                {status = Red(11.f); position = new Vector2(330.f, 325.f)}
            ]; 
            vehicles = [vehicle.TopVehicle];
            texture = plainTexture
            background = background
            vehicleSpawnCooldown = 2.f
        }
        ()


    override this.Update(gameTime) =
        let dt = gameTime.ElapsedGameTime.TotalSeconds |> float32
        do state <- SimulationState.update dt state 
        ()

  
    override this.Draw(gameTime) =
        do this.GraphicsDevice.Clear Color.Black
        spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        SimulationState.draw spritebatch state
        spritebatch.End()
        ()