module SimulationState

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Entities
open Coroutine

let stateUpdateBehaviour () =
    let spawNewVehicles =
        co{
            let! (state:SimulationState) = getState_
            let newVehicles = 
                    [
                        vehicle.TopLeftVehicle
                        vehicle.TopRightVehicle
                        vehicle.MiddleRightTopVehicle
                        vehicle.MiddleLeftTopVehicle
                        vehicle.BottomLeftVehicle
                        vehicle.BottomRightVehicle
                        vehicle.MiddleRightBottomVehicle
                        vehicle.MiddleLeftBottomVehicle
                    ] |> List.map (fun v -> {v with behaviour = Vehicle.getBehaviour()})
            let state' = {state with vehicles = newVehicles @ state.vehicles}
            do! updateState_ state'
        }

    let updateTrafficlights =
        co{
            let! (state:SimulationState) = getState_
            let! dt = getDeltaTime_
            let trafficlights' = state.trafficlights |> List.map(TrafficLight.update dt)
            do! updateState_ {state with trafficlights = trafficlights'}
        }

    let updateVehicles =
        co{
            let! state = getState_
            let! dt = getDeltaTime_
            let vehicles' = state.vehicles |> List.map (fun vh -> 
                                                            let b, (_,vehc) = (singleStep vh.behaviour (state,vh) dt) 
                                                            {vehc with behaviour = b}) 
            do! updateState_ {state with vehicles = vehicles'}
        }

    let deleteUnUsedVehicles =
        co{
            let! state = getState_
            do! updateState_ {state with vehicles = state.vehicles |> List.filter( fun vehicle -> vehicle.position.Y < 1050.f && vehicle.position.Y > -50.f && vehicle.position.X < 1650.f && vehicle.position.X > -50.f)}
        }

    repeat_
        (co{
            do! spawNewVehicles 
            do! wait_ 11.f
        })
    <||
    repeat_
        updateTrafficlights
    <||
    repeat_
        updateVehicles
    <||
    repeat_
        deleteUnUsedVehicles



let draw (spritebatch: SpriteBatch) (state: SimulationState) =
    spritebatch.Draw(state.background, Vector2.Zero, Color.White)
    state.vehicles |>
        List.iter(fun vehicle -> Vehicle.draw spritebatch state.texture vehicle)
    state.trafficlights |>
        List.iter(fun light -> TrafficLight.draw spritebatch state.texture light)