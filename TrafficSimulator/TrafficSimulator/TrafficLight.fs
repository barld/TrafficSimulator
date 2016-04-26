module TrafficLight

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics


type status = 
        | Green of float32
        | Orange of float32
        | Red of float32

     
type trafficLight = {status: status; position: Vector2}


let update dt light =
    match light.status with
    | Green(t) -> 
        if t < 0.f then
            {light with status = Orange(2.f)} 
        else 
            {light with status = Green(t-dt)}
    | Orange(t) when t < 0.f -> {light with status = Red(11.f)}
    | Orange(t) -> {light with status = Orange(t-dt)}
    | Red(t) -> 
        if t < 0.f then
            {light with status = Green(9.f)} 
        else 
            {light with status = Red(t-dt)}
   

let draw (spritebatch: SpriteBatch) (texture: Texture2D) (light: trafficLight) = 
    let color =
        match light.status with
        | Green(_) -> Color.Green
        | Orange(_) -> Color.Orange
        | Red(_) -> Color.Red

    spritebatch.Draw(texture, new Rectangle(light.position.X - 16.f |> int, light.position.Y - 16.f |> int, 32, 32), color)
    ()