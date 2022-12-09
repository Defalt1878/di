using TagCloudCreator.Domain.Providers;
using TagCloudCreator.Interfaces.Providers;
using TagCloudCreator.Interfaces.Settings;
using TagCloudCreatorExtensions.ImageSavers;

namespace TagCloudTests;


[TestFixture]
public class ImageSaverProvider_Should
{
    private IImageSaverProvider _imageSaverProvider = null!;
    private IImagePathSettings _pathSettings = null!;

    [SetUp]
    public void Setup()
    {
        _pathSettings = A.Fake<IImagePathSettings>();
        var pathSettingsProvider = A.Fake<IImagePathSettingsProvider>();
        A.CallTo(() => pathSettingsProvider.GetImagePathSettings())
            .Returns(_pathSettings);

        _imageSaverProvider = new ImageSaverProvider(
            new[]
            {
                new PngImageSaver(pathSettingsProvider)
            },
            pathSettingsProvider
        );
    }

    [TestCase("png", typeof(PngImageSaver), TestName = "Png")]
    public void ReturnCorrectSaver_ForCorrectExtension(string extension,
        Type expectedSaverType)
    {
        _pathSettings.ImagePath = $"image.{extension}";
        _imageSaverProvider.GetSaver().GetType().Should().Be(expectedSaverType);
    }

    [Test]
    public void ThrowOperationException_ForBadExtension()
    {
        _pathSettings.ImagePath = "image.abc";
        _imageSaverProvider.Invoking(provider => provider.GetSaver())
            .Should().Throw<InvalidOperationException>()
            .Which.Message.Should().Contain(".abc");
    }
}