VAR shouldSave = true
VAR saveString = "Mail"
VAR GO = false

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, jeg har en rekke e-postadresser og trenger hjelp til å vurdere hvilke som er trygge. Kan dere hjelpe meg? 
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
Hvis dere ombestemmer dere kan dere få en AI-disk hvis dere hjelper meg.
-> END

=== start ===
Tusen takk! Dere skal få en AI-disk etterpå! 
I rommet vil det ligge kasser med e-postadresser. Deres oppgave er å sortere de som enten trygg (grønt felt) eller utrygg (rødt felt). Dere kan plukke kassene opp og ned med handlingstasten.  
Når dere er fornøyde med sorteringen trykker dere på den røde knappen. Dere trenger 3 riktige svar for å få AI-disken. 
Hvis dere trenger tips underveis kan dere spørre meg! La oss starte! 
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
    +[Nei takk]
        Snakk med meg igjen hvis dere vil ha tips!
        ->DONE

-> END


