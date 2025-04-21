# Crater Diagnostics

Diagnostics are the error and warning messages the compiler will generate if it encounters problematic behavior in the source code.

## Diagnostic Categories

The diagnostics all have a unique code in the format `CRA[0-6][0-99]`.  
The first number represents the category in which the diagnostic belongs.

| Code Range | Severity | Category Name     | Description                                         |
|------------|----------|-------------------|-----------------------------------------------------|
| CRA1*xx*   | Error    | Syntax Errors     | Tokenization and parsing failures, malformed code.  |
| CRA2*xx*   | Error    | Type Errors       | Type mismatches, inference failures, invalid casts. |
| CRA3*xx*   | Error    | Name Resolution   | Undeclared variables, duplicate definitions.        |
| CRA4*xx*   | Warning  | Semantic Warnings | Unused variables, unreachable code.                 |
| CRA5*xx*   | Warning  | Linter Warnings   | Non-critical suggestions for cleaner code.          |
| CRA6*xx*   | Fatal    | Internal Errors   | Unexpected states, bugs in the compiler.            |

### Syntax Errors

### Type Errors

**[CRA200]** Type Mismatch  
> The types of two values do not match where they are expected to be the same.

**[CRA201]** Undefined Type  
> A variable or function expects a type that is not defined or recognized.

**[CRA202]** Nullable Type Mismatch
> There is a mismatch involving a possible `nil` assignment to a non-nullable type.

**[CRA203]** Unsupported Binary Operation
> A binary operator is applied between two incompatible types.

**[CRA204]** Unsupported Unary Operation
> A unary operator is applied to an incompatible type.

**[CRA205]** Unsupported Comparison Operation
> A comparison operator is applied between two incompatible types.

**[CRA206]** Invalid Type Conversion
> An implicit conversion between types failed because the source type cannot be converted to the target type.

**[CRA207]** Invalid Cast
> A type cast ir type conversion operation failed because the source type is not convertible to the target type.

**[CRA208]** Failed Type Resolution
> The type system could not determine the type of an expression.

**[CRA209]** Failed Type Inference
> The type system failed to infer a type where one was expected.

**[CRA210]** Generic Type Mismatch
> A generic type argument does not match the constraints of the generic type.

**[CRA211]** Argument Type Mismatch
> The type of an argument passed to a function or method does not match the expected type.

### Name Resolution

### Semantic Warnings

### Linter Warnings

### Internal Errors
