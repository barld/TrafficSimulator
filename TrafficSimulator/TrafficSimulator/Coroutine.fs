module Coroutine
                    //  state -> dt -> costep
type Coroutine<'s, 'a> = 's -> float32 -> CoStep<'s,'a>
    and CoStep<'s,'a> = 
    | Done of 'a * 's
    | Yield of Coroutine<'s, 'a> * 's


let rec (>>=) p k =
    fun s dt->
        match p s dt with
        | Done(a, s') -> k a s' dt
        | Yield(left, s') -> Yield((left >>= k), s')

let ret x = fun s dt -> Done(x,s)

type CoroutineBuilder() =
    member this.Bind(p,k) = p >>= k
    member this.Return(x) = ret x
    member this.ReturnFrom x = x

let co = new CoroutineBuilder()

let wrapDone_ (x:'a) =
    fun s _ ->
        Done(x,s)

let yield_ = fun s dt -> Yield((fun s dt -> Done((), s)), s)

let getState_ = fun s dt -> Done(s,s)
let getDeltaTime_ = fun s dt -> Done(dt,s)

let rec wait_ (time:float32) =
    co{
        let! dt = getDeltaTime_ 
        if (time - dt) < 0.f then
            return ()
        else
            do! yield_
            return! wait_ (time-dt)
    }

let wait_with_elapsedTime_ (time:float32) =
    let rec _wait_ time =
        co{
            let! dt = getDeltaTime_ 
            if (time - dt) < 0.f then
                return dt
            else
                do! yield_
                let! elapsedTime = _wait_ (time-dt)
                return dt + elapsedTime
        }
    _wait_ time

let costep coroutine state =
    let rec _costep coroutine state (lastTime: System.DateTime) =
        let dateTime = System.DateTime.Now
        let dt = (dateTime - lastTime).TotalSeconds |> float32        
        match coroutine state dt with
        | Done(a, newState) ->  a, newState
        | Yield(c', s') -> _costep c' s' dateTime
    _costep coroutine state System.DateTime.Now

let singleStep coroutine state dt=     
    match coroutine state dt with
    | Done(a, newState) ->  (fun s dt -> Done((), s)), newState
    | Yield(c', s') -> c', s' 


