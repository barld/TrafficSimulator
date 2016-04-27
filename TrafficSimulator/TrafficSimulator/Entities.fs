module Entities

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type status = 
        | Green of float32
        | Orange of float32
        | Red of float32

     
type trafficLight = {status: status; position: Vector2}

type vehicle = 
    {
        position : Vector2
        velocity : Vector2
        frontDirection : Vector2
        acceleration: float32
    } with
      static member TopVehicle = {position = new Vector2(375.f, -16.f); velocity = Vector2.Zero; frontDirection = new Vector2(0.f, 1.f); acceleration = 5.f}
      static member RightVehicle = {position = new Vector2(816.f, 275.f); velocity = Vector2.Zero; frontDirection = new Vector2(-1.f, 0.f); acceleration = 5.f}
      static member BottomVehicle = {position = new Vector2(425.f, 616.f); velocity = Vector2.Zero; frontDirection = new Vector2(0.f, -1.f); acceleration = 5.f}
      static member LeftVehicle = {position = new Vector2(-16.f, 325.f); velocity = Vector2.Zero; frontDirection = new Vector2(1.f, 0.f); acceleration = 5.f}

type SimulationState = {
    trafficlights : List<trafficLight>
    vehicles : List<vehicle>
    texture: Texture2D
    background: Texture2D
    vehicleSpawnCooldown: float32
}
