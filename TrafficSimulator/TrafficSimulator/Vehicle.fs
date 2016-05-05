module Vehicle
open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Entities
open Coroutine



let isInPrecisionRange (precision:float32) (v1:float32) (v2:float32) =
    abs (v1 - v2) < precision


let update (dt:float32) (state: SimulationState) vehicle = 
    let d = atan2 vehicle.frontDirection.Y  vehicle.frontDirection.X
    let light = state.trafficlights |> List.tryFind (fun light -> 
        let dTarget = atan2 (light.position.Y - vehicle.position.Y)  (light.position.X - vehicle.position.X) 
        let b1 = Vector2.Distance(light.position, vehicle.position) < 100.f 
        let b2 = isInPrecisionRange 0.05f d dTarget 
        let b3 = b1 && b2
        b3)

    let potentialVehicle = 
        state.vehicles 
        |> List.filter ( fun potentialVehicle -> Vector2.Distance(potentialVehicle.position, vehicle.position) < 100.f )
        |> List.sortBy ( fun potentialVehicle -> Vector2.Distance(potentialVehicle.position, vehicle.position))
        |> List.tryFind (fun potentialVehicle ->
            let dTarget = atan2 (potentialVehicle.position.Y - vehicle.position.Y)  (potentialVehicle.position.X - vehicle.position.X) 
            let b1 = Vector2.Distance(potentialVehicle.position, vehicle.position) < 100.f 
            let b2 = isInPrecisionRange 0.1f d dTarget 
            let b3 = potentialVehicle.position <> vehicle.position
            let b4 = b1 && b2 && b3
            b4)
    
    //http://stackoverflow.com/a/1088194
    let acc =
        match potentialVehicle with
        | Some(vehc) when vehc.velocity < vehicle.velocity -> 
            let distanceToOtherCar = Vector2.Distance(vehicle.position, vehc.position)
            if distanceToOtherCar < 50.f then
                (vehc.velocity**2.f - vehicle.velocity**2.f)/(2.f * (-100.f))
            else
                (vehc.velocity**2.f - vehicle.velocity**2.f)/(2.f * (distanceToOtherCar - 50.f))
        | _ ->
            match light with
            | Some(l) -> 
                let distanceToLight = Vector2.Distance(l.position, vehicle.position)
                match l.status with
                | Green(_) -> 9.f//drive
                | Orange(_) when distanceToLight < 40.f && vehicle.velocity > 30.f -> 11.f//drive
                | Orange(_) -> -(vehicle.velocity**2.f)/ (distanceToLight - 20.f)
                | Red(_) when distanceToLight < 10.f -> 11.f//drive
                | Red(_) -> -(vehicle.velocity**2.f)/ (distanceToLight - 20.f) //stop
            | None -> 9.f

    
    let v = 
        match acc with
        | acc when acc < 0.f && vehicle.velocity < 0.1f -> 0.f
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

//------------------------------------------------------------------------------------------------------------------------- coroutines

let updateVehicle (veh:vehicle) =
    fun (state,_) dt ->
        Done((),(state,veh))

let getVehicle () =
    fun (state, (vehicle:vehicle)) _ ->
        Done(vehicle,(state,vehicle))

let getSimulationState () =
    fun ((state:SimulationState),vehicle) _ ->
        Done(state,(state,vehicle))



let rec getDefaulBehaviour () =
    co{
        let! (state: SimulationState), (currentVehicle:vehicle) = getState_
        let! dt = getDeltaTime_
        do! updateVehicle (update dt state currentVehicle)
        do! yield_
        return! getDefaulBehaviour()
    }

let drive () =
    co{
        let! vehicle = getVehicle()
        let! dt = getDeltaTime_
        let v = 
            match vehicle.acceleration with
            | acc when acc < 0.f && vehicle.velocity < 0.1f -> 0.f
            | acc -> vehicle.velocity + acc * dt
        let pos = vehicle.position + vehicle.frontDirection * v * dt
        let newVehicle = {vehicle with velocity = v; position = pos }
        do! updateVehicle newVehicle
        do! yield_
    }

let rec driveNTime n =
    co{            
        if n < 0.f then
            return ()
        else
            let! dt = getDeltaTime_
            do! drive()
            return! driveNTime (n-dt)
        }

let tryGetLightForVehicke () =
    co{
        let! vehicle = getVehicle()
        let! simState = getSimulationState()
        let d = atan2 vehicle.frontDirection.Y  vehicle.frontDirection.X

        return simState.trafficlights 
            |> List.sortBy(fun light -> Vector2.Distance(light.position, vehicle.position))
            |> List.tryFind (fun light -> 
                let dTarget = atan2 (light.position.Y - vehicle.position.Y)  (light.position.X - vehicle.position.X) 
                Vector2.Distance(light.position, vehicle.position) < 100.f && isInPrecisionRange 0.05f d dTarget )
        }

let TryGetVehicleForVehicle () =
    co{
        let! vehicle = getVehicle()
        let! simState = getSimulationState()
        let d = atan2 vehicle.frontDirection.Y  vehicle.frontDirection.X

        return
            simState.vehicles
            |> List.filter ( fun potentialVehicle -> Vector2.Distance(potentialVehicle.position, vehicle.position) < 100.f )
            |> List.sortBy ( fun potentialVehicle -> Vector2.Distance(potentialVehicle.position, vehicle.position))
            |> List.tryFind (fun potentialVehicle ->
                let dTarget = atan2 (potentialVehicle.position.Y - vehicle.position.Y)  (potentialVehicle.position.X - vehicle.position.X) 

                Vector2.Distance(potentialVehicle.position, vehicle.position) < 100.f
                && isInPrecisionRange 0.1f d dTarget 
                && potentialVehicle.position <> vehicle.position)
    }

let calculateAccForLight (light:trafficLight Option) (maxAcc) =
    co{
        match light with
        | None -> return maxAcc
        | Some(light) ->
            let! vehicle = getVehicle()
            let distanceToLight = Vector2.Distance(vehicle.position, light.position)
            match light.status with
            | Green(_) -> return maxAcc
            | Orange(_) -> return -(vehicle.velocity**2.f)/ (2.f*(distanceToLight - 30.f))
            | Red(_) -> return -(vehicle.velocity**2.f)/ (2.f*(distanceToLight - 30.f))
    }

let calculateAccForVehicle (oVehicle: vehicle Option) (maxAcc) =
    co{
        let! vehicle = getVehicle()
        match oVehicle with
        | Some(vehc) when vehc.velocity < vehicle.velocity -> 
            let distanceToOtherCar = Vector2.Distance(vehicle.position, vehc.position)
            let acc =
                if distanceToOtherCar < 50.f then
                    (vehc.velocity**2.f - vehicle.velocity**2.f)/(2.f * (-100.f))
                else
                    (vehc.velocity**2.f - vehicle.velocity**2.f)/(2.f * (distanceToOtherCar - 50.f))
            return if acc < maxAcc then acc else maxAcc
        | _ -> return maxAcc
    }

let rec getBehaviour () =
    let maxVelocity = 50.f
    let maxAcc = 10.f

    co{
        do! driveNTime 0.5f
        let! vehicle = getVehicle()
        
        let! pvehicle = TryGetVehicleForVehicle()
        let! plight = tryGetLightForVehicke()

        let! lightAcc = calculateAccForLight plight maxAcc
        let! vehicleAcc = calculateAccForVehicle pvehicle maxAcc
             
            

        do! updateVehicle {vehicle with acceleration=(min lightAcc vehicleAcc)}
        return! getBehaviour()
    }