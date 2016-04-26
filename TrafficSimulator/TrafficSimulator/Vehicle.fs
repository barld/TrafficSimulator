module Vehicle
open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type vehicle = 
    {
        position : Vector2
        velocity : Vector2
        acceleration : Vector2
    } with
      static member TopVehicle = {position = new Vector2(375.f, -16.f); velocity = Vector2.Zero; acceleration = new Vector2(0.f, 40.f)}
      static member RightVehicle = {position = new Vector2(816.f, 275.f); velocity = Vector2.Zero; acceleration = new Vector2(-38.f, 0.f)}
      static member BottomVehicle = {position = new Vector2(425.f, 616.f); velocity = Vector2.Zero; acceleration = new Vector2(0.f, -40.f)}
      static member LeftVehicle = {position = new Vector2(-16.f, 325.f); velocity = Vector2.Zero; acceleration = new Vector2(38.f, 0.f)}


let update (dt:float32) vehicle = 
    let v = vehicle.velocity + vehicle.acceleration * dt
    let pos = vehicle.position + v * dt
    {
        vehicle with
            position = pos
            velocity = v
    }

let draw (spritebatch: SpriteBatch) (texture: Texture2D) (vehicle: vehicle) = 
    spritebatch.Draw(texture, new Rectangle(vehicle.position.X - 16.f |> int, vehicle.position.Y - 16.f |> int, 32, 32), Color.HotPink)
    ()