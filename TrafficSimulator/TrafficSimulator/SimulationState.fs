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
}

let draw (spritebatch: SpriteBatch) (state: SimulationState) =
    state.trafficlights |>
        List.iter(fun light -> TrafficLight.draw spritebatch state.texture light)
    state.vehicles |>
        List.iter(fun vehicle -> Vehicle.draw spritebatch state.texture vehicle)
    ()