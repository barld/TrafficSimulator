module SimulationState

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Entities


let update dt state =
    let cooldown, vehicle =
        match state.vehicleSpawnCooldown with
        | cooldown when cooldown > 0.f -> state.vehicleSpawnCooldown - dt, []
        | _ -> 20.f, [vehicle.TopVehicle; vehicle.RightVehicle; vehicle.BottomVehicle; vehicle.LeftVehicle]
    {
        state with 
            trafficlights = List.map(TrafficLight.update dt) state.trafficlights
            vehicles = vehicle @ List.map(Vehicle.update dt state) state.vehicles 
            |> List.filter(
                fun vehicle -> 
                    vehicle.position.Y < 650.f && 
                    vehicle.position.Y > -50.f && 
                    vehicle.position.X < 850.f &&
                    vehicle.position.X > -50.f
            )
            vehicleSpawnCooldown = cooldown
    }


let draw (spritebatch: SpriteBatch) (state: SimulationState) =
    spritebatch.Draw(state.background, Vector2.Zero, Color.White)
    state.vehicles |>
        List.iter(fun vehicle -> Vehicle.draw spritebatch state.texture vehicle)
    state.trafficlights |>
        List.iter(fun light -> TrafficLight.draw spritebatch state.texture light)