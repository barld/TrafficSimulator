module Entities

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Coroutine

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
        acceleration : float32
        behaviour : Coroutine<SimulationState*vehicle,Unit>
    } with
        static member Zero = {position=Vector2.Zero;velocity = 0.f;frontDirection=Vector2.Zero;acceleration=5.f;behaviour=(fun s dt -> Done((),s))}

        static member TopLeftVehicle = {vehicle.Zero with position = new Vector2(375.f, -16.f); frontDirection = new Vector2(0.f, 1.f)}
        static member TopRightVehicle = {vehicle.Zero with position = new Vector2(1175.f, -16.f); frontDirection = new Vector2(0.f, 1.f)}

        static member MiddleRightTopVehicle = {vehicle.Zero with position = new Vector2(1616.f, 275.f); frontDirection = new Vector2(-1.f, 0.f)}
        static member MiddleRightBottomVehicle = {vehicle.Zero with position = new Vector2(1616.f, 745.f); frontDirection = new Vector2(-1.f, 0.f)}

        static member MiddleLeftTopVehicle = {vehicle.Zero with position = new Vector2(-16.f, 325.f); frontDirection = new Vector2(1.f, 0.f)}
        static member MiddleLeftBottomVehicle = {vehicle.Zero with position = new Vector2(-16.f, 790.f); frontDirection = new Vector2(1.f, 0.f)}

        static member BottomLeftVehicle = {vehicle.Zero with position = new Vector2(425.f, 1016.f); frontDirection = new Vector2(0.f, -1.f)}
        static member BottomRightVehicle = {vehicle.Zero with position = new Vector2(1225.f, 1016.f); frontDirection = new Vector2(0.f, -1.f)}
and
    SimulationState = {
    trafficlights : List<trafficLight>
    vehicles : List<vehicle>
    texture: Texture2D
    background: Texture2D
    behaviour: Coroutine<SimulationState,Unit>
}
