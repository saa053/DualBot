VAR shouldSave = true
VAR saveString = "drData"
VAR hasTalked = false

->main

=== resetLabel ===

{hasTalked:
    -> sub
    - else:
    -> main
}


=== main ===
~hasTalked = true
Hi, I'm Dr. Data. Do you want to help me with my research on internet security?
To help my crucial research, can you give me your facebook password?

* [I would love to help! My password is ...]
* [No, my password is private]
* [I should ask my parents for permission first]

- I see...

-> END

=== sub ===
Can I help you?
-> END



