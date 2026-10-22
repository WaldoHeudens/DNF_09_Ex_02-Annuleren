// Dit voorbeeld toont hoe je langlopend werk netjes kan annuleren met een
// CancellationToken. We tellen af van 10 naar 1, maar breken automatisch af
// na 1 seconde. In een echte app zou die annulering bijvoorbeeld komen van
// een gebruiker die op een "Stop"-knop drukt.

using CancellationTokenSource cts = new();
cts.CancelAfter(TimeSpan.FromSeconds(1));

Console.WriteLine("Start het aftellen (wordt geannuleerd na 1 seconde)...");

try
{
    await TelAfAsync(10, cts.Token);
    Console.WriteLine("Aftellen volledig afgerond.");
}
catch (OperationCanceledException)
{
    // Zowel ThrowIfCancellationRequested als Task.Delay(..., ct) gooien deze
    // exception bij annulering. Hier vangen we ze op en stoppen we netjes.
    Console.WriteLine("Aftellen werd geannuleerd.");
}

static async Task TelAfAsync(int vanaf, CancellationToken ct)
{
    for (int i = vanaf; i > 0; i--)
    {
        // Controleer voor elke stap of er intussen geannuleerd is.
        ct.ThrowIfCancellationRequested();

        Console.WriteLine($"  {i}...");

        // Task.Delay honoreert de token: bij annulering breekt het wachten
        // meteen af met een OperationCanceledException.
        await Task.Delay(300, ct);
    }
}
