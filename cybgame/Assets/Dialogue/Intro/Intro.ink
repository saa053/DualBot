VAR shouldSave = true
VAR saveString = "intro"
VAR introDone = false

-> main

=== resetLabel ===
{introDone: -> tips | -> main}

=== main ===
Det er deres oppgave å reversere angrepet. For å få det til trenger dere 4 AI-disker. Dere får tak i AI-disker ved å hjelpe robotene i fabrikken. \
\
Når dere har samlet 4 AI-disker må dere finne hovedmaskinen og resette systemet. Lykke til!

~ introDone = true
-> END

=== tips ===
Skal jeg fortelle hva dere skal igjen?
    + [Ja]
        -> main
    + [Nei]
Lykke til!
-> END


