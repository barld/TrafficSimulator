module SimulationState

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open TrafficLight

type SimulationState {
    trafficlights : List<TrafficLight>
    cars : List<Vehicles>
}