open Simulation

[<EntryPoint>]
let main argv =     
    use simulation = new TrafficSimulator()
    simulation.Run()

    0 // return an integer exit code