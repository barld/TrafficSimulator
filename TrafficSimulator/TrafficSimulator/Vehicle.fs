﻿module Vehicle
open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Entities



let isInPrecisionRange (precision:float32) (v1:float32) (v2:float32) =
    abs (v1 - v2) < precision


let update (dt:float32) (state: SimulationState) vehicle = 
    let d = atan2 vehicle.frontDirection.Y  vehicle.frontDirection.X
    let light = state.trafficlights |> List.tryFind (fun light -> 
        let dTarget = atan2 (light.position.Y - vehicle.position.Y)  (light.position.X - vehicle.position.X) 
        let b1 = Vector2.Distance(light.position, vehicle.position) < 150.f 
        let b2 = isInPrecisionRange 0.05f d dTarget 
        let b3 = b1 && b2
        b3)
    
    let acc =
        match light with
        | Some(l) -> 
            let distanceToLight = Vector2.Distance(l.position, vehicle.position)
            match l.status with
            | Green(_) -> 5.f//drive
            | Orange(_) when distanceToLight < 40.f -> 6.f//drive
            | Orange(_) -> -10.f //stop
            | Red(_) when distanceToLight < 10.f -> 7.f//drive
            | Red(_) -> -12.f //stop
        | _ -> 5.f
    let v = 
        match acc with
        | acc when acc < 0.f && vehicle.velocity < 0.f -> 0.f
        | _ -> vehicle.velocity + acc * dt

    let pos = vehicle.position + vehicle.frontDirection * v * dt

    {
        vehicle with
            position = pos
            velocity = v
            acceleration = acc
    }

let draw (spritebatch: SpriteBatch) (texture: Texture2D) (vehicle: vehicle) = 
    spritebatch.Draw(texture, new Rectangle(vehicle.position.X - 16.f |> int, vehicle.position.Y - 16.f |> int, 32, 32), Color.HotPink)
    ()

