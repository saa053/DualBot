VAR shouldSave = true
VAR saveString = "intro"
VAR introDone = false

-> main

=== resetLabel ===
{introDone: -> tips | -> main}

=== main ===
Info

~ introDone = true
-> END

=== tips ===
Fortll igjen?
    + [Ja]
        -> main
    + [Nei]
Okei...
-> END


