﻿using TagCloudCreator.Interfaces;
using TagCloudCreator.Interfaces.Providers;

namespace TagCloudCreator.Domain.Providers;

public class WordsFileReaderProvider : IWordsFileReaderProvider
{
    private readonly Dictionary<string, IWordsFileReader> _wordsFileReaders;
    private readonly IWordsPathSettingsProvider _pathSettingsProvider;

    public WordsFileReaderProvider(
        IEnumerable<IWordsFileReader> wordsFileReaders,
        IWordsPathSettingsProvider pathSettingsProvider)
    {
        _wordsFileReaders = wordsFileReaders.ToDictionary(reader => reader.SupportedExtension);
        _pathSettingsProvider = pathSettingsProvider;
    }

    public IEnumerable<string> SupportedExtensions => _wordsFileReaders.Keys;

    public IWordsFileReader GetReader()
    {
        var wordsFileExtension = Path.GetExtension(_pathSettingsProvider.GetWordsPathSettings().WordsPath);
        if (_wordsFileReaders.TryGetValue(wordsFileExtension, out var result))
            return result;
        throw new ArgumentException($"No reader for extension: {wordsFileExtension}");
    }
}