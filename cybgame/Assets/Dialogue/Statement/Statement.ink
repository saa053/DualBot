VAR shouldSave = true
VAR saveString = "statements"
VAR GO = false

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, jeg har en rekke utsagn og trenger hjelp til å vurdere hvilke som er riktig. Kan dere hjelpe meg? 
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
Hvis dere ombestemmer dere kan dere få en AI-disk som premie.
-> END

=== start ===
Tusen takk! Dere skal få en AI-disk som premie etterpå! 
Jeg kommer til å vise ett og ett utsagn på skjermen bak meg og jeg vil at dere går til den knappen dere mener passer utsagnet. 
Dere trenger 3 riktige svar for å få premien. La oss starte!
    ~ GO = true
-> END

=== tips ===
Stå på!

-> END


