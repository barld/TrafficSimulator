module TrafficLight

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics


type status = 
        | Green
        | Orange
        | Red

     
type trafficLight = {status: status; position: Vector2}



let draw (spritebatch: SpriteBatch) (texture: Texture2D) (trafficlight: trafficLight) = 
    let color =
        match trafficlight.status with
        | Green -> Color.Green
        | Orange -> Color.Orange
        | Red -> Color.Red

    spritebatch.Draw(texture, new Rectangle(vehicle.position.X - 16.f |> int, vehicle.position.Y - 16.f |> int, 32, 32), color)
    ()