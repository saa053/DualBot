VAR shouldSave = true
VAR saveString = "portableMail"
VAR GO = false

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, jeg har en rekke e-postadresser og trenger hjelp til å vurdere hvilke som er trygge. Kan dere hjelpe meg? 
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
Okei... Hvis dere ombestemmer dere kan dere få en AI-disk som premie.
-> END

=== start ===
Tusen takk! Dere skal få en AI-disk som premie etterpå! 
I rommet ligger det en rekke e-poster med tilhørende adresser. Deres oppgave er å sortere e-postadressene som enten trygg (grønt felt) eller utrygg (rødt felt). Dere kan plukke opp og ned e-postene med handlingstasten.  
Når dere er fornøyde med sorteringen trykker dere på den røde knappen. Dere trenger 3 riktige svar for å få premien. Hvis dere trenger tips underveis kan dere spørre meg! La oss starte! 
    ~ GO = true
-> END

=== tips ===
Hvilket tips vil du ha?
    +[Nr.1]
        Svindlere vil ofte bruke adresser som er lik originalen, men vil endre en liten detalj, for eksempel bytte ‘i’ ut med ‘1’ eller fjerne en bokstav. 
        ->DONE
    +[Nr.2]
        Svindlere kan bruke et annet toppdomene, som er de siste bokstavene i adressen. Istedenfor ‘.no’ eller ‘.com’, bruker de kanskje ‘.biz’ sammen med kjente navn.
        ->DONE
    +[Nr.3]
        Svindlere kan utvide kjente avsenderadresser med ord som ser trygge ut, for eksempel ‘@bank-hjelp.no.’ istedenfor ‘@bank.no’. 
        ->DONE
    +[Nr.4]
        Større organisasjoner bruker sjeldent kjente e-post-tjenester som gmail og outlook.
        ->DONE

-> END


