﻿namespace Core.Diagnostics;

public interface IDiagnosticReporter
{
    void Report(Diagnostic diagnostic);
}