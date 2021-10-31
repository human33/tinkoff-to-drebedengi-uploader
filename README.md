# Tinkoff to drebedengi uploader

This app allows to upload to Drebedengi expenses uploaded from Tinkoff bank in a more convenient way - it removes all expenses duplicates (by sum and time). 

# How can I use it?


1. Download your expenses from Tinkoff bank
2. Download your expenses from Drebedengi
3. Run the app with these files as arguments: `t2d --tinkoff-dump <file-path> --drebedengi-dump <file-path> -o <result-path>`
4. Upload resulting file to Drebedengi

I hope, in future I'll be able to make it work without steps 2 and 4, but it will require to enter credentials every time.
