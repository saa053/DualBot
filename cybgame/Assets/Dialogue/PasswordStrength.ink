VAR shouldSave = true
VAR saveString = "passwordStrength"
VAR GO = false

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, jeg har en liste med passord og trenger hjelp til å vurdere hvilke som er gode.
Kan dere hjelpe meg?
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
:(
-> END

=== start ===
Tusen takk! Dere skal få en AI disk som premie etterpå!
Jeg skal vise et og et passord på skjermen.
Det er tre knapper i dette rommet markert "Svakt", "Middels" og "Sterkt".
Jeg vil at dere går til den knappen som passer styrken til passordet på skjermen.
Dere trenger 5 riktige svar for å få premien.
Hvis dere trenger tips underveis kan dere komme å snakke med meg!
La oss starte!
    ~ GO = true
-> END

=== tips ===
Her har du et tips:
{RANDOM(1, 5):
- 0: Dette er et tips
- 1: Tips 2 er...
- 2: Passord burde ha ...
- 3: Svake passord har ikke...
- 4: Tihi
- 5: Middels sterke passord er ofte...
}
-> END


