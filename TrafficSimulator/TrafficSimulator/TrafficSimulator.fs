module Simulation

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Entities
open Coroutine

type TrafficSimulator() as this =
    inherit Game()
    
    let graphics = new GraphicsDeviceManager(this)
    let mutable spritebatch = Unchecked.defaultof<SpriteBatch>
    let mutable state = Unchecked.defaultof<SimulationState>

    override this.Initialize() = 
        graphics.PreferredBackBufferHeight <- 1000
        graphics.PreferredBackBufferWidth <- 1600
        do base.Initialize()
   

    override this.LoadContent() =
        do spritebatch <- new SpriteBatch(this.GraphicsDevice)
        let plainTexture = new Texture2D(this.GraphicsDevice, 1, 1)
        let background = this.Content.Load<Texture2D> "background.jpg"
        plainTexture.SetData([|Color.White|])
        do state <- {
            trafficlights = 
                [
                     //topLeft
                    {status = Green(6.f); position = new Vector2(375.f, 230.f)}
                    {status = Green(6.f); position = new Vector2(425.f, 370.f)} 
                    {status = Red(11.f); position = new Vector2(470.f, 275.f)}
                    {status = Red(11.f); position = new Vector2(330.f, 325.f)}
                    //topRight
                    {status = Green(6.f); position = new Vector2(1175.f, 230.f)}
                    {status = Green(6.f); position = new Vector2(1225.f, 370.f)} 
                    {status = Red(11.f); position = new Vector2(1270.f, 275.f)}
                    {status = Red(11.f); position = new Vector2(1130.f, 325.f)}
                    //BottomLeft
                    {status = Green(6.f); position = new Vector2(375.f, 695.f)}
                    {status = Green(6.f); position = new Vector2(425.f, 835.f)} 
                    {status = Red(11.f); position = new Vector2(470.f, 745.f)}
                    {status = Red(11.f); position = new Vector2(330.f, 790.f)}
                    //BotoomRight
                    {status = Green(6.f); position = new Vector2(1175.f, 695.f)}
                    {status = Green(6.f); position = new Vector2(1225.f, 835.f)} 
                    {status = Red(11.f); position = new Vector2(1270.f, 745.f)}
                    {status = Red(11.f); position = new Vector2(1130.f, 790.f)}
                ]
            vehicles = []
            texture = plainTexture
            background = background
            behaviour = SimulationState.stateUpdateBehaviour ()
        }
        ()


    override this.Update(gameTime) =
        let dt = gameTime.ElapsedGameTime.TotalSeconds |> float32
        let behaviour', state' = singleStep state.behaviour state dt
        do state <- {state' with behaviour = behaviour'}
        ()

  
    override this.Draw(gameTime) =
        do this.GraphicsDevice.Clear Color.Black
        spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        SimulationState.draw spritebatch state
        spritebatch.End()
        ()