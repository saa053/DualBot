VAR shouldSave = true
VAR saveString = "statements"
VAR GO = false

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, statements, letsgo? 
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
Okei... Hvis dere ombestemmer dere kan dere få en AI-disk som premie.
-> END

=== start ===

    ~ GO = true
-> END

=== tips ===
Hvilket tips vil du ha?
    +[Nr.1]
        Tips1 
        ->DONE
    +[Nr.2]
        Tips2
        ->DONE
    +[Nr.3]
        Tips3 
        ->DONE
    +[Nr.4]
        Tips4
        ->DONE
    +[Neitakk]
        ->DONE

-> END


