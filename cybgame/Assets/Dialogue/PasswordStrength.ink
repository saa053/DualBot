{init:
    -> init
    -else:
    -> main
}

=== init ===
VAR shouldSave = true
VAR saveString = "passwordStrength"

VAR shouldInit = false
VAR GO = false


-> main


=== main ===
~ shouldInit = false
Hei, jeg har en liste med passord og må finne ut hvilke som er gode. Kan du hjelpe meg?
    
    + [Sure bro]
        -> start
    + [Nope]
    
:(
->END

=== start ===
Tusen takk! Du skal få en AI disk som premie etterpå!
Dere har tre knapper i dette rommet markert "Svakt", "Middels", "Sterkt"
Jeg skal nå vise et passord på skjermen. Så bruker dere knappene til å svare hvor godt passordet er
LETSGO!
    ~ GO = true
->END



