using System.Security;
using Localizr.Translations.Services;

namespace Localizr.Translations.Tests;

public class SimpleFileParserTests
{
    [Fact]
    public async Task WhenReadingSimpleFile_ItIsParsedToMultipleTranslationNodes()
    {
        // Arrange
        var parser = new TranslationsJsonFileParserService();
        var filename = "simple.json";
        var simpleExampleResourceName = typeof(SimpleFileParserTests).Assembly.GetManifestResourceNames()
            .FirstOrDefault(ers => ers.EndsWith(filename));
        var stream = typeof(SimpleFileParserTests).Assembly.GetManifestResourceStream(simpleExampleResourceName);

        // Act
        var result = await parser.ImportFromFile(stream, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.FirstOrDefault(r=> r.Key == "error").Value, "Error");

await         parser.ExportToFile("D:\\Temp\\Test.json", result, CancellationToken.None);
    }
}