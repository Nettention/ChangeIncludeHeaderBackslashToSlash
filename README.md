# ChangeIncludeHeaderBackslashToSlash

This tool changes backslash symbol to slash symbol in your C or C++ include header file statement.
For example,

```
#include "aa\bb.h"
```

will be changed to 

```
#include "aa/bb.h"
```

This tool works all C/C++ source files in the directory you specify. Yes, the batch work.


# Usage

Get this source project, then build it with any C# compiler.

Then run like this.

```
ChangeIncludeHeaderBackslashToSlash <your directory> <your file extensions>
```

Example:

```
ChangeIncludeHeaderBackslashToSlash.exe mywork\mysource h;inl;cpp
```

If you are using Linux, use Mono or something.
```
mono ChangeIncludeHeaderBackslashToSlash.exe mywork/mysource h;inl;cpp
```

# Meanwhile...
Don't forget to use [ProudNet](http://proudnet.com/) if you are developing a massive or performance-critical online multiplayer games.
