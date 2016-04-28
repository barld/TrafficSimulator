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

    let rec zip: list<'a> -> list<'b> -> list<'a*'b> =
        fun l1 l2 ->
            failwith "not implemented"

    let rec unZip: list<'a*'b> -> list<'a> * list<'b> =
        fun l ->
            failwith "not implemented"
