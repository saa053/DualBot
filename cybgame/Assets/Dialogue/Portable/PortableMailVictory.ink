VAR shouldSave = true
VAR saveString = "portableMail"
VAR GO = false

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, hjelp?
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
:(
-> END

=== start ===
#Tusen takk! Dere skal få en AI disk som premie etterpå!
#Jeg kommer til å vise et og et passord på skjermen bak meg.
#Jeg vil at dere går til den knappen i dette rommet som dere mener passer styrken til passordet på skjermen.
#Dere trenger 5 riktige svar for å få premien. Hvis dere trenger tips underveis kan dere komme å snakke med meg! La oss starte!
    ~ GO = true
-> END

=== tips ===
Hvilket tips vil du ha?
    +[Nr.1]
        Tips nr.1 er...
    +[Nr.2]
        Tips nr.2 er...
    +[Nr.3]
        Tips nr.3 er...
    +[Nr.4]
        Tips nr.4 er...

- -> tips

-> END


