VAR shouldSave = true
VAR saveString = "URL"
VAR GO = false
VAR slash = "/"

-> main

=== resetLabel ===
{GO: -> tips | -> main}

=== main ===
Hei, jeg har en rekke URL-er og trenger hjelp til å vurdere hvilke som er trygge. Kan dere hjelpe meg? 
    + [Ja]
        -> start
    + [Nei, ikke akkurat nå]
    
Hvis dere ombestemmer dere kan dere få en AI-disk hvis dere hjelper meg.
-> END

=== start ===
I rommet vil det ligge kasser med URL-er. Deres oppgave er å sortere de som enten trygg (grønt felt) eller utrygg (rødt felt). Dere kan plukke kassene opp og ned med handlingstasten. \
\
Når dere er fornøyde med sorteringen trykker dere på den røde knappen. Dere trenger minst 3 riktige svar for å få AI-disken.

Hvis dere trenger tips underveis kan dere spørre meg! La oss starte! 
    ~ GO = true
-> END

=== tips ===
Hvilket tips vil dere ha?
    +[Nr.1]
        Se etter URL-er som starter med 'https:{slash}{slash}' istedenfor 'http:{slash}{slash}'. ‘s’-en indikerer at det er en sikker tilkobling, som er viktig for å beskytte sensitiv informasjon.  NB! det er ikke garantert at nettsiden er trygg selv om det er 'https:{slash}{slash}'   
        ->DONE
    +[Nr.2]
        Svindlere kan etterligne trygge URL-er og endre små detaljer. Istedenfor ‘microsoft.com’ kan de bruke ‘rnicrosoft.com’ 
        ->DONE
    +[Nr.3]
        Svindlere kan bruke et annet toppdomene enn de du er kjent med, som er de siste bokstavene i URL-en. Istedenfor ‘.no’ eller ‘.com’, bruker de kanskje ‘.link’.
        ->DONE
    +[Nei takk]
        Snakk med meg igjen hvis dere vil ha tips!
        ->DONE

-> END


