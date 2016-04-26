module Vehicle
open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type vehicle = 
    {
        position : Vector2
        velocity : Vector2
        acceleration : Vector2
    }

let draw (spritebatch: SpriteBatch) (texture: Texture2D) (vehicle: vehicle) = 
    spritebatch.Draw(texture, new Rectangle(vehicle.position.X - 16.f |> int, vehicle.position.Y - 16.f |> int, 32, 32), Color.HotPink)
    ()