VAR shouldSave = true
VAR saveString = "passwordStrength"
VAR GO = false

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, jeg har en liste med passord og trenger hjelp til å vurdere hvilke som er gode. Kan dere hjelpe meg?
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
Hvis dere ombestemmer dere kan dere få en AI-disk hvis dere hjelper meg.
-> END

=== start ===
Tusen takk! Dere skal få en AI-disk etterpå!
Jeg kommer til å vise ett og ett passord på skjermen bak meg og jeg vil at dere går til den knappen som passer styrken til passordet.
#Jeg vil at dere går til den knappen i dette rommet som dere mener passer styrken til passordet på skjermen.
Dere trenger 3 riktige svar for å få AI-disken. Hvis dere trenger tips underveis kan dere spørre meg! La oss starte!
    ~ GO = true
-> END

=== tips ===
Hvilket tips vil dere ha?
    +[Nr.1]
        Sterke passord er ofte lange og består av en rekke ulike bokstaver, tall og spesialtegn.
        ->DONE
    +[Nr.2]
        Unngå å bruke vanlige ord
        ->DONE
    +[Nr.3]
        Unngå å bruke tall som kommer i rekkefølge, for eksempel '123' og '567'. 
        ->DONE
    +[Nr.4]
        Lange passord er bedre enn korte.
        ->DONE
    +[Nei takk]
        Snakk med meg igjen hvis dere vil ha tips!
        ->DONE
-> END


