VAR shouldSave = true
VAR saveString = "intro"
VAR introDone = false

-> main

=== resetLabel ===
{introDone: -> tips | -> main}

=== main ===
Hei, fullfør mini games ja?
    + [Ja]
        -> start
    + [Nei]
    
Neivel?
-> start

=== start ===
Letsgo!
    ~ introDone = true
-> END

=== tips ===
Hva venter du på? Gå å utforsk!
-> END


