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
    vehicleSpawnCooldown: float32
}


let update dt state =
    let cooldown, vehicle =
        match state.vehicleSpawnCooldown with
        | cooldown when cooldown > 0.f -> state.vehicleSpawnCooldown - dt, []
        | cooldown when cooldown <= 0.f-> 2.f, [vehicle.TopVehicle]
    {
        state with 
            trafficlights = List.map(TrafficLight.update dt) state.trafficlights
            vehicles = vehicle @ List.map(Vehicle.update dt) state.vehicles 
            |> List.filter(
                fun vehicle -> 
                    vehicle.position.Y < 816.f || 
                    vehicle.position.Y > -16.f || 
                    vehicle.position.X < 616.f || 
                    vehicle.position.X > -16.f
            )
            vehicleSpawnCooldown = cooldown
    }


let draw (spritebatch: SpriteBatch) (state: SimulationState) =
    spritebatch.Draw(state.background, Vector2.Zero, Color.White)
    state.trafficlights |>
        List.iter(fun light -> TrafficLight.draw spritebatch state.texture light)
    state.vehicles |>
        List.iter(fun vehicle -> Vehicle.draw spritebatch state.texture vehicle)
    ()