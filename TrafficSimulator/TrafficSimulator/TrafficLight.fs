module TrafficLight

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics


type status = 
        | Green
        | Orange
        | Red

     
type trafficLight = {status: status; position: Vector2}



let draw (spritebatch: SpriteBatch) (texture: Texture2D) (light: trafficLight) = 
    let color =
        match light.status with
        | Green -> Color.Green
        | Orange -> Color.Orange
        | Red -> Color.Red

    spritebatch.Draw(texture, new Rectangle(light.position.X - 16.f |> int, light.position.Y - 16.f |> int, 32, 32), color)
    ()