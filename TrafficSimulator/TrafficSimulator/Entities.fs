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
        velocity : float32
        frontDirection : Vector2
        acceleration: float32
    } with
      static member TopLeftVehicle = {position = new Vector2(375.f, -16.f); velocity = 0.f; frontDirection = new Vector2(0.f, 1.f); acceleration = 5.f}
      static member TopRightVehicle = {position = new Vector2(1175.f, -16.f); velocity = 0.f; frontDirection = new Vector2(0.f, 1.f); acceleration = 5.f}

      static member MiddleRightTopVehicle = {position = new Vector2(1616.f, 275.f); velocity = 0.f; frontDirection = new Vector2(-1.f, 0.f); acceleration = 5.f}
      static member MiddleRightBottomVehicle = {position = new Vector2(1616.f, 745.f); velocity = 0.f; frontDirection = new Vector2(-1.f, 0.f); acceleration = 5.f}

      static member MiddleLeftTopVehicle = {position = new Vector2(-16.f, 325.f); velocity = 0.f; frontDirection = new Vector2(1.f, 0.f); acceleration = 5.f}
      static member MiddleLeftBottomVehicle = {position = new Vector2(-16.f, 790.f); velocity = 0.f; frontDirection = new Vector2(1.f, 0.f); acceleration = 5.f}

      static member BottomLeftVehicle = {position = new Vector2(425.f, 1016.f); velocity = 0.f; frontDirection = new Vector2(0.f, -1.f); acceleration = 5.f}
      static member BottomRightVehicle = {position = new Vector2(1225.f, 1016.f); velocity = 0.f; frontDirection = new Vector2(0.f, -1.f); acceleration = 5.f}

type SimulationState = {
    trafficlights : List<trafficLight>
    vehicles : List<vehicle>
    texture: Texture2D
    background: Texture2D
    vehicleSpawnCooldown: float32
}
