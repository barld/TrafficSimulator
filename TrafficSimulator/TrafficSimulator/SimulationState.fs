module SimulationState

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open TrafficLight
open Vehicle

type SimulationState = {
    trafficlights : List<trafficLight>
    vehicles : List<vehicle>
    texture: Texture2D
    background: Texture2D
}


let update dt state =
    {
        state with 
            trafficlights = List.map(TrafficLight.update dt) state.trafficlights
    }



let draw (spritebatch: SpriteBatch) (state: SimulationState) =
    spritebatch.Draw(state.background, Vector2.Zero, Color.White)
    state.trafficlights |>
        List.iter(fun light -> TrafficLight.draw spritebatch state.texture light)
    state.vehicles |>
        List.iter(fun vehicle -> Vehicle.draw spritebatch state.texture vehicle)
    ()