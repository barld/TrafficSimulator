module PracticeList

    let rec map: ('a -> 'b) -> list<'a> -> list<'b> =
        fun mapper list ->
            failwith "not implemented"

    let rec filter: ('a -> bool) -> list<'a> -> list<'a> = 
        fun predicate list ->
            failwith "not implemented"

    let rec tryFind: ('a -> bool) -> list<'a> -> Option<'a> =
        fun predicate list ->
            failwith "not implemented"

    let rec iter: ('a -> Unit) -> list<'a> -> Unit =
        fun f list ->
            failwith "not implemented"
