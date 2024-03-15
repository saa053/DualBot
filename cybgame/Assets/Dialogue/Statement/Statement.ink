VAR shouldSave = true
VAR saveString = "statements"
VAR GO = false

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, jeg har en rekke utsagn og trenger hjelp til å vurdere hvilke som er riktige. Kan dere hjelpe meg? 
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
#Okei... Hvis dere ombestemmer dere kan dere få en AI-disk som premie.
-> END

=== start ===
Tusen takk! Dere skal få en AI-disk som premie etterpå! 
Jeg kommer til å vise et og et utsagn på skjermen bak meg og jeg vil at dere går til den knappen dere mener passer utsagnet. 
Dere trenger 3 riktige svar for å få premien. La oss starte!
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


