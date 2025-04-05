# Crater

Crater is a programming language based on Lua.  
The goal is to add static typing and compile directly into Lua source code.

## Planned features

- [ ] Standard Lua types
  - [ ] Numbers
    - [x] `integer`, `decimal`, `exponential`, `hexadecimal` and `binary`
    - [x] Binary `+`, `-`, `*`, `/`, `^` and `%`
    - [x] Unary `-`
    - [ ] Concatenation `..`
    - [ ] To string
  - [ ] Strings
  - [ ] Booleans
  - [ ] Tables
  - [ ] Tuples
  - [ ] Functions
    - [ ] Variable arguments
    - [ ] Multiple returns
- [ ] Custom type definition
- [ ] Support for `require`
- [ ] Generic support
  - [ ] Custom `List<T>` type
  - [ ] Custom `Dictionary<K, V>` type
- [ ] Handling special syntax
  - [ ] Garry's Mod support
- [ ] Asynchronous support
  - [ ] Coroutine type support
- [ ] Custom compilation settings
  - [ ] Custom tabulation
  - [ ] Custom spacing
  - [ ] Conserve comments
- [ ] Documentation generation
- [ ] Library definition

## Examples

```lua
local function printFormatted( msg: string, ...: any ): void
    local message = string.format( msg, ... )
    print( message )
end

local target: string = "World"

printFormtted( "Hello, %s!", target )
```
