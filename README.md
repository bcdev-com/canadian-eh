# Canadian, Eh?

Creator for a static, regex-based, site to convert text from your clipboard between Canadian and American spellings.

Most details are covered at https://regex.ca/about.

```dotnet run``` from the root of the project will create ```replacements.js``` in the /docs directory which defines the conversion process.  It generates documention that is added to the ```about-source.html``` template to create ```about.html``` at the same time.

New words or phrases can be added to ```RegexList.cs```.

```Conjugation.cs``` might be useful on its own for other manipulating of English text.

-Blake, June 2024