module TrafficLight

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Entities
open Coroutine





let update dt light =
    match light.status with
    | Green(t) -> 
        if t < 0.f then
            {light with status = Orange(3.f)} 
        else 
            {light with status = Green(t-dt)}
    | Orange(t) when t < 0.f -> {light with status = Red(11.f)}
    | Orange(t) -> {light with status = Orange(t-dt)}
    | Red(t) -> 
        if t < 0.f then
            {light with status = Green(6.f)} 
        else 
            {light with status = Red(t-dt)}
   

let draw (spritebatch: SpriteBatch) (texture: Texture2D) (light: trafficLight) = 
    let color =
        match light.status with
        | Green(_) -> Color.Green
        | Orange(_) -> Color.Orange
        | Red(_) -> Color.Red

    spritebatch.Draw(texture, new Rectangle(light.position.X - 8.f |> int, light.position.Y - 8.f |> int, 16, 16), color)
    ()
