### OriModding.BF.l10n

Localisation library for providing translations of mods.

To use localised strings, create `strings.csv` in the root of the mod directory. Example:

```csv
,English,Italian
SPIRIT_FLAME,Spirit Flame,Fiamma dello spirito
```

Then fetch it in code:

```c#
using OriModding.BF.l10n;
Strings.Get("SPIRIT_FLAME");
```

It will fall back to English for any given key if a translation isn't found.
