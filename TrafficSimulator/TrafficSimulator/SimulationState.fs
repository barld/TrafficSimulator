module SimulationState

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Entities
open Coroutine


let update dt state =
    let cooldown, vehicle =
        match state.vehicleSpawnCooldown with
        | cooldown when cooldown > 0.f -> state.vehicleSpawnCooldown - dt, []
        | _ -> 5.5f, {vehicle.TopLeftVehicle with behaviour = Vehicle.getBehaviour() }::
                      ([vehicle.TopRightVehicle; 
                      vehicle.MiddleRightTopVehicle; 
                      vehicle.MiddleLeftTopVehicle; 
                      vehicle.BottomLeftVehicle; 
                      vehicle.BottomRightVehicle; 
                      vehicle.MiddleRightBottomVehicle; 
                      vehicle.MiddleLeftBottomVehicle
                      ] |> List.map (fun v -> {v with behaviour = Vehicle.getDefaulBehaviour()}))
    {
        state with 
            trafficlights = List.map(TrafficLight.update dt) state.trafficlights//List.map(Vehicle.update dt state) state.vehicles 
            vehicles = vehicle @ state.vehicles |> List.map (fun vh -> 
                                                            let b, (_,vehc) = (singleStep vh.behaviour (state,vh) dt) 
                                                            {vehc with behaviour = b}) 
            |> List.filter(
                fun vehicle -> 
                    vehicle.position.Y < 1050.f && 
                    vehicle.position.Y > -50.f && 
                    vehicle.position.X < 1650.f &&
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