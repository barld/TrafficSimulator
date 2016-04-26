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
        do base.Initialize()
   

    override this.LoadContent() =
        do spritebatch <- new SpriteBatch(this.GraphicsDevice)
        let plainTexture = new Texture2D(this.GraphicsDevice, 1, 1)
        plainTexture.SetData([|Color.White|])
        do state <- {
            trafficlights = [{status = Green(9.f); position = new Vector2(400.f, 200.f)}]; 
            vehicles = [{position = new Vector2(50.f, 200.f); velocity = new Vector2(1.f, 1.f); acceleration = new Vector2(2.f, 2.f)}];
            texture = plainTexture
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